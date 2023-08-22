using Newtonsoft.Json;
using NWDCrucial.Models;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Models;
using NWDHub.Models;
using NWDWebEditor.Managers;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;

namespace NWDHub.Managers;



public static class NWDProjectDescriptionManager
{

    public static NWDProjectDescription? GetByPublicToken(string sPublicToken)
    {
        NWDProjectDescription? tProjectDescription = null;
        NWDProjectDescriptionStorage? tStorage = null;
        List<NWDProjectDescriptionStorage> tList = NWDWebDBDataManager.GetBy<NWDProjectDescriptionStorage>(new Dictionary<string, string>()
            { { nameof(NWDProjectDescriptionStorage.PublicToken), sPublicToken } });
        if (tList.Count == 1)
        {
            tStorage = tList[0];
            tProjectDescription = JsonConvert.DeserializeObject<NWDProjectDescription>(tStorage.Json);
        }

        return tProjectDescription;
    }
    
    public static NWDProjectDescriptionStorage? GetProjectDescriptionStorageByPublicToken(string sPublicToken)
    {
        NWDProjectDescriptionStorage? tStorage = null;
        List<NWDProjectDescriptionStorage> tList = NWDWebDBDataManager.GetBy<NWDProjectDescriptionStorage>(new Dictionary<string, string>()
            { { nameof(NWDProjectDescriptionStorage.PublicToken), sPublicToken } });
        if (tList.Count == 1)
        {
            tStorage = tList[0];
        }

        return tStorage;
    }

    public static void RemoveAllCache(NWDProject sProject)
    {
        List<NWDProjectDescriptionStorage> tList = NWDWebDBDataManager.GetBy<NWDProjectDescriptionStorage>(new Dictionary<string, string>()
            { { nameof(NWDProjectDescriptionStorage.ProjectReference), sProject.Reference.ToString() } });
        foreach (NWDProjectDescriptionStorage tProjectDescription in tList)
        {
            NWDWebDBDataManager.DeleteData(tProjectDescription);
        }
    }


    public static NWDProjectDescription? CreateInCacheFor(NWDProject sProject, List<NWDDataTrackDescription> sTrackList, List<NWDProjectCredentials> sProjectCredentials, NWDProjectRole sRole)
    {
        Dictionary<int, NWDTrackRights> tTracksRights = new Dictionary<int, NWDTrackRights>();
        
        //TODO Analyze rights for this role
        
        NWDProjectDescription rReturn = new NWDProjectDescription()
        {
            Name = sProject.Name,
            // Company = sProject.ProjectName.
            Description = sProject.Description,
            Project = sProject.Reference,
            ProjectId = sProject.ProjectUniqueId,
            Keys = sProjectCredentials.ToArray(),
            PublicToken = sRole.PublicToken,
            SecretToken = sRole.SecretToken,
            RoleName = sRole.Name,
            CanCreateMetaData = sRole.CanCreateMetaData,
            CanEditMetaDataInfos = sRole.CanEditMetaDataInfos,
            Track = sTrackList.ToArray(),
            TracksRights = tTracksRights,
            BaseLanguage = sProject.BaseLanguage,
            SupportLanguages = sProject.SupportLanguages,
        };
        NWDProjectDescriptionStorage tStorage = new NWDProjectDescriptionStorage()
        {
            ProjectReference = sProject.Reference,
            ProjectId = sProject.ProjectUniqueId,
            PublicToken = sRole.PublicToken,
            SecretToken = sRole.SecretToken,
            Json = JsonConvert.SerializeObject(rReturn)
        };
        NWDWebDBDataManager.SaveData(tStorage);
        return rReturn;
    }
}