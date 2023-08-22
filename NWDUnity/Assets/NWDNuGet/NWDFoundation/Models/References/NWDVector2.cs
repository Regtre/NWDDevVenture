using System;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDVector2 : INWDSubModel
    {
        public float X { set; get; }
        public float Y { set; get; }
    }
}
