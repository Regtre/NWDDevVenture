using NWDUnityShared.Managers;
using System.IO;
using UnityEngine;

namespace NWDUnityRuntime.Managers
{
    public class NWDUnityRuntimeLocalDBManager : NWDUnityLocalDBManager
    {
        protected override string StudioDataFileName => "StudioDatabase.sqlite";
        protected override string PlayerDataFileName => "PlayerDatabase.sqlite";

        protected override string GenerateDatabasePath(string sParentFolder, string sFileName)
        {
            string tProjectDirectory = Path.Combine(Application.persistentDataPath, "Net-Worked-Data", sParentFolder);

            if (!Directory.Exists(tProjectDirectory))
            {
                Directory.CreateDirectory(tProjectDirectory);
            }

            return Path.Combine(tProjectDirectory, sFileName);
        }
    }
}
