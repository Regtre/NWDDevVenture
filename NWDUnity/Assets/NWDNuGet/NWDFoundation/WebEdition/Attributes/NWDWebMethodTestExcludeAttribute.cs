using System;
using NWDFoundation.WebEdition.Enums;

namespace NWDFoundation.WebEdition.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class NWDWebMethodTestExcludeAttribute : Attribute
    {
        public NWDWebMethodTestExcludeAttribute(int sExpected = 200, NWDPageStandardStatusTag sTag = NWDPageStandardStatusTag.None)
        {
        }
    }
}