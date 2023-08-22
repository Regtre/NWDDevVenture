using Codice.CM.SEIDInfo;
using NWDCustomModels.Information;
using NWDFoundation.Attributes;
using NWDFoundation.Facades;
using NWDFoundation.Information;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDStandardModels.Information;
using NWDUnityEditor.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace NWDUnityEditor.Tools
{
    public class NWDModelType
    {
        static public List<NWDModelType> DiListco = new List<NWDModelType>();
        static public Dictionary<Type, NWDModelType> ModelTypeByType = new Dictionary<Type, NWDModelType>();
        static public Dictionary<string, List<NWDModelType>> ModelTypesByModuleName = new Dictionary<string, List<NWDModelType>>();
        static public Dictionary<Type, List<NWDModelType>> ChildrenTypesByType = new Dictionary<Type, List<NWDModelType>>();
        static public Dictionary<string, bool> DefaultModule = new Dictionary<string, bool>();
        static public Dictionary<Type, NWDModelTypeInformation> ModelTypeInformationByType = new Dictionary<Type, NWDModelTypeInformation> ();
        static private Dictionary<string, NWDTypeModelsStyle> ModelStyleByNamespace = new Dictionary<string, NWDTypeModelsStyle>
        {
            { typeof(NWDStandardModelsInformation).Namespace, NWDTypeModelsStyle.Standard },
            { typeof(NWDCustomModelsInformation).Namespace, NWDTypeModelsStyle.Custom },
        };
        static private Dictionary<NWDCustomPropertyDrawer, Type> CustompropertyDrawers = new Dictionary<NWDCustomPropertyDrawer, Type>();
        

        static bool Loaded = false;

        //public string CustomTagName = string.Empty;
        //public string CustomCheckName = string.Empty;
        public Type CustomTag;
        public Type CustomCheck;

        public string Module = NWDModelOptionsAttribute.None;
        public bool Default = true;
        public string Description = string.Empty;
        public string MenuName = string.Empty;
        public bool Editable = true;

        public NWDTypeModelsStyle ModelStyle = NWDTypeModelsStyle.None;
        public Type ClassType;
        //private Dictionary<NWDEnvironment, NWDDedicatedFactory> Factories = new Dictionary<NWDEnvironment, NWDDedicatedFactory>();

        public static NWDModelType GetForType(Type sType)
        {
            if (ModelTypeByType.TryGetValue(sType, out NWDModelType rReturn))
            {
                return rReturn;
            }
            return null;
        }

        public NWDModelType(Type sType, NWDModelOptionsAttribute sAttribute) // TO DO: Forbidden !
        {
            ClassType = sType;

            if (ModelStyleByNamespace.TryGetValue(ClassType.Namespace, out NWDTypeModelsStyle tStyle))
            {
                ModelStyle = tStyle;
            }

            foreach (NWDModelCustomTagAttribute tMacro in sType.GetCustomAttributes(typeof(NWDModelCustomTagAttribute), true))
            {
                NWDLogger.Debug("Found " + nameof(NWDModelCustomTagAttribute) + " for type: " + tMacro.EnumName);
                CustomTag = FindType (tMacro.EnumName);

                if (CustomTag != null && !CustomTag.IsEnum)
                {
                    CustomTag = null;
                }
            }

            foreach (NWDModelCustomCheckAttribute tMacro in sType.GetCustomAttributes(typeof(NWDModelCustomCheckAttribute), true))
            {
                NWDLogger.Debug("Found " + nameof(NWDModelCustomCheckAttribute) + " for type: " + tMacro.FlagsName);
                CustomCheck = FindType (tMacro.FlagsName);

                if (CustomCheck != null && (!CustomCheck.IsEnum || CustomCheck.GetCustomAttributes(typeof(FlagsAttribute), false).Length == 0))
                {
                    CustomCheck = null;
                }
            }

            if (sAttribute != null)
            {
                Module = sAttribute.Module;
                Default = sAttribute.Default;
                Description = sAttribute.Description;
                MenuName = sAttribute.MenuName;
                Editable = sAttribute.Editable;
            }
            if (string.IsNullOrEmpty(MenuName))
            {
                MenuName = sType.Name;
            }
            if (string.IsNullOrEmpty(Description))
            {
                Description = sType.AssemblyQualifiedName;
            }
            if (Module == NWDModelOptionsAttribute.Mandatory)
            {
                Default = true;
            }
            if (ModelTypesByModuleName.ContainsKey(Module) == false)
            {
                ModelTypesByModuleName.Add(Module, new List<NWDModelType>());
            }
            if (DefaultModule.ContainsKey(Module) == false)
            {
                DefaultModule.Add(Module, false);
            }
            ModelTypesByModuleName[Module].Add(this);
            if (Module != NWDModelOptionsAttribute.None)
            {
                if (Default == true)
                {
                    DefaultModule[Module] = true;
                }
            }
            if (ModelTypeByType.ContainsKey(ClassType) == true)
            {
                ModelTypeByType.Remove(ClassType);
            }
            ModelTypeByType.Add(ClassType, this);
            DiListco.Add(this);
            GetModelInformation(ClassType);

            //foreach (NWDEnvironmentUnityEditor tEnv in NWDConfig.KConfig.ReturnAllEnvironments())
            //{
            //    NWDDedicatedFactory tFactory = tEnv.Get(ClassType);
            //    Factories.Add(tEnv, tFactory);
            //}
        }

        private Type FindType (string sTypeName)
        {
            Type tType = FindTypeInFoundation (sTypeName);
            if (tType != null)
            {
                return tType;
            }

            tType = FindTypeInStandardModels(sTypeName);
            if (tType != null)
            {
                return tType;
            }

            return FindTypeInCustomModels(sTypeName);
        }

        private Type FindTypeInFoundation (string sTypeName)
        {
            Assembly tAssembly = typeof(NWDFoundationInformation).Assembly;
            return tAssembly.GetType(nameof(NWDFoundation) + "." + nameof(NWDFoundation.Models) + "." + sTypeName);
        }

        private Type FindTypeInStandardModels(string sTypeName)
        {
            Assembly tAssembly = typeof(NWDStandardModelsInformation).Assembly;
            return tAssembly.GetType(nameof(NWDStandardModels) + "." + nameof(NWDStandardModels.Models) + "." + sTypeName);
        }

        private Type FindTypeInCustomModels(string sTypeName)
        {
            Assembly tAssembly = typeof(NWDCustomModelsInformation).Assembly;
            return tAssembly.GetType(nameof(NWDCustomModels) + "." + nameof(NWDCustomModels.Models) + "." + sTypeName);
        }

        private NWDModelTypeInformation GetModelInformation (Type sType)
        {
            if (sType.IsGenericType)
            {
                sType = sType.GetGenericTypeDefinition();
            }
            NWDModelTypeInformation rResult;
            if (ModelTypeInformationByType.TryGetValue(sType, out rResult))
            {
                return rResult;
            }

            List<INWDPropertyDrawer> tFields = new List<INWDPropertyDrawer>();
            NWDModelTypeInformation tParent = null;
            PropertyInfo[] tPropertyInfo = sType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            for (int i = 0; i < tPropertyInfo.Length; i++)
            {
                if (TryGetModelType(tPropertyInfo[i].PropertyType, out INWDPropertyDrawer tModelTypeField))
                {
                    tModelTypeField.SetPropertyInfo(tPropertyInfo[i]);
                    tFields.Add(tModelTypeField);
                }
            }

            if (sType.BaseType != null)
            {
                tParent = GetModelInformation(sType.BaseType);
            }

            rResult = new NWDModelTypeInformation(sType, tParent, tFields.ToArray());
            ModelTypeInformationByType.Add(sType, rResult);
            return rResult;
        }

        private bool TryGetModelType(Type sType, out INWDPropertyDrawer sPropertyDrawer, bool sLookForArrays = true, bool sLookForObjects = true)
        {
            sPropertyDrawer = null;
            if (sType == typeof(string))
            {
                sPropertyDrawer = new NWDStringPropertyDrawer();
                return true;
            }
            if (sType == typeof(bool))
            {
                sPropertyDrawer = new NWDBoolPropertyDrawer();
                return true;
            }
            if (sType == typeof(sbyte))
            {
                sPropertyDrawer = new NWDSBytePropertyDrawer();
                return true;
            }
            if (sType == typeof(byte))
            {
                sPropertyDrawer = new NWDBytePropertyDrawer();
                return true;
            }
            if (sType == typeof(short))
            {
                sPropertyDrawer = new NWDShortPropertyDrawer();
                return true;
            }
            if (sType == typeof(ushort))
            {
                sPropertyDrawer = new NWDUShortPropertyDrawer();
                return true;
            }
            if (sType == typeof(int))
            {
                sPropertyDrawer = new NWDIntPropertyDrawer();
                return true;
            }
            if (sType == typeof(uint))
            {
                sPropertyDrawer = new NWDUIntPropertyDrawer();
                return true;
            }
            if (sType == typeof(long))
            {
                sPropertyDrawer = new NWDLongPropertyDrawer();
                return true;
            }
            if (sType == typeof(ulong))
            {
                sPropertyDrawer = new NWDULongPropertyDrawer();
                return true;
            }
            if (sType == typeof(float))
            {
                sPropertyDrawer = new NWDFloatPropertyDrawer();
                return true;
            }
            if (sType == typeof(double))
            {
                sPropertyDrawer = new NWDDoublePropertyDrawer();
                return true;
            }
            if (sType.IsEnum)
            {
                sPropertyDrawer = new NWDEnumPropertyDrawer(sType);
                return true;
            }
            if (sLookForArrays)
            {
                if (sType.IsArray && sType.GetArrayRank() == 1)
                {
                    Type tSubType = sType.GetElementType();
                    if (TryGetModelType(tSubType, out INWDPropertyDrawer tPropertyDrawer, false))
                    {
                        sPropertyDrawer = new NWDArrayPropertyDrawer(tPropertyDrawer);
                        return true;
                    }
                }
                else if (sType.IsGenericType && sType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Type tSubType = sType.GetGenericArguments()[0];
                    if (TryGetModelType(tSubType, out INWDPropertyDrawer tPropertyDrawer, false))
                    {
                        sPropertyDrawer = new NWDListPropertyDrawer(tPropertyDrawer);
                        return true;
                    }
                }

                if (sType.IsGenericType && sType.GetGenericTypeDefinition() == typeof(Dictionary<int, int>).GetGenericTypeDefinition())
                {
                    Type tKeyType = sType.GetGenericArguments()[0];
                    Type tValueType = sType.GetGenericArguments()[1];

                    INWDPropertyDrawer tKeyPropertyDrawer;
                    INWDPropertyDrawer tValuePropertyDrawer;

                    if (TryGetModelType(tKeyType, out tKeyPropertyDrawer, false, false) && TryGetModelType(tValueType, out tValuePropertyDrawer, false))
                    {
                        sPropertyDrawer = new NWDDictionaryPropertyDrawer(tKeyType, tKeyPropertyDrawer, tValuePropertyDrawer);
                        return true;
                    }
                }
            }

            if (sLookForObjects)
            {
                Type tPropertyDrawerType = CustompropertyDrawers.Where(x => x.Key.Match(sType)).Select(x => x.Value).FirstOrDefault();
                if (tPropertyDrawerType != null)
                {
                    sPropertyDrawer = (NWDPropertyDrawer)Activator.CreateInstance(tPropertyDrawerType);
                    return true;
                }
                if (sType.GetInterfaces().Contains(typeof(INWDSubModel)))
                {
                    sPropertyDrawer = new NWDObjectPropertyDrawer();
                    return true;
                }
            }
            return false;
        }

        /*public NWDDedicatedFactory GetDedicatedFactory(NWDEnvironmentUnityEditor sEnvironment)
        {
            NWDDedicatedFactory rReturn = null;
            if (Factories.ContainsKey(sEnvironment) == true)
            {
                rReturn = sEnvironment.Get(ClassType);
            }
            else
            {
                if (sEnvironment.Enable == true)
                {
                    rReturn = sEnvironment.Get(ClassType);
                    Factories.Add(sEnvironment, rReturn);
                }
            }
            return rReturn;
        }*/

        static public void LoadAllGoodType()
        {
            if (Loaded == false)
            {
                // Find all types
                foreach (Assembly tAssembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    IEnumerable<Type> tTypes = tAssembly.GetTypes().Where(bType => bType.IsClass);

                    foreach (Type tType in tTypes)
                    {
                        if (!tType.IsAbstract)
                        {
                            if (tType.IsSubclassOf(typeof(NWDStudioData)) || tType.IsSubclassOf(typeof(NWDPlayerData)) || tType.GetInterfaces().Contains(typeof(INWDSubModel)))
                            {
                                NWDModelOptionsAttribute tModuleAttribute = null;
                                foreach (NWDModelOptionsAttribute tMacro in tType.GetCustomAttributes(typeof(NWDModelOptionsAttribute), true))
                                {
                                    tModuleAttribute = tMacro;
                                }
                                new NWDModelType(tType, tModuleAttribute);
                            }
                            else if (tType.IsSubclassOf(typeof(NWDPropertyDrawer)))
                            {
                                NWDCustomPropertyDrawer tPropertyDrawerAttribute = tType.GetCustomAttribute<NWDCustomPropertyDrawer>();

                                if (tPropertyDrawerAttribute != null)
                                {
                                    CustompropertyDrawers.Add(tPropertyDrawerAttribute, tType);
                                }
                            }
                        }
                    }
                }

                // Find all children
                foreach (NWDModelType tModelType in DiListco)
                {
                    List<NWDModelType> tModelTypes = new List<NWDModelType> { tModelType };

                    foreach (NWDModelType tChildrenType in DiListco.Where(x => x.ClassType.IsSubclassOf(tModelType.ClassType)))
                    {
                        if (!tModelTypes.Contains(tChildrenType))
                        {
                            tModelTypes.Add(tChildrenType);
                        }
                    }

                    ChildrenTypesByType.Add(tModelType.ClassType, OrderedList(tModelTypes));
                }

                // Find property drawers
                foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (Type tType in a.GetTypes().Where(bType => (bType.IsClass && bType.IsAbstract == false && bType.IsSubclassOf(typeof(NWDStudioData))) || bType.GetInterfaces().Contains(typeof(INWDSubModel))))
                    {
                        NWDModelOptionsAttribute tModuleAttribute = null;
                        foreach (NWDModelOptionsAttribute tMacro in tType.GetCustomAttributes(typeof(NWDModelOptionsAttribute), true))
                        {
                            tModuleAttribute = tMacro;
                        }
                        new NWDModelType(tType, tModuleAttribute);
                    }
                }

                // report module default to true if only one is true
                foreach (KeyValuePair<string, List<NWDModelType>> tKV in ModelTypesByModuleName)
                {
                    foreach (NWDModelType tTypeImplement in tKV.Value)
                    {
                        if (tTypeImplement.Module != NWDModelOptionsAttribute.None)
                        {
                            if (tTypeImplement.Module == NWDModelOptionsAttribute.Mandatory)
                            {
                                tTypeImplement.Default = true;
                            }
                            else
                            {
                                tTypeImplement.Default = DefaultModule[tTypeImplement.Module];
                            }
                        }
                    }
                }

                Loaded = true;
            }
        }

        static private List<NWDModelType> OrderedList (List<NWDModelType> sList)
        {
            List<NWDModelType> rResult = new List<NWDModelType> { sList[0] };
            
            for (int i = 0; i < sList.Count; i++)
            {
                List<NWDModelType> tEnumerable = sList.Where(x => x.ClassType.BaseType == rResult[i].ClassType).OrderBy(x => x.ClassType.Name).ToList();
                rResult.AddRange(tEnumerable);
            }

            return rResult;
        }

        /*static public void SetDefault(NWDEnvironment sEnvironment)
        {
            LoadAllGoodType();
            List<string> tTypeActivateList = new List<string>();
            List<string> tTypeInactivateList = new List<string>();
            List<string> tModuleActivateList = new List<string>();
            List<string> tModuleInactivateList = new List<string>();

            foreach (NWDTypeImplement tTypeImplement in DiListco)
            {
                if (tTypeImplement.Default == true)
                {
                    tTypeActivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
                }
                else
                {
                    tTypeInactivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
                }
            }
            foreach (KeyValuePair<string, bool> tModule in DefaultModule)
            {
                if (tModule.Value == true)
                {
                    tModuleActivateList.Add(tModule.Key);
                }
                else
                {
                    tModuleInactivateList.Add(tModule.Key);
                }
            }

            tTypeActivateList.Sort();
            tTypeInactivateList.Sort();
            tModuleActivateList.Sort();
            tModuleInactivateList.Sort();

            sEnvironment.TypeActivateList = tTypeActivateList.ToArray();
            sEnvironment.TypeInactivateList = tTypeInactivateList.ToArray();
            sEnvironment.ModuleActivateList = tModuleActivateList.ToArray();
            sEnvironment.ModuleInactivateList = tModuleInactivateList.ToArray();

            Verif(sEnvironment);
        }

        static public void Verif(NWDEnvironment sEnvironment)
        {
            LoadAllGoodType();
            List<string> tTypeActivateList = new List<string>(sEnvironment.TypeActivateList);
            List<string> tTypeInactivateList = new List<string>(sEnvironment.TypeInactivateList);
            List<string> tModuleActivateList = new List<string>(sEnvironment.ModuleActivateList);
            List<string> tModuleInactivateList = new List<string>(sEnvironment.ModuleInactivateList);

            foreach (NWDTypeImplement tTypeImplement in DiListco)
            {
                if (tTypeImplement.Module == NWDModelOptionsAttribute.Mandatory)
                {
                    if (tTypeActivateList.Contains(tTypeImplement.ClassType.AssemblyQualifiedName) == false)
                    {
                        tTypeActivateList.Add(tTypeImplement.ClassType.AssemblyQualifiedName);
                    }
                    if (tTypeInactivateList.Contains(tTypeImplement.ClassType.AssemblyQualifiedName) == true)
                    {
                        tTypeInactivateList.Remove(tTypeImplement.ClassType.AssemblyQualifiedName);
                    }
                }
            }

            tTypeActivateList.Sort();
            tTypeInactivateList.Sort();
            tModuleActivateList.Sort();
            tModuleInactivateList.Sort();

            sEnvironment.TypeActivateList = tTypeActivateList.ToArray();
            sEnvironment.TypeInactivateList = tTypeInactivateList.ToArray();
            sEnvironment.ModuleActivateList = tModuleActivateList.ToArray();
            sEnvironment.ModuleInactivateList = tModuleInactivateList.ToArray();

        }*/
    }
}
