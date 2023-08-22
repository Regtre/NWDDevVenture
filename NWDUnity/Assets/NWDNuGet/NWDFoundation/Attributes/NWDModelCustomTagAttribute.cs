using System;

namespace NWDFoundation.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class NWDModelCustomTagAttribute : Attribute
    {
        public string EnumName;
        public NWDModelCustomTagAttribute(string sEnumName)
        {
            this.EnumName = sEnumName;
        }
        public NWDModelCustomTagAttribute(Type sEnum)
        {
            this.EnumName = sEnum.Name;
        }
    }
}
