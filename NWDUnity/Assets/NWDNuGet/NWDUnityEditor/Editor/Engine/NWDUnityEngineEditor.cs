using NWDFoundation.Benchmark;
using NWDFoundation.Config;
using NWDFoundation.Logger;
using NWDUnityEditor.Benchmark;
using NWDUnityEditor.Config;
using NWDUnityEditor.Logger;
using NWDUnityEditor.Managers;
using NWDUnityEditor.Requestors;
using NWDUnityEditor.SQLite;
using NWDUnityEditor.Tools;
using NWDUnityShared.Engine;
using NWDUnityShared.Enumerations;
using NWDUnityShared.Facades;
using NWDUnityShared.Managers;
using NWDUnityShared.TaskSchedulers;
using NWDUnityShared.Tools;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NWDUnityEditor.Engine
{
    public partial class NWDUnityEngineEditor : INWDUnityEngine
    {
        static private readonly object _lock = new object();
        static private NWDUnityEngineEditor instance = null;
        static public NWDUnityEngineEditor Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NWDUnityEngineEditor();
                    instance.Launch();
                }
                return instance;
            }
        }

        public NWDConnectionState connectionState = NWDConnectionState.Offline;
        public NWDConnectionState ConnectionState
        {
            get
            {
                lock (_lock)
                {
                    return connectionState;
                }
            }
            set // May need to be done from the main thread to prevent partial behaviors on display screens
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

        private NWDUnityEngineScheduler Scheduler = new NWDUnityEngineScheduler();

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

        private NWDUnityEngineEditor()
        {
            // initialise debug
            NWDLogger.SetWriter(NWDUserSettings.LoadOrDefault(new NWDLoggerUnityEditor(NWDLogLevel.Information)));

            DataManager = new NWDUnityEditorDataManager();
            EnvironmentManager = new NWDUnityEditorEnvironmentManager();
            ThreadManager = new NWDUnityEditorThreadManager();
            AccountManager = new NWDUnityAccountManager();
            DeviceManager = NWDUserSettings.LoadOrDefault(new NWDUnityEditorDeviceManager());
            LocalDBManager = new NWDUnityEditorLocalDBManager();
        }

        public void Launch ()
        {

            NWDModelType.LoadAllGoodType();

            Config = NWDConfigUnityEditor.LoadConfig();
            // initialize benchmark
            NWDBenchmark.SetParameters(new NWDBenchmarkParametersUnityEditor());

            if (GetConfig().IsReady())
            {
                Start();
            }
            else
            {
                Stop();
            }
        }

        public void Start ()
        {
            LocalDBManager.Start();
            DataManager.Start();

            LaunchOperation = NWDAsyncOperationFactory.NewTask(Scheduler, async (_, _) =>
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

                NWDSettingsRequestor.GetCurrent().StartScheduler();
            });
        }

        public void Stop()
        {
            LocalDBManager.Stop();
            DataManager.Stop();

            LaunchOperation = NWDAsyncOperationFactory.NewTask(Scheduler, (_, _) =>
            {
                //NWDVirtualDataManager.StopRequestor();
                NWDSettingsRequestor.GetCurrent().StopScheduler();
            });
        }
        public bool ConfigLoaded()
        {
            return Config != null;
        }

        public NWDConfigUnityEditor GetConfig()
        {
            if (Config == null)
            {
                NWDLogger.Warning("_Config is null");
            }
            return Config as NWDConfigUnityEditor;
        }

        public NWDUnityEditorDataManager GetDataManager()
        {
            return DataManager as NWDUnityEditorDataManager;
        }

        public NWDUnityEditorLocalDBManager GetLocalDBManager()
        {
            return LocalDBManager as NWDUnityEditorLocalDBManager;
        }
    }
}
