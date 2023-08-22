using System;
using System.Collections.Generic;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades.Back;
using NWDFoundation.Models;
using NWDRuntime.Models;

namespace NWDServerBack.MariaDb.ByReflexion;

public class NWDRelationShipDao_MariaDbByReflexion : NWDBackMariaDbByReflexion, INWDRelationshipDao
{
    public NWDRelationShipDao_MariaDbByReflexion(NWDDatabaseCredentials sDatabaseCredentials) : base(
        sDatabaseCredentials)
    {
    }

    public string FingerPrintTableName(NWDEnvironmentKind sEnvironment)
    {
        return FingerPrintName<NWDBasicModel>(sEnvironment);
    }
 
    public bool TableExists(NWDEnvironmentKind sEnvironment)
    {
        return TableExists<NWDBasicModel>(sEnvironment);
    }

    public void CreateTable(NWDEnvironmentKind sEnvironment)
    {
        CreateTable<NWDRelationship>(sEnvironment);
    }

    public void DeleteTable(NWDEnvironmentKind sEnvironment)
    {
        DeleteTable<NWDCrucialInformation>(sEnvironment);
    }

    public string FingerPrintTable(NWDEnvironmentKind sEnvironment)
    {
        return FingerPrint<NWDBasicModel>(sEnvironment);
    }

    public NWDRelationship Create(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDRelationship sModel)
    {
        return Create(sEnvironment,sProjectId, sModel, true, false);

    }
    public NWDRelationship Update(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDRelationship sModel)
    {
        return Update<NWDRelationship>(sEnvironment,sProjectId, sModel);
    }

    public void Delete(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sReference)
    {
        Delete<NWDRelationship>(sEnvironment,sProjectId, sReference);

    }

    public List<NWDRelationship> FindAll(NWDEnvironmentKind sEnvironment, ulong sProjectId)
    {
        return FindAll<NWDRelationship>(sEnvironment,sProjectId);
        
    }

    public List<NWDRelationship> FindAllModified(NWDEnvironmentKind sEnvironment, ulong sProjectId, ulong sSyncDate)
    {
        return FindAllModified<NWDRelationship>(sEnvironment,sProjectId, sSyncDate);
    }

    public NWDRelationship InsertOrUpdate(NWDEnvironmentKind sEnvironment, ulong sProjectId, NWDRelationship sModel)
    {
        return InsertOrUpdate(sEnvironment,sProjectId, sModel, true, false);
    }

    public List<NWDRelationship> GetBy(NWDEnvironmentKind sEnvironment, ulong sProjectId, Dictionary<string, string> sDictionary, string sAndWhere = "")
    {
        return GetBy<NWDRelationship>(sEnvironment,sProjectId, sDictionary, sAndWhere);
    }
}