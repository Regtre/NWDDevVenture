using NWDUnityShared.Tools;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDDictionaryPropertyDrawer : NWDPropertyDrawer
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
                HandleRect = new Rect(ItemRect.position, new Vector2(EditorGUIUtility.labelWidth, ItemRect.height));
                ControllId = EditorGUIUtility.GetControlID(GUIContent.none, FocusType.Passive, ItemRect);
            }
        }

        bool Foldout = true;
        INWDPropertyDrawer KeyModelTypeField;
        INWDPropertyDrawer ValueModelTypeField;
        List<ItemData> ItemsData = new List<ItemData>();
        int SelectedItem = -1;
        float NewKeyHeight = 0;
        public bool HorizontalDisplay;
        NWDSerializedProperty NewKeyProperty;
        object NewKey { get; set; } = null;

        public NWDDictionaryPropertyDrawer(Type sKeyType, INWDPropertyDrawer sKeyModelTypeField, INWDPropertyDrawer sValueModelTypeField, bool sHorizontalDisplay = true) : base()
        {
            Construct(sKeyType, sKeyModelTypeField, sValueModelTypeField, sHorizontalDisplay);
        }
        public NWDDictionaryPropertyDrawer(PropertyInfo sPropertyInfo, Type sKeyType, INWDPropertyDrawer sKeyModelTypeField, INWDPropertyDrawer sValueModelTypeField, bool sHorizontalDisplay = true) : base(sPropertyInfo)
        {
            Construct(sKeyType, sKeyModelTypeField, sValueModelTypeField, sHorizontalDisplay);
        }
        public void Construct(Type sKeyType, INWDPropertyDrawer sKeyModelTypeField, INWDPropertyDrawer sValueModelTypeField, bool sHorizontalDisplay)
        {
            KeyModelTypeField = sKeyModelTypeField;
            ValueModelTypeField = sValueModelTypeField;
            HorizontalDisplay = sHorizontalDisplay;

            PropertyInfo tPropertyInfo = typeof(NWDDictionaryPropertyDrawer).GetProperty(nameof(NewKey), BindingFlags.NonPublic | BindingFlags.Instance);
            NewKeyProperty = new NWDDictionaryKeySerializedProperty(tPropertyInfo, sKeyType);
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName)
        {
            INWDDictionarySerializedProperty sDictionary = sProperty as INWDDictionarySerializedProperty;
            sDictionary.SetIndexForKey(-1);

            Rect tPosition = sPosition;
            tPosition.width -= kSizeFieldWidth;
            tPosition.height = EditorGUIUtility.singleLineHeight;
            Foldout = EditorGUI.Foldout(tPosition, Foldout, sDisplayName);

            tPosition.width = kSizeFieldWidth;
            tPosition.x = sPosition.xMax - kSizeFieldWidth;
            int tLength = sDictionary.GetLength();

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

                InitItemData(sDictionary, tPosition);

                ProcessMouseDown(sDictionary);

                for (int i = 0; i < tLength; i++)
                {
                    DrawItem(sDictionary, i, ItemsData[i].ItemRect.y);
                }

                tPosition.y = tBottomButtons.y - NewKeyHeight - EditorGUIUtility.standardVerticalSpacing;
                tPosition.height = NewKeyHeight;

                DrawNewKey(tPosition);

                tBottomButtons.size = new Vector2(25, 16);
                tBottomButtons.x += EditorGUIUtility.standardVerticalSpacing * 2;

                EditorGUI.BeginDisabledGroup(!KeyModelTypeField.IsValidUniqueKeyValue(NewKeyProperty) || !sDictionary.IsValidNewKey(NewKey));
                if (GUI.Button(tBottomButtons, "+", ButtonForeground))
                {
                    sDictionary.Add(NewKey, default);
                    NewKey = null;
                }
                EditorGUI.EndDisabledGroup();

                tBottomButtons.x = tBottomButtons.xMax + 1;
                EditorGUI.BeginDisabledGroup(SelectedItem < 0);
                if (GUI.Button(tBottomButtons, "-", ButtonForeground))
                {
                    sDictionary.RemoveAt(SelectedItem);
                    SetSelectedItem(-1);
                    GUI.changed = true;
                }
                EditorGUI.EndDisabledGroup();
            }

            ProcessControls(sDictionary);
        }

        private void ProcessMouseDown(INWDDictionarySerializedProperty sDictionary)
        {
            if (Event.current.type == EventType.MouseDown)
            {
                bool tCanResetHotControl = false;
                int tLength = sDictionary.GetLength();
                for (int i = 0; i < tLength; i++)
                {
                    if (ItemsData[i].ItemRect.Contains(Event.current.mousePosition))
                    {
                        SetSelectedItem(i);
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

        private void ProcessControls (INWDDictionarySerializedProperty sDictionary)
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
                        if (CurrentControl >= 0 && CurrentControl < sDictionary.GetLength() - 1)
                        {
                            SetSelectedItem(CurrentControl + 1);
                        }
                        break;
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

        private void DrawItem (INWDDictionarySerializedProperty sDictionary, int sIndex, float sStartPosition)
        {
            Rect tKeyRect = ItemsData[sIndex].HandleRect;
            Rect tValueRect = ItemsData[sIndex].ItemRect;

            tValueRect.y = sStartPosition;
            tKeyRect.y = sStartPosition;

            if (Event.current.type == EventType.Repaint)
            {
                if (ItemsData[sIndex].ControllId == GUIUtility.keyboardControl)
                {
                    ItemOn.Draw(tValueRect, false, false, true, false);
                }
                else if (SelectedItem == sIndex)
                {
                    ItemActive.Draw(tValueRect, false, true, false, false);
                }
            }
            tValueRect.y += EditorGUIUtility.standardVerticalSpacing / 2;
            if (HorizontalDisplay)
            {
                tValueRect.height -= EditorGUIUtility.standardVerticalSpacing;

                tValueRect.xMin += tKeyRect.width + EditorGUIUtility.standardVerticalSpacing;
                tValueRect.width -= EditorGUIUtility.standardVerticalSpacing * 2;

                tKeyRect.height = tValueRect.height;
                tKeyRect.y = tValueRect.y;

                tKeyRect.xMin += EditorGUIUtility.standardVerticalSpacing * 2;
                tKeyRect.xMax -= EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                tValueRect.xMin += EditorGUIUtility.standardVerticalSpacing;
                tValueRect.xMax -= EditorGUIUtility.standardVerticalSpacing;

                tKeyRect = tValueRect;
                sDictionary.SetIndexForKey(sIndex);
                tKeyRect.height = KeyModelTypeField.GetPropertyHeight(sDictionary as NWDSerializedProperty, sDictionary.ElementName);

                tValueRect.yMin += tKeyRect.height + EditorGUIUtility.standardVerticalSpacing;
                sDictionary.SetIndexForValue(sIndex);
                tValueRect.height = ValueModelTypeField.GetPropertyHeight(sDictionary as NWDSerializedProperty, sDictionary.ElementName);
            }

            EditorGUI.BeginDisabledGroup(true);
            sDictionary.SetIndexForKey(sIndex);
            if (KeyModelTypeField.IsFoldableProperty)
            {
                tKeyRect.xMin += NWDGUI.kFieldIndent;
            }
            KeyModelTypeField.OnGUI(tKeyRect, sDictionary as NWDSerializedProperty, HorizontalDisplay ? "" : sDictionary.ElementName);
            EditorGUI.EndDisabledGroup();

            sDictionary.SetIndexForValue(sIndex);
            if (ValueModelTypeField.IsFoldableProperty)
            {
                tValueRect.xMin += NWDGUI.kFieldIndent;
            }
            ValueModelTypeField.OnGUI(tValueRect, sDictionary as NWDSerializedProperty, HorizontalDisplay ? "" : sDictionary.ElementName);
        }

        public override float GetPropertyHeight(NWDSerializedProperty sProperty, string sDisplayName)
        {
            INWDDictionarySerializedProperty sDictionary = (INWDDictionarySerializedProperty)sProperty;

            sDictionary.SetIndexForKey(-1);

            if (NewKey == null)
            {
                NewKey = NWDToolbox.CreateInstance(NewKeyProperty.PropertyType);
            }

            NewKeyHeight = ValueModelTypeField.GetPropertyHeight(NewKeyProperty, "");

            float rResult = EditorGUIUtility.singleLineHeight;
            if (Foldout)
            {
                int tLength = sDictionary.GetLength();
                for (int i = 0; i < tLength; i++)
                {
                    sDictionary.SetIndexForKey(i);
                    float tKeyHeight = KeyModelTypeField.GetPropertyHeight(sDictionary as NWDSerializedProperty, sDictionary.Name);

                    sDictionary.SetIndexForValue(i);
                    float tValueHeight = ValueModelTypeField.GetPropertyHeight(sDictionary as NWDSerializedProperty, sDictionary.Name);

                    if (HorizontalDisplay)
                    {
                        rResult += Mathf.Max(tKeyHeight, tValueHeight) + EditorGUIUtility.standardVerticalSpacing;
                    }
                    else
                    {
                        rResult += tKeyHeight + tValueHeight + EditorGUIUtility.standardVerticalSpacing * 2;
                    }
                }

                rResult += EditorGUIUtility.standardVerticalSpacing * 2;

                rResult += NewKeyHeight;

                rResult += EditorGUIUtility.standardVerticalSpacing * 2;

                rResult += EditorGUIUtility.singleLineHeight + kBottomButtonHeight;
            }
            return rResult;
        }

        public void SetSelectedItem (int sIndex)
        {
            SelectedItem = sIndex;

            if (sIndex >= 0)
            {
                GUIUtility.keyboardControl = ItemsData[sIndex].ControllId;
                EditorWindow.focusedWindow.Repaint();
            }
        }

        private void DrawNewKey (Rect sPosition)
        {
            if (NewKey == null)
            {
                NewKey = NWDToolbox.CreateInstance(NewKeyProperty.PropertyType);
            }

            sPosition.xMin += EditorGUIUtility.standardVerticalSpacing * 2;
            sPosition.xMax -= EditorGUIUtility.standardVerticalSpacing * 2;

            // Display the key here
            NewKeyProperty.PropertyObject = this;
            if (KeyModelTypeField.IsFoldableProperty)
            {
                sPosition.xMin += NWDGUI.kFieldIndent;
            }
            KeyModelTypeField.OnGUI(sPosition, NewKeyProperty, "New key");
        }

        public void InitItemData(INWDDictionarySerializedProperty sDictionary, Rect sPosition)
        {
            ItemsData.Clear();

            int tLength = sDictionary.GetLength();

            for (int i = 0; i < tLength; i++)
            {
                sDictionary.SetIndexForKey(i);
                float tKeyHeight = KeyModelTypeField.GetPropertyHeight(sDictionary as NWDSerializedProperty, sDictionary.ElementName);

                sDictionary.SetIndexForValue(i);
                float tValueHeight = ValueModelTypeField.GetPropertyHeight(sDictionary as NWDSerializedProperty, sDictionary.ElementName);

                if (HorizontalDisplay)
                {
                    sPosition.height = Mathf.Max(tKeyHeight, tValueHeight) + EditorGUIUtility.standardVerticalSpacing;
                }
                else
                {
                    sPosition.height = tKeyHeight + tValueHeight + EditorGUIUtility.standardVerticalSpacing * 2;
                }

                ItemsData.Add(new ItemData(sPosition));
                sPosition.y += sPosition.height;
            }
        }

        public override void SetPropertyInfo(PropertyInfo sPropertyInfo)
        {
            if (Property == null)
            {
                Property = new NWDDictionarySerializedProperty(sPropertyInfo, NewKeyProperty.PropertyType);
            }
        }
    }
}
