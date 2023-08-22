using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Facades.Back;
using NWDServerBack.MariaDb.ByReflexion;
using NWDServerBack.MariaDb.Specific;
using NWDServerBack.MySql.ByReflexion;
using NWDServerBack.MySql.Specific;
using NWDCrucial.Facades;
using NWDFoundation.Logger;

namespace NWDServerBack
{
    public static class NWDDevServerBackStaticFactory
    {
        public static List<T> GetDaoList<T>(List<NWDDatabaseCredentials> sConfigurationDatabase) where T : INWDDao
        {
            List<T> rDaoList = new List<T>();
            foreach (NWDDatabaseCredentials tConfig in sConfigurationDatabase)
            {
                T tDatabase = default(T);
                if (typeof(T) == typeof(INWDAccountDao))
                {
                    tDatabase = (T) GetAccountDao(tConfig);
                }
                else if (typeof(T) == typeof(INWDAccountServiceDao))
                {
                    tDatabase = (T) GetAccountServiceDao(tConfig);
                }
                else if (typeof(T) == typeof(INWDAccountSignDao))
                {
                    tDatabase = (T) GetAccountSignDao(tConfig);
                }
                else if (typeof(T) == typeof(INWDAccountOrderDao))
                {
                    tDatabase = (T) GetAccountOrderDao(tConfig);
                }
                else if (typeof(T) == typeof(INWDAccountInvoiceDao))
                {
                    tDatabase = (T) GetAccountInvoiceDao(tConfig);
                }
                else if (typeof(T) == typeof(INWDAccountTokenDao))
                {
                    tDatabase = (T) GetAccountTokenDao(tConfig);
                }
                else if (typeof(T) == typeof(INWDPlayerDataDao))
                {
                    tDatabase = (T) GetPlayerDataDao(tConfig);
                }
                else if (typeof(T) == typeof(INWDStudioDataDao))
                {
                    tDatabase = (T) GetStudioDataDao(tConfig);
                }
                else if (typeof(T) == typeof(INWDCrucialInformationDao))
                {
                    tDatabase = (T) GetCrucialInformationDao(tConfig);
                }
                else if (typeof(T) == typeof(INWDProjectCredentialsDao))
                {
                    tDatabase = (T) GetProjectByEnvironmentDao(tConfig);
                }
                else if (typeof(T) == typeof(INWDProjectServiceKeyDao))
                {
                    tDatabase = (T) GetServiceKeyDao(tConfig);
                }
                else if (typeof(T) == typeof(INWDRelationshipDao))
                {
                    tDatabase = (T) GetRelationshipDao(tConfig);
                }
                if (tDatabase != null)
                {
                    rDaoList.Add(tDatabase);
                }
            }
            return rDaoList;
        }
        private static INWDAccountDao GetAccountDao(NWDDatabaseCredentials sConfig)
        {
            INWDAccountDao tDatabase = new NWDAccountDao_MySqlByReflexion(sConfig);
            switch (sConfig.Kind)
            {
                case NWDDatabaseKind.None:
                    break;
                case NWDDatabaseKind.Memory:
                    break;
                case NWDDatabaseKind.SqLite:
                    break;
                case NWDDatabaseKind.NoSql:
                    break;
                case NWDDatabaseKind.MariaDb:
                    tDatabase = new NWDAccountDao_MariaDbSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMariaDb:
                    tDatabase = new NWDAccountDao_MariaDbByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.MySql:
                    tDatabase = new NWDAccountDao_MySqlSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMysql:
                    tDatabase = new NWDAccountDao_MySqlByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.PostgreSql:
                    break;
            }
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE CURRENT TIME : "+ tDatabase.GetCurrentDatetime().ToUniversalTime());
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE new commit id test : "+ tDatabase.GenerateNewCommitId());
            return tDatabase;
        }
        
