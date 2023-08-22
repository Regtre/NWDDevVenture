﻿using System;
using System.Collections.Generic;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDTimeRange: NWDDataType 
    {
        public int Min { set; get; } = 0;
        public int Max { set; get; } = 0;
    }
}
