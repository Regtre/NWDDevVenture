using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace NWDFoundation.Tools
{
    public partial class NWDToolbox
    {
        
        public static string Base64Encode(string sText) 
        {
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sText));
        }

        public static string Base64Decode(string sBase64EncodedData) 
        {
            return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(sBase64EncodedData));
        }
    }
}