using System;
using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;
using NWDWebRuntime.Tools;

namespace NWDFoundation.Logger
{
    public static class NWDLogger
    {
        private static INWDLogger Writer = new NWDConsoleLogger();
        public const string K_CONFIG_ALREADY_LOADED = "{0} already loaded before!";
        public const string K_RAZOR_RUNTIME_COMPILATION_ENABLE = "{0} active Razor Runtime Compilation by {1} configuration!";
        public const string K_RAZOR_RUNTIME_COMPILATION_DISABLE = "{0} disable Razor Runtime Compilation by {1} configuration (IsDevelopment = false)!";
        public const string K_RAZOR_COMPILE_NOT_FOR_DEV = "{0} disable Razor Runtime from parameter : sRuntimeCompileForDev";
        public const string K_FOUND_IN_APP_SETTINGS = "{0} config found in app" + "settings.json or in {0}.json!";
        public const string K_NOT_FOUND_IN_APP_SETTINGS = "{0} config not found in app" + "settings.json or in {0}.json!";
        public const string K_CONFIG_JSON_EXAMPLE = "{0}.json Example";

        public static void SetWriter(INWDLogger sWriter)
        {
            Writer = sWriter;
        }

        private static string[] SplitJson(string sJson)
        {
            return sJson.Replace(",", ",•").Replace("{", "{•").Replace("}", "•}").Split('•');
        }
        public static string[] SplitObjectSerializable(Object sObject)
        {
            return JsonConvert.SerializeObject(sObject, Formatting.Indented).Replace(",\\\"", ",\n\\\"").Replace("{\\\"", "{\n\\\"").Replace("\\\"}", "\\\"\n}").Split('\n');
        }
        // /// <summary>
        // /// Send a log with the default <see cref="NWDLogLevel"/> of the <see cref="Writer"/>.
        // /// </summary>
        // /// <param name="sString"></param>
        // /// <param name="sThis"></param>
        // public static void Log(string sString, object sThis = null)
        // {
        //     WriteLog(Writer?.DefaultLogLevel() ?? NWDLogLevel.None, sString, sThis);
        // }
        //
        // /// <summary>
        // /// Send a log.
        // /// </summary>
        // /// <param name="sLevel"></param>
        // /// <param name="sString"></param>
        // /// <param name="sThis"></param>
        // public static void Log(NWDLogLevel sLogLevel, string sString, object sThis = null)
        // {
        //     WriteLog(sLogLevel, sString, sThis);
        // }

        /// <summary>
        /// Checks if a log for a given <see cref="NWDLogLevel"/> can be written by the <see cref="Writer"/>.
        /// </summary>
        /// <param name="sLevel"></param>
        /// <returns></returns>
        private static bool CanWrite(NWDLogLevel sLogLevel)
        {
            return CanWrite(Writer, sLogLevel);
        }

        /// <summary>
        /// Checks if a log for a given <see cref="NWDLogLevel"/> can be written by a <see cref="INWDLogger"/>.
        /// </summary>
        /// <param name="sWriter"></param>
        /// <param name="sLogLevel"></param>
        /// <returns></returns>
        internal static bool CanWrite(INWDLogger sWriter, NWDLogLevel sLogLevel)
        {
            // Cannot write a log if the writer is not set
            if (sWriter == null)
            {
                return false;
            }

            // Cannot write a Log if the writer is not activated
            if (!sWriter.IsActivated())
            {
                return false;
            }

            // Cannot write a log if logLevel is below the wanted one or if it is equal to none
            if (sLogLevel < sWriter.LogLevel() || sLogLevel == NWDLogLevel.None)
            {
                return false;
            }

            return true;
        }

