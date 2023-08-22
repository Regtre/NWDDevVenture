using System;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDColor : INWDSubModel 
    {
        public float Red { set; get; }
        public float Green { set; get; }
        public float Blue { set; get; }
        public float Alpha { set; get; }
    }
}
