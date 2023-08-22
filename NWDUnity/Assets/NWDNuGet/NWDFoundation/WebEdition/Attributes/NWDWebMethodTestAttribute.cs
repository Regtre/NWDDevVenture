using System;
using NWDFoundation.WebEdition.Enums;

namespace NWDFoundation.WebEdition.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class NWDWebMethodTestAttribute : Attribute
    {
        private int _Expected;
        private NWDPageStandardStatusTag _Tag;

        public NWDWebMethodTestAttribute(int sExpected = 200, NWDPageStandardStatusTag sTag = NWDPageStandardStatusTag.None)
        {
            _Expected = sExpected;
            _Tag = sTag;
        }

        public int GetExpected()
        {
            return _Expected;
        }

        public NWDPageStandardStatusTag GetTag()
        {
            return _Tag;
        }
    }
}