using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using UAParser;
using ChartJSCore.Helpers;
using ChartJSCore.Models;
using NWDDatabaseAccess;
using NWDFoundation.Logger;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Tools.Sessions;
using NWDWebRuntime.Tools;
using NWDWebStandard.Configuration;
using NWDWebStandard.Models.Enums;

namespace NWDWebStandard.Models
{
    public class NWDStatisticsConsolidated
    {
        private static string TableName()
        {
            return NWDDatabaseConnectorConfiguration.KConfig.Credentials.TablePrefix+nameof(NWDStatisticsConsolidated);
        }
        public const string K_SESSION = "SESSION";
        public const string K_OS = "OS";
        public const string K_OS_VERSION = "OS-VERSION";
        public const string K_DEVICE = "DEVICE";
        public const string K_BROWSER = "BROWSER";

        public string StatKey;
        public string StatGroup;
        public string StatIndex;
        public string StatTitle;
        public string StatDescription;
        public double StatValue;

        static public NWDSessionBool StatisticsDefinition = new NWDSessionBool("Internal Statistics", "Statistics", "Internal Anonymized Statistics", NWDSessionDefinitionGroup.Analytics, false);
        
        static public NWDSessionBool CrawlerSessionDefinition = new NWDSessionBool("Crawler", "Crawler", "Are-you a crawler?", NWDSessionDefinitionGroup.Functional, false, false, false);

        public NWDStatisticsConsolidated()
        {
            StatKey = string.Empty;
            StatGroup = string.Empty;
            StatValue = 0;
            StatTitle = string.Empty;
            StatDescription = string.Empty;
            StatIndex = string.Empty;
        }
        public NWDStatisticsConsolidated(string sStatKey, string sStatGroup, string sStatTitle, string sStatDescription)
        {
            StatKey = sStatKey;
            StatGroup = sStatGroup;
            StatValue = 0;
            StatTitle = sStatTitle;
            StatDescription = sStatDescription;
            StatIndex = string.Empty;
        }

        private static string kPrefKey(string sKey, DateTime sDateTime, NWDStatisticsConsolidateRange sStatisticsConsolidateRange, int sOffset = 0)
        {
            string rReturn = sKey.Replace("_", "-");
            switch (sStatisticsConsolidateRange)
            {
#if DEBUG
                case NWDStatisticsConsolidateRange.ThisMinute:
                    {
                        DateTime tDateTime = sDateTime.AddHours(sOffset);
                        rReturn = rReturn + "_ti_" + tDateTime.ToString("yyyy-MM-dd-HH-mm");
                    }
                    break;
#endif

                case NWDStatisticsConsolidateRange.ThisHour:
                    {
                        DateTime tDateTime = sDateTime.AddHours(sOffset);
                        rReturn = rReturn + "_th_" + tDateTime.ToString("yyyy-MM-dd-HH");
                    }
                    break;
                case NWDStatisticsConsolidateRange.ThisDate:
                    {
                        DateTime tDateTime = sDateTime.AddDays(sOffset);
                        rReturn = rReturn + "_td_" + tDateTime.ToString("yyyy-MM-dd");
                    }
                    break;
                case NWDStatisticsConsolidateRange.ThisMonth:
                    {
                        DateTime tDateTime = sDateTime.AddMonths(sOffset);
                        rReturn = rReturn + "_tm_" + tDateTime.ToString("yyyy-MM");
                    }
                    break;
                case NWDStatisticsConsolidateRange.Day:
                    {
                        DateTime tDateTime = sDateTime.AddDays(sOffset * 7);
                        rReturn = rReturn + "_dy_" + tDateTime.ToString("ddd");
                    }
                    break;
                case NWDStatisticsConsolidateRange.Date:
                    {
                        DateTime tDateTime = sDateTime.AddDays(sOffset);
                        rReturn = rReturn + "_d_" + tDateTime.ToString("dd");
                    }
                    break;
                case NWDStatisticsConsolidateRange.Hour:
                    {
                        DateTime tDateTime = sDateTime.AddHours(sOffset);
                        rReturn = rReturn + "_h_" + tDateTime.ToString("HH");
                    }
                    break;
                case NWDStatisticsConsolidateRange.Month:
                    {
                        DateTime tDateTime = sDateTime.AddMonths(sOffset);
                        rReturn = rReturn + "_m_" + tDateTime.ToString("MM");
                    }
                    break;
                case NWDStatisticsConsolidateRange.Year:
                    {
                        DateTime tDateTime = sDateTime.AddYears(sOffset);
                        rReturn = rReturn + "_y_" + tDateTime.ToString("yyyy");
                    }
                    break;
            }
            return rReturn;
        }

