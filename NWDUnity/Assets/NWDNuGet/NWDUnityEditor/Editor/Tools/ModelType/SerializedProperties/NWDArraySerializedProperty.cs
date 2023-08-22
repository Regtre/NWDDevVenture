using NWDUnityShared.Tools;
using System;
using System.Reflection;

namespace NWDUnityEditor.Tools
{
    public class NWDArraySerializedProperty : NWDSerializedProperty, INWDCollectionSerializedProperty
    {
        int Index = -1;
        MethodInfo Get = null;
        MethodInfo Set = null;

        public override Type PropertyType => Index < 0 ? base.PropertyType : base.PropertyType.GetElementType();

        public string ElementName => "Element " + Index;

        public Type ArrayType => base.PropertyType.GetElementType();

        public NWDArraySerializedProperty(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
            Index = -1;
            Get = sPropertyInfo.PropertyType.GetMethod("Get");
            Set = sPropertyInfo.PropertyType.GetMethod("Set");
        }

        public object GetValueAt(int sIndex)
        {
            object tObject = base.GetValue();
            tObject = Get.Invoke(tObject, new object[] { sIndex });

            if (tObject == null)
            {
                tObject = NWDToolbox.CreateInstance(PropertyType);
            }
            return tObject;
        }

        public void SetValueAt(int sIndex, object sValue)
        {
            object tObject = base.GetValue();
            Set.Invoke(tObject, new object[] { sIndex, sValue });
        }

        public override object GetValue()
        {
            return GetValueAt(Index);
        }

        public override void SetValue(object sValue)
        {
            SetValueAt(Index, sValue);
        }

        public void SetIndex(int sIndex)
        {
            Index = sIndex;
        }

        public int GetLength ()
        {
            Array tArray = base.GetValue() as Array;

            if (tArray != null)
            {
                return tArray.Length;
            }
            return 0;
        }

        public void Resize (int sSize)
        {
            Array tOldArray = base.GetValue() as Array;
            Array tNewArray = Array.CreateInstance(ArrayType, sSize);

            sSize = Math.Min(sSize, tOldArray?.Length ?? 0);

            for (int i = 0; i < sSize; i++)
            {
                tNewArray.SetValue(tOldArray.GetValue(i), i);
            }

            base.SetValue(tNewArray);
        }


        public override int GetHashCode()
        {
            return (base.GetHashCode() + GetValue().GetHashCode()).GetHashCode();
        }
    }
}