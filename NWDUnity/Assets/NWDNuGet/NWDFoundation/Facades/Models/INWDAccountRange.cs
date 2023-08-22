using System;
using System.Security.Cryptography.X509Certificates;

namespace NWDFoundation.Models
{
    public interface INWDAccountRange 
    {
        /// <summary>
        /// Server to use to find its data
        /// </summary>
        public ushort Range { set; get; }

    }
}