        public static void TestLayout()
        {
            NWDLogger.TraceAttention("test of layout");
            NWDLogger.TraceError("test of layout");
            NWDLogger.TraceFailed("test of layout");
            NWDLogger.TraceSuccess("test of layout");
            NWDLogger.TraceTodo("test of layout");
            NWDLogger.Exception(null);
            NWDLogger.Exception(new Exception("Exception's message"));
            NWDLogger.Trace("test of layout");
            NWDLogger.Information("test of layout number 1");
            NWDLogger.Debug("test of layout for debug");
            NWDLogger.Warning("test of layout !warning! ");
            NWDLogger.Error("test of layout ... Arghhh an error");
            NWDLogger.Critical("test of layout ... critical ... I'am dead!");
            
            NWDLogger.Trace("test of layout");
            NWDLogger.Trace("my title",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc luctus purus sed justo viverra cursus. Cras tincidunt risus a dui lacinia ",
                "convallis. Etiam tempor velit vitae mi pharetra mollis a vel purus. Aenean sem lorem, pharetra eget mauris eget, porta ornare urna. ",
                "Pellentesque feugiat, tellus sit amet tempus dictum, felis nisi porta enim, at tempus nibh lectus at mauris. Morbi tincidunt, lacus ac ",
                "pellentesque porta, ligula nisl consectetur nunc, eget ultrices dui nunc ut orci. Proin ac quam vitae lacus sollicitudin dictum eget ut justo. ");


            NWDLogger.Information("test of layout number 1");
            NWDLogger.Information("my title number 1",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc luctus purus sed justo viverra cursus. Cras tincidunt risus a dui lacinia ",
                "convallis. Etiam tempor velit vitae mi pharetra mollis a vel purus. Aenean sem lorem, pharetra eget mauris eget, porta ornare urna. ",
                "Pellentesque feugiat, tellus sit amet tempus dictum, felis nisi porta enim, at tempus nibh lectus at mauris. Morbi tincidunt, lacus ac ",
                "pellentesque porta, ligula nisl consectetur nunc, eget ultrices dui nunc ut orci. Proin ac quam vitae lacus sollicitudin dictum eget ut justo. ");


            NWDLogger.Debug("test of layout for debug");
            NWDLogger.Debug("my title for debug",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc luctus purus sed justo viverra cursus. Cras tincidunt risus a dui lacinia ",
                "convallis. Etiam tempor velit vitae mi pharetra mollis a vel purus. Aenean sem lorem, pharetra eget mauris eget, porta ornare urna. ",
                "Pellentesque feugiat, tellus sit amet tempus dictum, felis nisi porta enim, at tempus nibh lectus at mauris. Morbi tincidunt, lacus ac ",
                "pellentesque porta, ligula nisl consectetur nunc, eget ultrices dui nunc ut orci. Proin ac quam vitae lacus sollicitudin dictum eget ut justo. ");


            NWDLogger.Warning("test of layout !warning! ");
            NWDLogger.Warning("my title !warning! ",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc luctus purus sed justo viverra cursus. Cras tincidunt risus a dui lacinia ",
                "convallis. Etiam tempor velit vitae mi pharetra mollis a vel purus. Aenean sem lorem, pharetra eget mauris eget, porta ornare urna. ",
                "Pellentesque feugiat, tellus sit amet tempus dictum, felis nisi porta enim, at tempus nibh lectus at mauris. Morbi tincidunt, lacus ac ",
                "pellentesque porta, ligula nisl consectetur nunc, eget ultrices dui nunc ut orci. Proin ac quam vitae lacus sollicitudin dictum eget ut justo. ");


            NWDLogger.Error("test of layout ... Arghhh an error");
            NWDLogger.Error("my title ... Arghhh an error",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc luctus purus sed justo viverra cursus. Cras tincidunt risus a dui lacinia ",
                "convallis. Etiam tempor velit vitae mi pharetra mollis a vel purus. Aenean sem lorem, pharetra eget mauris eget, porta ornare urna. ",
                "Pellentesque feugiat, tellus sit amet tempus dictum, felis nisi porta enim, at tempus nibh lectus at mauris. Morbi tincidunt, lacus ac ",
                "pellentesque porta, ligula nisl consectetur nunc, eget ultrices dui nunc ut orci. Proin ac quam vitae lacus sollicitudin dictum eget ut justo. ");


            NWDLogger.Critical("test of layout ... critical ... I'am dead!");
            NWDLogger.Critical("my title ... critical ... I'am dead!",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc luctus purus sed justo viverra cursus. Cras tincidunt risus a dui lacinia ",
                "convallis. Etiam tempor velit vitae mi pharetra mollis a vel purus. Aenean sem lorem, pharetra eget mauris eget, porta ornare urna. ",
                "Pellentesque feugiat, tellus sit amet tempus dictum, felis nisi porta enim, at tempus nibh lectus at mauris. Morbi tincidunt, lacus ac ",
                "pellentesque porta, ligula nisl consectetur nunc, eget ultrices dui nunc ut orci. Proin ac quam vitae lacus sollicitudin dictum eget ut justo. ");
        }

        /// <summary>
        /// Writes the log using the wanted <see cref="Writer"/>.
        /// </summary>
        /// <param name="sLogLevel"></param>
        /// <param name="sLogCategory"></param>
        /// <param name="sString"></param>
        /// <param name="sObject"></param>
        private static void WriteLog(NWDLogLevel sLogLevel, NWDLogCategory sLogCategory, string sString, object? sObject)
        {
            if (CanWrite(sLogLevel))
            {
                Writer.WriteLog(sLogLevel, sLogCategory, sString, sObject);
            }
        }

        /// <summary>
        /// Writes the log using the wanted <see cref="Writer"/>.
        /// </summary>
        /// <param name="sLogLevel"></param>
        /// <param name="sString"></param>
        /// <param name="sTitle"></param>
        /// <param name="sObject"></param>
        /// <param name="sLogCategory"></param>
        /// <param name="sMessages"></param>
        private static void WriteLog(NWDLogLevel sLogLevel, NWDLogCategory sLogCategory, string sTitle, object? sObject, params string[] sMessages)
        {
            if (CanWrite(sLogLevel))
            {
                Writer.WriteLog(sLogLevel, sLogCategory, sTitle, sObject, sMessages);
            }
        }

