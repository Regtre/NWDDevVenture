using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using NWDDatabaseAccess;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Tools;

namespace NWDServerBack.MariaDb.ByReflexion
{
    public class NWDBackMariaDbByReflexion : INWDDaoGeneric
    {
        private readonly NWDDatabaseConnector _DbConnector;
        private readonly ushort _Range;
        private readonly string _Infos;

        private string TableName(NWDEnvironmentKind sEnvironment, Type sType)
        {
            return NWDTableName.GenerateTableName(sType, sEnvironment, _DbConnector.GetTableNamePrefix() + "_Reflex");
        }

        public NWDBackMariaDbByReflexion(NWDDatabaseCredentials sDatabaseCredentials)
        {
            _Infos = /*sDatabaseCredentials.Server + " " +*/ sDatabaseCredentials.Database;
            _DbConnector = new NWDDatabaseConnector(sDatabaseCredentials);
            _Range = sDatabaseCredentials.Range;
        }

        public bool TableExists<T>(NWDEnvironmentKind sEnvironment) where T : NWDBasicModel
        {
            bool rReturn = false;
            MySqlConnection tConn = _DbConnector.Connection();
            _DbConnector.Open(tConn);
            string tSql = "SELECT CASE WHEN EXISTS((select '['+SCHEMA_NAME(schema_id)+'].['+name+']' AS name FROM [" + _DbConnector.GetDatabaseName() + "].sys.tables WHERE name = '" + TableName(sEnvironment, typeof(T)) + "')) THEN 1 ELSE 0 END;";
            try
            {
                MySqlCommand tCmd = new MySqlCommand(tSql, tConn);
                rReturn = (int)tCmd.ExecuteScalar() == 1;
                tCmd.Dispose();
            }
            catch
            {
                rReturn = false;
            }
            _DbConnector.Close(tConn);
            return rReturn;
        }

        public DateTime GetCurrentDatetime()
        {
            const string C_CURRENT_DATE_TIME = "CurrentDateTime";
            DateTime rReturn = DateTime.UnixEpoch;
            MySqlConnection tConn = _DbConnector.Connection();
            _DbConnector.Open(tConn);
            string tSql = "SELECT CURRENT_TIMESTAMP() AS " + C_CURRENT_DATE_TIME + ";";
            MySqlCommand tCmd = new MySqlCommand(tSql, tConn);
            MySqlDataReader tRdr = tCmd.ExecuteReader();
            while (tRdr.Read())
            {
                rReturn = tRdr.GetDateTime(C_CURRENT_DATE_TIME);
            }

            tRdr.Close();
            tRdr.Dispose();
            tCmd.Dispose();
            _DbConnector.Close(tConn);
            return rReturn;
        }

        public Int64 GenerateNewCommitId()
        {
            return Random.Shared.NextInt64();
        }

        public void CreateTable<T>(NWDEnvironmentKind sEnvironment) where T : NWDBasicModel
        {
            Type tType = typeof(T);
            CreateTable(sEnvironment, tType);
        }

        private void CreateTable(NWDEnvironmentKind sEnvironment, Type sType)
        {
            MySqlConnection tConn = _DbConnector.Connection();
            _DbConnector.Open(tConn);
            StringBuilder tSqlTable = ConstructCreateTableQuery(sEnvironment, sType);
            try
            {
                MySqlCommand tCmdTable = new MySqlCommand(tSqlTable.ToString(), tConn);
                tCmdTable.ExecuteNonQuery();
                tCmdTable.Dispose();
            }
            catch (Exception tEx)
            {
                NWDLogger.Exception(tSqlTable.ToString(), tEx);
            }

            foreach (PropertyInfo tFieldInfo in sType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                                    BindingFlags.Instance |
                                                                    BindingFlags.FlattenHierarchy))
            {
                string tAdd = "ALTER TABLE `" + TableName(sEnvironment, sType) + "` ADD COLUMN IF NOT EXISTS `" + tFieldInfo.Name + "` " + PropertyInfoToSqlType(tFieldInfo) + ";";
                try
                {
                    MySqlCommand tCmdTableAdd = new MySqlCommand(tAdd, tConn);
                    tCmdTableAdd.ExecuteNonQuery();
                    tCmdTableAdd.Dispose();
                }
                catch (Exception tEx)
                {
                    NWDLogger.Exception(tEx);
                }

                string tAlter = "ALTER TABLE `" + TableName(sEnvironment, sType) + "` MODIFY IF EXISTS `" + tFieldInfo.Name + "` " + PropertyInfoToSqlType(tFieldInfo) + ";";
                try
                {
                    MySqlCommand tCmdTableAlter = new MySqlCommand(tAlter, tConn);
                    tCmdTableAlter.ExecuteNonQuery();
                    tCmdTableAlter.Dispose();
                }
                catch (Exception tEx)
                {
                    NWDLogger.Exception(tAlter, tEx);
                }
            }

            _DbConnector.Close(tConn);
        }

        public string FingerPrintName<T>(NWDEnvironmentKind sEnvironment) where T : NWDBasicModel
        {
            Type tType = typeof(T);
            return TableName(sEnvironment, tType);
        }

