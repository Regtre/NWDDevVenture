using NWDUnityShared.Tools;
using System;
using System.Reflection;

namespace NWDUnityEditor.Tools
{
    public class NWDSerializedProperty
    {
        private Type TypeProperty;

        public PropertyInfo PropertyInfo;
        public object PropertyObject;

        public string Name => PropertyInfo.Name + " (" + PropertyInfo.PropertyType.Name + ")";
        public virtual Type PropertyType
        {
            get
            {
                if (TypeProperty != null)
                {
                    return TypeProperty;
                }
                return PropertyInfo.PropertyType;
            }
            set
            {
                TypeProperty = value;
            }
        }

        public NWDSerializedProperty (PropertyInfo sPropertyInfo)
        {
            PropertyInfo = sPropertyInfo;
        }

        public virtual object GetValue ()
        {
            object rReturn = PropertyInfo.GetValue(PropertyObject);
            if (rReturn == null)
            {
                rReturn = NWDToolbox.CreateInstance(PropertyType);
            }
            return rReturn;
        }

        public virtual void SetValue (object sValue)
        {
            PropertyInfo.SetValue(PropertyObject, sValue);
        }
    }
}