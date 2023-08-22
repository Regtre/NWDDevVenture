using System;
using System.Collections.Generic;

namespace NWDUnityEditor.Tools
{
    public class NWDTypeGUIInformation
    {
        static private Dictionary<Type, NWDTypeGUIInformation> kDico = new Dictionary<Type, NWDTypeGUIInformation>();
        public int EnvTabSeleced;
        public Type ClassType;
        //public NWDConstructionPlan tPlan;
        public NWDUnityEditorMultiGUIContent tClass;
        public NWDModelType ModelType;
        public Dictionary<string, string> DicoField = new Dictionary<string, string>();
        //public Dictionary<NWDEnvironmentUnityEditor, List<NWDVirtualDataGUI>> DatasByEnvironment = new Dictionary<NWDEnvironmentUnityEditor, List<NWDVirtualDataGUI>>();

        public bool IsEditorData()
        {
            //return ClassType.IsSubclassOf(typeof(NWDStudioData));
            return true;
        }

        public NWDTypeGUIInformation(Type sType)
        {
            ClassType = sType;
            //tPlan = NWDConfig.KConfig.ReturnActiveEnvironment().GetMachineTools(sType);
            //tClass = NWDUnityEditorMultiGUIContent.NewContent(tPlan.TheClassType, tPlan.TheClassType.Name);
            tClass = NWDUnityEditorMultiGUIContent.NewContent("Default", "Default");
            ModelType = NWDModelType.GetForType(sType);
            /*foreach (FieldInfo tField in tPlan.FieldInfoArray)
            {
                DicoField.Add(tField.Name.Replace("_", ""), tField.FieldType.Name);
            }*/
        }

        public static NWDTypeGUIInformation GetForType(Type sType)
        {
            NWDTypeGUIInformation rReturn = null;
            if (kDico.ContainsKey(sType))
            {
                rReturn = kDico[sType];
            }
            else
            {
                rReturn = new NWDTypeGUIInformation(sType);
                kDico.Add(sType, rReturn);
            }
            return rReturn;
        }

        internal void SyncAsync()
        {
            throw new NotImplementedException();
        }

        internal void Sync()
        {
            throw new NotImplementedException();
        }
    }
}
