using NWDFoundation.Exchanges;
using NWDFoundation.Models;
using NWDUnityShared.Engine;
using NWDUnityShared.Facades;
using NWDUnityShared.SQLite;
using NWDUnityShared.TaskSchedulers;
using NWDUnityShared.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NWDUnityShared.Managers
{
    public abstract class NWDUnityLocalDBManager : INWDUnityLocalDBManager
    {
        private object _lock = new object();

        public NWDLocalDBTaskScheduler Scheduler = new NWDLocalDBTaskScheduler();
        NWDSQLiteDbConnection PlayerDataConnection;
        NWDSQLiteDbConnection StudioDataConnection;

        private string StudioDataPath;
        private string PlayerDataPath;
        protected abstract string StudioDataFileName { get; }
        protected abstract string PlayerDataFileName { get; }

        public void Start()
        {
            string tProjectId = NWDUnityEngine.Instance.Config.GetProjectId().ToString();
            StudioDataPath = GenerateDatabasePath(tProjectId, StudioDataFileName);
            PlayerDataPath = GenerateDatabasePath(tProjectId, PlayerDataFileName);

            StudioDataConnection = new NWDSQLiteDbConnection(StudioDataPath);
            PlayerDataConnection = new NWDSQLiteDbConnection(PlayerDataPath);
        }

        public void Stop()
        {
            StudioDataPath = null;
            PlayerDataPath = null;
        }

        protected abstract string GenerateDatabasePath(string sParentFolder, string sFileName);

        public NWDAsyncOperation Initialiaze()
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, async (sHandler, _) =>
            {
                List<Task> tTasks = new List<Task>();
                if (!File.Exists(PlayerDataPath))
                {
                    tTasks.Add(Scheduler.Factory.StartNew(() =>
                    {
                        Internal_CreateDatabase<NWDPlayerDataStorage>(ref sHandler);
                    }));
                }

                if (tTasks.Count > 0)
                {
                    await Task.WhenAll(tTasks.ToArray());
                }
            });
        }

        public NWDAsyncOperation DeleteDatabase()
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                lock (_lock)
                {
                    if (File.Exists(StudioDataPath))
                    {
                        File.Delete(StudioDataPath);
                    }

                    if (File.Exists(PlayerDataPath))
                    {
                        File.Delete(PlayerDataPath);
                    }
                }
            });
        }

        public NWDAsyncOperation CreateDatabase<T>() where T : NWDDatabaseBasicModel
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                Internal_CreateDatabase<T>(ref sHandler);
            });
        }

        public NWDAsyncOperation RecordData<T>(List<T> sDataList) where T : NWDDatabaseBasicModel
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                lock (_lock)
                {
                    using (INWDDBConnection tConnection = ChooseDatabase(typeof(T)))
                    {
                        SQLiteToolbox.Reccords(ref sHandler, tConnection, sDataList, sDataList.Count);
                    }
                }
            });
        }

        public NWDAsyncOperation<List<T>> GetAll<T>() where T : NWDDatabaseBasicModel
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                lock (_lock)
                {
                    using (INWDDBConnection tConnection = ChooseDatabase(typeof(T)))
                    {
                        SQLiteToolbox.CreateTable<T>(ref sHandler, tConnection);
                        return SQLiteToolbox.GetAll<T>(ref sHandler, tConnection);
                    }
                }
            });
        }

        public NWDAsyncOperation<List<NWDPlayerDataStorage>> GetAll(ulong sAccountReference)
        {
            return NWDAsyncOperationFactory.NewTask(Scheduler, (sHandler, _) =>
            {
                lock (_lock)
                {
                    using (INWDDBConnection tConnection = ChooseDatabase(typeof(NWDPlayerDataStorage)))
                    {
                        string tWhereClause = string.Format("`{0}`={1}", nameof(NWDPlayerDataStorage.Account), sAccountReference);

                        SQLiteToolbox.CreateTable<NWDPlayerDataStorage>(ref sHandler, tConnection);
                        return SQLiteToolbox.GetAll<NWDPlayerDataStorage>(ref sHandler, tConnection, tWhereClause);
                    }
                }
            });
        }

        private void Internal_CreateDatabase<T>(ref NWDAsyncHandler sHandler) where T : NWDDatabaseBasicModel
        {
            lock (_lock)
            {
                using (INWDDBConnection tConnection = ChooseDatabase(typeof(T)))
                {
                    SQLiteToolbox.CreateTable<NWDPlayerDataStorage>(ref sHandler, tConnection);
                }
            }
        }

        private INWDDBConnection ChooseDatabase(Type sType)
        {
            INWDDBConnection tReturn;
            if (sType == typeof(NWDPlayerDataStorage))
            {
                tReturn = PlayerDataConnection;
            }
            else
            {
                tReturn = StudioDataConnection;
            }
            return tReturn.Open();
        }
    }
}
