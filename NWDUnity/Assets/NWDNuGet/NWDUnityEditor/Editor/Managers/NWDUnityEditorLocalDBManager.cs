using NWDUnityShared.Managers;
using System.IO;

namespace NWDUnityEditor.Managers
{
    public class NWDUnityEditorLocalDBManager : NWDUnityLocalDBManager
    {
        protected override string StudioDataFileName => "Virtual-StudioDatabase.sqlite";
        protected override string PlayerDataFileName => "Virtual-PlayerDatabase.sqlite";

        protected override string GenerateDatabasePath (string sParentFolder, string sFileName)
        {
            string tProjectDirectory = Path.Combine(Directory.GetParent(Path.GetTempPath()).FullName, "Net-Worked-Data", sParentFolder);

            if (!Directory.Exists(tProjectDirectory))
            {
                Directory.CreateDirectory(tProjectDirectory);
            }

            return Path.Combine(tProjectDirectory, sFileName);
        }
    }
}
