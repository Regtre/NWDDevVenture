using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDFoundation.Facades.Back
{
    public interface INWDDao
    {
        #region interfaces
        public string FingerPrintTable(NWDEnvironmentKind sEnvironment);
        public string FingerPrintTableName(NWDEnvironmentKind sEnvironment);

        public bool TableExists(NWDEnvironmentKind sEnvironment);

        public void CreateTable(NWDEnvironmentKind sEnvironment);
        public void DeleteTable(NWDEnvironmentKind sEnvironment);
        public ushort GetRange();
        public bool TestConnexion();
        public string GetInfos();
        public DateTime GetCurrentDatetime();
        public Int64 GenerateNewCommitId();

        #endregion
    }
}