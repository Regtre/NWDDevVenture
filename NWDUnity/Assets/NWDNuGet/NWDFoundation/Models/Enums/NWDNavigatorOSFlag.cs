using System;

namespace NWDFoundation.Models.Enums
{
    [Serializable]
    [Flags]
    public enum NWDNavigatorOSFlag : int
    {
        None = 0,
        Windows = 1,
        MacOS = 2,
        Linux = 4,
        
        
        iOS = 64, 
        Android = 128, 

        AllDesktops = Windows | MacOS | Linux,
        AllMobiles = iOS | Android,
        All = Windows | MacOS | Linux | iOS | Android
    }
}