using System.Collections;
using System.Data;
using System.Reflection;
using System.Text;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using NWDDatabaseAccess;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDWebRuntime.Configuration;

namespace NWDWebRuntime.Tools
{
    public class NWDWebMySqlByReflexion : INWDWebDBByReflexion
    {
        public NWDWebMySqlByReflexion()
        {
            //NWDLogger.Information(nameof(NWDWebMySqlByReflexion)+"() constructor");
        }

        public bool TableExists<T>(NWDEnvironmentKind sEnvironment) where T : NWDDatabaseWebBasicModel
        {
            bool rReturn = false;
            NWDDatabaseConnector tConnection =
                new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);
            MySqlConnection tConn = tConnection.Connection();
            tConnection.Open(tConn);
            string tSql = "SELECT CASE WHEN EXISTS((select '['+SCHEMA_NAME(schema_id)+'].['+name+']' AS name FROM [" +
                          tConnection.GetDatabaseName() + "].sys.tables WHERE name = '" + TableName<T>(sEnvironment) +
                          "')) THEN 1 ELSE 0 END;";
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

            return rReturn;
        }

        public string FingerPrintTable<T>(NWDEnvironmentKind sEnvironment) where T : NWDDatabaseWebBasicModel
        {
            Type tType = typeof(T);
            return NWDSecurityTools.GenerateSha(ConstructCreateTableQuery<T>(sEnvironment).ToString());
        }

        public string FingerPrintTableName<T>(NWDEnvironmentKind sEnvironment) where T : NWDDatabaseWebBasicModel
        {
            return TableName<T>(sEnvironment);
        }

        public void CreateTable<T>(NWDEnvironmentKind sEnvironment) where T : NWDDatabaseWebBasicModel
        {
            NWDDatabaseConnector tConnection =
                new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);
            Type tType = typeof(T);
            MySqlConnection tConn = tConnection.Connection();
            if (tConn != null)
            {
                tConnection.Open(tConn);
                StringBuilder tSqlTable = ConstructCreateTableQuery<T>(sEnvironment);
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

                foreach (PropertyInfo tFieldInfo in tType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                                        BindingFlags.Instance |
                                                                        BindingFlags.FlattenHierarchy))
                {
                    // string tAdd = "ALTER TABLE `" + TableName<T>(sEnvironment) + "` ADD COLUMN IF NOT EXISTS `" + tFieldInfo.Name + "` " + PropertyInfoToSqlType(tFieldInfo) + ";";
                    string tAdd = "ALTER TABLE `" + TableName<T>(sEnvironment) + "` ADD COLUMN `" + tFieldInfo.Name +
                                  "` " + PropertyInfoToSqlType(tFieldInfo) + ";";
                    try
                    {
                        MySqlCommand tCmdTableAdd = new MySqlCommand(tAdd, tConn);
                        tCmdTableAdd.ExecuteNonQuery();
                        tCmdTableAdd.Dispose();
                    }
                    catch (Exception tEx)
                    {
                        //NWDLogger.Exception(tEx);
                    }

                    // string tAlter = "ALTER TABLE `" + TableName<T>(sEnvironment) + "` MODIFY IF EXISTS `" +  tFieldInfo.Name + "` " + PropertyInfoToSqlType(tFieldInfo) + ";";
                    string tAlter = "ALTER TABLE `" + TableName<T>(sEnvironment) + "` MODIFY `" + tFieldInfo.Name +
                                    "` " + PropertyInfoToSqlType(tFieldInfo) + ";";
                    try
                    {
                        MySqlCommand tCmdTableAlter = new MySqlCommand(tAlter, tConn);
                        tCmdTableAlter.ExecuteNonQuery();
                        tCmdTableAlter.Dispose();
                    }
                    catch (Exception tEx)
                    {
                        //NWDLogger.Exception(tAlter, tEx);
                    }
                }

