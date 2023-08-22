using Microsoft.AspNetCore.Http;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;
using NWDWebRuntime.Extensions;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Tools;

namespace NWDHub.Models
{
    [Serializable]
    public class NWDProject : NWDPlayerData
    {
        [NWDWebPropertyHiddenDebugable] public bool NeedToBePublish { set; get; } = true;

        [NWDWebPropertyDescription("Name of project", NWDWebEditionStyle.Text, true, "", "The name of project. Only unix text accepted.", "", true)]
        public string Name { set; get; } = string.Empty;

        [NWDWebPropertyDescription("WebsiteDns", NWDWebEditionStyle.Text, true, "", "", "", true)]
        public string WebsiteDns { set; get; } = string.Empty;

        [NWDWebPropertyDescription("Description", NWDWebEditionStyle.Text,false, "", "", "", true)]
        public string Description { set; get; } = string.Empty;

        [NWDWebPropertyDescription("base Language", NWDWebEditionStyle.Language,false, "", "", "", true)]
        public string BaseLanguage { set; get; } = "en-US"; // must stay empty else it's populated by adds

        [NWDWebPropertyDescription("Support Languages", NWDWebEditionStyle.Languages,false, "", "", "", true)]
        public string[] SupportLanguages { set; get; } = new string[] {"en-US", "fr-FR"}; // must stay empty else it's populated by adds

        [NWDWebPropertyDescription("SocietyName", NWDWebEditionStyle.Text,false, "", "", "", true)]

        public string SocietyName { set; get; } = "idéMobi";

        [NWDWebPropertyDescription("SocietyAddress", NWDWebEditionStyle.Text,false, "", "", "", true)]
        public string SocietyAddress { set; get; } = "43, rue d'Atlanta";

        [NWDWebPropertyDescription("SocietyTown", NWDWebEditionStyle.Text,false, "", "", "", true)]
        public string SocietyTown { set; get; } = "Marcq-En-Barœul";

        [NWDWebPropertyDescription("SocietyZipCode", NWDWebEditionStyle.Text,false, "", "", "", true)]
        public string SocietyZipCode { set; get; } = "59700";

        [NWDWebPropertyDescription("SocietyCountry", NWDWebEditionStyle.Country,false, "", "", "", true)]
        public string SocietyCountry { set; get; } = "FRANCE";

        [NWDWebPropertyDescription("SocietySiret", NWDWebEditionStyle.Text,false, "", "", "", true)]
        public string SocietySiret { set; get; } = "44424955100011";

        [NWDWebPropertyDescription("SocietyApe", NWDWebEditionStyle.Text,false, "", "", "", true)]
        public string SocietyApe { set; get; } = "7722C";

        [NWDWebPropertyDescription("SocietyRcs", NWDWebEditionStyle.Text,false, "", "", "", true)]
        public string SocietyRcs { set; get; } = "LILLE";

        [NWDWebPropertyDescription("SocietyTva", NWDWebEditionStyle.Text,false, "", "", "", true)]
        public string SocietyTva { set; get; } = "FR93444249551";

        // use in show view
        // [NWDWebPropertyHiddenDebugable]
        [NWDWebPropertyDescription("Project ID", NWDWebEditionStyle.ShowTextOnly,false, "", "", "", true)]
        public ulong ProjectUniqueId { set; get; } = RandomProjectUniqueId();

        // use in show view
        [NWDWebPropertyHidden] public NWDReference<NWDProjectName> ProjectName { set; get; }

        // use in show view
        [NWDWebPropertyHidden] public NWDReference<NWDProjectPlan> ProjectPlan { set; get; }

        // use in show view
        [NWDWebPropertyHidden] public NWDReference<NWDProjectTags> ProjectTags { set; get; }

        // [NWDWebPropertyDescription("ProjectKey", NWDWebEditionStyle.Hidden, "", "", "", true)]
        // public string ProjectKey { set; get; } = NWDRandom.RandomNetWorkedDataToken();
        //
        // [NWDWebPropertyDescription("ProjectKey", NWDWebEditionStyle.Hidden, "", "", "", true)]
        // public string SecretKey { set; get; } = NWDRandom.RandomNetWorkedDataToken();
        //
        // [NWDWebPropertyDescription("TreatKey", NWDWebEditionStyle.Hidden, "", "", "", true)]
        // public string TreatKey { set; get; } = NWDRandom.RandomNetWorkedDataToken();

