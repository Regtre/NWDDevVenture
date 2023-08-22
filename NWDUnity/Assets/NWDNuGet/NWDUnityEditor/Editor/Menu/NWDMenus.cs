using NWDFoundation.Logger;
using NWDUnityEditor.Constants;
using NWDUnityEditor.Logger;
using NWDUnityEditor.Windows;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Menus
{
    static public class NWDMenus
    {
        public const string K_NETWORKEDDATA = "Net-Worked-Data v3/";
        public const string K_NETWORKEDDATAMODELS = K_NETWORKEDDATA + "Models/";
        public const int K_START_INDEX = 0;
        [MenuItem(K_NETWORKEDDATA + "Developed by idéMobi™", false, K_START_INDEX + 1)]
        public static void DevelopedBy()
        {
            if (EditorUtility.DisplayDialog(
                NWDEditorConstants.K_ALERT_IDEMOBI_TITLE,
                NWDEditorConstants.K_ALERT_IDEMOBI_MESSAGE,
                NWDEditorConstants.K_ALERT_IDEMOBI_OK,
                NWDEditorConstants.K_ALERT_IDEMOBI_SEE_WEBSITE))
            {
            }
            else
            {
                Application.OpenURL(NWDEditorConstants.K_ALERT_IDEMOBI_DOC_HTTP);
            }
        }
        public const int K_PLAN_INDEX = 50;
        //[MenuItem(NWXEditorMenu.K_NETWORKEDDATA + "Register Net-Worked-Data©", true, NWXEditorMenu.K_PLAN_INDEX + 1)]
        //public static bool NWDOnBoarding_Validate()
        //{
        //    return string.IsNullOrEmpty(NWDLauncherUnityEditor.GetConfig().GetUserSecretToken());
        //}
        [MenuItem(NWDMenus.K_NETWORKEDDATA + "Register Net-Worked-Data©", false, NWDMenus.K_PLAN_INDEX + 1)]
        public static void NWDOnBoarding_Menu()
        {
            NWDOnBoarding.SharedInstanceFocus();
        }

        //public const string K_PLAN = "Plan";
        //[MenuItem(NWDMenus.K_NETWORKEDDATA + "Plan", false, NWDMenus.K_PLAN_INDEX + 2)]
        //public static void NWDPlanManagement_Menu()
        //{
        //    NWDPlanManagement.SharedInstanceFocus();
        //}

        public const int K_EDITOR_INDEX = 100;
        public const string K_DEV_TEAM = "DevTeam/";

        public const string K_ENVIRONMENT = "Environments/";
        [MenuItem(NWDMenus.K_NETWORKEDDATA + NWDMenus.K_ENVIRONMENT + "Configuration", false, NWDMenus.K_EDITOR_INDEX + 1)]
        public static void NWDEnvironmentManagement_Menu()
        {
            NWDEnvironmentConfiguration.SharedInstanceFocus();
        }
        //[MenuItem(NWDMenus.K_NETWORKEDDATA + NWDMenus.K_ENVIRONMENT + "Chooser", false, NWDMenus.K_EDITOR_INDEX + 2)]
        //public static void NWDEnvironmentChooser_Menu()
        //{
        //    NWDEditorFooter.SharedInstanceFocus();
        //}
        [MenuItem(NWDMenus.K_NETWORKEDDATA + NWDMenus.K_ENVIRONMENT + "Fake devices", false, NWDMenus.K_EDITOR_INDEX + 21)]
        public static void NWDEnvironmentFakeAccount_Menu()
        {
            NWDFakeDeviceConfiguration.SharedInstanceFocus();
        }

        //public const int K_MODELS_INDEX = 200;
        //public const string K_MODELS = "Models/";
        //[MenuItem(K_NETWORKEDDATA + K_MODELS + "Create new models", true, K_MODELS_INDEX)]
        //public static bool ModelsCreationBool()
        //{
        //    NWDPlanResume tPlan = NWDPlanResume.GetPlanResume(NWDLauncherUnityEditor.GetConfig().Plan);
        //    return tPlan.CustomModels;
        //}
        //[MenuItem(K_NETWORKEDDATA + K_MODELS + "Create new models", false, K_MODELS_INDEX)]
        //public static void ModelsCreation()
        //{
        //}


        //public const string K_DOCUMENTATION = "Documentation/";
        //public const string K_APPLICATION = "Application/";
        //public const string K_LOCALIZATION = "Localization/";
        //public const string K_CLUSTER = "Cluster/";

        //public const int K_TOOLS_INDEX = 700;
        //public const string K_TOOLS = "Tools/";


        //public const int K_ENGINE_MANAGEMENT_INDEX = 800;
        //[MenuItem(K_NETWORKEDDATA + "Engine management", true, K_ENGINE_MANAGEMENT_INDEX)]
        //public static bool EngineModelsFalse() { return false; }
        //[MenuItem(K_NETWORKEDDATA + "Engine management", false, K_ENGINE_MANAGEMENT_INDEX)]
        //public static void EngineModels()
        //{
        //}

        //public const int K_PLAYER_MANAGEMENT_INDEX = 900;
        //[MenuItem(K_NETWORKEDDATA + "Players management", true, K_PLAYER_MANAGEMENT_INDEX)]
        //public static bool PlayersModelsFalse() { return false; }
        //[MenuItem(K_NETWORKEDDATA + "Players management", false, K_PLAYER_MANAGEMENT_INDEX)]
        //public static void PlayersModels()
        //{
        //}


        public const int K_PARAMETERS_INDEX = 900;
        [MenuItem(K_NETWORKEDDATA + "Parameters", true, K_PARAMETERS_INDEX)]
        public static bool ParametersFalse() { return false; }
        [MenuItem(K_NETWORKEDDATA + "Parameters", false, K_PARAMETERS_INDEX)]
        public static void Parameters()
        {
        }

        public const string K_LOGLEVEL = "Log Level/";
        [MenuItem(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Trace), true, K_PARAMETERS_INDEX + 1)]
        public static bool LogLevelTraceValidate()
        {
            Menu.SetChecked(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Trace), NWDLogger.LogLevel() == NWDLogLevel.Trace);
            return true;
        }
        [MenuItem(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Trace), false, K_PARAMETERS_INDEX + 1)]
        public static void LogLevelTrace()
        {
            NWDLogger.SetLogLevel(NWDLogLevel.Trace);
        }

        [MenuItem(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Debug), true, K_PARAMETERS_INDEX + 2)]
        public static bool LogLevelDebugValidate()
        {
            Menu.SetChecked(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Debug), NWDLogger.LogLevel() == NWDLogLevel.Debug);
            return true;
        }
        [MenuItem(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Debug), false, K_PARAMETERS_INDEX + 2)]
        public static void LogLevelDebug()
        {
            NWDLogger.SetLogLevel(NWDLogLevel.Debug);
        }

        [MenuItem(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Information), true, K_PARAMETERS_INDEX + 3)]
        public static bool LogLevelInformationValidate()
        {
            Menu.SetChecked(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Information), NWDLogger.LogLevel() == NWDLogLevel.Information);
            return true;
        }
        [MenuItem(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Information), false, K_PARAMETERS_INDEX + 3)]
        public static void LogLevelInformation()
        {
            NWDLogger.SetLogLevel(NWDLogLevel.Information);
        }

        [MenuItem(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Warning), true, K_PARAMETERS_INDEX + 4)]
        public static bool LogLevelWarningValidate()
        {
            Menu.SetChecked(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Warning), NWDLogger.LogLevel() == NWDLogLevel.Warning);
            return true;
        }
        [MenuItem(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Warning), false, K_PARAMETERS_INDEX + 4)]
        public static void LogLevelWarning()
        {
            NWDLogger.SetLogLevel(NWDLogLevel.Warning);
        }

        [MenuItem(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Error), true, K_PARAMETERS_INDEX + 5)]
        public static bool LogLevelErrorValidate()
        {
            Menu.SetChecked(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Error), NWDLogger.LogLevel() == NWDLogLevel.Error);
            return true;
        }
        [MenuItem(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Error), false, K_PARAMETERS_INDEX + 5)]
        public static void LogLevelError()
        {
            NWDLogger.SetLogLevel(NWDLogLevel.Error);
        }

        [MenuItem(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Critical), true, K_PARAMETERS_INDEX + 6)]
        public static bool LogLevelCriticalValidate()
        {
            Menu.SetChecked(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Critical), NWDLogger.LogLevel() == NWDLogLevel.Critical);
            return true;
        }
        [MenuItem(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.Critical), false, K_PARAMETERS_INDEX + 6)]
        public static void LogLevelCritical()
        {
            NWDLogger.SetLogLevel(NWDLogLevel.Critical);
        }

        [MenuItem(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.None), true, K_PARAMETERS_INDEX + 7)]
        public static bool LogLevelNoneValidate()
        {
            Menu.SetChecked(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.None), NWDLogger.LogLevel() == NWDLogLevel.None);
            return true;
        }
        [MenuItem(K_NETWORKEDDATA + K_LOGLEVEL + nameof(NWDLogLevel.None), false, K_PARAMETERS_INDEX + 7)]
        public static void LogLevelNone()
        {
            NWDLogger.SetLogLevel(NWDLogLevel.None);
        }

        public const int K_MODULES_MANAGEMENT_INDEX = 1000;
        [MenuItem(K_NETWORKEDDATA + "Modules management", true, K_MODULES_MANAGEMENT_INDEX)]
        public static bool ModulesModelsFalse() { return false; }
        [MenuItem(K_NETWORKEDDATA + "Modules management", false, K_MODULES_MANAGEMENT_INDEX)]
        public static void ModulesModels()
        {
        }

        //public const int K_CUSTOMS_MANAGEMENT_INDEX = 2000;
        //[MenuItem(K_NETWORKEDDATA + "Customs management", true, K_CUSTOMS_MANAGEMENT_INDEX - 1)]
        //public static bool CustomsModelsFalse() { return false; }
        //[MenuItem(K_NETWORKEDDATA + "Customs management", false, K_CUSTOMS_MANAGEMENT_INDEX - 1)]
        //public static void CustomsModels()
        //{
        //}
    }
}