using System.Collections;
using System.Reflection;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using NWDFoundation.Models;
using NWDFoundation.Tools;

namespace NWDWebRuntime.Tools;

public class NWDWebDBByReflexionTools
{
    public static MySqlCommand GetSqlCommandForInsertOrUpdate<T>(List<PropertyInfo> sFieldInfoList, string sSql,
        MySqlConnection tConn, T sModel) where T : NWDDatabaseWebBasicModel
    {
        MySqlCommand tCmd = new MySqlCommand(sSql, tConn);
        foreach (PropertyInfo tFieldInfo in sFieldInfoList)
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
                else if (tTypeOfThis == typeof(DateTime))
                {
                    DateTime tDateTime = tFieldInfo.GetValue(sModel) as DateTime? ?? DateTime.MinValue;
                    tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tDateTime.ToString());
                }
                else if (tTypeOfThis.IsAssignableTo(typeof(NWDDatabaseWebBasicModel)))
                {/*
                    string tReference = string.Empty;
                    if (tFieldInfo.GetValue(sModel) is NWDDatabaseWebBasicModel tValue)
                    {
                        tReference = tValue.Reference.ToString();
                    }*/

                    string value = JsonConvert.SerializeObject(tFieldInfo.GetValue(sModel));
                    tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name,
                        value);
                }
                else if (tTypeOfThis == typeof(string) || tTypeOfThis == typeof(String))
                {
                    string? tValue = tFieldInfo.GetValue(sModel) as string;
                    if (tValue == null)
                    {
                        tValue = string.Empty;
                    }

                    tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tValue);
                }
                else if (IsGenericReferencesArray(tTypeOfThis))
                {
                    string? tStringValue = string.Empty;
                    object? tValue = tFieldInfo.GetValue(sModel);
                    if (tValue != null)
                    {
                        tStringValue = (string)tValue.GetType()
                            .GetProperty(nameof(NWDReferencesArray<NWDBasicModel>.References))
                            ?.GetValue(tValue)!;
                    }

                    if (string.IsNullOrEmpty(tStringValue))
                    {
                        tStringValue = string.Empty;
                    }

                    tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tStringValue);
                }
                else if (NWDWebDBByReflexionTools.IsImplementingICollection(tTypeOfThis) && tTypeOfThis.GetGenericArguments().Length == 1
                         && tTypeOfThis.GetGenericArguments().First().IsAssignableTo(typeof(NWDDatabaseWebBasicModel)))
                {
                    string tStringValue = string.Empty;

                    Object tValue = tFieldInfo.GetValue(sModel);
                    if (tValue != null)
                    {
                        List<NWDDatabaseWebBasicModel>
                            tList = ((IList)tValue).Cast<NWDDatabaseWebBasicModel>().ToList();
                        {
                            List<string>? tReferenceList = tList?.Select(sItem => sItem.Reference.ToString()).ToList();
                            if (tReferenceList is { Count: > 0 })
                            {
                                tStringValue =
                                    tReferenceList.Aggregate((s1, s2) => s1 + NWDConstants.kFieldSeparatorA + s2);
                            }
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
                    string tStringValue = string.Empty;

                    object? tValue = tFieldInfo.GetValue(sModel);
                    if (tValue != null)
                    {
                        tStringValue = JsonConvert.SerializeObject(tValue);
                    }

                    tCmd.Parameters.AddWithValue("@s" + tFieldInfo.Name, tStringValue);
                }
            }
        }

        return tCmd;
    }
    public static bool IsGenericReference(Type sType)
    {
        return (sType.IsGenericType && (sType.GetGenericTypeDefinition() == typeof(NWDReference<>)));
    }

    public static bool IsGenericReferencesArray(Type sType)
    {
        return (sType.IsGenericType && (sType.GetGenericTypeDefinition() == typeof(NWDReferencesArray<>)));
    }

    public static bool IsImplementingICollection(Type sType)
    {
        return sType.GetInterfaces().Any(t => t.IsGenericType &&
                                              t.GetGenericTypeDefinition() == typeof(ICollection<>));
    }
}