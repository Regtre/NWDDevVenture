using System;

namespace NWDFoundation.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class NWDModelCustomCheckAttribute : Attribute
    {
        public string FlagsName;
        public NWDModelCustomCheckAttribute(string sFlagsName)
        {
            this.FlagsName = sFlagsName;
        }
        public NWDModelCustomCheckAttribute(Type sFlags)
        {
            this.FlagsName = sFlags.Name;
        }
    }
}