        private static string kLabel(DateTime sDateTime, NWDStatisticsConsolidateRange sStatisticsConsolidateRange, int sOffset = 0)
        {
            string rReturn = string.Empty;
            switch (sStatisticsConsolidateRange)
            {
#if DEBUG
                case NWDStatisticsConsolidateRange.ThisMinute:
                    {
                        DateTime tDateTime = sDateTime.AddHours(sOffset);
                        rReturn = tDateTime.ToString("yyyy/MM/dd HH:mm:00");
                    }
                    break;
#endif
                case NWDStatisticsConsolidateRange.ThisHour:
                    {
                        DateTime tDateTime = sDateTime.AddHours(sOffset);
                        rReturn = tDateTime.ToString("yyyy/MM/dd HH:00:00");
                    }
                    break;
                case NWDStatisticsConsolidateRange.ThisDate:
                    {
                        DateTime tDateTime = sDateTime.AddDays(sOffset);
                        rReturn = tDateTime.ToString("yyyy/MM/dd");
                    }
                    break;
                case NWDStatisticsConsolidateRange.ThisMonth:
                    {
                        DateTime tDateTime = sDateTime.AddMonths(sOffset);
                        rReturn = tDateTime.ToString("yyyy/MM");
                    }
                    break;
                case NWDStatisticsConsolidateRange.Day:
                    {
                        DateTime tDateTime = sDateTime.AddDays(sOffset * 7);
                        rReturn = tDateTime.ToString("ddd");
                    }
                    break;
                case NWDStatisticsConsolidateRange.Date:
                    {
                        DateTime tDateTime = sDateTime.AddDays(sOffset);
                        rReturn = tDateTime.ToString("dd");
                    }
                    break;
                case NWDStatisticsConsolidateRange.Hour:
                    {
                        DateTime tDateTime = sDateTime.AddHours(sOffset);
                        rReturn = tDateTime.ToString("HH");
                    }
                    break;
                case NWDStatisticsConsolidateRange.Month:
                    {
                        DateTime tDateTime = sDateTime.AddMonths(sOffset);
                        rReturn = tDateTime.ToString("MM");
                    }
                    break;
                case NWDStatisticsConsolidateRange.Year:
                    {
                        DateTime tDateTime = sDateTime.AddYears(sOffset);
                        rReturn = tDateTime.ToString("yyyy");
                    }
                    break;
            }
            return rReturn;
        }
        public static List<NWDStatisticsConsolidated> GetGroupWithHistoric(string sGroup, DateTime sDateTime, NWDStatisticsConsolidateRange sStatisticsConsolidateRange, int sRangeOffset = 0)
        {
            return GetGroupHistoric(sGroup, sDateTime, sStatisticsConsolidateRange, sRangeOffset);
        }

        private static List<NWDStatisticsConsolidated> GetGroupHistoric(string sGroup, DateTime sDateTime, NWDStatisticsConsolidateRange sStatisticsConsolidateRange, int sRangeOffset = 0)
        {
            List<NWDStatisticsConsolidated> rReturn = new List<NWDStatisticsConsolidated>();
            NWDDatabaseConnector tConnection = new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);

            MySqlConnection conn = tConnection.Connection();

            if (conn != null)
            {
                tConnection.Open(conn);
                try
                {
                    List<string> tList = new List<string>();
                    if (sRangeOffset >= 0)
                    {
                        for (int i = 0; i <= sRangeOffset; i++)
                        {
                            tList.Add(kPrefKey(sGroup, sDateTime, sStatisticsConsolidateRange, i));
                        }
                    }
                    else
                    {
                        for (int i = sRangeOffset; i <= 0; i++)
                        {
                            tList.Add(kPrefKey(sGroup, sDateTime, sStatisticsConsolidateRange, i));
                        }
                    }

                    string sql = "SELECT " +
                                 "`" + nameof(StatKey) + "`, `" + nameof(StatGroup) + "`, `" + nameof(StatIndex) + "`, `" + nameof(StatValue) + "`, `" + nameof(StatTitle) + "`, `" +
                                 nameof(StatDescription) + "` " +
                                 "FROM `" + TableName() + "` WHERE " +
                                 "`" + nameof(StatGroup) + "` IN ('" + string.Join("', '", tList) + "') ORDER BY `" + nameof(StatKey) + "`;";
                  
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        NWDStatisticsConsolidated tT = new NWDStatisticsConsolidated();
                        tT.StatKey = rdr.GetString(nameof(StatKey));
                        tT.StatGroup = rdr.GetString(nameof(StatGroup));
                        tT.StatIndex = rdr.GetString(nameof(StatIndex));
                        tT.StatValue = rdr.GetDouble(nameof(StatValue));
                        tT.StatTitle = rdr.GetString(nameof(StatTitle));
                        tT.StatDescription = rdr.GetString(nameof(StatDescription));
                        rReturn.Add(tT);
                    }

                    rdr.Close();
                    rdr.Dispose();
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    NWDLogger.Exception(ex);
                }

