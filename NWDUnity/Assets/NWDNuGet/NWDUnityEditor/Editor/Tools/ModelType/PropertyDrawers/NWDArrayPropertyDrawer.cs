using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDArrayPropertyDrawer : NWDPropertyDrawer
    {
        const int kSizeFieldWidth = 48;
        const int kBottomButtonHeight = 5;

        static private GUIStyle _ItemOn = null;
        static private GUIStyle _ItemActive = null;
        static private GUIStyle _Background = null;
        static private GUIStyle _ButtonForeground = null;
        static private GUIStyle _ButtonBackground = null;

        static protected GUIStyle ItemOn
        {
            get
            {
                if (_ItemOn == null)
                {
                    _ItemOn = new GUIStyle("CN EntryBackEven");
                }
                return _ItemOn;
            }
        }
        static protected GUIStyle ItemActive
        {
            get
            {
                if (_ItemActive == null)
                {
                    _ItemActive = new GUIStyle("RectangleToolSelection");
                }
                return _ItemActive;
            }
        }
        static protected GUIStyle Background
        {
            get
            {
                if (_Background == null)
                {
                    _Background = new GUIStyle("FrameBox");
                }
                return _Background;
            }
        }
        static protected GUIStyle ButtonForeground
        {
            get
            {
                if (_ButtonForeground == null)
                {
                    _ButtonForeground = new GUIStyle("RL FooterButton");
                    _ButtonForeground.fontSize = 20;
                }
                return _ButtonForeground;
            }
        }
        static protected GUIStyle ButtonBackground
        {
            get
            {
                if (_ButtonBackground == null)
                {
                    _ButtonBackground = new GUIStyle("RL Background");
                }
                return _ButtonBackground;
            }
        }

        protected struct ItemData
        {
            public int ControllId;
            public Rect HandleRect;
            public Rect ItemRect;
            public ItemData(Rect sPosition)
            {
                ItemRect = sPosition;
                HandleRect = new Rect(ItemRect.position, new Vector2(NWDGUI.kFieldIndent, ItemRect.y));
                ControllId = EditorGUIUtility.GetControlID(GUIContent.none, FocusType.Passive, ItemRect);
            }
        }

        bool Foldout = true;
        INWDPropertyDrawer ModelTypeField;
        List<ItemData> ItemsData = new List<ItemData>();
        int SelectedItem = -1;
        float StartYPosition = 0;
        float YDelta = 0;

        int GrabbedIndex = -1;
        int OnTopOfIndex = -1;

        public NWDArrayPropertyDrawer(INWDPropertyDrawer sModelTypeField) : base()
        {
            ModelTypeField = sModelTypeField;
        }
        public NWDArrayPropertyDrawer(PropertyInfo sPropertyInfo, INWDPropertyDrawer sModelTypeField) : base(sPropertyInfo)
        {
            ModelTypeField = sModelTypeField;
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName)
        {
            INWDCollectionSerializedProperty sArray = sProperty as INWDCollectionSerializedProperty;
            sArray.SetIndex(-1);

            Rect tPosition = sPosition;
            tPosition.width -= kSizeFieldWidth;
            tPosition.height = EditorGUIUtility.singleLineHeight;
            Foldout = EditorGUI.Foldout(tPosition, Foldout, sDisplayName);

            tPosition.width = kSizeFieldWidth;
            tPosition.x = sPosition.xMax - kSizeFieldWidth;
            int tLength = sArray.GetLength();
            int tNewSize = EditorGUI.IntField(tPosition, tLength);
            if (tNewSize != tLength && tNewSize >= 0)
            {
                sArray.Resize(tNewSize);
                tLength = tNewSize;

                if (tNewSize <= SelectedItem)
                {
                    SetSelectedItem(-1);
                }
            }

            if (Foldout)
            {
                tPosition.y += tPosition.height + EditorGUIUtility.standardVerticalSpacing;
                tPosition.height = sPosition.yMax - tPosition.y - EditorGUIUtility.singleLineHeight - kBottomButtonHeight + EditorGUIUtility.standardVerticalSpacing;
                tPosition.x = sPosition.x + NWDGUI.kFieldIndent;
                tPosition.width = sPosition.width - NWDGUI.kFieldIndent;

                Rect tBottomButtons = new Rect(tPosition.xMax - 68, tPosition.yMax - kBottomButtonHeight + EditorGUIUtility.standardVerticalSpacing * 2, 58, 20);

                if (Event.current.type == EventType.Repaint)
                {
                    Background.Draw(tPosition, false, false, false, false);
                    ButtonBackground.Draw(tBottomButtons, false, false, false, false);
                }

                tPosition.y += EditorGUIUtility.standardVerticalSpacing;
                tPosition.x++;
                tPosition.width -= 2;

                InitItemData(sArray, tPosition);

                ProcessMouseDown(sArray);

                for (int i = 0; i < tLength; i++)
                {
                    if (GrabbedIndex >= 0)
                    {
                        if (GrabbedIndex != i)
                        {
                            if (GrabbedIndex < i && OnTopOfIndex >= i)
                            {
                                DrawItem(sArray, i, ItemsData[i].ItemRect.y - ItemsData[GrabbedIndex].ItemRect.height);
                            }
                            else if (GrabbedIndex > i && OnTopOfIndex <= i)
                            {
                                DrawItem(sArray, i, ItemsData[i].ItemRect.y + ItemsData[GrabbedIndex].ItemRect.height);
                            }
                            else
                            {
                                DrawItem(sArray, i, ItemsData[i].ItemRect.y);
                            }
                        }
                    }
                    else
                    {
                        DrawItem(sArray, i, ItemsData[i].ItemRect.y);
                    }
                }

                if (GrabbedIndex >= 0)
                {
                    DrawItem(sArray, GrabbedIndex, ItemsData[GrabbedIndex].ItemRect.y + YDelta);
                }

                tBottomButtons.size = new Vector2(25, 16);
                tBottomButtons.x += 4;

                if (GUI.Button(tBottomButtons, "+", ButtonForeground))
                {
                    sArray.Resize(++tLength);
                    tLength = tNewSize;
                    GUI.changed = true;
                }

                tBottomButtons.x = tBottomButtons.xMax + 1;
                EditorGUI.BeginDisabledGroup(SelectedItem < 0);
                if (GUI.Button(tBottomButtons, "-", ButtonForeground))
                {
                    tLength--;
                    MoveInArray(sArray, SelectedItem, tLength);
                    sArray.Resize(tLength);
                    SetSelectedItem(-1);
                    GUI.changed = true;
                }
                EditorGUI.EndDisabledGroup();
            }

            ProcessControls(sArray);
        }

        private void ProcessMouseDown(INWDCollectionSerializedProperty sArray)
        {
            if (Event.current.type == EventType.MouseDown)
            {
                bool tCanResetHotControl = false;
                int tLength = sArray.GetLength();
                for (int i = 0; i < tLength; i++)
                {
                    if (ItemsData[i].ItemRect.Contains(Event.current.mousePosition))
                    {
                        SetSelectedItem(i);
                        StartYPosition = Event.current.mousePosition.y;
                        YDelta = 0;
                        tCanResetHotControl = false;
                    }
                    else if (GUIUtility.keyboardControl == ItemsData[i].ControllId)
                    {
                        tCanResetHotControl = true;
                    }
                }

                if (tCanResetHotControl)
                {
                    GUIUtility.keyboardControl = 0;
                    EditorWindow.focusedWindow.Repaint();
                }
            }
        }

        private void ProcessControls (INWDCollectionSerializedProperty sArray)
        {
            if (Event.current.type == EventType.MouseDrag)
            {
                if (GrabbedIndex < 0)
                {
                    GrabbedIndex = TryFindCurrentItemControlled();
                }

                if (GrabbedIndex >= 0)
                {
                    YDelta = Event.current.mousePosition.y - StartYPosition;
                    YDelta = Math.Clamp(YDelta, ItemsData[0].ItemRect.yMin - ItemsData[GrabbedIndex].ItemRect.yMin, ItemsData[sArray.GetLength() - 1].ItemRect.yMax - ItemsData[GrabbedIndex].ItemRect.yMax);

                    Rect tHoveringRect = ItemsData[GrabbedIndex].ItemRect;
                    tHoveringRect.y += YDelta;

                    for (int i = 0; i < ItemsData.Count; i++)
                    {
                        if (ItemsData[i].ItemRect.Contains(tHoveringRect.center))
                        {
                            OnTopOfIndex = i;
                            break;
                        }
                    }
                    Event.current.Use();
                }
            }

            if (GrabbedIndex >= 0 && Event.current.type == EventType.MouseUp)
            {
                YDelta = 0;

                if (GrabbedIndex != OnTopOfIndex)
                {
                    MoveInArray(sArray, GrabbedIndex, OnTopOfIndex);
                    SetSelectedItem(OnTopOfIndex);
                    GUI.changed = true;
                    Event.current.Use();
                }
                else
                {
                    EditorWindow.focusedWindow.Repaint();
                }
                GrabbedIndex = -1;
            }
            else
            {
                if (Event.current.type == EventType.KeyDown)
                {
                    int CurrentControl = -1;
                    switch (Event.current.keyCode)
                    {
                        case KeyCode.UpArrow:
                            CurrentControl = TryFindCurrentItemControlled();
                            if (CurrentControl > 0)
                            {
                                SetSelectedItem(CurrentControl - 1);
                            }
                            break;
                        case KeyCode.DownArrow:
                            CurrentControl = TryFindCurrentItemControlled();
                            if (CurrentControl >= 0 && CurrentControl < sArray.GetLength() - 1)
                            {
                                SetSelectedItem(CurrentControl + 1);
                            }
                            break;
                    }
                }
            }
        }

        private int TryFindCurrentItemControlled ()
        {
            for (int i = 0; i < ItemsData.Count; i++)
            {
                if (ItemsData[i].ControllId == GUIUtility.keyboardControl)
                {
                    return i;
                }
            }
            return -1;
        }

        private void DrawItem (INWDCollectionSerializedProperty sArray, int sIndex, float sStartPosition)
        {
            Rect tItemRect = ItemsData[sIndex].ItemRect;
            Rect tHandleRect = ItemsData[sIndex].HandleRect;

            tItemRect.y = sStartPosition;
            tHandleRect.y = sStartPosition;

            if (Event.current.type == EventType.Repaint)
            {
                if (ItemsData[sIndex].ControllId == GUIUtility.keyboardControl)
                {
                    ItemOn.Draw(tItemRect, false, false, true, false);
                }
                else if (SelectedItem == sIndex)
                {
                    ItemActive.Draw(tItemRect, false, true, false, false);
                }
            }

            tItemRect.height -= EditorGUIUtility.standardVerticalSpacing;
            tItemRect.y += EditorGUIUtility.standardVerticalSpacing / 2;

            tItemRect.xMin += tHandleRect.width;
            tItemRect.width -= EditorGUIUtility.standardVerticalSpacing * 2;

            tHandleRect.xMin += 5;
            tHandleRect.height = 10;
            tHandleRect.y += 5;

            sArray.SetIndex (sIndex);

            GUI.DrawTexture(tHandleRect, EditorGUIUtility.IconContent("Grip_VerticalContainer").image, ScaleMode.StretchToFill, true, 1, new Color(1, 1, 1, 0.2f), 0, 0);
            if (ModelTypeField.IsFoldableProperty)
            {
                tItemRect.xMin += NWDGUI.kFieldIndent;
            }
            ModelTypeField.OnGUI(tItemRect, sArray as NWDSerializedProperty, sArray.ElementName);
        }

        public override float GetPropertyHeight(NWDSerializedProperty sProperty, string sDisplayName)
        {
            INWDCollectionSerializedProperty sArray = (INWDCollectionSerializedProperty)sProperty;

            float rResult = EditorGUIUtility.singleLineHeight;
            if (Foldout)
            {
                int tLength = sArray.GetLength();
                for (int i = 0; i < tLength; i++)
                {
                    sArray.SetIndex(i);
                    rResult += ModelTypeField.GetPropertyHeight(sArray as NWDSerializedProperty, sArray.Name) + EditorGUIUtility.standardVerticalSpacing;
                }

                rResult += EditorGUIUtility.standardVerticalSpacing * 2;

                rResult += EditorGUIUtility.singleLineHeight + kBottomButtonHeight;
            }
            return rResult;
        }

        protected void MoveInArray (INWDCollectionSerializedProperty sArray, int sFromIndex, int sToIndex)
        {
            object tCache = sArray.GetValueAt(sFromIndex);
            if (sFromIndex < sToIndex)
            {
                for (int i = sFromIndex; i < sToIndex; i++)
                {
                    sArray.SetValueAt(i, sArray.GetValueAt(i + 1));
                }
            }
            else
            {
                for (int i = sFromIndex; i > sToIndex; i--)
                {
                    sArray.SetValueAt(i, sArray.GetValueAt(i - 1));
                }
            }
            sArray.SetValueAt(sToIndex, tCache);
        }

        public void SetSelectedItem (int sIndex)
        {
            SelectedItem = sIndex;
            OnTopOfIndex = sIndex;

            if (sIndex >= 0)
            {
                GUIUtility.keyboardControl = ItemsData[sIndex].ControllId;
                EditorWindow.focusedWindow.Repaint();
            }
        }

        public void InitItemData(INWDCollectionSerializedProperty sArray, Rect sPosition)
        {
            ItemsData.Clear();

            int tLength = sArray.GetLength();

            for (int i = 0; i < tLength; i++)
            {
                sArray.SetIndex(i);
                sPosition.height = ModelTypeField.GetPropertyHeight(sArray as NWDSerializedProperty, sArray.Name) + EditorGUIUtility.standardVerticalSpacing;
                ItemsData.Add(new ItemData(sPosition));
                sPosition.y += sPosition.height;
            }
        }

        public override void SetPropertyInfo(PropertyInfo sPropertyInfo)
        {
            if (Property == null)
            {
                Property = new NWDArraySerializedProperty(sPropertyInfo);
            }
        }
    }
}
