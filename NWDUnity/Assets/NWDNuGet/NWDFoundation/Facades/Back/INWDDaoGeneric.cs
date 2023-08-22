using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDFoundation.Facades.Back
{
    public interface INWDDaoGeneric
    {
        #region interfaces

        public DateTime GetCurrentDatetime();
        public Int64 GenerateNewCommitId();
        public bool TableExists<T>(NWDEnvironmentKind sEnvironment) where T : NWDBasicModel;
        public void CreateTable<T>(NWDEnvironmentKind sEnvironment) where T : NWDBasicModel;

        public T Create<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sModel, bool sRangeDependent, bool sWithNewReference)
            where T : NWDBasicModel;

        public T Update<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sModel
            , Dictionary<string, string>? sDictionary = null, string sAndWhere = "") where T : NWDBasicModel;
        public bool Delete<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sModel) where T : NWDBasicModel;
        public bool Delete<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference) where T : NWDBasicModel;
        public List<T> FindAll<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId) where T : NWDBasicModel;

        public List<T> FindAllModified<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
            where T : NWDBasicModel;

        public T InsertOrUpdate<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sModel, bool sRangeDependent,
            bool sWithNewReference, Dictionary<string, string>? sDictionary = null, string sAndWhere = "") where T : NWDBasicModel;

        public List<T> GetBy<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string>? sDictionary = null,
            string sAndWhere = "") where T : NWDBasicModel;

        public ushort GetRange();
        public bool TestConnexion();
        public string GetInfos();

        #endregion
    }
}