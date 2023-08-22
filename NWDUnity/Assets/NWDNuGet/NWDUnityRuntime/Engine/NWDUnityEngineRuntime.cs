using Newtonsoft.Json;
using NWDFoundation.Benchmark;
using NWDFoundation.Config;
using NWDFoundation.Logger;
using NWDUnityRuntime.Config;
using NWDUnityRuntime.Constants;
using NWDUnityRuntime.Logger;
using NWDUnityRuntime.Managers;
using NWDUnityRuntime.TaskSchedulers;
using NWDUnityShared.Engine;
using NWDUnityShared.Enumerations;
using NWDUnityShared.Facades;
using NWDUnityShared.Managers;
using NWDUnityShared.TaskSchedulers;
using NWDUnityShared.Tools;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace NWDUnityRuntime.Engine
{
    public class NWDUnityEngineRuntime : MonoBehaviour, INWDUnityEngine
    {
        static private readonly object _lock = new object();
        static private bool quitting = false;

        static private NWDUnityEngineRuntime instance = null;
        static public NWDUnityEngineRuntime Instance
        {
            get
            {
                if (instance == null && !quitting)
                {
                    GameObject gameObject = new GameObject("NWDEngine");
                    gameObject.AddComponent<NWDUnityEngineRuntime>();
                }
                return instance;
            }
        }

        public NWDConnectionState connectionState = NWDConnectionState.Offline;
        public NWDConnectionState ConnectionState
        {
            get
            {
                lock(_lock)
                {
                    return connectionState;
                }
            }
            set
            {
                lock (_lock)
                {
                    connectionState = value;
                }
            }
        }

        public INWDConfig Config { get; private set; }
        public INWDUnityAccountManager AccountManager { get; private set; }
        public INWDUnityDataManager DataManager { get; private set; }
        public INWDUnityDeviceManager DeviceManager { get; private set; }
        public INWDUnityEnvironmentManager EnvironmentManager { get; private set; }
        public INWDUnityThreadManager ThreadManager { get; private set; }
        public INWDUnityLocalDBManager LocalDBManager { get; private set; }

        private NWDUnityEngineScheduler Scheduler;

        private NWDAsyncOperation launchOperation = null;
        public NWDAsyncOperation LaunchOperation
        {
            get
            {
                lock (_lock)
                {
                    return launchOperation;
                }
            }
            private set
            {
                lock (_lock)
                {
                    launchOperation = value;
                }
            }
        }

        public event Action update;

        private void Awake()
        {
            if (instance == null)
            {
                // Prepare MonoBehavior Singleton
                instance = this;
                DontDestroyOnLoad(instance);
                Application.quitting += () => quitting = true;

                // Read configuration
                TextAsset tSerializedConfig = Resources.Load<TextAsset>(NWDUnityRuntimeConstants.kConfigFileName);
                NWDUnityRuntimeConfig tConfig = JsonConvert.DeserializeObject<NWDUnityRuntimeConfig>(tSerializedConfig.text);
                Config = tConfig;

                // Preapre managers and schedulers
                NWDLogger.SetWriter(new NWDLoggerUnityRuntime(NWDLogLevel.Information));

                DataManager = new NWDUnityRuntimeDataManager();
                EnvironmentManager = new NWDUnityRuntimeEnvironmentManager();
                ThreadManager = new NWDUnityRuntimeThreadManager();
                AccountManager = new NWDUnityAccountManager();
                DeviceManager = new NWDUnityRuntimeDeviceManager();
                LocalDBManager = new NWDUnityRuntimeLocalDBManager();

                // Schedulers
                Scheduler = new NWDUnityEngineScheduler();

                // Launch engine
                Launch();
            }
            else
            {
                Destroy(this);
            }
        }

        public void Launch()
        {
            launchOperation = AsyncLaunch();
        }

        public NWDAsyncOperation AsyncLaunch()
        {
            // Do main thread things here
            LocalDBManager.Start();

            // Do async stuff here
            return NWDAsyncOperationFactory.NewTask(Scheduler, async (_, _) =>
            {
                // Connect user to NWD account
                NWDAsyncOperation tSignIn = AccountManager.AutoSignIn();

                // Init local databases
                NWDAsyncOperation tLocalDBInit = LocalDBManager.Initialiaze();

                //// Wait for tasks completion
                await Task.WhenAll(tSignIn.Wait(), tLocalDBInit.Wait());

                // Load data in memory
                NWDAsyncOperation tLoadData = DataManager.Initialize();

                //// Wait for tasks completion
                await tLoadData.Wait();
            });
        }


        private void Update ()
        {
            update?.Invoke();
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}
