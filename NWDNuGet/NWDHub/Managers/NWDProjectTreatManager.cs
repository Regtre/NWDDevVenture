using NWDFoundation.Configuration.Environments;
using NWDFoundation.Tools;
using NWDHub.Models;
using NWDWebRuntime.Models;

namespace NWDHub.Managers
{
    public class NWDProjectTreatManager
    {
        // public static void RemoveAllCache(NWDProject sProject)
        // {
        //     List<NWDProjectTreatStorage> tListPTreat = NWDWebDBDataManager.GetBy<NWDProjectTreatStorage>(new Dictionary<string, string>()
        //         { { nameof(NWDProjectTreatStorage.ProjectReference), sProject.Reference.ToString() } });
        //     foreach (NWDProjectTreatStorage tProjectDescription in tListPTreat)
        //     {
        //         NWDWebDBDataManager.DeleteData(tProjectDescription);
        //     }
        // }
        public static NWDProjectTreatStorage? GetByProjectUniqueId(ulong sProjectUniqueId, NWDEnvironmentKind sEnvironmentKind)
        {
            NWDProjectTreatStorage tStorage = null;
            List<NWDProjectTreatStorage> tList = NWDWebDBDataManager.GetBy<NWDProjectTreatStorage>(new Dictionary<string, string>()
            {
                { nameof(NWDProjectTreatStorage.ProjectUniqueId), sProjectUniqueId.ToString() },
                { nameof(NWDProjectTreatStorage.Environment), ((int)sEnvironmentKind).ToString() },
            });
            if (tList.Count >= 1)
            {
                tStorage = tList[0];
            }
            return tStorage;
        }
        public static List<NWDProjectTreatStorage> GetAllByProjectUniqueId(ulong sProjectUniqueId)
        {
            List<NWDProjectTreatStorage> tList = NWDWebDBDataManager.GetBy<NWDProjectTreatStorage>(new Dictionary<string, string>()
            {
                { nameof(NWDProjectTreatStorage.ProjectUniqueId), sProjectUniqueId.ToString() },
            });
            return tList;
        }

        public static NWDProjectTreatStorage? CreateInCacheFor(NWDProject sProject, NWDEnvironmentKind sEnvironment)
        {
            NWDProjectTreatStorage tTreatStorage = GetByProjectUniqueId(sProject.ProjectUniqueId, sEnvironment);
            if (tTreatStorage != null)
            {
                tTreatStorage.Modification = NWDTimestamp.ToTimestamp(DateTime.UtcNow);
                NWDWebDBDataManager.SaveData(tTreatStorage, false);
            }
            else
            {
                tTreatStorage = new NWDProjectTreatStorage()
                {
                    Creation = NWDTimestamp.ToTimestamp(DateTime.UtcNow),
                    Modification = NWDTimestamp.ToTimestamp(DateTime.UtcNow),
                    ProjectReference = sProject.Reference,
                    ProjectUniqueId = sProject.ProjectUniqueId,
                    Environment = sEnvironment,
                    ProjectKey = NWDRandom.RandomNetWorkedDataToken(),
                    SecretKey = NWDRandom.RandomNetWorkedDataToken(),
                    TreatKey = NWDRandom.RandomNetWorkedDataToken(),
                };
                NWDWebDBDataManager.SaveData(tTreatStorage);
            }

            return tTreatStorage;
        }
    }
}