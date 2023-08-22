using NWDFoundation.Logger;
using System.Collections.Generic;

#nullable enable
namespace NWDWebRuntime.Tools
{
    public class NWDMultiLogger : INWDLogger
    {
        private List<INWDLogger> Writers = new List<INWDLogger>();
        private NWDLogLevel _Level = NWDLogLevel.None;

        public NWDMultiLogger(params INWDLogger[] sWriters)
        {
            SetLogLevel(NWDLogLevel.Trace);
            if (sWriters != null)
            {
                foreach (INWDLogger sWriter in sWriters)
                {
                    AddWritter(sWriter);
                }
            }
        }
        
        public NWDMultiLogger(NWDLogLevel sLogLevel, params INWDLogger[] sWriters)
        {
            SetLogLevel(sLogLevel);
            if (sWriters != null)
            {
                foreach (INWDLogger sWriter in sWriters)
                {
                    AddWritter(sWriter);
                }
            }
        }

        public void SetLogLevel(NWDLogLevel sLogLevel)
        {
            _Level = sLogLevel;
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

        public void WriteLog(NWDLogLevel sLogLevel, NWDLogCategory sLogCategory, string sString, object? sObject)
        {
            foreach (INWDLogger tWriter in Writers)
            {
                if (NWDLogger.CanWrite(tWriter, sLogLevel))
                {
                    tWriter.WriteLog(sLogLevel, sLogCategory, sString, sObject);
                }
            }
        }

        public void WriteLog(NWDLogLevel sLogLevel, NWDLogCategory sLogCategory, string sTitle, object? sObject, string[] sMessages)
        {
            foreach (INWDLogger tWriter in Writers)
            {
                if (NWDLogger.CanWrite(tWriter, sLogLevel))
                {
                    tWriter.WriteLog(sLogLevel, sLogCategory, sTitle, sObject, sMessages);
                }
            }
        }

        public bool AddWritter (INWDLogger sWriter)
        {
            bool rReturn = false;
            if (sWriter != null && !Writers.Contains(sWriter))
            {
                Writers.Add (sWriter);
                rReturn = true;
            }
            return rReturn;
        }

        public bool RemoveWritter(INWDLogger tWriter)
        {
            return Writers.Remove(tWriter);
        }
    }
}
#nullable disable