        private static INWDAccountServiceDao GetAccountServiceDao(NWDDatabaseCredentials sConfig)
        {
            INWDAccountServiceDao tDatabase = new NWDAccountServiceDao_MySqlByReflexion(sConfig);
            switch (sConfig.Kind)
            {
                case NWDDatabaseKind.None:
                    break;
                case NWDDatabaseKind.Memory:
                    break;
                case NWDDatabaseKind.SqLite:
                    break;
                case NWDDatabaseKind.NoSql:
                    break;
                case NWDDatabaseKind.MariaDb:
                    tDatabase = new NWDAccountServiceDao_MariaDbSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMariaDb:
                    tDatabase = new NWDAccountServiceDao_MariaDbByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.MySql:
                    tDatabase = new NWDAccountServiceDao_MySqlSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMysql:
                    tDatabase = new NWDAccountServiceDao_MySqlByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.PostgreSql:
                    break;
            }
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE CURRENT TIME : "+ tDatabase.GetCurrentDatetime().ToUniversalTime());
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE new commit id test : "+ tDatabase.GenerateNewCommitId());
            return tDatabase;
        }
        
        private static INWDAccountSignDao GetAccountSignDao(NWDDatabaseCredentials sConfig)
        {
            INWDAccountSignDao tDatabase = new NWDAccountSignDao_MySqlByReflexion(sConfig);
            switch (sConfig.Kind)
            {
                case NWDDatabaseKind.None:
                    break;
                case NWDDatabaseKind.Memory:
                    break;
                case NWDDatabaseKind.SqLite:
                    break;
                case NWDDatabaseKind.NoSql:
                    break;
                case NWDDatabaseKind.MariaDb:
                    tDatabase = new NWDAccountSignDao_MariaDbSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMariaDb:
                    tDatabase = new NWDAccountSignDao_MariaDbByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.MySql:
                    tDatabase = new NWDAccountSignDao_MySqlSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMysql:
                    tDatabase = new NWDAccountSignDao_MySqlByReflexion(sConfig);
                    break;
            }
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE CURRENT TIME : "+ tDatabase.GetCurrentDatetime().ToUniversalTime());
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE new commit id test : "+ tDatabase.GenerateNewCommitId());
            return tDatabase;
        }
        
        
        private static INWDAccountOrderDao GetAccountOrderDao(NWDDatabaseCredentials sConfig)
        {
            INWDAccountOrderDao tDatabase = new NWDAccountOrderDao_MySqlByReflexion(sConfig);
            switch (sConfig.Kind)
            {
                case NWDDatabaseKind.None:
                    break;
                case NWDDatabaseKind.Memory:
                    break;
                case NWDDatabaseKind.SqLite:
                    break;
                case NWDDatabaseKind.NoSql:
                    break;
                case NWDDatabaseKind.MariaDb:
                    tDatabase = new NWDAccountOrderDao_MariaDbSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMariaDb:
                    tDatabase = new NWDAccountOrderDao_MariaDbByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.MySql:
                    tDatabase = new NWDAccountOrderDao_MySqlSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMysql:
                    tDatabase = new NWDAccountOrderDao_MySqlByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.PostgreSql:
                    break;
            }
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE CURRENT TIME : "+ tDatabase.GetCurrentDatetime().ToUniversalTime());
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE new commit id test : "+ tDatabase.GenerateNewCommitId());
            return tDatabase;
        }
        private static INWDAccountInvoiceDao GetAccountInvoiceDao(NWDDatabaseCredentials sConfig)
        {
            INWDAccountInvoiceDao tDatabase = new NWDAccountInvoiceDao_MySqlByReflexion(sConfig);
            switch (sConfig.Kind)
            {
                case NWDDatabaseKind.None:
                    break;
                case NWDDatabaseKind.Memory:
                    break;
                case NWDDatabaseKind.SqLite:
                    break;
                case NWDDatabaseKind.NoSql:
                    break;
                case NWDDatabaseKind.MariaDb:
                    tDatabase = new NWDAccountInvoiceDao_MariaDbSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMariaDb:
                    tDatabase = new NWDAccountInvoiceDao_MariaDbByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.MySql:
                    tDatabase = new NWDAccountInvoiceDao_MySqlSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMysql:
                    tDatabase = new NWDAccountInvoiceDao_MySqlByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.PostgreSql:
                    break;
            }
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE CURRENT TIME : "+ tDatabase.GetCurrentDatetime().ToUniversalTime());
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE new commit id test : "+ tDatabase.GenerateNewCommitId());
            return tDatabase;
        }
        private static INWDAccountTokenDao GetAccountTokenDao(NWDDatabaseCredentials sConfig)
        {
            INWDAccountTokenDao tDatabase = new NWDAccountTokenDao_MySqlByReflexion(sConfig);
            switch (sConfig.Kind)
            {
                case NWDDatabaseKind.None:
                    break;
                case NWDDatabaseKind.Memory:
                    break;
                case NWDDatabaseKind.SqLite:
                    break;
                case NWDDatabaseKind.NoSql:
                    break;
                case NWDDatabaseKind.MariaDb:
                    tDatabase = new NWDAccountTokenDao_MariaDbSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMariaDb:
                    tDatabase = new NWDAccountTokenDao_MariaDbByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.MySql:
                    tDatabase = new NWDAccountTokenDao_MySqlSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMysql:
                    tDatabase = new NWDAccountTokenDao_MySqlByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.PostgreSql:
                    break;
            }
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE CURRENT TIME : "+ tDatabase.GetCurrentDatetime().ToUniversalTime());
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE new commit id test : "+ tDatabase.GenerateNewCommitId());
            return tDatabase;
        }
        private static INWDPlayerDataDao GetPlayerDataDao(NWDDatabaseCredentials sConfig)
        {
            INWDPlayerDataDao tDatabase = new NWDPlayerDataDao_MySqlByReflexion(sConfig);
            switch (sConfig.Kind)
            {
                case NWDDatabaseKind.None:
                    break;
                case NWDDatabaseKind.Memory:
                    break;
                case NWDDatabaseKind.SqLite:
                    break;
                case NWDDatabaseKind.NoSql:
                    break;
                case NWDDatabaseKind.MariaDb:
                    tDatabase = new NWDPlayerDataDao_MariaDbSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMariaDb:
                    tDatabase = new NWDPlayerDataDao_MariaDbByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.MySql:
                    tDatabase = new NWDPlayerDataDao_MySqlSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMysql:
                    tDatabase = new NWDPlayerDataDao_MySqlByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.PostgreSql:
                    break;
            }
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE CURRENT TIME : "+ tDatabase.GetCurrentDatetime().ToUniversalTime());
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE new commit id test : "+ tDatabase.GenerateNewCommitId());
            return tDatabase;
        }
        private static INWDStudioDataDao GetStudioDataDao(NWDDatabaseCredentials sConfig)
        {
            INWDStudioDataDao tDatabase = new NWDStudioDataDao_MySqlByReflexion(sConfig);
            switch (sConfig.Kind)
            {
                case NWDDatabaseKind.None:
                    break;
                case NWDDatabaseKind.Memory:
                    break;
                case NWDDatabaseKind.SqLite:
                    break;
                case NWDDatabaseKind.NoSql:
                    break;
                case NWDDatabaseKind.MariaDb:
                    tDatabase = new NWDStudioDataDao_MariaDbSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMariaDb:
                    tDatabase = new NWDStudioDataDao_MariaDbByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.MySql:
                    tDatabase = new NWDStudioDataDao_MySqlSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMysql:
                    tDatabase = new NWDStudioDataDao_MySqlByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.PostgreSql:
                    break;
            }
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE CURRENT TIME : "+ tDatabase.GetCurrentDatetime().ToUniversalTime());
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE new commit id test : "+ tDatabase.GenerateNewCommitId());
            return tDatabase;
        }
        private static INWDCrucialInformationDao GetCrucialInformationDao(NWDDatabaseCredentials sConfig)
        {
            INWDCrucialInformationDao tDatabase = new NWDCrucialInformationDao_MySqlByReflexion(sConfig);
            switch (sConfig.Kind)
            {
                case NWDDatabaseKind.None:
                    break;
                case NWDDatabaseKind.Memory:
                    break;
                case NWDDatabaseKind.SqLite:
                    break;
                case NWDDatabaseKind.NoSql:
                    break;
                case NWDDatabaseKind.MariaDb:
                    tDatabase = new NWDCrucialInformationDao_MariaDbSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMariaDb:
                    tDatabase = new NWDCrucialInformationDao_MariaDbByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.MySql:
                    tDatabase = new NWDCrucialInformationDao_MySqlSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMysql:
                    tDatabase = new NWDCrucialInformationDao_MySqlByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.PostgreSql:
                    break;
            }
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE CURRENT TIME : "+ tDatabase.GetCurrentDatetime().ToUniversalTime());
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE new commit id test : "+ tDatabase.GenerateNewCommitId());
            return tDatabase;
        }
        private static INWDProjectCredentialsDao GetProjectByEnvironmentDao(NWDDatabaseCredentials sConfig)
        {
            INWDProjectCredentialsDao tDatabase = new NWDProjectCredentialsDaoMySqlByReflexion(sConfig);
            switch (sConfig.Kind)
            {
                case NWDDatabaseKind.None:
                    break;
                case NWDDatabaseKind.Memory:
                    break;
                case NWDDatabaseKind.SqLite:
                    break;
                case NWDDatabaseKind.NoSql:
                    break;
                case NWDDatabaseKind.MariaDb:
                    tDatabase = new NWDProjectCredentialsDaoMariaDbSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMariaDb:
                    tDatabase = new NWDProjectCredentialsDaoMariaDbByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.MySql:
                    tDatabase = new NWDProjectCredentialsDaoMySqlSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMysql:
                    tDatabase = new NWDProjectCredentialsDaoMySqlByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.PostgreSql:
                    break;
            }
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE CURRENT TIME : "+ tDatabase.GetCurrentDatetime().ToUniversalTime());
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE new commit id test : "+ tDatabase.GenerateNewCommitId());
            return tDatabase;
        }
        private static INWDProjectServiceKeyDao GetServiceKeyDao(NWDDatabaseCredentials sConfig)
        {
            INWDProjectServiceKeyDao tDatabase = new NWDProjectServiceKeyDao_MySqlByReflexion(sConfig);
            switch (sConfig.Kind)
            {
                case NWDDatabaseKind.None:
                    break;
                case NWDDatabaseKind.Memory:
                    break;
                case NWDDatabaseKind.SqLite:
                    break;
                case NWDDatabaseKind.NoSql:
                    break;
                case NWDDatabaseKind.MariaDb:
                    tDatabase = new NWDProjectServiceKeyDao_MariaDbSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMariaDb:
                    tDatabase = new NWDProjectServiceKeyDao_MariaDbByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.MySql:
                    tDatabase = new NWDProjectServiceKeyDao_MySqlSpecific(sConfig);
                    break;
                case NWDDatabaseKind.ReflexionMysql:
                    tDatabase = new NWDProjectServiceKeyDao_MySqlByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.PostgreSql:
                    break;
            }
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE CURRENT TIME : "+ tDatabase.GetCurrentDatetime().ToUniversalTime());
            // NWDLogger.Trace(tDatabase.GetInfos() +", DATABASE new commit id test : "+ tDatabase.GenerateNewCommitId());
            return tDatabase;
        }

