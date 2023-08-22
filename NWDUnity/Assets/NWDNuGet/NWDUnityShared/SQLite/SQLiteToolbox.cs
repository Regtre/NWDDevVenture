using NWDFoundation.Models;
using NWDUnityShared.Tools;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace NWDUnityShared.SQLite
{
    public static class SQLiteToolbox
    {
        public static void CreateTable<T>(ref NWDAsyncHandler sOperationHandler, INWDDBConnection sConnection) where T : NWDDatabaseBasicModel
        {
            Type tType = typeof(T);
            StringBuilder sqlTable = new StringBuilder();
            sqlTable.Append("CREATE TABLE IF NOT EXISTS `" + tType.Name + "` (");
            List<PropertyInfo> tFieldInfoList = new List<PropertyInfo>();
            List<string> tFieldInfoSQL = new List<string>();
            foreach (PropertyInfo tFieldInfo in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                sqlTable.Append("`" + tFieldInfo.Name + "` " + PropertyInfoToSQLType(tFieldInfo) + ", ");
            }
            if (tType.IsSubclassOf(typeof(NWDDataBasicStorageModel)))
            {
                sqlTable.Append("PRIMARY KEY(`" + nameof(NWDDataBasicStorageModel.Reference) + "`, `" + nameof(NWDDataBasicStorageModel.DataTrack) + "`));");
            }
            else
            {
                sqlTable.Append("PRIMARY KEY(`" + nameof(NWDDatabaseBasicModel.Reference) + "`));");
            }

            sConnection.Exec(sqlTable.ToString());
        }

        public static List<T> GetAll<T>(ref NWDAsyncHandler sOperationHandler, INWDDBConnection sConnection, string sAndWhere = "") where T : NWDDatabaseBasicModel
        {
            List<T> rReturn = new List<T>();
            Type tType = typeof(T);
            List<string> tFieldInfoSQL = new List<string>();
            PropertyInfo[] tGetProperties = tType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            foreach (PropertyInfo tFieldInfo in tGetProperties)
            {
                tFieldInfoSQL.Add(tFieldInfo.Name);
            }
            int tCount = 0;

            string tQuery = "SELECT COUNT(*) FROM `" + tType.Name + "`";
            if (string.IsNullOrEmpty(sAndWhere) == false)
            {
                tQuery += " WHERE " + sAndWhere;
            }

            using (INWDDBRequest tIRequest = sConnection.StartRequest(tQuery))
            {
                NWDSQLiteDbRequest tRequest = (NWDSQLiteDbRequest)tIRequest;
                if (tRequest.Step() == SQLite3.Result.Row)
                {
                    tCount = SQLite3.ColumnInt(tRequest, 0);
                }
            }
            tQuery = "SELECT `" + string.Join("`, `", tFieldInfoSQL) + "` FROM `" + tType.Name + "`";
            if (string.IsNullOrEmpty(sAndWhere) == false)
            {
                tQuery += " WHERE " + sAndWhere;
            }

            using (INWDDBRequest tIRequest = sConnection.StartRequest(tQuery))
            {
                NWDSQLiteDbRequest tRequest = (NWDSQLiteDbRequest)tIRequest;
                int i = 0;
                while (tRequest.Step() == SQLite3.Result.Row)
                {
                    sOperationHandler.Progress = i / tCount;
                    T tReturn = Activator.CreateInstance<T>();
                    for (int tI = 0; tI < tGetProperties.Length; tI++)
                    {
                        PropertyInfo tProp = tGetProperties[tI];
                        Type tTypeOfThis = tProp.PropertyType;
                        if (tTypeOfThis == typeof(byte) ||
                            tTypeOfThis == typeof(sbyte) ||
                            tTypeOfThis == typeof(short) ||
                            tTypeOfThis == typeof(ushort) ||
                            tTypeOfThis == typeof(int) ||
                            tTypeOfThis.IsEnum)
                        {
                            int tValue = SQLite3.ColumnInt(tRequest, tI);
                            if (tTypeOfThis == typeof(byte))
                            {
                                tProp.SetValue(tReturn, Convert.ToByte (tValue));
                            }
                            else if (tTypeOfThis == typeof(sbyte))
                            {
                                tProp.SetValue(tReturn, Convert.ToSByte (tValue));
                            }
                            else if (tTypeOfThis == typeof(short))
                            {
                                tProp.SetValue(tReturn, Convert.ToInt16 (tValue));
                            }
                            else if(tTypeOfThis == typeof(ushort))
                            {
                                tProp.SetValue(tReturn, Convert.ToUInt16 (tValue));
                            }
                            else
                            {
                                tProp.SetValue(tReturn, tValue);
                            }
                        }
                        else if (tTypeOfThis == typeof(long) ||
                            tTypeOfThis == typeof(uint))
                        {
                            tProp.SetValue(tReturn, SQLite3.ColumnInt64(tRequest, tI));
                        }
                        else if (tTypeOfThis == typeof(ulong))
                        {
                            tProp.SetValue(tReturn, NWDConverter.ULongFromString(SQLite3.ColumnString(tRequest, tI)));
                        }
                        else if (tTypeOfThis == typeof(double))
                        {
                            tProp.SetValue(tReturn, SQLite3.ColumnDouble(tRequest, tI));
                        }
                        else if (tTypeOfThis == typeof(float))
                        {
                            tProp.SetValue(tReturn, (float)SQLite3.ColumnDouble(tRequest, tI));
                        }
                        else if (tTypeOfThis == typeof(bool))
                        {
                            tProp.SetValue(tReturn, SQLite3.ColumnInt(tRequest, tI) == 1);
                        }
                        else if (tTypeOfThis == typeof(string))
                        {
                            tProp.SetValue(tReturn, SQLite3.ColumnString(tRequest, tI));
                        }
                    }
                    rReturn.Add(tReturn);
                    i++;
                }
            }

            return rReturn;
        }

        public static void Reccords<T>(ref NWDAsyncHandler sOperationHandler, INWDDBConnection sConnection, IEnumerable<T> sObjects, int sCount) where T : NWDDatabaseBasicModel
        {
            Type tType = typeof(T);
            List<string> tFieldInfoSQL = new List<string>();
            List<string> tFieldInfoSQLINSERTORCREATE = new List<string>();
            PropertyInfo[] tGetProperties = tType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            foreach (PropertyInfo tFieldInfo in tGetProperties)
            {
                tFieldInfoSQL.Add(" `" + tFieldInfo.Name + "`");
            }
            foreach (T sObject in sObjects)
            {
                List<string> tsFieldInfoSQL = new List<string>();
                foreach (PropertyInfo tFieldInfo in tGetProperties)
                {
                    Type tTypeOfThis = tFieldInfo.PropertyType;
                    string tValueString = string.Empty;
                    object tValue = tFieldInfo.GetValue(sObject, null);
                    if (tValue == null)
                    {
                        if (tTypeOfThis.IsEnum)
                        {
                            tValueString = "0";
                        }
                        else if (tTypeOfThis == typeof(string))
                        {
                            tValueString = string.Empty;
                        }
                        else if (tTypeOfThis == typeof(bool))
                        {
                            tValueString = NWDConverter.BoolToIntString(false);
                        }
                        else if (tTypeOfThis == typeof(byte) ||
                            tTypeOfThis == typeof(sbyte) ||
                            tTypeOfThis == typeof(short) ||
                            tTypeOfThis == typeof(ushort) ||
                            tTypeOfThis == typeof(int) ||
                            tTypeOfThis == typeof(uint) ||
                            tTypeOfThis == typeof(long) ||
                            tTypeOfThis == typeof(ulong) ||
                            tTypeOfThis == typeof(float) ||
                            tTypeOfThis == typeof(double))
                        {
                            tValueString = "0";
                        }
                        else
                        {
                            tValueString = string.Empty;
                        }
                    }
                    else
                    {
                        if (tTypeOfThis.IsEnum)
                        {
                            int tInt = (int)tValue;
                            tValueString = tInt.ToString();
                        }
                        else if (tTypeOfThis == typeof(string))
                        {
                            tValueString = tValue.ToString().Replace("\"", "\"\"");
                        }
                        else if (tTypeOfThis == typeof(bool))
                        {
                            tValueString = NWDConverter.BoolToIntString((bool)tValue);
                        }
                        else if (tTypeOfThis == typeof(byte) ||
                            tTypeOfThis == typeof(sbyte) ||
                            tTypeOfThis == typeof(short) ||
                            tTypeOfThis == typeof(ushort) ||
                            tTypeOfThis == typeof(int) ||
                            tTypeOfThis == typeof(uint) ||
                            tTypeOfThis == typeof(long))
                        {
                            tValueString = tValue.ToString();
                        }
                        else if (tTypeOfThis == typeof(ulong))
                        {
                            tValueString = NWDConverter.ULongToString((ulong)tValue);
                        }
                        else if (tTypeOfThis == typeof(float))
                        {
                            tValueString = NWDConverter.FloatToString((float)tValue);
                        }
                        else if (tTypeOfThis == typeof(double))
                        {
                            tValueString = NWDConverter.DoubleToString((double)tValue);
                        }
                        else
                        {
                            tValueString = tValue.ToString().Replace("\"", "\"\"");
                        }
                    }
                    tsFieldInfoSQL.Add(tValueString);
                }
                // More efficient (and safer) to use prepare statements
                string sql = "INSERT OR REPLACE INTO `" + tType.Name + "` (" + string.Join(",", tFieldInfoSQL) + ") VALUES (\"" + string.Join("\", \"", tsFieldInfoSQL) + "\");";
                tFieldInfoSQLINSERTORCREATE.Add(sql);
            }

            using (INWDDBTransaction sTransaction = sConnection.StartTransaction())
            {
                try
                {
                    for (int i = 0; i < tFieldInfoSQLINSERTORCREATE.Count; i++)
                    {
                        sOperationHandler.Progress = i / sCount;
                        sConnection.Exec(tFieldInfoSQLINSERTORCREATE[i]);
                    }
                }
                catch
                {
                    sTransaction.Rollback();
                }
            }
        }

        private static string PropertyInfoToSQLType(PropertyInfo sPropertyInfo)
        {
            /*if (sPropertyInfo.Name == nameof(NWDDatabaseBasicModel.Reference))
            {
                return "VARCHAR(256) NOT NULL DEFAULT ''";
            }*/
            string rReturn = "TEXT NOT NULL DEFAULT '' ";
            Type tTypeOfThis = sPropertyInfo.PropertyType;

            if (tTypeOfThis == typeof(int) ||
                tTypeOfThis == typeof(long) ||
                tTypeOfThis == typeof(Int16) ||
                tTypeOfThis == typeof(Int32) ||
                tTypeOfThis == typeof(Int64) ||
                tTypeOfThis == typeof(UInt16) ||
                tTypeOfThis == typeof(UInt32) ||
                tTypeOfThis == typeof(UInt64)
                )
            {
                rReturn = " INT(11) NOT NULL DEFAULT 0";
            }
            if (tTypeOfThis == typeof(uint) ||
                tTypeOfThis == typeof(ulong)
               )
            {
                rReturn = " VARCHAR(20) NOT NULL DEFAULT 0";
            }
            else if (tTypeOfThis.IsEnum)
            {
                rReturn = "VARCHAR(256) NOT NULL DEFAULT ''";
            }
            else if (tTypeOfThis == typeof(float) ||
                tTypeOfThis == typeof(double) ||
                tTypeOfThis == typeof(Double))
            {
                rReturn = "DOUBLE NOT NULL DEFAULT 0";
            }
            else if (tTypeOfThis == typeof(bool))
            {
                rReturn = "INT(1) NOT NULL DEFAULT 0";
            }
            else if (tTypeOfThis == typeof(string) || tTypeOfThis == typeof(String))
            {
                rReturn = "TEXT NOT NULL DEFAULT '' ";
            }
            return rReturn;
        }
    }
}
