using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;
using NWDHub.Models;
using NWDWebEditor.Controllers;
using NWDWebRuntime.Managers;

namespace NWDHub.Controllers
{
    public class NWDProjectServiceEditionController : NWDModelEditionAsyncController<NWDProjectService>
    {
        #region Virtual methods to filter

        protected override Func<T, bool> Filter<T>(Dictionary<string, string> sDictionary)
        {
            Func<NWDProjectService, bool> tTest = (sData => sData.Reference > 0);
            if (sDictionary.ContainsKey(nameof(NWDProjectSubObject.Project)))
            {
                ulong tProjectReference = ulong.Parse(sDictionary[nameof(NWDProjectSubObject.Project)]);
                tTest = (sData => sData.Project == tProjectReference);
            }

            return (Func<T, bool>)tTest;
        }

        public override void NewAddon(NWDProjectService sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
        {
            sObject.Publish = NWDProjectPartStatus.New;
            ulong tProjectReference = ulong.Parse(sValues[nameof(NWDProjectSubObject.Project)]);
            NWDProject? tProject = NWDWebDataManager.GetDataByReference<NWDProject>(sHttpContext, tProjectReference);
            if (tProject != null)
            {
                sObject.AssociateToProject(tProject);
                List<NWDProjectService> tList = NWDWebDataManager.GetAllData<NWDProjectService>(sHttpContext);
                tList.Remove(sObject);
                bool tTestTrack = false;
                sObject.ServiceId = NWDRandom.ShortNumeric(4);
                while (tTestTrack == false)
                {
                    tTestTrack = true;
                    foreach (NWDProjectService tObject in tList)
                    {
                        if (tObject.ServiceId == sObject.ServiceId)
                        {
                            tTestTrack = false;
                            sObject.ServiceId = NWDRandom.ShortNumeric(4);
                            break;
                        }
                    }
                }

                if (sObject.Publish != NWDProjectPartStatus.New)
                {
                    sObject.Publish = NWDProjectPartStatus.Upgrading;
                }

                tProject.NeedToBePublish = true;
                AddJavascript("ShowOrHiddeContent('ProjectNeedBeUpdate', true);");
                AddJavascript("ShowOrHiddeContent('ProjectNotBeUpdate', false);");
                NWDWebDataManager.SaveData(sHttpContext, tProject);
            }
        }

        public override void UpdateAddon(NWDProjectService sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
        {
            if (sValues.ContainsKey(nameof(NWDProjectSubObject.Project)))
            {
                ulong tProjectReference = ulong.Parse(sValues[nameof(NWDProjectSubObject.Project)]);
                sObject.Project = tProjectReference;
                sObject.Publish = NWDProjectPartStatus.Upgrading;

                NWDProject? tProject = NWDWebDataManager.GetDataByReference<NWDProject>(sHttpContext, tProjectReference);
                NWDProjectService? tOldObject = NWDWebDataManager.GetDataByReference<NWDProjectService>(sHttpContext, sObject.Reference);
                string tOldJson = JsonConvert.SerializeObject(tOldObject);
                string tNewJson = JsonConvert.SerializeObject(sObject);
                if (tOldJson != tNewJson)
                {
                    if (sObject.Publish != NWDProjectPartStatus.New)
                    {
                        sObject.Publish = NWDProjectPartStatus.Upgrading;
                    }
                    if (tProject != null)
                    {
                        tProject.NeedToBePublish = true;
                        AddJavascript("ShowOrHiddeContent('ProjectNeedBeUpdate', true);");
                        AddJavascript("ShowOrHiddeContent('ProjectNotBeUpdate', false);");
                        NWDWebDataManager.SaveData(sHttpContext, tProject);
                    }
                }
            }
        }

        public override void DeleteAddon(NWDProjectService sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
        {
            if (sValues.ContainsKey(nameof(NWDProjectSubObject.Project)))
            {
                ulong tProjectReference = ulong.Parse(sValues[nameof(NWDProjectSubObject.Project)]);
                sObject.Project = tProjectReference;
                sObject.Publish = NWDProjectPartStatus.Inactive;
                NWDProject? tProject = NWDWebDataManager.GetDataByReference<NWDProject>(sHttpContext, tProjectReference);
                sObject.Publish = NWDProjectPartStatus.Upgrading;
                if (tProject != null)
                {
                    tProject.NeedToBePublish = true;
                    AddJavascript("ShowOrHiddeContent('ProjectNeedBeUpdate', true);");
                    AddJavascript("ShowOrHiddeContent('ProjectNotBeUpdate', false);");
                    NWDWebDataManager.SaveData(sHttpContext, tProject);
                }
            }
        }

        #endregion
    }
}