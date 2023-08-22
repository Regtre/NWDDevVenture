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
using static System.Reflection.BindingFlags;

namespace NWDWebRuntime.Tools
{
    public class NWDWebMariaByReflexion : INWDWebDBByReflexion
    {
        public NWDWebMariaByReflexion()
        {
            //NWDLogger.Information(nameof(NWDWebMariaByReflexion)+"() constructor");
        }

        public bool TableExists<T>(NWDEnvironmentKind sEnvironment) where T : NWDDatabaseWebBasicModel
        {
            bool rReturn = false;
            NWDDatabaseConnector tConnection = new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);
            MySqlConnection tConn = tConnection.Connection();
            tConnection.Open(tConn);
            string tSql = "SELECT CASE WHEN EXISTS((select '['+SCHEMA_NAME(schema_id)+'].['+name+']' AS name FROM [" + tConnection.GetDatabaseName() + "].sys.tables WHERE name = '" + TableName<T>(sEnvironment) + "')) THEN 1 ELSE 0 END;";
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
            tConnection.Close(tConn);
            return rReturn;
        }

        private string TableName<T>(NWDEnvironmentKind sEnvironment) where T : NWDDatabaseWebBasicModel
        {
            return NWDTableName.GenerateTableName(typeof(T), sEnvironment,
                NWDDatabaseConnectorConfiguration.KConfig.Credentials.TablePrefix);
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

                foreach (PropertyInfo tFieldInfo in tType.GetProperties(Default | FlattenHierarchy | Instance |
                                                                        NonPublic | Public))
                {
                    string tAdd = "ALTER TABLE `" + TableName<T>(sEnvironment) + "` ADD COLUMN IF NOT EXISTS `" +
                                  tFieldInfo.Name + "` " + PropertyInfoToSqlType(tFieldInfo) + ";";
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

                    string tAlter = "ALTER TABLE `" + TableName<T>(sEnvironment) + "` MODIFY IF EXISTS `" +
                                    tFieldInfo.Name + "` " + PropertyInfoToSqlType(tFieldInfo) + ";";
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

                tConnection.Close(tConn);
            }
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
            foreach (PropertyInfo tFieldInfo in tType.GetProperties(Public | NonPublic |
                                                                    Instance |
                                                                    FlattenHierarchy))
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

        public bool Delete<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sObject)
            where T : NWDDatabaseWebBasicModel
        {
            return Delete<T>(sEnvironment, sProjectId, sObject.Reference);
        }

