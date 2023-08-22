using NWDFoundation.Logger;
using System.Collections.Generic;

#nullable enable
namespace NWDWebRuntime.Tools
{
    public class NWDComplexLogger : INWDLogger
    {
        private List<INWDLogger> DefaultWriters = new List<INWDLogger>();
        private Dictionary<NWDLogLevel, List<INWDLogger>> SpecificWriters = new Dictionary<NWDLogLevel, List<INWDLogger>>();
        private NWDLogLevel _Level = NWDLogLevel.None;

        public NWDComplexLogger(params INWDLogger[] sWriters)
        {
            SetLogLevel(NWDLogLevel.Trace);
            if (sWriters != null)
            {
                foreach (INWDLogger sWriter in sWriters)
                {
                    AddDefaultWritter(sWriter);
                }
            }
        }
        
        public NWDComplexLogger(NWDLogLevel sLogLevel, params INWDLogger[] sWriters)
        {
            SetLogLevel(sLogLevel);
            if (sWriters != null)
            {
                foreach (INWDLogger sWriter in sWriters)
                {
                    AddDefaultWritter(sWriter);
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
            foreach (INWDLogger tWriter in DefaultWriters)
            {
                if (NWDLogger.CanWrite(tWriter, sLogLevel))
                {
                    tWriter.WriteLog(sLogLevel, sLogCategory, sString, sObject);
                }
            }

            if (SpecificWriters.TryGetValue(sLogLevel, out List<INWDLogger> tWriters))
            {
                foreach (INWDLogger tWriter in tWriters)
                {
                    tWriter.WriteLog(sLogLevel, sLogCategory, sString, sObject);
                }
            }
        }

        public void WriteLog(NWDLogLevel sLogLevel, NWDLogCategory sLogCategory, string sTitle, object? sObject, string[] sMessages)
        {
            foreach (INWDLogger tWriter in DefaultWriters)
            {
                if (NWDLogger.CanWrite(tWriter, sLogLevel))
                {
                    tWriter.WriteLog(sLogLevel, sLogCategory, sTitle, sObject, sMessages);
                }
            }

            if (SpecificWriters.TryGetValue(sLogLevel, out List<INWDLogger> tWriters))
            {
                foreach (INWDLogger tWriter in tWriters)
                {
                    tWriter.WriteLog(sLogLevel, sLogCategory, sTitle, sObject, sMessages);
                }
            }
        }

        public bool AddDefaultWritter (INWDLogger sWriter)
        {
            bool rReturn = false;
            if (sWriter != null && !DefaultWriters.Contains(sWriter))
            {
                DefaultWriters.Add (sWriter);
                rReturn = true;
            }
            return rReturn;
        }

        public bool RemoveDefaultWritter(INWDLogger sWriter)
        {
            return DefaultWriters.Remove(sWriter);
        }

        public bool AddSpecificWriter (INWDLogger sWriter, NWDLogLevel sLogLevels)
        {
            bool rReturn = false;
            if (sWriter != null)
            {
                if (sLogLevels.HasFlag(NWDLogLevel.Trace))
                {
                    rReturn |= AddSpecificWriterToDictionary(sWriter, NWDLogLevel.Trace);
                }
                if (sLogLevels.HasFlag(NWDLogLevel.Debug))
                {
                    rReturn |= AddSpecificWriterToDictionary(sWriter, NWDLogLevel.Debug);
                }
                if (sLogLevels.HasFlag(NWDLogLevel.Information))
                {
                    rReturn |= AddSpecificWriterToDictionary(sWriter, NWDLogLevel.Information);
                }
                if (sLogLevels.HasFlag(NWDLogLevel.Warning))
                {
                    rReturn |= AddSpecificWriterToDictionary(sWriter, NWDLogLevel.Warning);
                }
                if (sLogLevels.HasFlag(NWDLogLevel.Error))
                {
                    rReturn |= AddSpecificWriterToDictionary(sWriter, NWDLogLevel.Error);
                }
                if (sLogLevels.HasFlag(NWDLogLevel.Critical))
                {
                    rReturn |= AddSpecificWriterToDictionary(sWriter, NWDLogLevel.Critical);
                }
            }

            return rReturn;
        }

        private bool AddSpecificWriterToDictionary (INWDLogger sWriter, NWDLogLevel sLogLevel)
        {
            bool rReturn = false;
            if (!SpecificWriters.ContainsKey(sLogLevel))
            {
                SpecificWriters.Add(sLogLevel, new List<INWDLogger>());
            }
            
            if (!SpecificWriters[sLogLevel].Contains(sWriter))
            {
                SpecificWriters[sLogLevel].Add(sWriter);
                rReturn = true;
            }
            return rReturn;
        }

        public bool RemoveSpecificWriter (INWDLogger sWriter)
        {
            bool rReturn = false;
            if (sWriter != null)
            {
                foreach (KeyValuePair<NWDLogLevel, List<INWDLogger>> tWriters in SpecificWriters)
                {
                    rReturn |= tWriters.Value.Remove(sWriter);
                }
            }

            return rReturn;
        }

        public bool RemoveSpecificWriter(INWDLogger sWriter, NWDLogLevel sLogLevels)
        {
            bool rReturn = false;
            if (sWriter != null)
            {
                if (sLogLevels.HasFlag(NWDLogLevel.Trace))
                {
                    rReturn |= RemoveSpecificWriterFromDictionary(sWriter, NWDLogLevel.Trace);
                }
                if (sLogLevels.HasFlag(NWDLogLevel.Debug))
                {
                    rReturn |= RemoveSpecificWriterFromDictionary(sWriter, NWDLogLevel.Debug);
                }
                if (sLogLevels.HasFlag(NWDLogLevel.Information))
                {
                    rReturn |= RemoveSpecificWriterFromDictionary(sWriter, NWDLogLevel.Information);
                }
                if (sLogLevels.HasFlag(NWDLogLevel.Warning))
                {
                    rReturn |= RemoveSpecificWriterFromDictionary(sWriter, NWDLogLevel.Warning);
                } 
                if (sLogLevels.HasFlag(NWDLogLevel.Error))
                {
                    rReturn |= RemoveSpecificWriterFromDictionary(sWriter, NWDLogLevel.Error);
                }
                if (sLogLevels.HasFlag(NWDLogLevel.Critical))
                {
                    rReturn |= RemoveSpecificWriterFromDictionary(sWriter, NWDLogLevel.Critical);
                }
            }

            return rReturn;
        }

        private bool RemoveSpecificWriterFromDictionary(INWDLogger sWriter, NWDLogLevel sLogLevel)
        {
            bool rReturn = false;
            if (SpecificWriters.ContainsKey(sLogLevel))
            {
                rReturn = SpecificWriters[sLogLevel].Remove(sWriter);
            }
            return rReturn;
        }
    }
}
#nullable disable