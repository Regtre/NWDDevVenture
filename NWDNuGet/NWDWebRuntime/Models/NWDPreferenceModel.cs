using System.Globalization;
using NWDWebRuntime.Configuration;

namespace NWDWebRuntime.Models
{
    public class NWDPreferenceModel
    {
        public static NWDPreferenceModel Counter = new NWDPreferenceModel("Counter");
        private static readonly NWDPreferenceModel BuildDebug = new NWDPreferenceModel("BuildDebug");
        public static NWDPreferenceModel BuildPublish = new NWDPreferenceModel("BuildPublish");
        private static readonly NWDPreferenceModel DateTimeRuntime = new NWDPreferenceModel("DateTimeRuntime");

        private static string KPrefKey(string sKey, DateTime sDateTime, PreferenceRange sPreferenceRange)
        {
            string rReturn = sKey + "-";
            switch (sPreferenceRange)
            {
                case PreferenceRange.ThisHour:
                    {
                        rReturn = rReturn + sDateTime.ToString("yyyy-MM-dd-HH");
                    }
                    break;
                case PreferenceRange.ThisDate:
                    {
                        rReturn = rReturn + sDateTime.ToString("yyyy-MM-dd");
                    }
                    break;
                case PreferenceRange.ThisMonth:
                    {
                        rReturn = rReturn + sDateTime.ToString("yyyy-MM");
                    }
                    break;
                case PreferenceRange.Day:
                    {
                        rReturn = rReturn + sDateTime.ToString("ddd");
                    }
                    break;
                case PreferenceRange.Date:
                    {
                        rReturn = rReturn + sDateTime.ToString("dd");
                    }
                    break;
                case PreferenceRange.Hour:
                    {
                        rReturn = rReturn + sDateTime.ToString("HH");
                    }
                    break;
                case PreferenceRange.Month:
                    {
                        rReturn = rReturn + sDateTime.ToString("MM");
                    }
                    break;
                case PreferenceRange.Year:
                    {
                        rReturn = rReturn + sDateTime.ToString("yyyy");
                    }
                    break;
            }
            return rReturn;
        }

        //public static double GetDoubleForKey(string sKey, DateTime tDateTime, PreferenceRange sPreferenceRange)
        //{
        //    NWDPreferenceModel tModel = new NWDPreferenceModel(kPrefKey(sKey, tDateTime, sPreferenceRange));
        //    return tModel.GetDoubleValue();
        //}

        //public static void IncrementIntForValue(string sKey, DateTime tDateTime, PreferenceRange sPreferenceRange)
        //{
        //    NWDPreferenceModel tModel = new NWDPreferenceModel(kPrefKey(sKey, tDateTime, sPreferenceRange));
        //    tModel.SetDoubleValue(tModel.GetDoubleValue() + 1);
        //}

        public static string BuildInLine()
        {
#if DEBUG
            string rReturn = " Runtime n°" + BuildDebug.GetIntValue() + "-???-" + NWDWebRuntimeConfiguration.KConfig.GitCommit + " since " + DateTimeRuntime.GetDateTimeValue().ToLongDateString() + " ";
#else
            string rReturn = " Runtime n°" + BuildPublish.GetIntValue() + "-" + Counter.GetIntValue() + "-" + NWDWebRuntimeConfiguration.KConfig.GitCommit + " since " + DateTimeRuntime.GetDateTimeValue().ToLongDateString() + " ";
#endif
            return rReturn;
        }

        private string _PrefValue = string.Empty;
        private string _PrefKey;

        public NWDPreferenceModel(string sPrefKey)
        {
            _PrefKey = sPrefKey;
            GetValue();
        }

        public bool GetBoolValue()
        {
            bool.TryParse(_PrefValue, out var rReturn);
            return rReturn;
        }

        public int GetIntValue()
        {
            int.TryParse(_PrefValue, out var rReturn);
            return rReturn;
        }

        public long GetLongValue()
        {
            long.TryParse(_PrefValue, out var rReturn);
            return rReturn;
        }

        public float GetFloatValue()
        {
            float.TryParse(_PrefValue, out var rReturn);
            return rReturn;
        }

        public double GetDoubleValue()
        {
            double.TryParse(_PrefValue, out var rReturn);
            return rReturn;
        }

        public string GetStringValue()
        {
            return _PrefValue;
        }

        public DateTime GetDateTimeValue()
        {
            return new DateTime(GetLongValue());
        }