        public T Update<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sModel)
            where T : NWDDatabaseWebBasicModel
        {
            NWDDatabaseConnector tConnection =
                new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);
            Type tType = typeof(T);
            sModel.ProjectId = sProjectId;
            List<PropertyInfo> tFieldInfoList = new List<PropertyInfo>();
            List<string> tFieldInfoSql = new List<string>();
            foreach (PropertyInfo tFieldInfo in tType.GetProperties(Public | NonPublic |
                                                                    Instance |
                                                                    FlattenHierarchy))
            {
                tFieldInfoList.Add(tFieldInfo);
                if (tFieldInfo.Name != nameof(NWDDatabaseWebBasicModel.RowId))
                {
                    tFieldInfoSql.Add("`" + tFieldInfo.Name + "` = @s" + tFieldInfo.Name + "");
                }
            }

            MySqlConnection tConn = tConnection.Connection();
            tConnection.Open(tConn);
            try
            {
                string tSql = "UPDATE `" + TableName<T>(sEnvironment) + "` SET " + string.Join(",", tFieldInfoSql) +
                              " WHERE " +
                              "`" + nameof(NWDDatabaseWebBasicModel.Reference) + "` = " + sModel.Reference + " AND " +
                              "`" + nameof(NWDDatabaseWebBasicModel.ProjectId) + "` = " + sProjectId +
                              ";";
                
                MySqlCommand tCmd = NWDWebDBByReflexionTools.GetSqlCommandForInsertOrUpdate(
                    tFieldInfoList,tSql, tConn,sModel);

                tCmd.ExecuteNonQuery();
                tCmd.Dispose();
            }
            catch (Exception tEx)
            {
                NWDLogger.Exception(tEx);
            }

            tConnection.Close(tConn);

            return sModel;
        }

        public T Record<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sObject,
            bool sWithNewReference) where T : NWDDatabaseWebBasicModel
        {
            NWDDatabaseConnector tConnection =
                new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);
            T rReturn = sObject;
            Type tType = typeof(T);
            if (sWithNewReference || sObject.Reference == 0)
            {
                sObject.Reference = NewValidReference<T>(sEnvironment, sProjectId);
            }

            sObject.ProjectId = sProjectId;
            List<PropertyInfo> tFieldInfoList = new List<PropertyInfo>();
            List<string> tFieldInfoSql = new List<string>();
            List<string> tSFieldInfoSql = new List<string>();
            foreach (PropertyInfo tFieldInfo in tType.GetProperties(Public | NonPublic |
                                                                    Instance |
                                                                    FlattenHierarchy))
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
                    MySqlCommand tCmd = NWDWebDBByReflexionTools.GetSqlCommandForInsertOrUpdate(
                        tFieldInfoList, tSql, tConn, sObject);

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

        public T? GetByReference<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, string sReference)
            where T : NWDDatabaseWebBasicModel
        {
            T? rReturn = null;
            List<T> tList = GetBy<T>(sEnvironment, sProjectId,
                new Dictionary<string, string> { { nameof(NWDDatabaseWebBasicModel.Reference), sReference } });
            if (tList.Count > 0)
            {
                rReturn = tList[0];
            }

            return rReturn;
        }

        public T? GetFirstBy<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            Dictionary<string, string> sDictionary) where T : NWDDatabaseWebBasicModel
        {
            T? rReturn = null;
            List<T> tList = GetBy<T>(sEnvironment, sProjectId, sDictionary);
            if (tList.Count > 0)
            {
                rReturn = tList[0];
            }

            return rReturn;
        }

        public List<T> GetBy<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            Dictionary<string, string>? sDictionary,
            string sAndWhere = "") where T : NWDDatabaseWebBasicModel
        {
            // NWDLogger.Critical(nameof(NWDWebRuntimeConfiguration.KConfig.ProjectId) + " : " +NWDWebRuntimeConfiguration.KConfig.ProjectId.ToString());
            // NWDLogger.Critical(nameof(NWDWebRuntimeConfiguration.KConfig.Environment) + " : " +NWDWebRuntimeConfiguration.KConfig.Environment.ToString());

            NWDDatabaseConnector tConnection =
                new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);
            List<T> rReturn = new List<T>();
            Type tType = typeof(T);
            List<PropertyInfo> tFieldInfoList = new List<PropertyInfo>();
            foreach (PropertyInfo tFieldInfo in tType.GetProperties(Public | NonPublic |
                                                                    Instance |
                                                                    FlattenHierarchy))
            {
                tFieldInfoList.Add(tFieldInfo);
            }

            MySqlConnection tConn = tConnection.Connection();
            if (tConn != null)
            {
                tConnection.Open(tConn);
                try
                {
                    List<string> tWhere = new List<string>
                        { "`" + nameof(NWDDatabaseWebBasicModel.ProjectId) + "` = '" + sProjectId + "' " };
                    if (sDictionary != null)
                    {
                        tWhere.AddRange(sDictionary.Select(tKv => "`" + tKv.Key + "` = @s" + tKv.Key + ""));
                    }

                    string tSql = "SELECT * FROM `" + TableName<T>(sEnvironment) + "` ";
                    if (tWhere.Count > 0)
                    {
                        tSql = tSql + " WHERE " + string.Join(" AND ", tWhere) + " " + sAndWhere + ";";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(sAndWhere))
                        {
                            tSql = tSql + " WHERE 1=1 AND " + sAndWhere + ";";
                        }
                    }

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
                        T? tObject = Activator.CreateInstance(tType) as T;
                        foreach (PropertyInfo tFieldInfo in tFieldInfoList)
                        {
                            Type tTypeOfThis = tFieldInfo.PropertyType;
                            if (tTypeOfThis.IsEnum)
                            {
                                string tEnumValue = tRdr.GetString(tFieldInfo.Name);
                                if (!string.IsNullOrEmpty(tEnumValue))
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
                                else if (IsGenericReferencesArray(tTypeOfThis))
                                {
                                    string tValue = tRdr.GetString(tFieldInfo.Name);
                                    if (string.IsNullOrEmpty(tValue))
                                    {
                                        tValue = string.Empty;
                                    }

                                    object? tValueObject = tFieldInfo.GetValue(tObject);
                                    tFieldInfo.SetValue(tObject, tValueObject);
                                }
                                else if (NWDWebDBByReflexionTools.IsImplementingICollection(tTypeOfThis) && tTypeOfThis.GetGenericArguments().Length == 1
                                         && tTypeOfThis.GetGenericArguments().First().IsAssignableTo(typeof(NWDDatabaseWebBasicModel)))
                                {
                                    string tValue = tRdr.GetString(tFieldInfo.Name);
                                    if (string.IsNullOrEmpty(tValue))
                                    {
                                        tValue = string.Empty;
                                    }
                               
                                    // Get Method GetByReference by Reflexion so you can call it with generic type in variable
                                    Type tTypeOfList = tTypeOfThis.GetGenericArguments().First();
                                    MethodInfo tMethod = typeof(NWDWebMariaByReflexion).GetMethod(nameof(GetByReference));
                                    MethodInfo tMethodTyped = tMethod.MakeGenericMethod(tTypeOfList);
                                    
                                    
                                    List<NWDDatabaseWebBasicModel?> tResult = tValue.
                                        Split( NWDConstants.kFieldSeparatorA, StringSplitOptions.RemoveEmptyEntries).
                                        ToList().
                                        Select(sReference => tMethodTyped.
                                            Invoke(this,new object?[]{ sEnvironment, sProjectId,sReference }) as NWDDatabaseWebBasicModel).
                                        ToList();
                                    

                                    tFieldInfo.SetValue(tObject,typeof(Enumerable).GetMethod("ToList") // Cast Enumerator to List 
                                        ?.MakeGenericMethod(tTypeOfList)
                                        .Invoke(null, new [] { typeof(Enumerable).GetMethod("Cast") //Cast List of item retrieve from Reference in DB
                                            ?.MakeGenericMethod(tTypeOfList)
                                            .Invoke(null, new object[] { tResult }) })  );
                                    
                                }
                                else
                                {
                                    string tValue = tRdr.GetString(tFieldInfo.Name);
                                    if (string.IsNullOrEmpty(tValue))
                                    {
                                        tValue = string.Empty;
                                    }

                                    tFieldInfo.SetValue(tObject, JsonConvert.DeserializeObject(tValue, tTypeOfThis));
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

        public int CountBy<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary,
            string sAndWhere = "") where T : NWDDatabaseWebBasicModel
        {
            NWDDatabaseConnector tConnection =
                new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);
            int rReturn = 0;
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
                        tWhere.AddRange(sDictionary.Select(tKv => "`" + tKv.Key + "` = @s" + tKv.Key + ""));
                    }

                    string tSql = "SELECT COUNT(`" + nameof(NWDDatabaseWebBasicModel.RowId) + "`) FROM `" +
                                  TableName<T>(sEnvironment) + "` WHERE " + string.Join(" AND ", tWhere) + sAndWhere +
                                  ";";
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

        private ulong NewReference()
        {
            ulong rReturn = NWDRandom.UnsignedLongNumeric(NWDConstants.K_REFERENCE_UNIQUE_SIZE);
            return rReturn;
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

        private static string PropertyInfoToSqlType(PropertyInfo sPropertyInfo)
        {
            if (sPropertyInfo.Name == nameof(NWDDatabaseWebBasicModel.RowId))
            {
                return " BIGINT UNSIGNED NOT NULL AUTO_INCREMENT";
            }

            string rReturn = " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL DEFAULT '' ";
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
                    rReturn = " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL DEFAULT '' ";
                }
                else if (IsGenericReference(tTypeOfThis))
                {
                    rReturn = " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL DEFAULT '' ";
                }
                else
                {
                    rReturn = " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL DEFAULT '' ";
                }
                //Json ?
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
            Record(sEnvironment, sProjectId, sModel, sWithNewReference);
            return sModel;
        }

        public bool Delete<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
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
                    string tSql = "DELETE FROM `" + TableName<T>(sEnvironment) + "` WHERE " +
                                  "`" + nameof(NWDDatabaseWebBasicModel.Reference) + "` = @s" +
                                  nameof(NWDDatabaseWebBasicModel.Reference) +
                                  " AND " +
                                  "`" + nameof(NWDDatabaseWebBasicModel.ProjectId) + "` = @s" +
                                  nameof(NWDDatabaseWebBasicModel.ProjectId) + " " +
                                  ";";
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
                " " + nameof(sSyncDate) + " >" + sSyncDate + " ");
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

    }
}