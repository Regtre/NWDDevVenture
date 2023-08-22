using System.Collections.Generic;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDFoundation.Facades.Back
{
    public interface INWDRelationshipDao : INWDDao
    {
        #region interfaces

        public NWDRelationship Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDRelationship sModel);
        public NWDRelationship Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDRelationship sModel);
        public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference);
        public List<NWDRelationship> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId);
        public List<NWDRelationship> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate);
        public NWDRelationship InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDRelationship sModel);

        public List<NWDRelationship> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId,
            Dictionary<string, string> sDictionary, string sAndWhere = "");

        #endregion
    }
}