                tConnection.Close(conn);
            }

            return rReturn;
        }



        public static List<NWDStatisticsConsolidated> GetKeyWithHistoric(string sKey, DateTime sDateTime, NWDStatisticsConsolidateRange sStatisticsConsolidateRange, int sRangeOffset = 0)
        {
            return GetHistoric(sKey, sDateTime, sStatisticsConsolidateRange, sRangeOffset);
        }

        private static List<NWDStatisticsConsolidated> GetHistoric(string sKey, DateTime sDateTime, NWDStatisticsConsolidateRange sStatisticsConsolidateRange, int sRangeOffset = 0)
        {
            NWDDatabaseConnector tConnection = new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);

            List<NWDStatisticsConsolidated> rReturn = new List<NWDStatisticsConsolidated>();
            MySqlConnection conn = tConnection.Connection();
            if (conn != null)
            {
                tConnection.Open(conn);
                try
                {
                    List<string> tList = new List<string>();
                    if (sRangeOffset >= 0)
                    {
                        for (int i = 0; i <= sRangeOffset; i++)
                        {
                            tList.Add(kPrefKey(sKey, sDateTime, sStatisticsConsolidateRange, i));
                        }
                    }
                    else
                    {
                        for (int i = sRangeOffset; i <= 0; i++)
                        {
                            tList.Add(kPrefKey(sKey, sDateTime, sStatisticsConsolidateRange, i));
                        }
                    }

                    string sql = "SELECT " +
                                 "`" + nameof(StatKey) + "`, `" + nameof(StatGroup) + "`, `" + nameof(StatIndex) + "`, `" + nameof(StatValue) + "`, `" + nameof(StatTitle) + "`, `" +
                                 nameof(StatDescription) + "` " +
                                 "FROM `" + TableName() + "` WHERE " +
                                 "`" + nameof(StatKey) + "` IN ('" + string.Join("', '", tList) + "') ORDER BY `" + nameof(StatKey) + "`;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        NWDStatisticsConsolidated tT = new NWDStatisticsConsolidated();
                        tT.StatKey = rdr.GetString(nameof(StatKey));
                        tT.StatGroup = rdr.GetString(nameof(StatGroup));
                        tT.StatIndex = rdr.GetString(nameof(StatIndex));
                        tT.StatValue = rdr.GetDouble(nameof(StatValue));
                        tT.StatTitle = rdr.GetString(nameof(StatTitle));
                        tT.StatDescription = rdr.GetString(nameof(StatDescription));
                        rReturn.Add(tT);
                    }

                    rdr.Close();
                    rdr.Dispose();
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    NWDLogger.Exception(ex);
                }

                tConnection.Close(conn);
            }

            return rReturn;
        }


        public static double GetValueForKey(string sKey, DateTime sDateTime, NWDStatisticsConsolidateRange sStatisticsConsolidateRange, int sOffset = 0)
        {
            return GetForKey(sKey, sDateTime, sStatisticsConsolidateRange, sOffset);
        }

        private static double GetForKey(string sKey, DateTime sDateTime, NWDStatisticsConsolidateRange sStatisticsConsolidateRange, int sOffset = 0)
        {
            NWDDatabaseConnector tConnection = new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);

            double rResult = 0;
            MySqlConnection conn = tConnection.Connection();
            if (conn != null)
            {
                tConnection.Open(conn);
                try
                {
                    string sql = "SELECT `" + nameof(StatValue) + "` FROM `" + TableName() + "` WHERE " +
                                 "`" + nameof(StatKey) + "` = @sPrefKey;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@sPrefKey", kPrefKey(sKey, sDateTime, sStatisticsConsolidateRange, sOffset));
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        rResult = rdr.GetDouble(0);
                    }

                    rdr.Close();
                    rdr.Dispose();
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    NWDLogger.Exception(ex);
                }

                tConnection.Close(conn);
            }

            return rResult;
        }

        static List<string> Crawlers3 = new List<string>()
{
    "bot","crawler","spider","80legs","baidu","yahoo! slurp","ia_archiver","mediapartners-google",
    "lwp-trivial","nederland.zoek","ahoy","anthill","appie","arale","araneo","ariadne",
    "atn_worldwide","atomz","bjaaland","ukonline","calif","combine","cosmos","cusco",
    "cyberspyder","digger","grabber","downloadexpress","ecollector","ebiness","esculapio",
    "esther","felix ide","hamahakki","kit-fireball","fouineur","freecrawl","desertrealm",
    "gcreep","golem","griffon","gromit","gulliver","gulper","whowhere","havindex","hotwired",
    "htdig","ingrid","informant","inspectorwww","iron33","teoma","ask jeeves","jeeves",
    "image.kapsi.net","kdd-explorer","label-grabber","larbin","linkidator","linkwalker",
    "lockon","marvin","mattie","mediafox","merzscope","nec-meshexplorer","udmsearch","moget",
    "motor","muncher","muninn","muscatferret","mwdsearch","sharp-info-agent","webmechanic",
    "netscoop","newscan-online","objectssearch","orbsearch","packrat","pageboy","parasite",
    "patric","pegasus","phpdig","piltdownman","pimptrain","plumtreewebaccessor","getterrobo-plus",
    "raven","roadrunner","robbie","robocrawl","robofox","webbandit","scooter","search-au",
    "searchprocess","senrigan","shagseeker","site valet","skymob","slurp","snooper","speedy",
    "curl_image_client","suke","www.sygol.com","tach_bw","templeton","titin","topiclink","udmsearch",
    "urlck","valkyrie libwww-perl","verticrawl","victoria","webscout","voyager","crawlpaper",
    "webcatcher","t-h-u-n-d-e-r-s-t-o-n-e","webmoose","pagesinventory","webquest","webreaper",
    "webwalker","winona","occam","robi","fdse","jobo","rhcs","gazz","dwcp","yeti","fido","wlm",
    "wolp","wwwc","xget","legs","curl","webs","wget","sift","cmc"
};

        public static void IncrementForValue(string sKey, string sGroup, string sTitle, string sDescription, HttpContext sHttpContext)
        {
            NWDDatabaseConnector tConnection = new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);

            bool tBot = false;
            if (sHttpContext != null)
            {
                if (sHttpContext.Request.Headers.ContainsKey("User-Agent"))
                {
                    string? tUserAgent = sHttpContext.Request.Headers["User-Agent"];
                    //sHttpContext.Request.Br
                    //var userBrowser = new HttpBrowserCapabilities { Capabilities = new Hashtable { { string.Empty, tUserAgent.First() } } };

                    // https://stackoverflow.com/questions/544450/detecting-honest-web-crawlers
                    //tBot = Regex.IsMatch(tUserAgent, @"bot|crawler|baiduspider|80legs|ia_archiver|voyager|curl|wget|yahoo! slurp|mediapartners-google", RegexOptions.IgnoreCase);
                    if (string.IsNullOrEmpty(tUserAgent) == false)
                    {
                        string tUa = tUserAgent.ToLower();
                        tBot = Crawlers3.Exists(sX => tUa.Contains(sX));
                    }
                }
            }
            if (tBot == false)
            {
                List<NWDStatisticsConsolidated> tKeyList = new List<NWDStatisticsConsolidated>();
                if (string.IsNullOrEmpty(sGroup))
                {
                    sGroup = "STAT";
                }
                tKeyList.Add(new NWDStatisticsConsolidated(sKey, sGroup, sTitle, sDescription));

                if (sHttpContext != null)
                {
                    //if (sHttpContext.Session.GetString("SessionStat") == null)
                    if (StatisticsDefinition.GetValue(sHttpContext) == false)
                    {
                        // stats only once per session
                        //sHttpContext.Session.SetString("SessionStat", "yes");
                        StatisticsDefinition.SetValue(sHttpContext, true);
                        tKeyList.Add(new NWDStatisticsConsolidated(K_SESSION, K_SESSION, "Session unique", ""));

                        if (sHttpContext.Request != null)
                        {
                            if (sHttpContext.Request.Headers != null)
                            {
                                if (sHttpContext.Request.Headers.ContainsKey("User-Agent"))
                                {
                                    string? tUserAgent = sHttpContext.Request.Headers["User-Agent"];

                                    if (tUserAgent != null)
                                    {
                                        //DeviceDetector tDeviceDetector = new DeviceDetector(tUserAgent);
                                        //tDeviceDetector.Parse();
                                        //tBot = tDeviceDetector.IsBot();
                                        if (tBot == false)
                                        {
                                            //string clientInfo = tDeviceDetector.GetClient().ToString(); // holds information about browser, feed reader, media player, ...
                                            //string osInfo = tDeviceDetector.GetOs().ToString();
                                            //string device = tDeviceDetector.GetDeviceName().ToString();
                                            //string brand = tDeviceDetector.GetBrandName().ToString();
                                            //string model = tDeviceDetector.GetModel().ToString();

                                            Parser tParser = Parser.GetDefault();
                                            ClientInfo tClientInfo = tParser.Parse(tUserAgent);

                                            string osInfo = tClientInfo.OS.Family;
                                            string osInfoVersion = tClientInfo.OS.Major;
                                            osInfo = osInfo.Replace(" ", "");

                                            if (osInfo == "Mac OS X")
                                            {
                                                osInfoVersion = tClientInfo.OS.Minor;
                                            }


                                            if (tUserAgent.Contains("Android"))
                                            {
                                                osInfo = "Android";
                                            }
                                            else if (tUserAgent.Contains("iPad"))
                                            {
                                                osInfo = "iOS";
                                            }
                                            else if (tUserAgent.Contains("iPhone"))
                                            {
                                                osInfo = "iOS";
                                            }
                                            else if (tUserAgent.Contains("Linux"))
                                            {
                                                osInfo = "Linux";
                                            }
                                            else if (tUserAgent.Contains("Windows Phone"))
                                            {
                                                osInfo = "Windows";
                                            }
                                            else if (tUserAgent.Contains("Mac"))
                                            {
                                                osInfo = "MacOSX";
                                                osInfoVersion = tClientInfo.OS.Minor;
                                            }
                                            else if (tUserAgent.Contains("Windows"))
                                            {
                                                osInfo = "Windows";
                                            }

                                            string device = tClientInfo.Device.Family;
                                            device = NWDToolBox.LimitToOneWord(device);
                                            string browser = tClientInfo.UA.Family;

                                            // add OS and version
                                            tKeyList.Add(new NWDStatisticsConsolidated(osInfo, K_OS, osInfo, ""));
                                            tKeyList.Add(new NWDStatisticsConsolidated(osInfo + "-" + osInfoVersion, K_OS_VERSION, osInfo + " " + osInfoVersion, ""));
                                            // add web agent
                                            tKeyList.Add(new NWDStatisticsConsolidated(browser, K_BROWSER, browser, ""));
                                            // add device type
                                            tKeyList.Add(new NWDStatisticsConsolidated(device, K_DEVICE, device, ""));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                   
                }
                MySqlConnection tConn = tConnection.Connection();

                if (tConn != null)
                {
                    tConnection.Open(tConn);
                    try
                    {
                        MySqlTransaction tTransaction = tConn.BeginTransaction();
                        DateTime tDateTime = DateTime.Now;
                        string sql = "INSERT INTO `" + TableName() + "`" +
                                     "(`" + nameof(StatKey) + "`, `" + nameof(StatGroup) + "`, `" + nameof(StatIndex) + "`, `" + nameof(StatValue) + "`, `" + nameof(StatTitle) + "`, `" +
                                     nameof(StatDescription) + "`) " +
                                     "VALUES " +
                                     "(@s" + nameof(StatKey) + ", @s" + nameof(StatGroup) + ", @s" + nameof(StatIndex) + ", 1, @s" + nameof(StatTitle) + ", @s" + nameof(StatDescription) + ") " +
                                     "ON DUPLICATE KEY UPDATE" +
                                     "`" + nameof(StatGroup) + "` = @s" + nameof(StatGroup) + ", " +
                                     "`" + nameof(StatIndex) + "` = @s" + nameof(StatIndex) + ", " +
                                     "`" + nameof(StatTitle) + "` = @s" + nameof(StatTitle) + ", " +
                                     "`" + nameof(StatDescription) + "` = @s" + nameof(StatDescription) + ", " +
                                     "`" + nameof(StatValue) + "` = (`" + nameof(StatValue) + "` + 1);";
                        MySqlCommand cmd = new MySqlCommand(sql, tConn);
                        foreach (NWDStatisticsConsolidated tKey in tKeyList)
                        {
                            foreach (NWDStatisticsConsolidateRange tStat in (NWDStatisticsConsolidateRange[])Enum.GetValues(typeof(NWDStatisticsConsolidateRange)))
                            {
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@s" + nameof(StatKey), kPrefKey(tKey.StatKey, tDateTime, tStat));
                                cmd.Parameters.AddWithValue("@s" + nameof(StatGroup), kPrefKey(tKey.StatGroup, tDateTime, tStat));
                                cmd.Parameters.AddWithValue("@s" + nameof(StatIndex), kLabel(tDateTime, tStat));
                                cmd.Parameters.AddWithValue("@s" + nameof(StatTitle), tKey.StatTitle);
                                cmd.Parameters.AddWithValue("@s" + nameof(StatDescription), tKey.StatDescription);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        tTransaction.Rollback();
                        cmd.Dispose();
                        tTransaction.Dispose();
                    }
                    catch (Exception ex)
                    {
                        NWDLogger.Exception(ex);
                    }

                    tConnection.Close(tConn);
                }
            }
            else
            {
                CrawlerSessionDefinition.SetValue(sHttpContext, true);
            }
        }
        #region Managment
        static public void DeleteTable()
        {
            NWDDatabaseConnector tConnection = new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);

            MySqlConnection tConn = tConnection.Connection();
            if (tConn != null)
            {
                tConnection.Open(tConn);
                try
                {
                    MySqlCommand cmd = new MySqlCommand("DROP TABLE `" + TableName() + "`;", tConn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    NWDLogger.Exception(ex);
                }

                tConnection.Close(tConn);
            }
        }
        static public void CreateTable()
        {
            NWDDatabaseConnector tConnection = new NWDDatabaseConnector(NWDDatabaseConnectorConfiguration.KConfig.Credentials);

            MySqlConnection tConn = tConnection.Connection();
            if (tConn != null)
            {
                tConnection.Open(tConn);
                try
                {
                    string sqlTable = "CREATE TABLE IF NOT EXISTS `" + TableName() + "` (" +
                                      "`" + nameof(StatKey) + "` varchar(256) NOT NULL DEFAULT '', " +
                                      "`" + nameof(StatGroup) + "` varchar(256) NOT NULL DEFAULT '', " +
                                      "`" + nameof(StatIndex) + "` varchar(64) NOT NULL DEFAULT '', " +
                                      "`" + nameof(StatTitle) + "` varchar(256) NOT NULL DEFAULT '', " +
                                      "`" + nameof(StatDescription) + "` varchar(256) NOT NULL DEFAULT '', " +
                                      "`" + nameof(StatValue) + "` BIGINT DEFAULT 0, " +
                                      "PRIMARY KEY(`" + nameof(StatKey) + "`), " +
                                      "KEY `" + nameof(StatKey) + "Index` (`" + nameof(StatKey) + "`(256)), " +
                                      "KEY `" + nameof(StatGroup) + "Index` (`" + nameof(StatGroup) + "`(256)) " +
                                      ") ENGINE = MyISAM  DEFAULT CHARSET = utf8 COMMENT = 'Statistic';";
                    
                    MySqlCommand cmdTable = new MySqlCommand(sqlTable, tConn);
                    cmdTable.ExecuteNonQuery();
                    cmdTable.Dispose();

                    //string sqlAlter = "ALTER TABLE `" + nameof(NWDStatisticsConsolidated) + "` " +
                    //"ADD " +
                    //"`" + nameof(StatIndex) + "` varchar(64) NOT NULL DEFAULT ''" +
                    //";";
                    
                    //MySqlCommand cmdAlter = new MySqlCommand(sqlAlter, conn);
                    //cmdAlter.ExecuteNonQuery();
                    //cmdAlter.Dispose();

                    //string sqlIndex = "CREATE INDEX `" + nameof(StatGroup) + "Index` " +
                    //    "ON `" + nameof(NWDStatisticsConsolidated) + "` " +
                    //    "(`" + nameof(StatGroup) + "`(256));";
                    
                    //MySqlCommand cmdIndex = new MySqlCommand(sqlIndex, conn);
                    //cmdIndex.ExecuteNonQuery();
                    //cmdIndex.Dispose();
                }
                catch (Exception ex)
                {
                    NWDLogger.Exception(ex);
                }

                tConnection.Close(tConn);
            }
        }
        #endregion
        #region Style

        static List<ChartColor> kColor = new List<ChartColor>()
        {
            ChartColor.FromRgb(255, 99, 132),
            ChartColor.FromRgb(45, 220, 83),
            ChartColor.FromRgb(54, 162, 235),
            ChartColor.FromRgb(253, 121, 255),
            ChartColor.FromRgb(255, 159, 64),
            //ChartColor.FromRgb(255, 244, 93),
            //ChartColor.FromRgb(123, 104, 255),
            //ChartColor.FromRgb(158, 255, 94),
            ChartColor.FromRgb(153, 102, 255),
            ChartColor.FromRgb(109, 255, 247),
            ChartColor.FromRgb(136, 141, 137),
            ChartColor.FromRgb(40, 179, 182),
            ChartColor.FromRgb(91, 94, 91)
        };

        private static ChartColor GetChartColor(int sIndex)
        {
            ChartColor rReturn = GetBorderColor(sIndex);
            return ChartColor.FromRgba(rReturn.Red, rReturn.Green, rReturn.Blue, 0.2f);
        }


        private static ChartColor GetBorderColor(int sIndex)
        {
            int tIndex = sIndex % kColor.Count;
            ChartColor rReturn = kColor[tIndex];
            return rReturn;
        }
        #endregion
        private static List<string> GetLabels(DateTime sDateTime, NWDStatisticsConsolidateRange sStatisticsConsolidateRange, int sRangeOffset = 0)
        {
            List<string> rReturn = new List<string>();
            if (sRangeOffset >= 0)
            {
                for (int i = 0; i <= sRangeOffset; i++)
                {
                    rReturn.Add(kLabel(sDateTime, sStatisticsConsolidateRange, i));
                }
            }
            else
            {
                for (int i = sRangeOffset; i <= 0; i++)
                {
                    rReturn.Add(kLabel(sDateTime, sStatisticsConsolidateRange, i));
                }
            }
            return rReturn;
        }




        public static Chart GenerateBarChartWithGroup(string sTitle, string sGroup, DateTime sDateTime, NWDStatisticsConsolidateRange sStatisticsConsolidateRange, int sRangeOffset = 0)
        {
            Dictionary<string, Dictionary<string, double>> tDatas = new Dictionary<string, Dictionary<string, double>>();

            List<NWDStatisticsConsolidated> tList = GetGroupHistoric(sGroup, sDateTime, sStatisticsConsolidateRange, sRangeOffset);
            List<string> tLabelindex = GetLabels(sDateTime, sStatisticsConsolidateRange, sRangeOffset);
            foreach (NWDStatisticsConsolidated tV in tList)
            {
                if (tDatas.ContainsKey(tV.StatTitle) == false)
                {
                    Dictionary<string, double> tDatasVierge = new Dictionary<string, double>();
                    foreach (string tI in tLabelindex)
                    {
                        tDatasVierge.Add(tI, 0);
                    }
                    tDatas.Add(tV.StatTitle, tDatasVierge);
                }
                if (tDatas[tV.StatTitle].ContainsKey(tV.StatIndex))
                {
                    tDatas[tV.StatTitle][tV.StatIndex] = tV.StatValue;
                }
            }
            return GenerateBarChartWithData(sTitle, tDatas, sDateTime, sStatisticsConsolidateRange, sRangeOffset);
        }

        private static Chart GenerateBarChartWithData(string sTitle, Dictionary<string, Dictionary<string, double>> sDataPairs, DateTime sDateTime, NWDStatisticsConsolidateRange sStatisticsConsolidateRange, int sRangeOffset = 0)
        {
            Chart chart = new Chart();
            chart.Type = ChartJSCore.Models.Enums.ChartType.Bar;
            Data data = new Data();
            data.Labels = GetLabels(sDateTime, sStatisticsConsolidateRange, sRangeOffset);
            data.Datasets = new List<Dataset>();
            int tIndex = 0;
            foreach (KeyValuePair<string, Dictionary<string, double>> tKV in sDataPairs)
            {

                IList<double?> tData = new List<double?>();
                foreach (KeyValuePair<string, double> tKVa in tKV.Value)
                {
                    tData.Add(tKVa.Value);
                }
                ChartColor tBackgroundColor = GetChartColor(tIndex);
                ChartColor tBorderColor = GetBorderColor(tIndex);
                BarDataset dataset = new BarDataset()
                {
                    Label = tKV.Key,
                    Data = tData,
                    BackgroundColor = new List<ChartColor>() { tBackgroundColor, },
                    BorderColor = new List<ChartColor> { tBorderColor, },
                    BorderWidth = new List<int>() { 1 }
                };
                data.Datasets.Add(dataset);
                tIndex++;
            }
            chart.Data = data;
            var options = new Options
            {
                Scales = new Dictionary<string, Scale>()
            };
            // var scales = new Scale
            // {
            //     YAxes = new List<Scale>
            //     {
            //         new CartesianScale
            //         {
            //             Ticks = new CartesianLinearTick
            //             {
            //                 BeginAtZero = true
            //             }
            //         }
            //     },
            //     XAxes = new List<Scale>
            //     {
            //         new BarScale
            //         {
            //             BarPercentage = 1,
            //             BarThickness = 6,
            //             MaxBarThickness = 8,
            //             MinBarLength = 2,
            //             GridLines = new GridLine()
            //             {
            //                 OffsetGridLines = true
            //             }
            //         }
            //     }
            // };
            // options.Scales = scales;
            // chart.Options = options;
            // chart.Options.Layout = new Layout
            // {
            //     Padding = new Padding
            //     {
            //         PaddingObject = new PaddingObject
            //         {
            //             Left = 10,
            //             Right = 12
            //         }
            //     }
            // };
            // chart.Options.Title = new Title();
            // chart.Options.Title.Text = sTitle;
            // chart.Options.Title.Display = true;
            return chart;
        }


        public static Chart GenerateBarChartForGroup(string sTitle, string sGroup, DateTime sDateTime, NWDStatisticsConsolidateRange sStatisticsConsolidateRange, int sOffset = 0)
        {
            Dictionary<string, double> tDatas = new Dictionary<string, double>();

            List<NWDStatisticsConsolidated> tList = GetGroupHistoric(sGroup, sDateTime, sStatisticsConsolidateRange, sOffset);
            foreach (NWDStatisticsConsolidated tV in tList)
            {
                tDatas.Add(tV.StatTitle, tV.StatValue);
            }
            return GenerateBarChartForData(sTitle, tDatas);
        }

        private static Chart GenerateBarChartForData(string sTitle, Dictionary<string, double> sKeyValue)
        {
            Chart chart = new Chart();
            chart.Type = ChartJSCore.Models.Enums.ChartType.Bar;
            Data data = new Data();
            data.Labels = new List<string>();
            IList<double?> tData = new List<double?>();
            foreach (KeyValuePair<string, double> tKV in sKeyValue)
            {
                data.Labels.Add(tKV.Key);
                tData.Add(tKV.Value);
            }
            BarDataset dataset = new BarDataset()
            {
                Label = sTitle,
                Data = tData,
                BackgroundColor = new List<ChartColor>
                {
                    ChartColor.FromRgba(255, 99, 132, 0.2),
                    ChartColor.FromRgba(54, 162, 235, 0.2),
                    ChartColor.FromRgba(255, 206, 86, 0.2),
                    ChartColor.FromRgba(75, 192, 192, 0.2),
                    ChartColor.FromRgba(153, 102, 255, 0.2),
                    ChartColor.FromRgba(255, 159, 64, 0.2),
                },
                BorderColor = new List<ChartColor>
                {
                    ChartColor.FromRgb(255, 99, 132),
                    ChartColor.FromRgb(54, 162, 235),
                    ChartColor.FromRgb(255, 206, 86),
                    ChartColor.FromRgb(75, 192, 192),
                    ChartColor.FromRgb(153, 102, 255),
                    ChartColor.FromRgb(255, 159, 64),
                },
                BorderWidth = new List<int>() { 1 }
            };
            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);
            chart.Data = data;
            var options = new Options
            {
                Scales = new Dictionary<string, Scale>()
            };
            // var scales = new Scales
            // {
            //     YAxes = new List<Scale>
            //     {
            //         new CartesianScale
            //         {
            //             Ticks = new CartesianLinearTick
            //             {
            //                 BeginAtZero = true
            //             }
            //         }
            //     },
            //     XAxes = new List<Scale>
            //     {
            //         new BarScale
            //         {
            //             BarPercentage = 1,
            //             BarThickness = 6,
            //             MaxBarThickness = 8,
            //             MinBarLength = 2,
            //             GridLines = new GridLine()
            //             {
            //                 OffsetGridLines = true
            //             }
            //         }
            //     }
            // };
            // options.Scales = scales;
            chart.Options = options;
            chart.Options.Layout = new Layout
            {
                Padding = new Padding
                {
                    PaddingObject = new PaddingObject
                    {
                        Left = 10,
                        Right = 12
                    }
                }
            };
            return chart;
        }
    }
}
