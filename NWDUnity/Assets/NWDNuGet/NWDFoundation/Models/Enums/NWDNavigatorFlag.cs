using System;

namespace NWDFoundation.Models.Enums
{
    [Serializable]
    [Flags]
    public enum NWDNavigatorFlag : int
    {
        None = 0,
        Safari = 1,
        Chrome = 2,
        Firefox = 4,
        Edge = 8,
        
        Game = 128,
        
        AllBrowsers = Safari | Chrome | Firefox | Edge,
        All = Safari | Chrome | Firefox | Edge | Game
    }

}