        public void SetBoolValue(bool sPrefValue)
        {
            SetValue(sPrefValue.ToString(CultureInfo.InvariantCulture));
        }

        public void SetIntValue(int sPrefValue)
        {
            SetValue(sPrefValue.ToString(CultureInfo.InvariantCulture));
        }

        public void SetLongValue(long sPrefValue)
        {
            SetValue(sPrefValue.ToString(CultureInfo.InvariantCulture));
        }

        public void SetFloatValue(float sPrefValue)
        {
            SetValue(sPrefValue.ToString(CultureInfo.InvariantCulture));
        }

        public void SetDoubleValue(double sPrefValue)
        {
            SetValue(sPrefValue.ToString(CultureInfo.InvariantCulture));
        }

        public void SetStringValue(string sPrefValue)
        {
            SetValue(sPrefValue);
        }

        public void SetDateTimeValue(DateTime sPrefValue)
        {
            SetLongValue(sPrefValue.Ticks);
        }

        private string Value()
        {
            return _PrefValue;
        }

        private void SetValue(string sPrefValue)
        {
            _PrefValue = sPrefValue;
            // MySqlConnection conn = SQLConnector.Connection();
            // SQLConnector.Open(conn);
            // try
            // {
            //     string sql = "INSERT INTO `Preferences`" +
            //         "(`PrefKey`, `PrefValue`) " +
            //         "VALUES " +
            //         "(@sPrefKey, @sPrefValue) " +
            //         "ON DUPLICATE KEY UPDATE" +
            //         "`PrefKey` = @sPrefKey, " +
            //         "`PrefValue` = @sPrefValue";
            //     //NWDLogger.WriteLine(sql);
            //     MySqlCommand cmd = new MySqlCommand(sql, conn);
            //     cmd.Parameters.AddWithValue("@sPrefKey", PrefKey);
            //     cmd.Parameters.AddWithValue("@sPrefValue", PrefValue);
            //     cmd.ExecuteNonQuery();
            //     //NWDLogger.WriteLine(cmd.CommandText);
            //     cmd.Dispose();
            //     //conn.Close();
            //     //conn.Dispose();
            // }
            // catch (Exception ex)
            // {
            //     NWDLogger.WriteLine(ex.ToString());
            // }
            // SQLConnector.Close(conn);
        }

        static public void CreateTable()
        {
            // MySqlConnection conn = SQLConnector.Connection();
            // SQLConnector.Open(conn);
            // try
            // {
            //     string sqlTable = "CREATE TABLE IF NOT EXISTS `Preferences` (" +
            //     "`PrefKey` varchar(256) NOT NULL, " +
            //     "`PrefValue` varchar(256) NOT NULL, " +
            //     "PRIMARY KEY(`PrefKey`), " +
            //     "KEY `PrefKeyIndex` (`PrefKey`(256)) " +
            //     ") ENGINE = MyISAM  DEFAULT CHARSET = utf8 COMMENT = 'Preferences'; ";
            //     //NWDLogger.WriteLine(sqlTable);
            //     MySqlCommand cmdTable = new MySqlCommand(sqlTable, conn);
            //     cmdTable.ExecuteNonQuery();
            //     cmdTable.Dispose();
            //     //conn.Close();
            //     //conn.Dispose();
            // }
            // catch (Exception ex)
            // {
            //     NWDLogger.WriteLine(ex.ToString());
            // }
            // SQLConnector.Close(conn);
        }

        private void GetValue()
        {
            _PrefValue = string.Empty;
            // MySqlConnection conn = SQLConnector.Connection();
            // SQLConnector.Open(conn);
            // try
            // {
            //     string sql = "SELECT `PrefValue` FROM `Preferences` WHERE " +
            //     "`PrefKey` LIKE @sPrefKey;";
            //     MySqlCommand cmd = new MySqlCommand(sql, conn);
            //     cmd.Parameters.AddWithValue("@sPrefKey", PrefKey);
            //     MySqlDataReader rdr = cmd.ExecuteReader();
            //     while (rdr.Read())
            //     {
            //         PrefValue = rdr[0] as string;
            //     }
            //     rdr.Close();
            //     rdr.Dispose();
            //     cmd.Dispose();
            //     //conn.Close();
            //     //conn.Dispose();
            // }
            // catch (Exception ex)
            // {
            //     NWDLogger.WriteLine(ex.ToString());
            // }
            // SQLConnector.Close(conn);
        }
    }
}
