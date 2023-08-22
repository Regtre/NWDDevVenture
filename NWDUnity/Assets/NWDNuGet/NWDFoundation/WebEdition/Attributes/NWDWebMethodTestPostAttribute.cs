using System;
using NWDFoundation.WebEdition.Enums;

namespace NWDFoundation.WebEdition.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class NWDWebMethodTestPostAttribute : Attribute
    {
        private string _PostLinearized;
        private int _Expected;
        private NWDPageStandardStatusTag _Tag;

        public NWDWebMethodTestPostAttribute(string sPostLinearized, int sExpected = 200, NWDPageStandardStatusTag sTag = NWDPageStandardStatusTag.None)
        {
            _PostLinearized = sPostLinearized;
            _Expected = sExpected;
            _Tag = sTag;
        }

        public string PostLinearizedJson()
        {
            return _PostLinearized;
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