        public string FingerPrint<T>(NWDEnvironmentKind sEnvironment) where T : NWDBasicModel
        {
            Type tType = typeof(T);
            return NWDSecurityTools.GenerateSha(ConstructCreateTableQuery(sEnvironment, tType).ToString());
        }

        private StringBuilder ConstructCreateTableQuery(NWDEnvironmentKind sEnvironment, Type sType)
        {
            //NWDLogger.WriteLine("Try to create and alter " + TableName(sEnvironment, sType));
            StringBuilder tSqlTable = new StringBuilder();
            tSqlTable.Append("CREATE TABLE IF NOT EXISTS `" + TableName(sEnvironment, sType) + "` (");
            List<PropertyInfo> tFieldInfoList = new List<PropertyInfo>();
            if (tFieldInfoList == null) throw new ArgumentNullException(nameof(tFieldInfoList));
            List<string> tFieldInfoSql = new List<string>();
            if (tFieldInfoSql == null) throw new ArgumentNullException(nameof(tFieldInfoSql));
            foreach (PropertyInfo tFieldInfo in sType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                                    BindingFlags.Instance |
                                                                    BindingFlags.FlattenHierarchy))
            {
                tFieldInfoList.Add(tFieldInfo);
                tFieldInfoSql.Add("`" + tFieldInfo.Name + "` " + PropertyInfoToSqlType(tFieldInfo));
                tSqlTable.Append("`" + tFieldInfo.Name + "` " + PropertyInfoToSqlType(tFieldInfo) + ", ");
            }

            tSqlTable.Append(" PRIMARY KEY(`" + nameof(NWDBasicModel.RowId) + "`) ");
            tSqlTable.Append(", KEY `RefByProject` (`" + nameof(NWDBasicModel.ProjectId) + "`, `" +
                             nameof(NWDBasicModel.Reference) + "`) ");

