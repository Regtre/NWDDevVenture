using NWDEditor;
using NWDEditor.Exchanges;
using NWDEditor.Exchanges.Payloads;
using NWDHub.Models;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Factories;
using System;
using System.Collections.Generic;

namespace NWDUnityEditor.Services
{
    public class NWDMetaDataEditorService : NWDEditorService
    {
        const string K_LOCKER_NAME_SEPARATION = ":";

        static public List<NWDMetaData> SyncAll (List<NWDMetaData> sMetaDataList)
        {
            NWDUpPayloadSyncMetaData tUpPayload = new NWDUpPayloadSyncMetaData();
            tUpPayload.MetaDataList = sMetaDataList;
            tUpPayload.ProjectId = NWDUnityEngineEditor.Instance.GetConfig().GetProjectId();
            NWDRequestEditor tRequest = NWDRequestEditorFactory.New(NWDExchangeEditorKind.SyncMetaData, tUpPayload);
            NWDResponseEditor tResponse = exchanger.SendSync(tRequest, ForgeURL(exchanger.DefaultURI));

            CheckServerResponse(tResponse);

            NWDDownPayloadSyncMetaData tDownPayload = NWDResponseEditorFactory.GetPayload<NWDDownPayloadSyncMetaData>(tResponse);
            return tDownPayload.MetaDataList;
        }

        static public NWDMetaData Create(Type sType)
        {
            NWDUpPayloadCreateMetaData tUpPayload = new NWDUpPayloadCreateMetaData(sType.AssemblyQualifiedName);
            NWDRequestEditor tRequest = NWDRequestEditorFactory.New(NWDExchangeEditorKind.CreateMetaData, tUpPayload);

            NWDResponseEditor tResponse = exchanger.SendSync(tRequest, ForgeURL(exchanger.DefaultURI));

            CheckServerResponse(tResponse);

            NWDDownPayloadCreateMetaData tDownPayload = NWDResponseEditorFactory.GetPayload<NWDDownPayloadCreateMetaData>(tResponse);
            return tDownPayload.MetaData;
        }

        static public NWDMetaData Lock(NWDMetaData sMetaData)
        {
            NWDUpPayloadMetaDataLock tPayload = new NWDUpPayloadMetaDataLock();
            tPayload.MetaDataReference = sMetaData.Reference;
            tPayload.LockerName = UniqueLockerName();
            NWDRequestEditor tRequest = NWDRequestEditorFactory.New(NWDExchangeEditorKind.MetaDataLock, tPayload);
            NWDResponseEditor tResponse = exchanger.SendSync(tRequest, ForgeURL(exchanger.DefaultURI));
            CheckServerResponse(tResponse);

            NWDDownPayloadLockMetaData tDownPayload = NWDResponseEditorFactory.GetPayload<NWDDownPayloadLockMetaData>(tResponse);
            return tDownPayload.MetaData;
        }

        static public string UniqueLockerName ()
        {
            return NWDUnityEngineEditor.Instance.GetConfig().GetDeviceUniqueId() + K_LOCKER_NAME_SEPARATION + NWDUnityEngineEditor.Instance.GetConfig().GetNickname();
        }

        static public string NicknameFromLockerName (string sLockerName)
        {
            if (sLockerName == null)
            {
                return "";
            }
            int sIndex = sLockerName.IndexOf (K_LOCKER_NAME_SEPARATION);
            if (sIndex < 0)
            {
                return sLockerName;
            }
            return sLockerName.Substring(sIndex + 1);
        }

        static public NWDMetaData Unlock(NWDMetaData sMetaData)
        {
            NWDUpPayloadMetaDataUnlock tPayload = new NWDUpPayloadMetaDataUnlock();
            tPayload.MetaData = sMetaData;
            tPayload.LockerName = UniqueLockerName();
            NWDRequestEditor tRequest = NWDRequestEditorFactory.New(NWDExchangeEditorKind.MetaDataUnlock, tPayload);
            NWDResponseEditor tResponse = exchanger.SendSync(tRequest, ForgeURL(exchanger.DefaultURI));
            CheckServerResponse(tResponse);

            NWDDownPayloadUnlockMeta tDownPayload = NWDResponseEditorFactory.GetPayload<NWDDownPayloadUnlockMeta>(tResponse);
            return tDownPayload.MetaData;
        }
    }
}
