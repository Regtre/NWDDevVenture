using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NWDFoundation.Models;

namespace NWDRuntime.Models
{
    public class NWDDataInMemory
    {
        public ulong SyncIndex = 0;
        private ConcurrentDictionary<Type, ConcurrentDictionary<ulong, NWDBasicModel>> _DataMemoryDictionary = new ConcurrentDictionary<Type, ConcurrentDictionary<ulong, NWDBasicModel>>();
        public List<NWDBasicModel> DataMemory
        {
            get { return _DataMemoryDictionary.Values.SelectMany(x => x.Values).ToList(); }
        }

        public void AddDataInMemory(NWDBasicModel sObject, Type sType)
        {
            if (_DataMemoryDictionary.ContainsKey(sType) == false)
            {
                _DataMemoryDictionary.TryAdd(sType, new ConcurrentDictionary<ulong, NWDBasicModel>());
            }
            if (_DataMemoryDictionary[sType].ContainsKey(sObject.Reference))
            {
                _DataMemoryDictionary[sType].Remove( sObject.Reference, out NWDBasicModel _);
            }
            _DataMemoryDictionary[sType].TryAdd(sObject.Reference, sObject);
        }
        public void AddDataInMemory<T>(T sObject) where T : NWDBasicModel
        {
            Type tType = sObject.GetType();
            if (_DataMemoryDictionary.ContainsKey(tType) == false)
            {
                _DataMemoryDictionary.TryAdd(tType, new ConcurrentDictionary<ulong, NWDBasicModel>());
            }
            
            if (_DataMemoryDictionary[tType].ContainsKey(sObject.Reference))
            {
                _DataMemoryDictionary[tType].Remove( sObject.Reference, out NWDBasicModel _);
            }
            
            _DataMemoryDictionary[tType].TryAdd(sObject.Reference, sObject);
        }
        public void AddDataInMemory(NWDDataInMemory sDataInMemory)
        {
            foreach (KeyValuePair<Type, ConcurrentDictionary<ulong, NWDBasicModel>> tType in sDataInMemory._DataMemoryDictionary)
            {
                foreach (KeyValuePair<ulong, NWDBasicModel> tObject in tType.Value)
                {
                    AddDataInMemory(tObject.Value, tType.Key);
                }
            }
            
        }
        public void RemoveDataFromMemory<T>(T sObject) where T : NWDBasicModel
        {
            Type tType = typeof(T);
            
            if (_DataMemoryDictionary[tType].ContainsKey(sObject.Reference) == true)
            {
                _DataMemoryDictionary[tType].Remove( sObject.Reference, out NWDBasicModel _);
            }
        }
        public T? GetDataByReference<T>(ulong sReference) where T : NWDBasicModel
        {
            T? rReturn = null;
            Type tType = typeof(T);
            if (_DataMemoryDictionary.ContainsKey(tType) == true)
            {
                if (_DataMemoryDictionary[tType].ContainsKey(sReference) == true)
                {
                    rReturn = _DataMemoryDictionary[tType][sReference] as T;
                }
            }
            return rReturn;
        }
        public List<T> GetDataByClass<T>() where T : NWDBasicModel
        {
            List<T> rReturn = new List<T>();
            Type tType = typeof(T);
            if (_DataMemoryDictionary.ContainsKey(tType))
            {
                rReturn = _DataMemoryDictionary[tType].Values.Cast<T>().ToList();
            }
            return rReturn;
        }
        public List<T> GetAllData<T>() where T : NWDBasicModel
        {
            List<T> rReturn = new List<T>();
            foreach (ConcurrentDictionary<ulong,NWDBasicModel> tDataBasicModels in _DataMemoryDictionary.Values)
            {
                if (tDataBasicModels.Values != null)
                {
                    tDataBasicModels.Values.ToList().ForEach(sX => rReturn.Add((T)sX));
                }
            }
            return rReturn;
        }
    }
}