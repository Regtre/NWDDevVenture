using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using NWDFoundation.Logger;

namespace NWDFoundation.Tools
{
    public static class NWDReflexion
    {
        #region static methods

        public static List<Type> GetAllTypesSubclassOf(Type sType, Assembly sAssembly)
        {
            List<Type> rReturn = new List<Type>();
            foreach (Type tType in sAssembly.GetTypes())
            {
                if (tType.BaseType == sType)
                {
                    rReturn.Add(tType);
                }
                else if (tType.IsSubclassOf(sType))
                {
                    rReturn.Add(tType);
                }
                else if (tType.BaseType!= null && tType.BaseType.IsGenericType && tType.BaseType.GetGenericTypeDefinition() == sType)
                {
                    rReturn.Add(tType);
                }
                else
                {
                    // bye bye
                }
            }

            return rReturn;
        }
        public static List<Type> GetAllTypesSubclassOf(Type sType, Assembly[] sAssemblies)
        {
            List<Type> rReturn = new List<Type>();
            foreach (Assembly tAssembly in sAssemblies)
            {
                rReturn.AddRange(GetAllTypesSubclassOf(sType, tAssembly));
            }

            return rReturn;
        }

        public static List<Type> GetAllTypesInAllAssembliesSubclassOf(Type sType)
        {
            return GetAllTypesSubclassOf(sType, AppDomain.CurrentDomain.GetAssemblies());
        }

        #endregion
    }
}