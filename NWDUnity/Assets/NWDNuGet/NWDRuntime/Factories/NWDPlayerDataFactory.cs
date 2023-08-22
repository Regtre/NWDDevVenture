using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NWDFoundation.Models;

namespace NWDRuntime.Factories
{
    public class NWDPlayerDataFactory
    {
        public static NWDPlayerDataStorage ToDataPlayerStorage(NWDPlayerData sO, ulong sAccountReference)
        {

            NWDPlayerDataStorage tPlayerData = new NWDPlayerDataStorage
            {
                Creation = sO.Creation,
                Modification = sO.Modification,
                AvailableForWeb = sO.AvailableForWeb,
                AvailableForApp = sO.AvailableForApp,
                AvailableForGame = sO.AvailableForGame,
                DataTrack = sO.DataTrack,
                ClassName = sO.GetType().AssemblyQualifiedName,
                Json = JsonConvert.SerializeObject(sO),
                Account = sO.Account,
                Reference = sO.Reference,
                Trashed = sO.Trashed
            };
            return tPlayerData;
        }

        public static List<NWDPlayerDataStorage> ToPlayerDatasStorage(List<NWDPlayerData> list, ulong sAccountReference)
        {
            return list.Where(tBasicModel => tBasicModel != null).Select(tBasicModel => ToDataPlayerStorage(tBasicModel, sAccountReference)).ToList();
        }


        public static NWDPlayerData FromPlayerDataStorage(NWDPlayerDataStorage sData)
        {
            Type tType = Type.GetType(sData.ClassName);
            NWDPlayerData tObject = JsonConvert.DeserializeObject(sData.Json, tType) as NWDPlayerData;
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