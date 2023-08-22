using NWDEditor;
using NWDFoundation.Models;

namespace NWDHub.Factories
{
    static public class NWDStudioDataStorageFactory
    {
        static public NWDStudioDataStorage FromMetaData (NWDMetaData sMetaData, NWDSubMetaData sSubMetaData)
        {
            NWDStudioDataStorage rReturn = new NWDStudioDataStorage()
            {
                AvailableForWeb = sMetaData.AvailableForWeb,
                AvailableForApp = sMetaData.AvailableForApp,
                AvailableForGame = sMetaData.AvailableForGame,
                DataTrack = sSubMetaData.TrackId,
                ClassName = sMetaData.ClassName,
                Json = sSubMetaData.Data,
                Reference = sMetaData.Reference,
                Commit = 1
            };
            return rReturn;
        }
    }
}