                tConnection.Close(tConn);
            }
        }

        public bool Delete<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sObject)
            where T : NWDDatabaseWebBasicModel
        {
            return Delete<T>(sEnvironment, sProjectId, sObject.Reference);
        }

        public T? GetByReference<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, string sReference)
            where T : NWDDatabaseWebBasicModel
        {
            T? rReturn = null;
            List<T> tList = GetBy<T>(sEnvironment, sProjectId,
                new Dictionary<string, string>() { { nameof(NWDDatabaseWebBasicModel.Reference), sReference } });
            if (tList.Count > 0)
            {
                rReturn = tList[0];
            }

            return rReturn;
        }

        public T? GetFirstBy<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            Dictionary<string, string> sDictionary)
            where T : NWDDatabaseWebBasicModel
        {
            T? rReturn = null;
            List<T> tList = GetBy<T>(sEnvironment, sProjectId, sDictionary);
            if (tList.Count > 0)
            {
                rReturn = tList[0];
            }

            return rReturn;
        }

        public int CountBy<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "") where T : NWDDatabaseWebBasicModel
        {
            NWDDatabaseConnector tConnection =
                new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);

            int rReturn = 0;
            Type tType = typeof(T);
            MySqlConnection tConn = tConnection.Connection();
            if (tConn != null)
            {
                tConnection.Open(tConn);
                try
                {
                    List<string> tWhere = new List<string>();
                    tWhere.Add("`" + nameof(NWDDatabaseWebBasicModel.ProjectId) + "` = " + sProjectId + " ");
                    if (sDictionary != null)
                    {
                        foreach (KeyValuePair<string, string> tKv in sDictionary)
                        {
                            tWhere.Add("`" + tKv.Key + "` = @s" + tKv.Key + "");
                        }
                    }

                    string tSql = "SELECT COUNT(`" + nameof(NWDDatabaseWebBasicModel.RowId) + "`) FROM `" +
                                  TableName<T>(sEnvironment) + "` WHERE " + string.Join(" AND ", tWhere) + sAndWhere +
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

                tConnection.Close(tConn);
            }

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

        public ulong NewValidReference<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId)
            where T : NWDDatabaseWebBasicModel
        {
            ulong rReturn = NewReference();
            while (TestIfReferenceExists<T>(sEnvironment, sProjectId, rReturn))
            {
                rReturn = NewReference();
            }

            return rReturn;
        }

        public bool TestIfReferenceExists<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
            where T : NWDDatabaseWebBasicModel
        {
            NWDDatabaseConnector tConnection =
                new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);

            bool rReturn = false;
            MySqlConnection tConn = tConnection.Connection();
            if (tConn != null)
            {
                tConnection.Open(tConn);
                try
                {
                    string tSql = "SELECT `" + nameof(NWDDatabaseWebBasicModel.Reference) + "` FROM `" +
                                  TableName<T>(sEnvironment) + "` WHERE " +
                                  "`" + nameof(NWDDatabaseWebBasicModel.Reference) + "` = @s" +
                                  nameof(NWDDatabaseWebBasicModel.Reference) +
                                  " AND " +
                                  "`" + nameof(NWDDatabaseWebBasicModel.ProjectId) + "` = @s" +
                                  nameof(NWDDatabaseWebBasicModel.ProjectId) + " ;";
                    MySqlCommand tCmd = new MySqlCommand(tSql, tConn);
                    tCmd.Parameters.AddWithValue("@s" + nameof(NWDDatabaseWebBasicModel.Reference) + "", sReference);
                    tCmd.Parameters.AddWithValue("@s" + nameof(NWDDatabaseWebBasicModel.ProjectId) + "", sProjectId);
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

                tConnection.Close(tConn);
            }

            return rReturn;
        }

        public bool TestConnexion()
        {
            NWDDatabaseConnector tConnection =
                new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);

            bool rReturn = false;
            MySqlConnection tConn = tConnection.Connection();
            if (tConn != null)
            {
                tConnection.Open(tConn);
                if (tConn.State == ConnectionState.Open)
                {
                    rReturn = true;
                }

                tConnection.Close(tConn);
            }

            return rReturn;
        }

        public T Create<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sModel, bool sWithNewReference)
            where T : NWDDatabaseWebBasicModel
        {
            Record<T>(sEnvironment, sProjectId, sModel, sWithNewReference);
            return sModel;
        }

        public bool Delete<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
            where T : NWDDatabaseWebBasicModel
        {
            NWDDatabaseConnector tConnection =
                new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);

            bool rReturn = false;
            Type tType = typeof(T);
            MySqlConnection tConn = tConnection.Connection();

            if (tConn != null)
            {
                tConnection.Open(tConn);
                try
                {
                    string tSql = "DELETE FROM `" + TableName<T>(sEnvironment) + "` WHERE " +
                                  "`" + nameof(NWDDatabaseWebBasicModel.Reference) + "` = @s" +
                                  nameof(NWDDatabaseWebBasicModel.Reference) +
                                  " AND " +
                                  "`" + nameof(NWDDatabaseWebBasicModel.ProjectId) + "` = @s" +
                                  nameof(NWDDatabaseWebBasicModel.ProjectId) + " " +
                                  ";";
                    // NWDLogger.WriteLine(tSql);
                    MySqlCommand tCmd = new MySqlCommand(tSql, tConn);
                    tCmd.Parameters.AddWithValue("@s" + nameof(NWDDatabaseWebBasicModel.Reference), sReference);
                    tCmd.Parameters.AddWithValue("@s" + nameof(NWDDatabaseWebBasicModel.ProjectId), sProjectId);
                    tCmd.ExecuteNonQuery();
                    tCmd.Dispose();
                    rReturn = true;
                }
                catch (Exception tEx)
                {
                    NWDLogger.Exception(tEx);
                }

                tConnection.Close(tConn);
            }

            return rReturn;
        }

        public List<T> FindAll<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId) where T : NWDDatabaseWebBasicModel
        {
            return GetBy<T>(sEnvironment, sProjectId, null);
        }

        public List<T> FindAllModified<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
            where T : NWDDatabaseWebBasicModel
        {
            return GetBy<T>(sEnvironment, sProjectId, null,
                " " + nameof(sSyncDate) + " >" + sSyncDate.ToString() + " ");
        }

        public T InsertOrUpdate<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sModel, bool sWithNewReference)
            where T : NWDDatabaseWebBasicModel
        {
            T? rReturn = GetByReference<T>(sEnvironment, sProjectId, sModel.Reference.ToString());
            if (rReturn == null)
            {
                rReturn = Record(sEnvironment, sProjectId, sModel, sWithNewReference);
            }
            else
            {
                rReturn = Update(sEnvironment, sProjectId, sModel);
            }

            return rReturn;
        }

        // public void CreateTable<T>(string sEnvironment) where T : NWDWebDBBasicModel
        // {
        //     Type tType = typeof(T);
        //     CreateTable(sEnvironment, tType);
        // }

        private string TableName<T>(NWDEnvironmentKind sEnvironment) where T : NWDDatabaseWebBasicModel
        {
            return NWDTableName.GenerateTableName(typeof(T), sEnvironment,
                NWDDatabaseConnectorConfiguration.KConfig.Credentials.TablePrefix);
        }

        private StringBuilder ConstructCreateTableQuery<T>(NWDEnvironmentKind sEnvironment)
            where T : NWDDatabaseWebBasicModel
        {
            Type tType = typeof(T);
            StringBuilder tSqlTable = new StringBuilder();
            tSqlTable.Append("CREATE TABLE IF NOT EXISTS `" + TableName<T>(sEnvironment) + "` (");
            List<PropertyInfo> tFieldInfoList = new List<PropertyInfo>();
            if (tFieldInfoList == null) throw new ArgumentNullException(nameof(tFieldInfoList));
            List<string> tFieldInfoSql = new List<string>();
            if (tFieldInfoSql == null) throw new ArgumentNullException(nameof(tFieldInfoSql));
            foreach (PropertyInfo tFieldInfo in tType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                                    BindingFlags.Instance |
                                                                    BindingFlags.FlattenHierarchy))
            {
                tFieldInfoList.Add(tFieldInfo);
                tFieldInfoSql.Add("`" + tFieldInfo.Name + "` " + PropertyInfoToSqlType(tFieldInfo));
                tSqlTable.Append("`" + tFieldInfo.Name + "` " + PropertyInfoToSqlType(tFieldInfo) + ", ");
            }

            tSqlTable.Append(" PRIMARY KEY(`" + nameof(NWDDatabaseWebBasicModel.RowId) + "`) ");
            tSqlTable.Append(", KEY `RefByProject` (`" + nameof(NWDDatabaseWebBasicModel.ProjectId) + "`, `" +
                             nameof(NWDDatabaseWebBasicModel.Reference) + "`) ");

            tSqlTable.Append(") ENGINE = MyISAM DEFAULT CHARSET = utf8 COMMENT = '" + tType.Name + " table';");
            return tSqlTable;
        }


        public void DeleteTable<T>(NWDEnvironmentKind sEnvironment) where T : NWDDatabaseWebBasicModel
        {
            NWDDatabaseConnector tConnection =
                new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);
            Type tType = typeof(T);
            MySqlConnection tConn = tConnection.Connection();
            if (tConn != null)
            {
                tConnection.Open(tConn);
                StringBuilder tSqlTable = ConstructDeleteTableQuery<T>(sEnvironment);
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

                tConnection.Close(tConn);
            }
        }

        private StringBuilder ConstructDeleteTableQuery<T>(NWDEnvironmentKind sEnvironment)
            where T : NWDDatabaseWebBasicModel
        {
            Type tType = typeof(T);
            StringBuilder tSqlTable = new StringBuilder();
            tSqlTable.Append("DROP TABLE `" + TableName<T>(sEnvironment) + "`;");
            return tSqlTable;
        }

        private T Update<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sModel)
            where T : NWDDatabaseWebBasicModel
        {
            NWDDatabaseConnector tConnection =
                new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);

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
                if (tFieldInfo.Name != nameof(NWDDatabaseWebBasicModel.RowId))
                {
                    tFieldInfoSql.Add("`" + tFieldInfo.Name + "` = @s" + tFieldInfo.Name + "");
                }
            }

            MySqlConnection tConn = tConnection.Connection();
            if (tConn != null)
            {
                tConnection.Open(tConn);
                try
                {
                    string tSql = "UPDATE `" + TableName<T>(sEnvironment) + "` SET " + string.Join(",", tFieldInfoSql) +
                                  " WHERE " +
                                  "`" + nameof(NWDDatabaseWebBasicModel.Reference) + "` = " + sModel.Reference +
                                  " AND " +
                                  "`" + nameof(NWDDatabaseWebBasicModel.ProjectId) + "` = " + sProjectId +
                                  ";";
                    // NWDLogger.WriteLine(tSql);
                    MySqlCommand tCmd = NWDWebDBByReflexionTools.GetSqlCommandForInsertOrUpdate(
                        tFieldInfoList, tSql, tConn, sModel);

                    tCmd.ExecuteNonQuery();
                    tCmd.Dispose();
                }
                catch (Exception tEx)
                {
                    NWDLogger.Exception(tEx);
                }

                tConnection.Close(tConn);
            }

            return sModel;
        }

        private static void SerializeObjectToJson<T>(T sObject, Type sType, out PropertyInfo? sFieldInfoJson,
            out PropertyInfo? sFieldInfoIntegrity) where T : NWDDatabaseWebBasicModel
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

        private T Record<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sObject,
            bool sWithNewReference) where T : NWDDatabaseWebBasicModel
        {
            NWDDatabaseConnector tConnection =
                new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);

            // NWDLogger.WriteLine(nameof(Record) + "()");
            T rReturn = sObject;
            Type tType = typeof(T);
            if (sWithNewReference || sObject.Reference == 0)
            {
                sObject.Reference = NewValidReference<T>(sEnvironment, sProjectId);
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

            MySqlConnection tConn = tConnection.Connection();
            if (tConn != null)
            {
                tConnection.Open(tConn);
                try
                {
                    string tSql = "INSERT INTO `" + TableName<T>(sEnvironment) + "` (" +
                                  string.Join(",", tFieldInfoSql) + ") VALUES (" + string.Join(",", tSFieldInfoSql) +
                                  ");";
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
                            else if (tTypeOfThis == typeof(DateTime))
                            {
                                DateTime tDateTime = tFieldInfo.GetValue(sObject) as DateTime? ?? DateTime.MinValue;
                                tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tDateTime.ToString());
                            }
                            else if (tTypeOfThis.IsAssignableTo(typeof(NWDDatabaseWebBasicModel)))
                            {
                                tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name,
                                    JsonConvert.SerializeObject(tFieldInfo.GetValue(sObject)));
                            }
                            else if (tTypeOfThis == typeof(string) || tTypeOfThis == typeof(String))
                            {
                                string? tValue = tFieldInfo.GetValue(sObject) as string;
                                if (tValue == null)
                                {
                                    tValue = string.Empty;
                                }

                                tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tValue);
                            }
                            // else if (IsGenericReference(tTypeOfThis))
                            // {
                            //     string tStringValue = string.Empty;
                            //     Object tValue = tFieldInfo.GetValue(sObject);
                            //     if (tValue != null)
                            //     {
                            //         tStringValue = (string)tValue.GetType()
                            //             .GetProperty(nameof(NWDReference<NWDWebDBBasicModel>.Reference))?.GetValue(tValue);
                            //     }
                            //
                            //     if (string.IsNullOrEmpty(tStringValue))
                            //     {
                            //         tStringValue = string.Empty;
                            //     }
                            //     tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tStringValue);
                            // }
                            else if (IsGenericReferencesArray(tTypeOfThis))
                            {
                                string? tStringValue = string.Empty;
                                Object? tValue = tFieldInfo.GetValue(sObject);
                                if (tValue != null)
                                {
                                    tStringValue = (string)tValue.GetType()
                                        .GetProperty(nameof(NWDReferencesArray<NWDDatabaseWebBasicModel>.References))
                                        ?.GetValue(tValue)!;
                                }

                                if (string.IsNullOrEmpty(tStringValue))
                                {
                                    tStringValue = string.Empty;
                                }

                                tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tStringValue);
                            }
                            else if (IsImplementingICollection(tTypeOfThis))
                            {
                                string tStringValue = string.Empty;

                                Object tValue = tFieldInfo.GetValue(sObject);
                                if (tValue != null)
                                {
                                    List<NWDDatabaseWebBasicModel> tList =
                                        ((IList)tValue).Cast<NWDDatabaseWebBasicModel>().ToList();
                                    tStringValue = tList.Select(sItem => sItem.Reference.ToString())
                                        .Aggregate((s1, s2) => s1 + NWDConstants.kFieldSeparatorA + s2);
                                }

                                tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tStringValue);
                            }
                            else if (IsImplementingIDictionary(tTypeOfThis))
                            {
                                string tStringValue = string.Empty;

                                Object tValue = tFieldInfo.GetValue(sObject);
                                if (tValue != null)
                                {
                                    tStringValue = JsonConvert.SerializeObject(tValue);
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

                tConnection.Close(tConn);
            }

            return rReturn;
        }

        public List<T> GetBy<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            Dictionary<string, string>? sDictionary,
            string sAndWhere = "") where T : NWDDatabaseWebBasicModel
        {
            NWDDatabaseConnector tConnection =
                new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);

            List<T> rReturn = new List<T>();
            Type tType = typeof(T);
            List<PropertyInfo> tFieldInfoList = new List<PropertyInfo>();
            foreach (PropertyInfo tFieldInfo in tType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                                    BindingFlags.Instance |
                                                                    BindingFlags.FlattenHierarchy))
            {
                tFieldInfoList.Add(tFieldInfo);
            }

            MySqlConnection tConn = tConnection.Connection();
            if (tConn != null)
            {
                tConnection.Open(tConn);
                try
                {
                    List<string> tWhere = new List<string>();
                    tWhere.Add("`" + nameof(NWDDatabaseWebBasicModel.ProjectId) + "` = '" + sProjectId + "' ");
                    if (sDictionary != null)
                    {
                        foreach (KeyValuePair<string, string> tKv in sDictionary)
                        {
                            tWhere.Add("`" + tKv.Key + "` = @s" + tKv.Key + "");
                        }
                    }

                    string tSql = "SELECT * FROM `" + TableName<T>(sEnvironment) + "` ";
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
                        // NWDLogger.WriteLine("Find reference " + tRdr.GetString(nameof(NWDWebDBBasicModel.RowId)));
                        // NWDLogger.WriteLine("found one row in table");
                        T? tObject = Activator.CreateInstance(tType) as T;
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
                                else if (tTypeOfThis == typeof(DateTime))
                                {
                                    tFieldInfo.SetValue(tObject, DateTime.Parse(tRdr.GetString(tFieldInfo.Name)));
                                }
                                else if (tTypeOfThis.IsAssignableTo(typeof(NWDDatabaseWebBasicModel)))
                                {
                                    tFieldInfo.SetValue(tObject,
                                        JsonConvert.DeserializeObject(tRdr.GetString(tFieldInfo.Name), tTypeOfThis));
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
                                // else if (IsGenericReference(tTypeOfThis))
                                // {
                                //     string tValue = tRdr.GetString(tFieldInfo.Name);
                                //     if (string.IsNullOrEmpty(tValue))
                                //     {
                                //         tValue = string.Empty;
                                //     }
                                //
                                //     //TODO : optimize and fix that
                                //     Object tValueObject = tFieldInfo.GetValue(tObject);
                                //     ulong tValueToULong = 0;
                                //     ulong.TryParse(tValue, out tValueToULong);
                                //     tValueObject.GetType().GetProperty(nameof(NWDReference<NWDWebDBBasicModel>.Reference))
                                //         ?.SetValue(tValueObject, tValueToULong);
                                //     tFieldInfo.SetValue(tObject, tValueObject);
                                // }
                                else if (IsGenericReferencesArray(tTypeOfThis))
                                {
                                    string tValue = tRdr.GetString(tFieldInfo.Name);
                                    if (string.IsNullOrEmpty(tValue))
                                    {
                                        tValue = string.Empty;
                                    }

                                    //TODO : optimize and fix that
                                    Object? tValueObject = tFieldInfo.GetValue(tObject);
                                    if (tValueObject != null)
                                    {
                                        tValueObject.GetType()
                                            .GetProperty(
                                                nameof(NWDReferencesArray<NWDDatabaseWebBasicModel>.References))
                                            ?.SetValue(tValueObject, tValue);
                                    }
                                }
                                else if (IsImplementingICollection(tTypeOfThis))
                                {
                                    string tValue = tRdr.GetString(tFieldInfo.Name);
                                    if (string.IsNullOrEmpty(tValue))
                                    {
                                        tValue = string.Empty;
                                    }

                                    // Get Method GetByReference by Reflexion so you can call it with generic type in variable
                                    Type tTypeOfList = tTypeOfThis.GetGenericArguments().First();
                                    MethodInfo tMethod =
                                        typeof(NWDWebMariaByReflexion).GetMethod(nameof(GetByReference));
                                    MethodInfo tMethodTyped = tMethod.MakeGenericMethod(tTypeOfList);


                                    List<NWDDatabaseWebBasicModel?> tResult = tValue
                                        .Split(NWDConstants.kFieldSeparatorA, StringSplitOptions.RemoveEmptyEntries)
                                        .ToList().Select(sReference =>
                                            tMethodTyped.Invoke(this,
                                                    new object?[] { sEnvironment, sProjectId, sReference }) as
                                                NWDDatabaseWebBasicModel).ToList();


                                    tFieldInfo.SetValue(tObject,
                                        typeof(Enumerable).GetMethod("ToList") // Cast Enumerator to List 
                                            ?.MakeGenericMethod(tTypeOfList)
                                            .Invoke(null, new[]
                                            {
                                                typeof(Enumerable)
                                                    .GetMethod("Cast") //Cast List of item retrieve from Reference in DB
                                                    ?.MakeGenericMethod(tTypeOfList)
                                                    .Invoke(null, new object[] { tResult })
                                            }));
                                }
                                else if (IsImplementingIDictionary(tTypeOfThis))
                                {
                                    string tValue = tRdr.GetString(tFieldInfo.Name);
                                    if (string.IsNullOrEmpty(tValue))
                                    {
                                        tValue = string.Empty;
                                    }

                                    tFieldInfo.SetValue(tObject, JsonConvert.DeserializeObject(tValue, tTypeOfThis));
                                }
                                else
                                {
                                    //JSON ?
                                }
                            }
                        }

                        if (tObject != null)
                        {
                            rReturn.Add(tObject);
                        }
                    }

                    tRdr.Close();
                    tRdr.Dispose();
                    tCmd.Dispose();
                }
                catch (Exception tEx)
                {
                    NWDLogger.Exception(tEx);
                }

                tConnection.Close(tConn);
            }

            return rReturn;
        }

        private ulong NewReference()
        {
            ulong rReturn = NWDRandom.UnsignedLongNumeric(NWDConstants.K_REFERENCE_UNIQUE_SIZE);
            return rReturn;
        }

        private static string PropertyInfoToSqlType(PropertyInfo sPropertyInfo)
        {
            if (sPropertyInfo.Name == nameof(NWDDatabaseWebBasicModel.RowId))
            {
                return " BIGINT UNSIGNED NOT NULL AUTO_INCREMENT";
            }

            // string rReturn = " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL DEFAULT '' ";
            string rReturn = " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL ";
            Type tTypeOfThis = sPropertyInfo.PropertyType;
            if (tTypeOfThis.IsEnum)
            {
                rReturn = " VARCHAR(256) NOT NULL DEFAULT ''";
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
                    // rReturn = " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL DEFAULT '' ";
                    rReturn = " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL ";
                }
                else if (IsGenericReference(tTypeOfThis))
                {
                    // NWDLogger.WriteLine("IsSubclassOf "+nameof(NWDReference<NWDWebDBBasicModel>)+" ");
                    // rReturn = " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL DEFAULT '' ";
                    rReturn = " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL ";
                }
                else if (IsGenericReferencesArray(tTypeOfThis) || IsImplementingICollection(tTypeOfThis) ||
                         IsImplementingIDictionary(tTypeOfThis))
                {
                    // NWDLogger.WriteLine("IsSubclassOf "+nameof(NWDReferencesArray<NWDWebDBBasicModel>)+" ");
                    // rReturn = " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL DEFAULT '' ";
                    rReturn = " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL ";
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

        private static bool IsImplementingICollection(Type sType)
        {
            return sType.GetInterfaces().Any(t => t.IsGenericType &&
                                                  t.GetGenericTypeDefinition() == typeof(ICollection<>));
        }

        private static bool IsImplementingIDictionary(Type sType)
        {
            return sType.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        }
    }
}