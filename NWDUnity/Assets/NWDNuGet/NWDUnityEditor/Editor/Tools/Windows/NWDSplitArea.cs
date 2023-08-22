using System;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    [Serializable]
    public class NWDSplitArea
    {
        public class AreaOffset
        {
            public float Left;
            public float Top;
            public float Right;
            public float Bottom;

            public AreaOffset() {
                Left = 0;
                Top = 0;
                Right = 0;
                Bottom = 0;
            }

            public AreaOffset(float sOffset)
            {
                Left = sOffset;
                Top = sOffset;
                Right = sOffset;
                Bottom = sOffset;
            }

            public AreaOffset(float sLeft, float sTop, float sRight, float sBottom)
            {
                Left = sLeft;
                Top = sTop;
                Right = sRight;
                Bottom = sBottom;
            }
        }

        static private uint LineWidth = 1;

        static public void SetGlobalLineWidth(uint sWidth = 1)
        {
            if (sWidth > 0)
            {
                LineWidth = sWidth;
            }
        }
        static private uint LineOffset = 1;

        static public void SetGlobalLineOffset(uint sOffset = 0)
        {
            LineOffset = sOffset;
        }

        static private NWDSplitArea ActiveZone = null;

        public float Split = 0.5f;
        public float Min {
            set
            {
                MinAreaOne = value;
                MinAreaTwo = value;
            }
        }
        public float MinAreaOne = 50.0f;
        public float MinAreaTwo = 50.0f;
        public float HeaderSize = 0;
        public NWDSplitDirection Direction = NWDSplitDirection.Horizontal;
        public Rect Origin;
        public Rect AreaOneHeader;
        public Rect AreaOne;
        public Rect AreaTwoHeader;
        public Rect AreaTwo;
        public Rect Line;
        public float Width = 0.50f;
        public float Offset = 0.0f;
        public Rect LineAction;
        public int ActionOffset = 4;
        public bool Resizable = true;
        public bool AreaOneDraw = false;
        public bool AreaTwoDraw = false;
        public Color AreaOneColor = Color.black;
        public Color AreaTwoColor = Color.black;

        public static NWDSplitArea NewArea(NWDSplitDirection sDirection = NWDSplitDirection.Vertical)
        {
            NWDSplitArea rReturn = new NWDSplitArea();
            rReturn.Width = LineWidth / 2.0f;
            rReturn.Offset = LineOffset;
            rReturn.Direction = sDirection;
            return rReturn;
        }

        public void SetResizable(bool sResizable = true)
        {
            Resizable = sResizable;
        }

        public void SetLineOffset(int sOffset = 0)
        {
            Offset = Mathf.Abs(sOffset);
        }

        public void SetLineWidth(int sWidth = 1)
        {
            if (sWidth > 0)
            {
                Width = ((float)sWidth) / 2.0f;
            }
        }

        public void SetLineLarge()
        {
            SetLineWidth(2);
        }

        public void SetLineThin()
        {
            SetLineWidth(1);
        }

        public void SetColorAreaOne(Color sColor)
        {
            AreaOneDraw = true;
            AreaOneColor = sColor;
        }

        public void SetColorAreaTwo(Color sColor)
        {
            AreaTwoDraw = true;
            AreaTwoColor = sColor;
        }

        public void ResetColorAreaOne()
        {
            AreaOneDraw = false;
        }

        public void ResetColorAreaTwo()
        {
            AreaTwoDraw = false;
        }

        public void BeginAreaOneHeader (int sMarge = 0)
        {
            Rect tRect = GetAreaOneHeader(sMarge);
            GUILayout.BeginArea(tRect);
        }
        public void BeginAreaOneHeader(AreaOffset sOffset)
        {
            Rect tRect = GetAreaOneHeader(sOffset);
            GUILayout.BeginArea(tRect);
        }

        public void EndAreaOneHeader ()
        {
            GUILayout.EndArea();
        }

        public void BeginAreaOne(int sMarge = 0)
        {
            Rect tRect = GetAreaOne(sMarge);
            GUILayout.BeginArea(tRect);
        }
        public void BeginAreaOne(AreaOffset sOffset)
        {
            Rect tRect = GetAreaOne(sOffset);
            GUILayout.BeginArea(tRect);
        }

        public void EndAreaOne()
        {
            GUILayout.EndArea();
        }

        public void BeginAreaTwoHeader (int sMarge = 0)
        {
            Rect tRect = GetAreaTwoHeader(sMarge);
            GUILayout.BeginArea(tRect);
        }
        public void BeginAreaTwoHeader(AreaOffset sOffset)
        {
            Rect tRect = GetAreaTwoHeader(sOffset);
            GUILayout.BeginArea(tRect);
        }

        public void EndAreaTwoHeader ()
        {
            GUILayout.EndArea();
        }

        public void BeginAreaTwo(int sMarge = 0)
        {
            Rect tRect = GetAreaTwo(sMarge);
            GUILayout.BeginArea(tRect);
        }
        public void BeginAreaTwo(AreaOffset sOffset)
        {
            Rect tRect = GetAreaTwo(sOffset);
            GUILayout.BeginArea(tRect);
        }

        public void EndAreaTwo()
        {
            GUILayout.EndArea();
        }

        public Rect GetAreaOneHeader(int sMarge = 0)
        {
            if (sMarge != 0)
            {
                return Reducer(AreaOneHeader, sMarge);
            }
            else
            {
                return AreaOneHeader;
            }
        }
        public Rect GetAreaOneHeader(AreaOffset sOffset)
        {
            if (sOffset != null)
            {
                return Reducer(AreaOneHeader, sOffset);
            }
            else
            {
                return AreaOneHeader;
            }
        }

        public Rect GetAreaOne(int sMarge = 0)
        {
            if (sMarge != 0)
            {
                return Reducer(AreaOne, sMarge);
            }
            else
            {
                return AreaOne;
            }
        }
        public Rect GetAreaOne(AreaOffset sOffset)
        {
            if (sOffset != null)
            {
                return Reducer(AreaOne, sOffset);
            }
            else
            {
                return AreaOne;
            }
        }

        public Rect GetAreaTwoHeader(int sMarge = 0)
        {
            if (sMarge != 0)
            {
                return Reducer(AreaTwoHeader, sMarge);
            }
            else
            {
                return AreaTwoHeader;
            }

        }
        public Rect GetAreaTwoHeader(AreaOffset sOffset)
        {
            if (sOffset != null)
            {
                return Reducer(AreaTwoHeader, sOffset);
            }
            else
            {
                return AreaTwoHeader;
            }
        }

        public Rect GetAreaTwo(int sMarge = 0)
        {
            if (sMarge != 0)
            {
                return Reducer(AreaTwo, sMarge);
            }
            else
            {
                return AreaTwo;
            }
        }
        public Rect GetAreaTwo(AreaOffset sOffset)
        {
            if (sOffset != null)
            {
                return Reducer(AreaTwo, sOffset);
            }
            else
            {
                return AreaTwo;
            }
        }

        private Rect Reducer(Rect sRect, int sMarge)
        {
            return new Rect(sRect.x + sMarge, sRect.y + sMarge, sRect.width - sMarge * 2.0f, sRect.height - sMarge * 2.0f);
        }
        private Rect Reducer(Rect sRect, AreaOffset sOffset)
        {
            sRect.xMin += sOffset.Left;
            sRect.yMin += sOffset.Top;
            sRect.xMax -= sOffset.Right;
            sRect.yMax -= sOffset.Bottom;

            return sRect;
        }

        public void ResizeSplit (EditorWindow sEditorWind)
        {
            ResizeSplit(new Rect(0, 0, sEditorWind.position.width, sEditorWind.position.height));
        }

        public void ResizeSplit(Rect sOrigin)
        {
            Origin = sOrigin;
            if (Direction == NWDSplitDirection.Vertical)
            {
                float tVA = Origin.width * Split - Width;
                float tVB = Origin.width * (1 - Split) - Width;
                if (tVA < MinAreaOne)
                {
                    tVA = MinAreaOne;
                    tVB = Origin.width - tVA - Width * 2;
                }
                if (tVB < MinAreaTwo)
                {
                    tVB = MinAreaTwo;
                    tVA = Origin.width - tVB - Width * 2;
                }
                AreaOneHeader = new Rect(Origin.x, Origin.y, tVA, HeaderSize);
                AreaOne = new Rect(Origin.x, Origin.y + HeaderSize, tVA, Origin.height - HeaderSize);
                AreaTwoHeader = new Rect(Origin.x + tVA + Width * 2, Origin.y, tVB + Width, HeaderSize);
                AreaTwo = new Rect(Origin.x + tVA + Width * 2, Origin.y + HeaderSize, tVB + Width, Origin.height - HeaderSize);
                Line = new Rect(Origin.x + tVA, Origin.y + Offset - 1, Width * 2, Origin.height - Offset + 1);
            }
            else
            {
                float tVA = Origin.height * Split - Width;
                float tVB = Origin.height * (1 - Split) - Width;
                if (tVA < MinAreaOne)
                {
                    tVA = MinAreaOne;
                    tVB = Origin.height - tVA - Width * 2;
                }
                if (tVB < MinAreaTwo)
                {
                    tVB = MinAreaTwo;
                    tVA = Origin.height - tVB - Width * 2;
                }
                AreaOneHeader = new Rect(Origin.x, Origin.y, HeaderSize, tVA);
                AreaOne = new Rect(Origin.x + HeaderSize, Origin.y, Origin.width - HeaderSize, tVA);
                AreaTwoHeader = new Rect(Origin.x, Origin.y + tVA + Width * 2, HeaderSize, tVB + Width);
                AreaTwo = new Rect(Origin.x + HeaderSize, Origin.y + tVA + Width * 2, Origin.width - HeaderSize, tVB + Width);
                Line = new Rect(Origin.x + Offset - 1, Origin.y + tVA, Origin.width - Offset + 1, Width * 2);
            }
            LineAction = Reducer(Line, -ActionOffset);
        }

        Rect OnLayoutRect = Rect.zero;
        public void OnGUILayout(EditorWindow sEditorWind)
        {
            if (Event.current.type == EventType.Repaint)
            {
                OnLayoutRect = GUILayoutUtility.GetLastRect();
            }
            //Debug.Log("tLastRect = " + OnLayoutRect.ToString() + Event.current.type.ToString());
            OnGUI(sEditorWind, new Rect(0, OnLayoutRect.position.y + OnLayoutRect.height, sEditorWind.position.width, sEditorWind.position.height - OnLayoutRect.position.y + OnLayoutRect.height));
        }

        public void OnGUI(EditorWindow sEditorWind, bool sResizeToWindow = true)
        {
            if (sResizeToWindow)
            {
                OnGUI(sEditorWind, new Rect(0, 0, sEditorWind.position.width, sEditorWind.position.height));
            }
            else
            {
                OnGUI(sEditorWind, Origin);
            }
        }

        public void OnGUI(EditorWindow sEditorWind, Rect sOrigin)
        {
            ResizeSplit(sOrigin);
            if (Resizable == true)
            {
                if (Direction == NWDSplitDirection.Vertical)
                {
                    EditorGUIUtility.AddCursorRect(LineAction, MouseCursor.SplitResizeLeftRight);
                }
                else
                {
                    EditorGUIUtility.AddCursorRect(LineAction, MouseCursor.SplitResizeUpDown);
                }
                if (Event.current.type == EventType.MouseDown && LineAction.Contains(Event.current.mousePosition))
                {
                    ActiveZone = this;
                }
                if (Event.current.type == EventType.MouseUp)
                {
                    ActiveZone = null;
                }
                if (Event.current.type == EventType.MouseDrag && ActiveZone == this)
                {
                    if (Direction == NWDSplitDirection.Vertical)
                    {
                        Split = (Event.current.mousePosition.x - Origin.x) / Origin.width;
                        ResizeSplit(Origin);
                    }
                    else
                    {
                        Split = (Event.current.mousePosition.y - Origin.y) / Origin.height;
                        ResizeSplit(Origin);
                    }
                    sEditorWind.Repaint();
                }
                if (AreaOneDraw == true) { EditorGUI.DrawRect(AreaOne, AreaOneColor); }
                if (AreaTwoDraw == true) { EditorGUI.DrawRect(AreaTwo, AreaTwoColor); }
                if (EditorGUIUtility.isProSkin)
                {
                    EditorGUI.DrawRect(Line, Color.black);
                }
                else
                {
                    EditorGUI.DrawRect(Line, Color.gray);
                }
            }
            else
            {
                if (AreaOneDraw == true) { EditorGUI.DrawRect(AreaOne, AreaOneColor); }
                if (AreaTwoDraw == true) { EditorGUI.DrawRect(AreaTwo, AreaTwoColor); }
                if (EditorGUIUtility.isProSkin)
                {
                    EditorGUI.DrawRect(Line, new Color(0.5f, 0.5f, 0.5f, 0.5f));
                }
                else
                {
                    EditorGUI.DrawRect(Line, new Color(0.5f, 0.5f, 0.5f, 0.5f));
                }
            }
        }

    }
}