        private static INWDRelationshipDao GetRelationshipDao(NWDDatabaseCredentials sConfig)
        {
            INWDRelationshipDao tRelationship = new NWDRelationShipDao_MariaDbByReflexion(sConfig);
            
            switch (sConfig.Kind)
            {
                case NWDDatabaseKind.None:
                    break;
                case NWDDatabaseKind.Memory:
                    break;
                case NWDDatabaseKind.SqLite:
                    break;
                case NWDDatabaseKind.NoSql:
                    break;
                case NWDDatabaseKind.MariaDb:
                    /*tRelationship = new NWDProjectServiceKeyDao_MariaDbSpecific(sConfig);*/
                    // TODO
                    break;
                case NWDDatabaseKind.ReflexionMariaDb:
                    tRelationship = new NWDRelationShipDao_MariaDbByReflexion(sConfig);
                    break;
                case NWDDatabaseKind.MySql:
                    // tRelationship = new NWDProjectServiceKeyDao_MySqlSpecific(sConfig);
                    // TODO
                    break;
                case NWDDatabaseKind.ReflexionMysql:
                    // tRelationship = new NWDProjectServiceKeyDao_MySqlByReflexion(sConfig);
                    // TODO
                    break;
                case NWDDatabaseKind.PostgreSql:
                    break;
            }
            
            return tRelationship;
        }
    }
}