using System;
using NWDFoundation.WebEdition.Enums;

namespace NWDFoundation.WebEdition.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class NWDWebMethodTestGetAttribute : Attribute
    {
        private string _GetLinearizedParams;
        private int _Expected;
        private NWDPageStandardStatusTag _Tag;
        public NWDWebMethodTestGetAttribute(string sGetLinearizedParams, int sExpected = 200, NWDPageStandardStatusTag sTag = NWDPageStandardStatusTag.None)
        {
            _GetLinearizedParams = sGetLinearizedParams;
            _Expected = sExpected;
            _Tag = sTag;
        }

        public string GetLinearizedParams()
        {
            return _GetLinearizedParams;
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