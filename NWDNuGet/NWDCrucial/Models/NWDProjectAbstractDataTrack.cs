using System;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;

namespace NWDCrucial.Models
{
    [Serializable]
    [NWDWebClassDescription("Environment model", "far fa-calendar", "Avec une description à la con pour voir si ça marche...", new int[] { 5, 10, 20 }, false, false, false)]
    public class NWDProjectAbstractDataTrack : NWDBasicModel
    {
        #region properties
        public NWDEnvironmentKind Kind { set; get; }
        public int Track { set; get; }
        #endregion

        #region constructor

        public NWDProjectAbstractDataTrack()
        {
        }
        
        #endregion
    }
}