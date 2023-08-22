using Microsoft.AspNetCore.Http;
using NWDFoundation.Models;

namespace NWDWebRuntime.Extensions;

public static class NWDReferencesArrayExtension
{
    public static List<T> GetAllObjects<T>(this NWDReferencesArray<T> sObject , HttpContext sHttpContext) where T : NWDDatabaseBasicModel 
    {
        List<T> rReturn = new List<T>();
        //TODO : NWDConfigRuntime.KConfig. ... get object of Type T with References
        return rReturn;
    }
}