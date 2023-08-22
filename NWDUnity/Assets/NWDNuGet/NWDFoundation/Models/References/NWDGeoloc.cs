using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDGeolocation: NWDDataType 
    {
        public float Latitude { set; get; }
        public float Longitude { set; get; }
        public float Altitude { set; get; }
    }
}
