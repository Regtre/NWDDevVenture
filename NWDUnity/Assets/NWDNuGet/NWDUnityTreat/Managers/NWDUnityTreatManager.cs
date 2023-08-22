using Newtonsoft.Json;
using NWDFoundation.Models;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Models;
using NWDUnityEditor.Tools;
using NWDUnityShared.Tools;
using NWDUnityTreat.Services;
using NWDUnityTreat.TaskSchedulers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NWDUnityEditor.Managers
{
    [Serializable]
    public class NWDUnityTreatManager
    {
        static private NWDUnityTreatManager instance = null;

        public static NWDUnityTreatManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = NWDUserSettings.LoadOrDefault(new NWDUnityTreatManager());
                }
                return instance;
            }
        }

        private object _lock = new object();

        [JsonProperty]
        private List<NWDService> Services = new List<NWDService> ();

        public List<NWDService> GetServices ()
        {
            lock (_lock)
            {
                return Services;
            }
        }
        public NWDService GetService(int sIndex)
        {
            lock (_lock)
            {
                NWDService rResult = null;
                if (sIndex >= 0 && sIndex < Services.Count)
                {
                    rResult = Services[sIndex];
                }
                return rResult;
            }
        }

        public bool AddService ()
        {
            return AddService (new NWDService());
        }

        private bool AddService (NWDService sService)
        {
            lock (_lock)
            {
                bool rResult = false;
                if (sService != null && !Services.Any (s => s.Reference == sService.Reference))
                {
                    Services.Add (sService);
                    rResult = true;
                    Save();
                }
                return rResult;
            }
        }

        public bool RemoveServiceAt (int sIndex)
        {
            lock (_lock)
            {
                bool rResult = false;
                if (sIndex >=0 && sIndex < Services.Count)
                {
                    Services.RemoveAt(sIndex);
                    rResult = true;
                    Save();
                }
                return rResult;
            }
        }

        public void Save()
        {
            NWDUnityEngineEditor.Instance.ThreadManager.UniqueCallOnMainThread(ProcessSave);
        }

        private void ProcessSave()
        {
            NWDUserSettings.Save(this);
        }

        public NWDAsyncOperation Associate (NWDAccountService sAccountService)
        {
            return NWDAsyncOperationFactory.NewTask(NWDTreatTaskScheduler.Instance, (_,_) =>
            {
                NWDTreatService.AssociateService(sAccountService);
            });
        }
    }
}
