using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDReferencePropertyDrawer : NWDPropertyDrawer
    {
        static private Dictionary<NWDSerializedProperty, bool> Foldout = new Dictionary<NWDSerializedProperty, bool>();

        public override bool IsFoldableProperty => true;

        static private bool GetFoldout(NWDSerializedProperty sProperty)
        {
            if (sProperty == null)
            {
                return false;
            }

            if (!Foldout.ContainsKey(sProperty))
            {
                Foldout.Add(sProperty, false);
            }

            return Foldout[sProperty];
        }

        Type ReferenceType;

        public NWDReferencePropertyDrawer(Type sReferenceType) : base()
        {
            ReferenceType = sReferenceType;
        }

        public NWDReferencePropertyDrawer(Type sReferenceType, PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
            ReferenceType = sReferenceType;
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName)
        {
            bool tFoldout = GetFoldout(sProperty); // TO DO: should work by property path > Not ready yet in NWDSerializedProperty!
            ulong tValue = (ulong)sProperty.GetValue();
            tValue = NWDGUI.DataField(sPosition, new GUIContent(sDisplayName), ReferenceType, tValue, ref tFoldout);
            if ((ulong)sProperty.GetValue() != tValue)
            {
                sProperty.SetValue(tValue);
            }
            Foldout[sProperty] = tFoldout;
        }

        public override bool IsValidUniqueKeyValue(NWDSerializedProperty sProperty)
        {
            return (ulong)sProperty.GetValue() != 0;
        }
    }
}
