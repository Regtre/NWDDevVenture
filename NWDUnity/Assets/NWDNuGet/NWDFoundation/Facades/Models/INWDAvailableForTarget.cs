using System;
using System.Security.Cryptography.X509Certificates;

namespace NWDFoundation.Models
{
    public interface INWDAvailableForTarget
    {
        public bool AvailableForWeb { set; get; }
        public bool AvailableForGame { set; get; }
        public bool AvailableForApp { set; get; }

    }
}