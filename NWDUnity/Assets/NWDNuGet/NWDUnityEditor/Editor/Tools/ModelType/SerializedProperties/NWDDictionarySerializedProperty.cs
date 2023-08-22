using NWDUnityShared.Tools;
using System;
using System.Collections;
using System.Reflection;

namespace NWDUnityEditor.Tools
{
    public class NWDDictionarySerializedProperty : NWDSerializedProperty, INWDDictionarySerializedProperty
    {
        bool IsKeyIndex = true;
        int Index = -1;
        MethodInfo GetMethod = null;
        MethodInfo SetMethod = null;
        MethodInfo AddMethod = null;
        MethodInfo RemoveMethod = null;
        MethodInfo KeysMethod = null;
        MethodInfo ContainsKeyMethod = null;
        MethodInfo CountMethod = null;

        public override Type PropertyType
        {
            get
            {
                Type rReturn = null;

                if (Index < 0)
                {
                    rReturn = base.PropertyType;
                }
                else if (IsKeyIndex)
                {
                    rReturn = base.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    rReturn = base.PropertyType.GetGenericArguments()[1];
                }

                return rReturn;
            }
        }

        public string ElementName => (IsKeyIndex ? "Key " : "Value ") + Index;

        public Type ArrayType => base.PropertyType.GetElementType();

        public NWDDictionarySerializedProperty(PropertyInfo sPropertyInfo, Type sKeyType) : base(sPropertyInfo)
        {
            Index = -1;
            GetMethod = sPropertyInfo.PropertyType.GetMethod("get_Item");
            SetMethod = sPropertyInfo.PropertyType.GetMethod("set_Item");
            AddMethod = sPropertyInfo.PropertyType.GetMethod("Add");
            RemoveMethod = sPropertyInfo.PropertyType.GetMethod("Remove", new Type[] { sKeyType });
            KeysMethod = sPropertyInfo.PropertyType.GetMethod("get_Keys");
            ContainsKeyMethod = sPropertyInfo.PropertyType.GetMethod("ContainsKey");
            CountMethod = sPropertyInfo.PropertyType.GetMethod("get_Count");
        }

        public object GetValueAt(int sIndex)
        {
            object tObject = base.GetValue();
            object tKey = GetKeyAt(sIndex);
            if (IsKeyIndex)
            {
                return tKey;
            }
            tObject = GetMethod.Invoke(tObject, new object[] { tKey });
            if (tObject == null)
            {
                tObject = NWDToolbox.CreateInstance(PropertyType);
            }
            return tObject;
        }

        public void SetValueAt(int sIndex, object sValue)
        {
            if (IsKeyIndex)
            {
                return;
            }

            object tObject = base.GetValue();
            object tKey = GetKeyAt(sIndex);
            SetMethod.Invoke(tObject, new object[] { tKey, sValue });
        }

        public override object GetValue()
        {
            return GetValueAt(Index);
        }

        public override void SetValue(object sValue)
        {
            SetValueAt(Index, sValue);
        }

        public void SetIndexForKey(int sIndex)
        {
            IsKeyIndex = true;
            Index = sIndex;
        }

        public void SetIndexForValue(int sIndex)
        {
            IsKeyIndex = false;
            Index = sIndex;
        }

        public void Add(object sKey, object sValue)
        {
            object tObject = base.GetValue();
            AddMethod.Invoke(tObject, new object[] { sKey, sValue });
            base.SetValue(tObject);
        }

        public object GetKeyAt(int sIndex)
        {
            object rReturn = null;
            object tObject = base.GetValue();
            IEnumerable tKeys = (IEnumerable)KeysMethod.Invoke(tObject, new object[0]);
            foreach (object tKey in tKeys)
            {
                if (sIndex-- == 0)
                {
                    rReturn = tKey;
                    break;
                }
            }
            return rReturn;
        }

        public void RemoveAt(int sIndex)
        {
            object tObject = base.GetValue();
            object tKey = GetKeyAt(sIndex);
            RemoveMethod.Invoke(tObject, new object[] { tKey });
            base.SetValue(tObject);
        }

        public bool IsValidNewKey(object sKey)
        {
            object tObject = base.GetValue();
            return sKey != null && !(bool)ContainsKeyMethod.Invoke(tObject, new object[] { sKey });
        }

        public int GetLength ()
        {
            object tObject = base.GetValue();
            return (int)CountMethod.Invoke(tObject, new object[0]);
        }

        public override int GetHashCode()
        {
            return (base.GetHashCode() + GetValue().GetHashCode()).GetHashCode();
        }
    }
}