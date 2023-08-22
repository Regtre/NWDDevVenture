using Microsoft.AspNetCore.Http;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;

namespace NWDHub.Models
{
    public class NWDProjectDevDataTrack : NWDProjectDataTrack
    {
        #region constructor
        public NWDProjectDevDataTrack()
        {
            Kind = NWDEnvironmentKind.Dev;
        }
        public NWDProjectDevDataTrack(NWDProject sProject)
        {
            Kind = NWDEnvironmentKind.Dev;
            this.AssociateToProject(sProject);
        }
        #endregion
    }
    public class NWDProjectPlayTestDataTrack : NWDProjectDataTrack
    {
        #region constructor
        public NWDProjectPlayTestDataTrack()
        {
            Kind = NWDEnvironmentKind.PlayTest;
        }
        public NWDProjectPlayTestDataTrack(NWDProject sProject)
        {
            Kind = NWDEnvironmentKind.PlayTest;
            this.AssociateToProject(sProject);
        }
        #endregion
    }
    public class NWDProjectQualificationDataTrack : NWDProjectDataTrack
    {
        #region constructor
        public NWDProjectQualificationDataTrack()
        {
            Kind = NWDEnvironmentKind.Qualification;
        }
        public NWDProjectQualificationDataTrack(NWDProject sProject)
        {
            Kind = NWDEnvironmentKind.Qualification;
            this.AssociateToProject(sProject);
        }

        public NWDProjectPublishDataTrack SubCreate(NWDProject sProject, NWDEnvironmentKind sEnvironmentKind)
        {
            NWDProjectPublishDataTrack rReturn = new NWDProjectPublishDataTrack()
            {
                Kind = sEnvironmentKind,
                Track = Track,
                UsualName = UsualName,
                Color = Color,
                TrackFollow = Track
            };
            switch (sEnvironmentKind)
            {
                case NWDEnvironmentKind.PreProduction:
                    rReturn.Track = 1000 + Track;
                    break;
                case NWDEnvironmentKind.Production:
                    rReturn.Track = 2000 + Track;
                    break;
                case NWDEnvironmentKind.PostProduction:
                    rReturn.Track = 3000 + Track;
                    break;
                default:
                    break;
            }
            rReturn.AssociateToProject(sProject);
            return rReturn;
        }
        #endregion
    }
    public class NWDProjectPublishDataTrack : NWDProjectDataTrack
    {
        #region constructor
        public NWDProjectPublishDataTrack()
        {
            Kind = NWDEnvironmentKind.PreProduction;
        }
        public NWDProjectPublishDataTrack(NWDProject sProject)
        {
            Kind = NWDEnvironmentKind.PreProduction;
            this.AssociateToProject(sProject);
        }
        #endregion
    }

    [Serializable]
    [NWDWebClassDescription("Environment model", "far fa-calendar", "Avec une description à la con pour voir si ça marche...", new int[] { 5, 10, 20 }, false, false, false)]
    public class NWDProjectDataTrack : NWDProjectSubObject
    {
        #region properties
        [NWDWebPropertyHidden]
        public NWDEnvironmentKind Kind { set; get; } = NWDEnvironmentKind.Dev;
        /// <summary>
        /// Name of this environment
        /// </summary>
        //[JsonProperty("Nam")]
        [NWDWebPropertyDescription("Title", NWDWebEditionStyle.AsciiText,true, "", "my Title", "my Title Placeholder", true, true, true)]
        public string UsualName { set; get; } = string.Empty;

        [NWDWebPropertyDescription("Color", NWDWebEditionStyle.Color,true, "", "my Color", "my Color Placeholder", false, true, false)]
        public string Color { set; get; } = NWDRandom.RandomHexadecimalColor("#");


        [NWDWebPropertyDescription("Track", NWDWebEditionStyle.ShowIntOnly,false, "", "Track", "Track", true, true)]
        public int Track { set; get; } = 0;
        [NWDWebPropertyDescription("Follow", NWDWebEditionStyle.Track,false, "", "Follow track", "Follow track", true, true)]
        public int TrackFollow { set; get; } = 0;

        #endregion

        #region constructor

        public NWDProjectDataTrack()
        {
        }
        
        public NWDProjectDataTrack(NWDProject sProject)
        {
            this.AssociateToProject(sProject);
        }
        

        public NWDDataTrackDescription ToDataTrackDescription(Dictionary<int, NWDEnvironmentKind> sTrackEnvironment, Dictionary<int, ulong> sTrackReference)
        {
            ulong tStalkedReference = 0;
            if (sTrackReference.ContainsKey(TrackFollow))
            {
                tStalkedReference = sTrackReference[TrackFollow];
            }
            return new NWDDataTrackDescription()
            {
                Track = Track,
                Reference = Reference,
                Kind = Kind,
                Name = UsualName,
                Color = Color.Replace("#",""),
                //StalkedTrack  = new KeyValuePair<int, NWDEnvironmentKind> (TrackFollow, sTrackEnvironment[TrackFollow]),
                StalkedReference = tStalkedReference
            };
        }
        #endregion
    }
}