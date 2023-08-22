using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDAnimationCurve : NWDDataType
    {
        private string Values { set; get; } = string.Empty;
    }
}
