using Newtonsoft.Json;
using NWDFoundation.Logger;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Tools;
using System;
using UnityEngine;

namespace NWDUnityEditor.Logger
{
    [Serializable]
    public class NWDLoggerUnityEditor : INWDLogger
    {
        private const string K_PROJECT_PREF = nameof(NWDLoggerUnityEditor) + nameof(NWDLogLevel);
        [JsonProperty]
        private NWDLogLevel Level = NWDLogLevel.None;

        public NWDLoggerUnityEditor(NWDLogLevel sLogLevel)
        {
            Level = sLogLevel;
        }

        public void SetLogLevel(NWDLogLevel sLogLevel)
        {
            Level = sLogLevel;
            NWDUnityEngineEditor.Instance.ThreadManager.UniqueCallOnMainThread(() => NWDUserSettings.Save(this));
        }

        public NWDLogLevel DefaultLogLevel()
        {
            return NWDLogLevel.Information;
        }

        public void WriteLog(NWDLogLevel sLogLevel, NWDLogCategory sLogCategory, string sString, object sObject)
        {
            WriteLog(sLogLevel, sString, sObject);
        }

        public void WriteLog(NWDLogLevel sLogLevel, NWDLogCategory sLogCategory, string sTitle, object sObject, string[] sMessages)
        {
            WriteLog(sLogLevel, sTitle + "\n" + string.Join('\n', sMessages), sObject);
        }

        public bool IsActivated()
        {
            return true;
        }

        public NWDLogLevel LogLevel()
        {
            return Level;
        }

        public void WriteLog(NWDLogLevel level, string sString, object sObject)
        {
            sString = "[" + level.ToString() + "] " + sString;

            UnityEngine.Object tObject = sObject as UnityEngine.Object;
            Exception tException = sObject as Exception;

            switch (level)
            {
                case NWDLogLevel.Trace:
                case NWDLogLevel.Debug:
                case NWDLogLevel.Information:
                    Debug.Log(sString, tObject);
                    break;
                case NWDLogLevel.Warning:
                    Debug.LogWarning(sString, tObject);
                    break;
                case NWDLogLevel.Error:
                case NWDLogLevel.Critical:
                    if (tException != null)
                    {
                        Debug.LogException(tException, tObject);
                    }
                    else
                    {
                        Debug.LogError(sString, tObject);
                    }

                    break;
                case NWDLogLevel.None:
                    break;
            }
        }
    }
}