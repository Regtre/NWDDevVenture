#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NWDFoundation.Facades;

namespace NWDFoundation.Configuration
{
    public  static class NWDConfigurationInstalled
    {
        private static readonly Dictionary<string, Object> ConfigurationsInstalled = new Dictionary<string, Object>(){ { "A", "A" }};
        //private static readonly Dictionary<string, Object> ConfigurationsInstalled = new Dictionary<string, Object>();
        private static readonly List<INWDConfiguration> ConfigurationList = new List<INWDConfiguration>();
        public static void AddConfiguration(INWDConfiguration sObject)
        {
            if (ConfigurationsInstalled.ContainsKey(sObject.GetType().Name) == false)
            {
                ConfigurationsInstalled.Add(sObject.GetType().Name, sObject);
            }
            if (ConfigurationList.Contains(sObject) == false)
            {
                ConfigurationList.Add( sObject);
            }
        }

        public static string ExportConfiurations()
        {
            List<INWDConfiguration> tSortedList = ConfigurationList.OrderBy(o=>o.GetType().Name).ToList();
            Dictionary<string, Object> tConfigurations = new Dictionary<string, Object>();
            tConfigurations.Add("A","A");
            foreach (Object tOb in tSortedList)
            {
                tConfigurations.Add(tOb.GetType().Name, tOb);
            }
            return JsonConvert.SerializeObject(tConfigurations, Formatting.Indented);
        }
        public static string ExportConfiurationsDefault()
        {
            List<INWDConfiguration> tSortedList = ConfigurationList.OrderBy(o=>o.GetType().Name).ToList();
            Dictionary<string, Object> tConfigurations = new Dictionary<string, Object>();
            tConfigurations.Add("A","A");
            foreach (Object tOb in tSortedList)
            {
                INWDConfiguration tObject = (INWDConfiguration)Activator.CreateInstance(tOb.GetType())!;
                tObject.RandomFake();
                tConfigurations.Add(tOb.GetType().Name, tObject);
            }
            return JsonConvert.SerializeObject(tConfigurations, Formatting.Indented);
        }
    }
}
#nullable disable
