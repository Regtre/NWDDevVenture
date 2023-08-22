using System;
using System.Globalization;
using NWDFoundation.Configuration.Environments;

namespace NWDFoundation.Tools
{
    public abstract class NWDConstants
    {
        #region fake instanciation of project

        public const ulong K_FAKE_PROJECT_ID = 11111;
        public const NWDEnvironmentKind K_FAKE_PROJECT_ENVIRONMENT = NWDEnvironmentKind.Dev;
        public const string K_FAKE_PROJECT_KEY = "78945318964df89zegter845ds1f23s4897TREFG4Q65SF4CD8SQ67F89ZR4G5DS64";
        public const string K_FAKE_SECRET_KEY = "zegt78945318964df89er845ds1f23s4897TREF67F89ZR4G5DS64G4Q65SF4CD8SQ";
        public const string K_FAKE_CRUCIAL_KEY = "89er845ds1f23zegtREF67F89ZR4G5DS64G4Q65SF4CD8SQ78945318964dfs4897T";
        public const string K_FAKE_TREAT_KEY = "egtREF67F889er845ds1f23z9ZR4G5DS64GCD8SQ78945318964dfs4897T4Q65SF4";

        
        public static ulong K_RANDOM_PROJECT_ID = NWDRandom.UnsignedLongNumeric(5);
        public static NWDEnvironmentKind K_RANDOM_PROJECT_ENVIRONMENT = NWDEnvironmentKind.Production;
        public static string K_RANDOM_PROJECT_KEY = NWDRandom.RandomNetWorkedDataToken();
        public static string K_RANDOM_SECRET_KEY = NWDRandom.RandomNetWorkedDataToken();
        public static string K_RANDOM_CRUCIAL_KEY = NWDRandom.RandomNetWorkedDataToken();
        public static string K_RANDOM_TREAT_KEY = NWDRandom.RandomNetWorkedDataToken();
        #endregion

        #region Reference Construction

        public const string K_RESOURCES = "Resources";

// ulong :0 to 18 446 744 073 709 551 615
// ushort :	0 to 65 535 (use as range to prevent migration)
// reference with ulong =>  1 | 8 446 7 |  44 073 709 551 615
// reference => 0/1 | ushort | 10^14
// number of row : 1 000                     : 10^3  : thousand (kilo)
// number of row : 1 000 000                 : 10^6  : million (mega)
// number of row : 1 000 000 000             : 10^9  : billion (giga)
// number of row : 1 000 000 000 000         : 10^12 : trillion (tera)
// for security use 10^12 for 
// example for range 1244 => 1 01244 00 xxxxxxxxxxxx
        public const ushort K_REFERENCE_SIZE = 12;
        public const UInt64 K_REFERENCE_AREA_RANGE = 100000000000000;
        public const UInt64 K_REFERENCE_AREA_GLOBAL = K_REFERENCE_AREA_RANGE * 100000;

        public const ushort K_REFERENCE_UNIQUE_SIZE = 16;

        #endregion

        #region Projects folders tree

        public const string NWD3Assemblies = "###NWD3Assemblies";

        public const string NWDCore = "NWDCore";

        public const string NWDCustomModels = "NWDCustomModels";
        public const string NWDEditionToolbox = "NWDEditionToolbox";
        public const string NWDEngine = "NWDEngine";
        public const string NWDFoundation = "NWDFoundation";
        public const string NWDStandardModels = "NWDStandardModels";

        public const string NWDDevTeam = "NWDDevTeam";

        public const string NWDUnity = "NWDUnity";
        public const string NWDUnityCustomEdition = "NWDUnityCustomEdition";
        public const string NWDUnityEditor = "NWDUnityEditor";
        public const string NWDUnityEditorTest = "NWDUnityEditorTest";
        public const string NWDUnityRuntime = "NWDUnityRuntime";
        public const string NWDUnityRuntimeTest = "NWDUnityRuntimeTest";
        public const string NWDUnityStandardEdition = "NWDUnityStandardEdition";

        public const string NWDWeb = "NWDWeb";
        public const string NWDCluster = "NWDCluster";

        #endregion

        #region For editor webservices delay

        public const int Editor_CheckRightsRepeatEvery = 60;
        public const int Editor_CheckDatasRepeatEvery = 30;
        public const int Editor_UpdateDataSelectedRepeatEvery = 10;
        public const int Editor_RemoveLockerAfter = 600;

        #endregion

        #region Constants Values

        public const string K_NWDURL = "https://www.net-worked-data.com/";
        public const string C_EMPTY_STRING = "";

        public const string K_MINUS = "-";
        public const string K_HASHTAG = "#";
        public const string K_A = "A";

        public const string K_ReturnLine = "\n";

        public const string K_NWDConfigExtension = ".json";
        public const string K_NWDConfigUnityEditorPath = "NWDConfigUnityEditor" + K_NWDConfigExtension;
        public const string K_NWDConfigUnityRuntimePath = "NWDConfigUnityRuntime" + K_NWDConfigExtension;
        public const string K_NWDConfigClusterPath = "appsettings" + K_NWDConfigExtension;

        #endregion

        #region Statics Values

        public static string K_EMPTY_STRING = string.Empty;

        public static CultureInfo FormatCountry = CultureInfo.InvariantCulture;

        public static string FloatFormat = "F5";
        public static string FloatSQLFormat = "5";
        public static string DoubleFormat = "F5";
        public static string DoubleSQLFormat = "5";

        public static string K_DEVELOPMENT_NAME = "Preview";
        public static string K_QUALIFICATION_NAME = "Qualification";
        public static string K_PREPRODUCTION_NAME = "Preprod";
        public static string K_PLAYTEST_NAME = "Playtest";
        public static string K_PRODUCTION_NAME = "Prod";
        public static string K_POSTPRODUCTION_NAME = "Postprod";

        public static string kStandardSeparator = "|";
        public static string kStandardSeparatorSubstitute = "@0#";

        public static string kFieldNone = "none";
        public static string kFieldEmpty = "empty";
        public static string kFieldNotEmpty = "not empty";
        public static char kFieldSeparatorA_char = '•';
        public static char kFieldSeparatorB_char = ':';
        public static char kFieldSeparatorC_char = '_';
        public static char kFieldSeparatorD_char = '∆';
        public static char kFieldSeparatorE_char = '∂';
        public static char kFieldSeparatorF_char = ';';

        public static string kFieldSeparatorA = string.Empty + kFieldSeparatorA_char;
        public static string kFieldSeparatorB = string.Empty + kFieldSeparatorB_char;
        public static string kFieldSeparatorC = string.Empty + kFieldSeparatorC_char;
        public static string kFieldSeparatorD = string.Empty + kFieldSeparatorD_char;
        public static string kFieldSeparatorE = string.Empty + kFieldSeparatorE_char;
        public static string kFieldSeparatorF = string.Empty + kFieldSeparatorF_char;

        public static string kFieldSeparatorASubstitute = "@1#";
        public static string kFieldSeparatorBSubstitute = "@2#";
        public static string kFieldSeparatorCSubstitute = "@3#";
        public static string kFieldSeparatorDSubstitute = "@4#";
        public static string kFieldSeparatorESubstitute = "@5#";

        public static string kPrefSaltValidKey = "SaltValid";
        public static string kPrefSaltAKey = "SaltA";
        public static string kPrefSaltBKey = "SaltB";

        #endregion
    }
}