            tSqlTable.Append(") ENGINE = MyISAM DEFAULT CHARSET = utf8 COMMENT = '" + sType.Name + " table';");
            return tSqlTable;
        }

        public void DeleteTable<T>(NWDEnvironmentKind sEnvironment) where T : NWDBasicModel
        {
            Type tType = typeof(T);
            DeleteTable(sEnvironment, tType);
        }

        private void DeleteTable(NWDEnvironmentKind sEnvironment, Type sType)
        {
            MySqlConnection tConn = _DbConnector.Connection();
            _DbConnector.Open(tConn);
            StringBuilder tSqlTable = ConstructDeleteTableQuery(sEnvironment, sType);
            try
            {
                MySqlCommand tCmdTable = new MySqlCommand(tSqlTable.ToString(), tConn);
                tCmdTable.ExecuteNonQuery();
                tCmdTable.Dispose();
            }
            catch (Exception tEx)
            {
                NWDLogger.Exception(tSqlTable.ToString(), tEx);
            }

            _DbConnector.Close(tConn);
        }

        private StringBuilder ConstructDeleteTableQuery(NWDEnvironmentKind sEnvironment, Type sType)
        {
            StringBuilder tSqlTable = new StringBuilder();
            tSqlTable.Append("DROP TABLE `" + TableName(sEnvironment, sType) + "`;");
            return tSqlTable;
        }

        public bool Delete<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sObject) where T : NWDBasicModel
        {
            return Delete<T>(sEnvironment, sProjectId, sObject.Reference);
        }

        public T Update<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sModel
            , Dictionary<string, string> sDictionary = null, string sAndWhere = "") where T : NWDBasicModel
        {
            // NWDLogger.WriteLine(nameof(Update) + "()");
            Type tType = typeof(T);
            //Serialize object in JSON
            // SerializeObjectToJson(sModel, tType, out PropertyInfo tFieldInfoJSON, out PropertyInfo tFieldInfoIntegrity);
            //end serialize object
            //end
            sModel.ProjectId = sProjectId;
            List<PropertyInfo> tFieldInfoList = new List<PropertyInfo>();
            List<string> tFieldInfoSql = new List<string>();
            foreach (PropertyInfo tFieldInfo in tType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                                    BindingFlags.Instance |
                                                                    BindingFlags.FlattenHierarchy))
            {
                tFieldInfoList.Add(tFieldInfo);
                if (tFieldInfo.Name != nameof(NWDBasicModel.RowId))
                {
                    tFieldInfoSql.Add("`" + tFieldInfo.Name + "` = @s" + tFieldInfo.Name + "");
                }
            }

            List<string> tWhere = new List<string>();
            tWhere.Add("`" + nameof(NWDBasicModel.ProjectId) + "` = '" + sProjectId + "' ");
            if (sDictionary != null)
            {
                foreach (KeyValuePair<string, string> tKv in sDictionary)
                {
                    tWhere.Add("`" + tKv.Key + "` = @s" + tKv.Key + "");
                }
            }

            MySqlConnection tConn = _DbConnector.Connection();
            _DbConnector.Open(tConn);
            try
            {
                string tSql = "UPDATE `" + TableName(sEnvironment, tType) + "` SET " + string.Join(",", tFieldInfoSql);
                if (tWhere.Count > 0)
                {
                    tSql = tSql + " WHERE " + string.Join(" AND ", tWhere) + " " + sAndWhere + ";";
                }
                else
                {
                    if (string.IsNullOrEmpty(sAndWhere) == false)
                    {
                        tSql = tSql + " WHERE 1=1 AND " + sAndWhere + ";";
                    }
                }
                // NWDLogger.WriteLine(tSql);
                MySqlCommand tCmd = new MySqlCommand(tSql, tConn);
                foreach (PropertyInfo tFieldInfo in tFieldInfoList)
                {
                    Type tTypeOfThis = tFieldInfo.PropertyType;
                    if (tTypeOfThis.IsEnum)
                    {
                        tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sModel));
                    }
                    else
                    {
                        if (tTypeOfThis == typeof(Int16))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sModel));
                        }
                        else if (tTypeOfThis == typeof(int) || tTypeOfThis == typeof(Int32))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sModel));
                        }
                        else if (tTypeOfThis == typeof(long) || tTypeOfThis == typeof(Int64))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sModel));
                        }
                        else if (tTypeOfThis == typeof(UInt16))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sModel));
                        }
                        else if (tTypeOfThis == typeof(uint) || tTypeOfThis == typeof(UInt32))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sModel));
                        }
                        else if (tTypeOfThis == typeof(ulong) || tTypeOfThis == typeof(UInt64))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sModel));
                        }
                        else if (tTypeOfThis == typeof(float))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sModel));
                        }
                        else if (tTypeOfThis == typeof(double) || tTypeOfThis == typeof(Double))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sModel));
                        }
                        else if (tTypeOfThis == typeof(bool))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sModel));
                        }
                        else if (tTypeOfThis == typeof(string) || tTypeOfThis == typeof(String))
                        {
                            string tValue = (string)tFieldInfo.GetValue(sModel);
                            if (tValue == null)
                            {
                                tValue = string.Empty;
                            }

                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tValue);
                        }
                        else if (IsGenericReference(tTypeOfThis))
                        {
                            string tStringValue = string.Empty;
                            Object tValue = tFieldInfo.GetValue(sModel);
                            if (tValue != null)
                            {
                                tStringValue = (string)tValue.GetType()
                                    .GetProperty(nameof(NWDReference<NWDBasicModel>.Reference))?.GetValue(tValue).ToString();
                            }

                            if (string.IsNullOrEmpty(tStringValue))
                            {
                                tStringValue = string.Empty;
                            }

                            // NWDLogger.WriteLine("Record "+nameof(NWDReference<NWDBasicModel>)+" found : value is " + tStringValue);
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tStringValue);
                        }
                        else if (IsGenericReferencesArray(tTypeOfThis))
                        {
                            string tStringValue = string.Empty;
                            Object tValue = tFieldInfo.GetValue(sModel);
                            if (tValue != null)
                            {
                                tStringValue = (string)tValue.GetType()
                                    .GetProperty(nameof(NWDReferencesArray<NWDBasicModel>.References))
                                    ?.GetValue(tValue);
                            }

                            if (string.IsNullOrEmpty(tStringValue))
                            {
                                tStringValue = string.Empty;
                            }

                            // NWDLogger.WriteLine("Record "+nameof(NWDReferencesArray<NWDBasicModel>)+" found : value is " + tStringValue);
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tStringValue);
                        } else if (tTypeOfThis == typeof(IList<NWDBasicModel>))
                        {
                            string tStringValue = string.Empty;
                            IList<NWDBasicModel> tList = new List<NWDBasicModel>(); 

                            Object tValue = tFieldInfo.GetValue(sModel);
                            if (tValue != null)
                            {
                                tList = (IList<NWDBasicModel>)tValue.GetType()
                                    .GetProperty(nameof(IList<NWDBasicModel>))
                                    ?.GetValue(tValue);
                                if (tList != null)
                                {
                                    tStringValue = tList.Select(sItem => sItem.Reference.ToString())
                                        .Aggregate((s1, s2) => s1 + NWDConstants.kFieldSeparatorA + s2);
                                }
                            }

                            if (string.IsNullOrEmpty(tStringValue))
                            {
                                tStringValue = string.Empty;
                            }
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tStringValue);

                        }
                        else
                        {
                            //TODO : add json for object!
                            //JSON ? cmd.Parameters.AddWithValue("@s" + tFieldInfo.ServiceId, tFieldInfo.GetValue(sModel));
                        }
                    }
                }

                tCmd.ExecuteNonQuery();
                tCmd.Dispose();
            }
            catch (Exception tEx)
            {
                NWDLogger.Exception(tEx);
            }

            _DbConnector.Close(tConn);
            return sModel;
        }

        private static void SerializeObjectToJson<T>(T sObject, Type sType, out PropertyInfo sFieldInfoJson,
            out PropertyInfo sFieldInfoIntegrity) where T : NWDBasicModel
        {
            sFieldInfoJson = sType.GetProperty("JSON", BindingFlags.NonPublic | BindingFlags.Instance);
            sFieldInfoIntegrity = sType.GetProperty("Integrity", BindingFlags.NonPublic | BindingFlags.Instance);
            if (sFieldInfoJson != null && sFieldInfoIntegrity != null)
            {
                sFieldInfoJson.SetValue(sObject, string.Empty);
                sFieldInfoIntegrity.SetValue(sObject, string.Empty);
                string tJson = JsonConvert.SerializeObject(sObject);
                sFieldInfoJson.SetValue(sObject, tJson);
                //tFieldInfoIntegrity.SetValue(sModel, ToolBox.Integrity(tJson));
            }
        }

        public T Record<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sObject, bool sRangeDependent,
            bool sWithNewReference) where T : NWDBasicModel
        {
            // NWDLogger.WriteLine(nameof(Record) + "()");
            T rReturn = sObject;
            Type tType = typeof(T);
            if (sWithNewReference)
            {
                sObject.Reference = NewValidReference(sEnvironment, sProjectId, tType, sRangeDependent);
            }

            sObject.ProjectId = sProjectId;
            /*
            //Serialize object in JSON
            PropertyInfo tFieldInfoJSON = tType.GetProperty("JSON", BindingFlags.NonPublic | BindingFlags.Instance);
            PropertyInfo tFieldInfoIntegrity = tType.GetProperty("Integrity", BindingFlags.NonPublic | BindingFlags.Instance);
            if (tFieldInfoJSON != null && tFieldInfoIntegrity != null)
            {
                tFieldInfoJSON.SetValue(sModel, string.Empty);
                tFieldInfoIntegrity.SetValue(sModel, string.Empty);
                string tJson = JsonConvert.SerializeObject(sModel);
                tFieldInfoJSON.SetValue(sModel, tJson);
                tFieldInfoIntegrity.SetValue(sModel, ToolBox.Integrity(tJson));
            }*/
            //end serialize object
            // end
            List<PropertyInfo> tFieldInfoList = new List<PropertyInfo>();
            List<string> tFieldInfoSql = new List<string>();
            List<string> tSFieldInfoSql = new List<string>();
            foreach (PropertyInfo tFieldInfo in tType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                                    BindingFlags.Instance |
                                                                    BindingFlags.FlattenHierarchy))
            {
                tFieldInfoList.Add(tFieldInfo);
                tFieldInfoSql.Add(" `" + tFieldInfo.Name + "`");
                tSFieldInfoSql.Add(" @s" + tFieldInfo.Name + "");
            }

            MySqlConnection tConn = _DbConnector.Connection();
            _DbConnector.Open(tConn);
            try
            {
                string tSql = "INSERT INTO `" + TableName(sEnvironment, tType) + "` (" +
                              string.Join(",", tFieldInfoSql) + ") VALUES (" + string.Join(",", tSFieldInfoSql) + ");";
                // NWDLogger.WriteLine(tSql);
                MySqlCommand tCmd = new MySqlCommand(tSql, tConn);
                foreach (PropertyInfo tFieldInfo in tFieldInfoList)
                {
                    Type tTypeOfThis = tFieldInfo.PropertyType;
                    if (tTypeOfThis.IsEnum)
                    {
                        tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sObject));
                    }
                    else
                    {
                        if (tTypeOfThis == typeof(Int16))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sObject));
                        }
                        else if (tTypeOfThis == typeof(int) || tTypeOfThis == typeof(Int32))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sObject));
                        }
                        else if (tTypeOfThis == typeof(long) || tTypeOfThis == typeof(Int64))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sObject));
                        }
                        else if (tTypeOfThis == typeof(UInt16))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sObject));
                        }
                        else if (tTypeOfThis == typeof(uint) || tTypeOfThis == typeof(UInt32))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sObject));
                        }
                        else if (tTypeOfThis == typeof(ulong) || tTypeOfThis == typeof(UInt64))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sObject));
                        }
                        else if (tTypeOfThis == typeof(float))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sObject));
                        }
                        else if (tTypeOfThis == typeof(double) || tTypeOfThis == typeof(Double))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sObject));
                        }
                        else if (tTypeOfThis == typeof(bool))
                        {
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tFieldInfo.GetValue(sObject));
                        }
                        else if (tTypeOfThis == typeof(string) || tTypeOfThis == typeof(String))
                        {
                            string tValue = (string)tFieldInfo.GetValue(sObject);
                            if (tValue == null)
                            {
                                tValue = string.Empty;
                            }

                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tValue);
                        }
                        else if (IsGenericReference(tTypeOfThis))
                        {
                            string tStringValue = string.Empty;
                            Object tValue = tFieldInfo.GetValue(sObject);
                            if (tValue != null)
                            {
                                tStringValue = (string)tValue.GetType()
                                    .GetProperty(nameof(NWDReference<NWDBasicModel>.Reference))?.GetValue(tValue).ToString();
                            }

                            if (string.IsNullOrEmpty(tStringValue))
                            {
                                tStringValue = string.Empty;
                            }

                            // NWDLogger.WriteLine("Record "+nameof(NWDReference<NWDBasicModel>)+" found : value is " + tStringValue);
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tStringValue);
                        }
                        else if (IsGenericReferencesArray(tTypeOfThis))
                        {
                            string tStringValue = string.Empty;
                            object tValue = tFieldInfo.GetValue(sObject);
                            if (tValue != null)
                            {
                                tStringValue = (string)tValue.GetType()
                                    .GetProperty(nameof(NWDReferencesArray<NWDBasicModel>.References))
                                    ?.GetValue(tValue);
                            }

                            if (string.IsNullOrEmpty(tStringValue))
                            {
                                tStringValue = string.Empty;
                            }

                            // NWDLogger.WriteLine("Record "+nameof(NWDReferencesArray<NWDBasicModel>)+" found : value is " + tStringValue);
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tStringValue);
                        }
                        else if (tTypeOfThis == typeof(IList<NWDBasicModel>))
                        {
                            string tStringValue = string.Empty;
                            IList<NWDBasicModel> tList = new List<NWDBasicModel>();

                            Object tValue = tFieldInfo.GetValue(sObject);
                            if (tValue != null)
                            {
                                tList = (IList<NWDBasicModel>)tValue.GetType()
                                    .GetProperty(nameof(IList<NWDBasicModel>))
                                    ?.GetValue(tValue);
                                tStringValue = tList.Select(sItem => sItem.Reference.ToString()).Aggregate((s1, s2) => s1 + NWDConstants.kFieldSeparatorA + s2);
                                
                            }
                            tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tStringValue);

                        }

                        else
                        {
                            //TODO : add json for object!
                            //JSON ?  cmd.Parameters.AddWithValue("@s" + tFieldInfo.ServiceId, tFieldInfo.GetValue(sModel));
                        }
                    }
                }

                tCmd.ExecuteNonQuery();
                tCmd.Dispose();
            }
            catch (Exception tEx)
            {
                NWDLogger.Exception(tEx);
            }

            _DbConnector.Close(tConn);
            return rReturn;
        }

        public T GetByReference<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, string sReference) where T : NWDBasicModel
        {
            T rReturn = null;
            List<T> tList = GetBy<T>(sEnvironment, sProjectId,
                new Dictionary<string, string>() { { nameof(NWDBasicModel.Reference), sReference } });
            if (tList.Count > 0)
            {
                rReturn = tList[0];
            }

            return rReturn;
        }

        public T GetFirstBy<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary, string sAndWhere = "")
            where T : NWDBasicModel
        {
            T rReturn = null;
            List<T> tList = GetBy<T>(sEnvironment, sProjectId, sDictionary, sAndWhere);
            if (tList.Count > 0)
            {
                rReturn = tList[0];
            }

            return rReturn;
        }

        public List<T> GetBy<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary = null,
            string sAndWhere = "") where T : NWDBasicModel
        {
            List<T> rReturn = new List<T>();
            Type tType = typeof(T);
            List<PropertyInfo> tFieldInfoList = new List<PropertyInfo>();
            foreach (PropertyInfo tFieldInfo in tType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                                    BindingFlags.Instance |
                                                                    BindingFlags.FlattenHierarchy))
            {
                tFieldInfoList.Add(tFieldInfo);
            }

            MySqlConnection tConn = _DbConnector.Connection();
            _DbConnector.Open(tConn);
            try
            {
                List<string> tWhere = new List<string>();
                tWhere.Add("`" + nameof(NWDBasicModel.ProjectId) + "` = '" + sProjectId + "' ");
                if (sDictionary != null)
                {
                    foreach (KeyValuePair<string, string> tKv in sDictionary)
                    {
                        tWhere.Add("`" + tKv.Key + "` = @s" + tKv.Key + "");
                    }
                }

                string tSql = "SELECT * FROM `" + TableName(sEnvironment, tType) + "` ";
                if (tWhere.Count > 0)
                {
                    tSql = tSql + " WHERE " + string.Join(" AND ", tWhere) + " " + sAndWhere + ";";
                }
                else
                {
                    if (string.IsNullOrEmpty(sAndWhere) == false)
                    {
                        tSql = tSql + " WHERE 1=1 AND " + sAndWhere + ";";
                    }
                }

                // NWDLogger.WriteLine(tSql);
                MySqlCommand tCmd = new MySqlCommand(tSql, tConn);
                if (sDictionary != null)
                {
                    foreach (KeyValuePair<string, string> tKv in sDictionary)
                    {
                        tCmd.Parameters.AddWithValue("@s" + tKv.Key, tKv.Value);
                        //NWDLogger.WriteLine("Key:" + tKv.Key);
                        //NWDLogger.WriteLine("Value: "+tKv.Value);
                    }
                }

                // NWDLogger.WriteLine("Executing query "+ tSql);
                MySqlDataReader tRdr = tCmd.ExecuteReader();
                while (tRdr.Read())
                {
                    // NWDLogger.WriteLine("Find reference " + tRdr.GetString(nameof(NWDBasicModel.RowId)));
                    // NWDLogger.WriteLine("found one row in table");
                    T tObject = (T)Activator.CreateInstance(tType);
                    foreach (PropertyInfo tFieldInfo in tFieldInfoList)
                    {
                        // NWDLogger.WriteLine(tFieldInfo.ServiceId);
                        Type tTypeOfThis = tFieldInfo.PropertyType;
                        if (tTypeOfThis.IsEnum)
                        {
                            string tEnumValue = tRdr.GetString(tFieldInfo.Name);
                            if (string.IsNullOrEmpty(tEnumValue) == false)
                            {
                                tFieldInfo.SetValue(tObject, Enum.Parse(tFieldInfo.PropertyType, tEnumValue, true),
                                    null);
                            }
                        }
                        else
                        {
                            if (tTypeOfThis == typeof(Int16))
                            {
                                tFieldInfo.SetValue(tObject, tRdr.GetInt16(tFieldInfo.Name));
                            }
                            else if (tTypeOfThis == typeof(int) || tTypeOfThis == typeof(Int32))
                            {
                                tFieldInfo.SetValue(tObject, tRdr.GetInt32(tFieldInfo.Name));
                            }
                            else if (tTypeOfThis == typeof(long) || tTypeOfThis == typeof(Int64))
                            {
                                tFieldInfo.SetValue(tObject, tRdr.GetInt64(tFieldInfo.Name));
                            }
                            else if (tTypeOfThis == typeof(UInt16))
                            {
                                tFieldInfo.SetValue(tObject, tRdr.GetUInt16(tFieldInfo.Name));
                            }
                            else if (tTypeOfThis == typeof(uint) || tTypeOfThis == typeof(UInt32))
                            {
                                tFieldInfo.SetValue(tObject, tRdr.GetUInt32(tFieldInfo.Name));
                            }
                            else if (tTypeOfThis == typeof(ulong) || tTypeOfThis == typeof(UInt64))
                            {
                                tFieldInfo.SetValue(tObject, tRdr.GetUInt64(tFieldInfo.Name));
                            }
                            else if (tTypeOfThis == typeof(float))
                            {
                                tFieldInfo.SetValue(tObject, tRdr.GetFloat(tFieldInfo.Name));
                            }
                            else if (tTypeOfThis == typeof(double) || tTypeOfThis == typeof(Double))
                            {
                                tFieldInfo.SetValue(tObject, tRdr.GetDouble(tFieldInfo.Name));
                            }
                            else if (tTypeOfThis == typeof(bool))
                            {
                                tFieldInfo.SetValue(tObject, tRdr.GetBoolean(tFieldInfo.Name));
                            }
                            else if (tTypeOfThis == typeof(string) || tTypeOfThis == typeof(String))
                            {
                                string tValue = tRdr.GetString(tFieldInfo.Name);
                                if (string.IsNullOrEmpty(tValue))
                                {
                                    tValue = string.Empty;
                                }

                                tFieldInfo.SetValue(tObject, tValue);
                            }
                            else if (IsGenericReference(tTypeOfThis))
                            {
                                string tValue = tRdr.GetString(tFieldInfo.Name);
                                if (string.IsNullOrEmpty(tValue))
                                {
                                    tValue = string.Empty;
                                }

                                //TODO : optimize and fix that
                                Object tValueObject = tFieldInfo.GetValue(tObject);
                                ulong tValueToULong = 0;
                                ulong.TryParse(tValue, out tValueToULong);
                                tValueObject.GetType().GetProperty(nameof(NWDReference<NWDBasicModel>.Reference))
                                    ?.SetValue(tValueObject, tValueToULong);
                                tFieldInfo.SetValue(tObject, tValueObject);
                            }
                            else if (IsGenericReferencesArray(tTypeOfThis))
                            {
                                string tValue = tRdr.GetString(tFieldInfo.Name);
                                if (string.IsNullOrEmpty(tValue))
                                {
                                    tValue = string.Empty;
                                }

                                //TODO : optimize and fix that
                                Object tValueObject = tFieldInfo.GetValue(tObject);
                                tValueObject.GetType().GetProperty(nameof(NWDReferencesArray<NWDBasicModel>.References))
                                    ?.SetValue(tValueObject, tValue);
                            }
                            else if (tTypeOfThis == typeof(IList<>))
                            {
                                string tValue = tRdr.GetString(tFieldInfo.Name);
                                if (string.IsNullOrEmpty(tValue))
                                {
                                    tValue = string.Empty;
                                }
                               
                                // Get Method GetByReference by Reflexion so you can call it with generic type in variable
                                Type tTypeOfList = tTypeOfThis.GetGenericArguments().First();
                                MethodInfo tMethod = typeof(NWDBackMariaDbByReflexion).GetMethod(nameof(NWDBackMariaDbByReflexion.GetByReference));
                                MethodInfo tMethodTyped = tMethod.MakeGenericMethod(tTypeOfList);
                                
                                IList<NWDBasicModel> tResult = tValue.
                                    Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries).
                                    ToList().
                                    Select(sReference => tMethodTyped.
                                        Invoke(new { sEnvironment, sProjectId, tReference = sReference }, null) as NWDBasicModel).
                                    ToList();


                                object tValueObject = tFieldInfo.GetValue(tObject);
                                tValueObject.GetType().GetProperty(nameof(IList<NWDBasicModel>))
                                    ?.SetValue(tValueObject,tResult);
                            }
                            
                            else
                            {
                                //JSON ?
                            }
                        }
                    }

                    rReturn.Add(tObject);
                }

                tRdr.Close();
                tRdr.Dispose();
                tCmd.Dispose();
            }
            catch (Exception tEx)
            {
                NWDLogger.Exception(tEx);
            }

            _DbConnector.Close(tConn);
            return rReturn;
        }

        public ushort GetRange()
        {
            return _Range;
        }

        public int CountBy<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "") where T : NWDBasicModel
        {
            int rReturn = 0;
            Type tType = typeof(T);
            MySqlConnection tConn = _DbConnector.Connection();
            _DbConnector.Open(tConn);
            try
            {
                List<string> tWhere = new List<string>();
                tWhere.Add("`" + nameof(NWDBasicModel.ProjectId) + "` = " + sProjectId + " ");
                if (sDictionary != null)
                {
                    foreach (KeyValuePair<string, string> tKv in sDictionary)
                    {
                        tWhere.Add("`" + tKv.Key + "` = @s" + tKv.Key + "");
                    }
                }

                string tSql = "SELECT COUNT(`" + nameof(NWDBasicModel.RowId) + "`) FROM `" +
                              TableName(sEnvironment, tType) + "` WHERE " + string.Join(" AND ", tWhere) + sAndWhere +
                              ";";
                // NWDLogger.WriteLine(tSql);
                MySqlCommand tCmd = new MySqlCommand(tSql, tConn);
                if (sDictionary != null)
                {
                    foreach (KeyValuePair<string, string> tKv in sDictionary)
                    {
                        tCmd.Parameters.AddWithValue("@s" + tKv.Key, tKv.Value);
                    }
                }

                MySqlDataReader tRdr = tCmd.ExecuteReader();
                while (tRdr.Read())
                {
                    rReturn = tRdr.GetInt32(0);
                }

                tRdr.Close();
                tRdr.Dispose();
                tCmd.Dispose();
            }
            catch (Exception tEx)
            {
                NWDLogger.Exception(tEx);
            }

            _DbConnector.Close(tConn);
            return rReturn;
        }

        public string RandomStringNumber(int sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "0123456789";
            int tCharLenght = cChars.Length;
            Random tRand = new Random();
            while (rReturn.Length < sLength)
            {
                int tR = tRand.Next(0, tCharLenght);
                rReturn.Append(cChars[tR]);
            }

            return rReturn.ToString();
        }

        private ulong NewReference(bool sRangeDependent)
        {
            ulong rReturn = 0;
            if (sRangeDependent)
            {
                ulong tRef = NWDRandom.UnsignedLongNumeric(NWDConstants.K_REFERENCE_SIZE);
                rReturn = tRef + (ulong)_Range * (ulong)(NWDConstants.K_REFERENCE_AREA_RANGE) + (ulong)(NWDConstants.K_REFERENCE_AREA_GLOBAL);
            }
            else
            {
                rReturn = NWDRandom.UnsignedLongNumeric(NWDConstants.K_REFERENCE_UNIQUE_SIZE);
            }

            return rReturn;
        }

        public ulong NewValidReference(NWDEnvironmentKind sEnvironment, ulong sProjectId, Type sType, bool sRangeDependent)
        {
            ulong rReturn = NewReference(sRangeDependent);
            while (TestIfReferenceExists(sEnvironment, sProjectId, sType, rReturn))
            {
                rReturn = NewReference(sRangeDependent);
            }

            return rReturn;
        }

        private bool TestIfReferenceExists(NWDEnvironmentKind sEnvironment, ulong sProjectId, Type sType, ulong sReference)
        {
            bool rReturn = false;
            MySqlConnection tConn = _DbConnector.Connection();
            _DbConnector.Open(tConn);
            try
            {
                string tSql = "SELECT `" + nameof(NWDBasicModel.Reference) + "` FROM `" +
                              TableName(sEnvironment, sType) + "` WHERE " +
                              "`" + nameof(NWDBasicModel.Reference) + "` = @s" + nameof(NWDBasicModel.Reference) +
                              " AND " +
                              "`" + nameof(NWDBasicModel.ProjectId) + "` = @s" + nameof(NWDBasicModel.ProjectId) + " ;";
                MySqlCommand tCmd = new MySqlCommand(tSql, tConn);
                tCmd.Parameters.AddWithValue("@s" + nameof(NWDBasicModel.Reference) + "", sReference);
                tCmd.Parameters.AddWithValue("@s" + nameof(NWDBasicModel.ProjectId) + "", sProjectId);
                MySqlDataReader tRdr = tCmd.ExecuteReader();
                while (tRdr.Read())
                {
                    rReturn = true;
                }

                tRdr.Close();
                tRdr.Dispose();
                tCmd.Dispose();
            }
            catch (Exception tEx)
            {
                NWDLogger.Exception(tEx);
            }

            _DbConnector.Close(tConn);
            return rReturn;
        }

        private static string PropertyInfoToSqlType(PropertyInfo sPropertyInfo)
        {
            if (sPropertyInfo.Name == nameof(NWDBasicModel.RowId))
            {
                return " BIGINT UNSIGNED NOT NULL AUTO_INCREMENT";
            }

            string rReturn = " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL DEFAULT '' ";

            Type tTypeOfThis = sPropertyInfo.PropertyType;
            if (tTypeOfThis.IsEnum)
            {
                //rReturn = " VARCHAR(256) NOT NULL DEFAULT ''";
                rReturn = " VARCHAR(256) NOT NULL";
            }
            else
            {
                if (tTypeOfThis == typeof(Int16))
                {
                    rReturn = " SMALLINT NOT NULL DEFAULT 0";
                }
                else if (tTypeOfThis == typeof(UInt16))
                {
                    rReturn = " SMALLINT UNSIGNED NOT NULL DEFAULT 0";
                }
                else if (tTypeOfThis == typeof(int) || tTypeOfThis == typeof(Int32))
                {
                    rReturn = " INT NOT NULL DEFAULT 0";
                }
                else if (tTypeOfThis == typeof(uint) || tTypeOfThis == typeof(UInt32))
                {
                    rReturn = " INT UNSIGNED NOT NULL DEFAULT 0";
                }
                else if (tTypeOfThis == typeof(long) || tTypeOfThis == typeof(Int64))
                {
                    rReturn = " BIGINT NOT NULL DEFAULT 0";
                }
                else if (tTypeOfThis == typeof(ulong) || tTypeOfThis == typeof(UInt64))
                {
                    rReturn = " BIGINT UNSIGNED NOT NULL DEFAULT 0";
                }
                else if (tTypeOfThis == typeof(float))
                {
                    rReturn = " FLOAT NOT NULL DEFAULT 0";
                }
                else if (tTypeOfThis == typeof(double) || tTypeOfThis == typeof(Double))
                {
                    rReturn = " DOUBLE NOT NULL DEFAULT 0";
                }
                else if (tTypeOfThis == typeof(bool))
                {
                    rReturn = " BOOLEAN NOT NULL DEFAULT 0";
                }
                else if (tTypeOfThis == typeof(string) || tTypeOfThis == typeof(String))
                {
                    rReturn = " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL DEFAULT '' ";
                }
                else if (IsGenericReference(tTypeOfThis))
                {
                    // NWDLogger.WriteLine("IsSubclassOf "+nameof(NWDReference<NWDBasicModel>)+" ");
                    rReturn = " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL DEFAULT '' ";
                }
                else if (IsGenericReferencesArray(tTypeOfThis) || tTypeOfThis == typeof(IList<>))
                {
                    // NWDLogger.WriteLine("IsSubclassOf "+nameof(NWDReferencesArray<NWDBasicModel>)+" ");
                    rReturn = " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL DEFAULT '' ";
                }
                else
                {
                    //Json ?
                }
            }

            return rReturn;
        }

        private static bool IsGenericReference(Type sType)
        {
            return (sType.IsGenericType && (sType.GetGenericTypeDefinition() == typeof(NWDReference<>)));
        }

        private static bool IsGenericReferencesArray(Type sType)
        {
            return (sType.IsGenericType && (sType.GetGenericTypeDefinition() == typeof(NWDReferencesArray<>)));
        }

        public bool TestConnexion()
        {
            bool rReturn = false;
            MySqlConnection tConn = _DbConnector.Connection();
            _DbConnector.Open(tConn);
            if (tConn.State == ConnectionState.Open)
            {
                rReturn = true;
            }

            _DbConnector.Close(tConn);
            return rReturn;
        }

        public string GetInfos()
        {
            return _Infos;
        }

        public T Create<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sModel, bool sRangeDependent, bool sWithNewReference)
            where T : NWDBasicModel
        {
            Record<T>(sEnvironment, sProjectId, sModel, sRangeDependent, sWithNewReference);
            return sModel;
        }

        public bool Delete<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference) where T : NWDBasicModel
        {
            bool rReturn = false;
            Type tType = typeof(T);
            MySqlConnection tConn = _DbConnector.Connection();
            _DbConnector.Open(tConn);
            try
            {
                string tSql = "DELETE FROM `" + TableName(sEnvironment, tType) + "` WHERE " +
                              "`" + nameof(NWDBasicModel.Reference) + "` = @s" + nameof(NWDBasicModel.Reference) +
                              " AND " +
                              "`" + nameof(NWDBasicModel.ProjectId) + "` = @s" + nameof(NWDBasicModel.ProjectId) + " " +
                              ";";
                // NWDLogger.WriteLine(tSql);
                MySqlCommand tCmd = new MySqlCommand(tSql, tConn);
                tCmd.Parameters.AddWithValue("@s" + nameof(NWDBasicModel.Reference), sReference);
                tCmd.Parameters.AddWithValue("@s" + nameof(NWDBasicModel.ProjectId), sProjectId);
                tCmd.ExecuteNonQuery();
                tCmd.Dispose();
                rReturn = true;
            }
            catch (Exception tEx)
            {
                NWDLogger.Exception(tEx);
            }

            _DbConnector.Close(tConn);
            return rReturn;
        }

        public List<T> FindAll<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId) where T : NWDBasicModel
        {
            return GetBy<T>(sEnvironment, sProjectId, null);
        }

        public List<T> FindAllModified<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate) where T : NWDBasicModel
        {
            return GetBy<T>(sEnvironment, sProjectId, null,
                " " + nameof(sSyncDate) + " >" + sSyncDate.ToString() + " ");
        }

        public T InsertOrUpdate<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sModel, bool sRangeDependent,
            bool sWithNewReference, Dictionary<string, string> sDictionary = null, string sAndWhere = "") where T : NWDBasicModel
        {
            if (sDictionary == null)
            {
                sDictionary = new Dictionary<string, string>();
            }

            if (!sDictionary.ContainsKey(nameof(NWDBasicModel.Reference)))
            {
                sDictionary.Add(nameof(NWDBasicModel.Reference), sModel.Reference.ToString());
            }

            T rReturn = GetFirstBy<T>(sEnvironment, sProjectId, sDictionary, sAndWhere);
            if (rReturn == null)
            {
                rReturn = Record(sEnvironment, sProjectId, sModel, sRangeDependent, sWithNewReference);
            }
            else
            {
                rReturn = Update(sEnvironment, sProjectId, sModel, sDictionary, sAndWhere);
            }

            return rReturn;
        }
    }
}