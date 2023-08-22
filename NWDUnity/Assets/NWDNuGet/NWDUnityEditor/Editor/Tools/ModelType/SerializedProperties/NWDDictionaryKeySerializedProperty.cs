using System;
using System.Reflection;

namespace NWDUnityEditor.Tools
{
    public class NWDDictionaryKeySerializedProperty : NWDSerializedProperty
    {
        private Type KeyType;

        public override Type PropertyType => KeyType;

        public NWDDictionaryKeySerializedProperty(PropertyInfo sPropertyInfo, Type sKeyType) : base(sPropertyInfo)
        {
            KeyType = sKeyType;
        }
    }
}