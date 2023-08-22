using System.Collections.Generic;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDFoundation.Facades.Back
{
    //TODO : Rename this with correct name : INWDWebDatabaseConnector
    public interface INWDWebDBByReflexion
    {
        public string FingerPrintTable<T>(NWDEnvironmentKind sEnvironment) where T : NWDDatabaseWebBasicModel;
        public string FingerPrintTableName<T>(NWDEnvironmentKind sEnvironment) where T : NWDDatabaseWebBasicModel;
        public bool TableExists<T>(NWDEnvironmentKind sEnvironment) where T : NWDDatabaseWebBasicModel;
        public void CreateTable<T>(NWDEnvironmentKind sEnvironment) where T : NWDDatabaseWebBasicModel;
        public void DeleteTable<T>(NWDEnvironmentKind sEnvironment) where T : NWDDatabaseWebBasicModel;
        public bool Delete<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sObject) where T : NWDDatabaseWebBasicModel;
        public T? GetByReference<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, string sReference) where T : NWDDatabaseWebBasicModel;
        public T? GetFirstBy<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary) where T : NWDDatabaseWebBasicModel;

        public List<T> GetBy<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string>? sDictionary, string sAndWhere = "") where T : NWDDatabaseWebBasicModel;
        public string RandomStringNumber(int sLength);
        public ulong NewValidReference<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId) where T : NWDDatabaseWebBasicModel;
        public bool TestIfReferenceExists<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference) where T : NWDDatabaseWebBasicModel;
        public bool TestConnexion();
        public T Create<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sModel, bool sWithNewReference) where T : NWDDatabaseWebBasicModel;
        public bool Delete<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference) where T : NWDDatabaseWebBasicModel;
        public List<T> FindAll<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId) where T : NWDDatabaseWebBasicModel;
        public List<T> FindAllModified<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate) where T : NWDDatabaseWebBasicModel;
        public T InsertOrUpdate<T>(NWDEnvironmentKind sEnvironment, ulong sProjectId, T sModel, bool sWithNewReference) where T : NWDDatabaseWebBasicModel;

    }
}