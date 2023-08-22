using NWDFoundation.Logger;
using System;
#nullable enable
namespace NWDWebRuntime.Tools
{
    public class NWDConsoleLogger : INWDLogger
    {
        private NWDLogLevel _Level = NWDLogLevel.None;
        private const string _INDENT = " ";
        private const string _PADDING = "  ";
        private const string _BORDER_LEFT= " ";
        private const string _SEPARATOR = "--------------------------------------------------------------------------------------------------------------------------------";
        public NWDConsoleLogger()
        {
            _Level = NWDLogLevel.Trace;
        }
        
        public NWDConsoleLogger(NWDLogLevel sLogLevel)
        {
            _Level = sLogLevel;
        }

        public void SetLogLevel(NWDLogLevel sLogLevel)
        {
            _Level = sLogLevel;
        }

        public void LoadLogLevel()
        {
        }

        public NWDLogLevel DefaultLogLevel()
        {
            return NWDLogLevel.Information;
        }

        public bool IsActivated()
        {
            return true;
        }

        public NWDLogLevel LogLevel()
        {
            return _Level;
        }

        private void WriteCategory(NWDLogCategory sLogCategory)
        {
            Console.ResetColor();
            switch (sLogCategory)
            {
                case NWDLogCategory.No:
                    break;
                case NWDLogCategory.Todo:
            Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("    TODO    ");
                    break;
                case NWDLogCategory.Success:
                    Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("   SUCCESS   ");
                    break;
                case NWDLogCategory.Error:
                    Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("   ERROR    ");
                    break;
                case NWDLogCategory.Attention:
                    Console.Write(" ");
                    // Console.Write(" ⚠️ ");
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("  WARNING   ");
                    break;
                case NWDLogCategory.Exception:
                    Console.Write(" ");
                    // Console.Write(" 💀 ");
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" EXCEPTION  ");
                    break;
                case NWDLogCategory.Failed:
                    Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("   FAILED   ");
                    break;
            }
            Console.ResetColor();
            Console.Write(" ");
        }
        private void WriteIcon(NWDLogLevel sLogLevel, string sAdd="")
        {
                    Console.ResetColor();
            switch (sLogLevel)
            {
                case NWDLogLevel.Trace:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(" 🤖   trace" + sAdd + " ");
                    break;
                case NWDLogLevel.Debug:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(" 🤔   debug" + sAdd + " ");
                    break;
                case NWDLogLevel.Information:
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" 🧐    info"  + sAdd + " ");
                    break;
                case NWDLogLevel.Warning:
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" 😰 warning" + sAdd + " ");
                    break;
                case NWDLogLevel.Error:
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" 🤬   error" + sAdd + " ");
                    break;
                case NWDLogLevel.Critical:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" 💀critical" + sAdd + " ");
                    break;
                case NWDLogLevel.None:
                    break;
            }
            Console.ResetColor();
        }

        public void WriteObject( object? sObject)
        {
            if (sObject != null)
            {
                Exception? tException = sObject as Exception;
                if (tException != null)
                {
                    Console.Write(_PADDING);
                    Console.WriteLine(tException.ToString());
                }
            }
        }

        public void WriteLog(NWDLogLevel sLogLevel, NWDLogCategory sLogCategory, string sString, object? sObject)
        {
            WriteIcon(sLogLevel, ":");
            WriteCategory(sLogCategory);
            Console.ResetColor();
            switch (sLogLevel)
            {
                case NWDLogLevel.Trace:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(sString);
                    WriteObject(sObject);
                    break;
                case NWDLogLevel.Debug:
                    // NWDLogger.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(sString);
                    WriteObject(sObject);
                    break;
                case NWDLogLevel.Information:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(sString);
                    WriteObject(sObject);
                    Console.ResetColor();
                    break;
                case NWDLogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(sString);
                    WriteObject(sObject);
                    break;
                case NWDLogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(sString);
                    WriteObject(sObject);
                    break;
                case NWDLogLevel.Critical:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(sString);
                    WriteObject(sObject);
                    break;
                case NWDLogLevel.None:
                    break;
            }
            Console.ResetColor();
            Console.Write(string.Empty);
        }
        
        public void WriteLog(NWDLogLevel sLogLevel, NWDLogCategory sLogCategory, string sTitle, object? sObject, string[] sMessages)
        {
            WriteIcon(sLogLevel, ">");
            WriteCategory(sLogCategory);
            Console.ResetColor();
            string tTitle = sTitle;
            if (tTitle.Length < _SEPARATOR.Length - 2)
            {
                string tSeparator = _SEPARATOR.Substring(0, _SEPARATOR.Length - ((int)sTitle.Length + 2));
                tTitle = tSeparator.Substring(0, (int)(tSeparator.Length / 2)) + " " + sTitle + " ";
                tTitle = tTitle + _SEPARATOR.Substring(0, _SEPARATOR.Length - tTitle.Length);
            }
            ConsoleColor tBack = ConsoleColor.Black;
            ConsoleColor tFore = ConsoleColor.Gray;
            switch (sLogLevel)
            {
                case NWDLogLevel.Trace:
                    tBack = ConsoleColor.Black;
                    tFore = ConsoleColor.Gray;
                    Console.WriteLine(" ");
                    Console.Write(_PADDING);
                    Console.BackgroundColor = tBack;
                    Console.ForegroundColor = tFore;
                    Console.Write( tTitle);
                    Console.ResetColor();
                    Console.WriteLine(" ");
                    Console.Write(string.Empty);
                    foreach (string tStr in sMessages)
                    {
                        Console.Write(_PADDING);
                        Console.BackgroundColor = tBack;
                        Console.ForegroundColor = tFore;
                        Console.Write(_BORDER_LEFT);
                        Console.ResetColor();
                        Console.WriteLine(_INDENT + tStr);
                    }
                    WriteObject(sObject);
                    Console.Write(_PADDING);
                    Console.BackgroundColor = tBack;
                    Console.ForegroundColor = tFore;
                    Console.Write(_SEPARATOR);
                    Console.ResetColor();
                    Console.WriteLine(" ");
                    Console.WriteLine(" ");
                    break;
                
                case NWDLogLevel.Debug:
                    tBack = ConsoleColor.Black;
                    tFore = ConsoleColor.Gray;
                    Console.WriteLine(" ");
                    Console.Write(_PADDING);
                    Console.BackgroundColor = tBack;
                    Console.ForegroundColor = tFore;
                    Console.Write( tTitle);
                    Console.ResetColor();
                    Console.WriteLine(" ");
                    Console.Write(string.Empty);
                    foreach (string tStr in sMessages)
                    {
                        Console.Write(_PADDING);
                        Console.BackgroundColor = tBack;
                        Console.ForegroundColor = tFore;
                        Console.Write(_BORDER_LEFT);
                        Console.ResetColor();
                        Console.WriteLine(_INDENT + tStr);
                    }
                    WriteObject(sObject);
                    Console.Write(_PADDING);
                    Console.BackgroundColor = tBack;
                    Console.ForegroundColor = tFore;
                    Console.Write(_SEPARATOR);
                    Console.ResetColor();
                    Console.WriteLine(" ");
                    Console.WriteLine(" ");
                    break;
                case NWDLogLevel.Information:
                    tBack = ConsoleColor.Blue;
                    tFore = ConsoleColor.Black;
                    Console.WriteLine(" ");
                    Console.Write(_PADDING);
                    Console.BackgroundColor = tBack;
                    Console.ForegroundColor = tFore;
                    Console.Write( tTitle);
                    Console.ResetColor();
                    Console.WriteLine(" ");
                    Console.Write(string.Empty);
                    foreach (string tStr in sMessages)
                    {
                        Console.Write(_PADDING);
                        Console.BackgroundColor = tBack;
                        Console.ForegroundColor = tFore;
                        Console.Write(_BORDER_LEFT);
                        Console.ResetColor();
                        Console.WriteLine(_INDENT + tStr);
                    }
                    WriteObject(sObject);
                    Console.Write(_PADDING);
                    Console.BackgroundColor = tBack;
                    Console.ForegroundColor = tFore;
                    Console.Write(_SEPARATOR);
                    Console.ResetColor();
                    Console.WriteLine(" ");
                    Console.WriteLine(" ");
                    break;
                case NWDLogLevel.Warning:
                    tBack = ConsoleColor.Yellow;
                    tFore = ConsoleColor.Black;
                    Console.WriteLine(" ");
                    Console.Write(_PADDING);
                    Console.BackgroundColor = tBack;
                    Console.ForegroundColor = tFore;
                    Console.Write( tTitle);
                    Console.ResetColor();
                    Console.WriteLine(" ");
                    Console.Write(string.Empty);
                    foreach (string tStr in sMessages)
                    {
                        Console.Write(_PADDING);
                        Console.BackgroundColor = tBack;
                        Console.ForegroundColor = tFore;
                        Console.Write(_BORDER_LEFT);
                        Console.ResetColor();
                        Console.WriteLine(_INDENT + tStr);
                    }
                    WriteObject(sObject);
                    Console.Write(_PADDING);
                    Console.BackgroundColor = tBack;
                    Console.ForegroundColor = tFore;
                    Console.Write(_SEPARATOR);
                    Console.ResetColor();
                    Console.WriteLine(" ");
                    Console.WriteLine(" ");
                    break;
                case NWDLogLevel.Error:
                    tBack = ConsoleColor.Red;
                    tFore = ConsoleColor.Black;
                    Console.WriteLine(" ");
                    Console.Write(_PADDING);
                    Console.BackgroundColor = tBack;
                    Console.ForegroundColor = tFore;
                    Console.Write( tTitle);
                    Console.ResetColor();
                    Console.WriteLine(" ");
                    Console.Write(string.Empty);
                    foreach (string tStr in sMessages)
                    {
                        Console.Write(_PADDING);
                        Console.BackgroundColor = tBack;
                        Console.ForegroundColor = tFore;
                        Console.Write(_BORDER_LEFT);
                        Console.ResetColor();
                        Console.WriteLine(_INDENT + tStr);
                    }
                    WriteObject(sObject);
                    Console.Write(_PADDING);
                    Console.BackgroundColor = tBack;
                    Console.ForegroundColor = tFore;
                    Console.Write(_SEPARATOR);
                    Console.ResetColor();
                    Console.WriteLine(" ");
                    Console.WriteLine(" ");
                    break;
                case NWDLogLevel.Critical:
                    tBack = ConsoleColor.DarkRed;
                    tFore = ConsoleColor.Black;
                    Console.WriteLine(" ");
                    Console.Write(_PADDING);
                    Console.BackgroundColor = tBack;
                    Console.ForegroundColor = tFore;
                    Console.Write( tTitle);
                    Console.ResetColor();
                    Console.WriteLine(" ");
                    Console.Write(string.Empty);
                    foreach (string tStr in sMessages)
                    {
                        Console.Write(_PADDING);
                        Console.BackgroundColor = tBack;
                        Console.ForegroundColor = tFore;
                        Console.Write(_BORDER_LEFT);
                        Console.ResetColor();
                        Console.WriteLine(_INDENT + tStr);
                    }
                    WriteObject(sObject);
                    Console.Write(_PADDING);
                    Console.BackgroundColor = tBack;
                    Console.ForegroundColor = tFore;
                    Console.Write(_SEPARATOR);
                    Console.ResetColor();
                    Console.WriteLine(" ");
                    Console.WriteLine(" ");
                    break;
                case NWDLogLevel.None:
                    break;
            }
            Console.ResetColor();
            Console.Write(string.Empty);
        }
    }
}
#nullable disable