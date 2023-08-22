using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NWDFoundation.Models;

namespace NWDRuntime.Factories
{
    public class NWDStudioDataFactory
    {
        public static NWDStudioDataStorage ToStudioDataStorage(NWDStudioData sO)
        {
            NWDStudioDataStorage tStudioData = new NWDStudioDataStorage
            {
                AvailableForWeb = sO.AvailableForWeb,
                AvailableForApp = sO.AvailableForApp,
                AvailableForGame = sO.AvailableForGame,
                DataTrack = sO.DataTrack,
                ClassName = sO.GetType().FullName,
                Json = JsonConvert.SerializeObject(sO),
                Reference = sO.Reference
            };
            return tStudioData;
        }

        public static List<NWDStudioDataStorage> ToStudioDatasStorage(List<NWDStudioData> list)
        {
            return list.Select(tBasicModel => ToStudioDataStorage(tBasicModel)).ToList();
        }


        public static NWDStudioData FromStudioDataStorage(NWDStudioDataStorage sData)
        {
            Type tType = Type.GetType(sData.ClassName);
            NWDStudioData tObject = JsonConvert.DeserializeObject(sData.Json, tType) as NWDStudioData;
            if (tObject != null)
            {
                tObject.SyncDatetime = sData.SyncDatetime;
                tObject.Commit = sData.Commit;
                tObject.Modification = sData.Modification;
            }

            return tObject;
        }
    }
}