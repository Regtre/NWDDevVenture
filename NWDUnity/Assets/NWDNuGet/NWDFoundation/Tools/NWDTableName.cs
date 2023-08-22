using System;
using NWDFoundation.Configuration.Environments;

namespace NWDFoundation.Tools
{
    public static class NWDTableName
    {
        public static string GenerateTableName(Type sType, NWDEnvironmentKind sEnvironment, string sPrefix)
        {
            string rReturn = "_";
            switch (sEnvironment)
            {
                case NWDEnvironmentKind.Dev:
                    rReturn = sPrefix + "_Dev_" + sType.Name;
                    break;
                case NWDEnvironmentKind.PlayTest:
                    rReturn = sPrefix + "_PlayT_" + sType.Name;
                    break;
                case NWDEnvironmentKind.Qualification:
                    rReturn = sPrefix + "_Qual_" + sType.Name;
                    break;
                case NWDEnvironmentKind.PreProduction:
                    rReturn = sPrefix + "_Pre_" + sType.Name;
                    break;
                case NWDEnvironmentKind.Production:
                    rReturn = sPrefix + "_Prod_" + sType.Name;
                    break;
                case NWDEnvironmentKind.PostProduction:
                    rReturn = sPrefix + "_Post_" + sType.Name;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sEnvironment), sEnvironment, null);
            }

            return rReturn;
        }
    }
}