        /// <summary>
        /// Sends a <see cref="NWDLogLevel.Trace"/> log using the <see cref="Writer"/>.
        /// </summary>
        /// <param name="sMessage"></param>
        /// <param name="sObject"></param>
        public static void Trace(string sMessage, object? sObject = null)
        {
            WriteLog(NWDLogLevel.Trace, NWDLogCategory.No, sMessage, sObject);
        }

        public static void TraceTodo(string sMessage)
        {
            WriteLog(NWDLogLevel.Trace, NWDLogCategory.Todo, sMessage, null);
        }

        public static void TraceSuccess(string sMessage)
        {
            WriteLog(NWDLogLevel.Trace, NWDLogCategory.Success, sMessage, null);
        }

        public static void TraceFailed(string sMessage)
        {
            WriteLog(NWDLogLevel.Trace, NWDLogCategory.Failed, sMessage, null);
        }

        public static void TraceAttention(string sMessage)
        {
            WriteLog(NWDLogLevel.Trace, NWDLogCategory.Attention, sMessage, null);
        }

        public static void TraceError(string sMessage)
        {
            WriteLog(NWDLogLevel.Trace, NWDLogCategory.Error, sMessage, null);
        }

        public static void Trace(string sTitle, params string[] sMessages)
        {
            WriteLog(NWDLogLevel.Trace, NWDLogCategory.No, sTitle, null, sMessages);
        }

        /// <summary>
        /// Sends a <see cref="NWDLogLevel.Debug"/> log using the <see cref="Writer"/>.
        /// </summary>
        /// <param name="sMessage"></param>
        /// <param name="sObject"></param>
        public static void Debug(string sMessage, object? sObject = null)
        {
            WriteLog(NWDLogLevel.Debug, NWDLogCategory.No, sMessage, sObject);
        }

        public static void Debug(string sTitle, params string[] sMessages)
        {
            WriteLog(NWDLogLevel.Debug, NWDLogCategory.No, sTitle, null, sMessages);
        }

        /// <summary>
        /// Sends a <see cref="NWDLogLevel.Information"/> log using the <see cref="Writer"/>.
        /// </summary>
        /// <param name="sMessage"></param>
        /// <param name="sObject"></param>
        public static void Information(string sMessage, object? sObject = null)
        {
            WriteLog(NWDLogLevel.Information, NWDLogCategory.No, sMessage, sObject);
        }

        public static void Information(string sTitle, params string[] sMessages)
        {
            WriteLog(NWDLogLevel.Information, NWDLogCategory.No, sTitle, null, sMessages);
        }

        public static void Exception(Exception sObject)
        {
            WriteLog(NWDLogLevel.Warning, NWDLogCategory.Exception, "Exception!", sObject);
        }

        public static void Exception(string sMessage, Exception sObject)
        {
            WriteLog(NWDLogLevel.Warning, NWDLogCategory.Exception, "Exception!", sObject, sMessage);
        }

        /// <summary>
        /// Sends a <see cref="NWDLogLevel.Warning"/> log using the <see cref="Writer"/>.
        /// </summary>
        /// <param name="sMessage"></param>
        /// <param name="sObject"></param>
        public static void Warning(string sMessage, object? sObject = null)
        {
            WriteLog(NWDLogLevel.Warning, NWDLogCategory.No, sMessage, sObject);
        }

        public static void Warning(string sTitle, params string[] sMessages)
        {
            WriteLog(NWDLogLevel.Warning, NWDLogCategory.No, sTitle, null, sMessages);
        }

        /// <summary>
        /// Sends a <see cref="NWDLogLevel.Error"/> log using the <see cref="Writer"/>.
        /// </summary>
        /// <param name="sMessage"></param>
        /// <param name="sObject"></param>
        public static void Error(string sMessage, object? sObject = null)
        {
            WriteLog(NWDLogLevel.Error, NWDLogCategory.No, sMessage, sObject);
        }

        public static void Error(string sTitle, params string[] sMessages)
        {
            WriteLog(NWDLogLevel.Error, NWDLogCategory.No, sTitle, null, sMessages);
        }

        /// <summary>
        /// Sends a <see cref="NWDLogLevel.Critical"/> log using the <see cref="Writer"/>.
        /// </summary>
        /// <param name="sMessage"></param>
        /// <param name="sObject"></param>
        public static void Critical(string sMessage, object? sObject = null)
        {
            WriteLog(NWDLogLevel.Critical, NWDLogCategory.No, sMessage, sObject);
        }

        public static void Critical(string sTitle, params string[] sMessages)
        {
            WriteLog(NWDLogLevel.Critical, NWDLogCategory.No, sTitle, null, sMessages);
        }

        public static NWDLogLevel LogLevel()
        {
            return Writer?.LogLevel() ?? NWDLogLevel.Information;
        }

        public static void SetLogLevel(NWDLogLevel sLevel)
        {
            Writer?.SetLogLevel(sLevel);
        }
    }
}