        // [NWDWebPropertyDescription("TokenAccess", NWDWebEditionStyle.Hidden, "", "", "", true)]
        // public string TokenAccess { set; get; } = NWDRandom.RandomStringToken(128);

        // use in show view
        [NWDWebPropertyHidden] public NWDReference<NWDProjectByEnvironment> Development { set; get; }

        // use in show view
        [NWDWebPropertyHidden] public NWDReference<NWDProjectByEnvironment> PlayTest { set; get; }

        // use in show view
        [NWDWebPropertyHidden] public NWDReference<NWDProjectByEnvironment> Qualification { set; get; }

        // use in show view
        [NWDWebPropertyHidden] public NWDReference<NWDProjectByEnvironment> PreProduction { set; get; }

        // use in show view
        [NWDWebPropertyHidden] public NWDReference<NWDProjectByEnvironment> Production { set; get; }

        // use in show view
        [NWDWebPropertyHidden] public NWDReference<NWDProjectByEnvironment> PostProduction { set; get; }

        // use in show view
        [NWDWebPropertyHidden] public NWDReferencesArray<NWDProjectDataTrack> Tracks { set; get; } = new NWDReferencesArray<NWDProjectDataTrack>();

        // use in show view
        [NWDWebPropertyHidden] public NWDReferencesArray<NWDClassConstruction> ClassesCustomStudio { set; get; } = new NWDReferencesArray<NWDClassConstruction>();

        // use in show view
        [NWDWebPropertyHidden] public NWDReferencesArray<NWDClassConstruction> ClassesCustomPLayer { set; get; } = new NWDReferencesArray<NWDClassConstruction>();

        // use in show view
        [NWDWebPropertyHidden] public NWDReferencesArray<NWDProjectService> Services { set; get; } = new NWDReferencesArray<NWDProjectService>();

        static ulong RandomProjectUniqueId()
        {
            //TODO Verify project is unique by SQL
            return (ulong)(NWDToolBox.ToTimestampUnix(DateTime.Now) * 10000 + NWDRandom.IntNumeric(5));
        }

        public void TestTrackFollow(HttpContext sHttpContext)
        {
            // TODO Test TrackFollow validity
            // get all data track for project 
            List<NWDProjectDataTrack> tAllTracks = Tracks.GetAllObjects(sHttpContext);
        }

        public void TestDevTrackZero(HttpContext sHttpContext)
        {
            List<NWDProjectDevDataTrack> tList = NWDWebDataManager.GetAllData<NWDProjectDevDataTrack>(sHttpContext);
            NWDProjectDevDataTrack? tDevZero = null;
            foreach (NWDProjectDevDataTrack tObject in tList)
            {
                if (tObject.Project == Reference)
                {
                    if (tObject.TrackFollow == 0)
                    {
                        if (tObject.Track == 100)
                        {
                            tDevZero = tObject;
                        }
                    }
                }
            }

            if (tDevZero == null)
            {
                tDevZero = new NWDProjectDevDataTrack()
                {
                    Project = Reference,
                    ProjectId = ProjectUniqueId,
                    Kind = NWDEnvironmentKind.Dev,
                    UsualName = "dev standard",
                    Color = NWDRandom.RandomHexadecimalColor(),
                    Track = 100,
                    TrackFollow = 0
                };
                NWDWebDataManager.SaveData(sHttpContext, tDevZero);
            }
        }

        static bool FalseValues(String sText)
        {
            NWDLogger.Critical("FalseValues => "+sText+" ? "+bool.FalseString.ToLower()+"...");
            return sText == bool.FalseString.ToLower();
        }

        public void Check()
        {
            NWDLogger.Critical("ok je passe dans Check() BaseLanguage = " +BaseLanguage);
            List<string> tSupportLanguage = new List<string>(SupportLanguages);
            tSupportLanguage.RemoveAll(FalseValues);
            if (string.IsNullOrEmpty(BaseLanguage) == true)
            {
                BaseLanguage = "en-US";
            }
            if (tSupportLanguage.Contains(BaseLanguage) == false)
            {
                tSupportLanguage.Add(BaseLanguage);
            }
            SupportLanguages = tSupportLanguage.ToArray();
        }
    }
}