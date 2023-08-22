namespace NWDRuntime.Models
{
    public class NWDDataInMemoryPlayerAndStudio
    {
        public NWDDataInMemory PlayerData { set; get; } = new NWDDataInMemory();
        public NWDDataInMemory StudioData { get; set; } = new NWDDataInMemory();
    }
}