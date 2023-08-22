using System;

namespace NWDUnityEditor.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class NWDCustomPropertyDrawer : Attribute
    {
        public Type Type;
        private bool AllowChildren;

        public NWDCustomPropertyDrawer(Type sType, bool sAllowChildren = false)
        {
            Type = sType;
            AllowChildren = sAllowChildren;
        }

        public bool Match (Type sType)
        {
            bool sReturn;
            if (AllowChildren)
            {
                sReturn = Type == sType || sType.IsSubclassOf(Type);
            }
            else
            {
                sReturn = Type == sType;
            }

            return sReturn;
        }
    }
}
