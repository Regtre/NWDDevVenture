using NWDUnityShared.Tools;
using System;
using System.Reflection;

namespace NWDUnityEditor.Tools
{
    public class NWDListSerializedProperty : NWDSerializedProperty, INWDCollectionSerializedProperty
    {
        int Index = -1;
        MethodInfo Get = null;
        MethodInfo Set = null;
        MethodInfo Count = null;
        MethodInfo Add = null;
        MethodInfo RemoveAt = null;

        public override Type PropertyType => Index < 0 ? base.PropertyType : base.PropertyType.GetElementType();

        public string ElementName => "Element " + Index;

        public Type ArrayType => base.PropertyType.GetElementType();

        public NWDListSerializedProperty(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
            Index = -1;
            Get = sPropertyInfo.PropertyType.GetMethod("get_Item");
            Set = sPropertyInfo.PropertyType.GetMethod("set_Item");
            Count = sPropertyInfo.PropertyType.GetMethod("get_Count");
            Add = sPropertyInfo.PropertyType.GetMethod("Add");
            RemoveAt = sPropertyInfo.PropertyType.GetMethod("RemoveAt");
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

        public int GetLength()
        {
            object tObject = base.GetValue();
            if (tObject == null)
            {
                return 0;
            }
            return GetLength(tObject);
        }

        private int GetLength (object sObject)
        {
            object rResult = Count.Invoke(sObject, new object[0]);

            if (rResult != null && rResult.GetType() == typeof(int))
            {
                return (int)rResult;
            }
            return 0;
        }

        public void Resize(int sSize)
        {
            object tObject = base.GetValue();
            int tLength = 0;
            if (tObject == null)
            {
                tObject = base.PropertyType.GetConstructor(new Type[0]).Invoke(new object[0]);
            }
            else
            {
                tLength = GetLength(tObject);
            }

            if (tLength == sSize)
            {
                return;
            }

            if (tLength > sSize)
            {
                for (int i = 0; i < tLength - sSize; i++)
                {
                    RemoveAt.Invoke(tObject, new object[1] { sSize });
                }
            }
            else
            {
                for (int i = 0; i < sSize - tLength; i++)
                {
                    Add.Invoke(tObject, new object[1] { default });
                }
            }

            base.SetValue(tObject);
        }


        public override int GetHashCode()
        {
            return (base.GetHashCode() + GetValue().GetHashCode()).GetHashCode();
        }
    }
}