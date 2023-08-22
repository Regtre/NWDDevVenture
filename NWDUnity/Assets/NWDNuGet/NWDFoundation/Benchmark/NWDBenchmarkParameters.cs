using Newtonsoft.Json;
using System;

namespace NWDFoundation.Benchmark
{
    [Serializable]
    public class NWDBenchmarkParameters
    {
        public bool IsEnable = false;
        public float FrameRate = -1;
        public float BenchmarkLimit = 60.0F;
        public bool BenchmarkShowStart = true;
        public string Green = "#007626FF";
        public string Orange = "#B45200FF";
        public string Red = "#890000FF";
        public string Blue = "#002089FF";
        [JsonIgnore]
        public float kWarningDefault = 0.05f;
        [JsonIgnore]
        public float kMaxDefault = 0.015f;

        public virtual void PrefReload() { }
        public virtual void Save() { }
    }
}