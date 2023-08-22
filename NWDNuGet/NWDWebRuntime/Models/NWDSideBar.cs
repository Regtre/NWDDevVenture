namespace NWDWebRuntime.Models;

public class NWDSideBar
{
    public List<NWDSideBarBlock> Blocks { set; get; } = new List<NWDSideBarBlock>();
    public List<NWDSideBarAnnexe> Annexes { set; get; } = new List<NWDSideBarAnnexe>();
}