using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDHub.Models;
using NWDWebEditor.Controllers;
using NWDWebRuntime.Managers;
using NWDWebStandard.Controllers;
namespace NWDHub.Controllers;
public class NWDStudioClassConstructionEditionController : NWDModelEditionAsyncController<NWDStudioClassConstruction>
{
    #region Virtual methods to filter

    protected override Func<T, bool> Filter<T>(Dictionary<string, string> sDictionary)
    {
        Func<NWDStudioClassConstruction, bool> tTest = (sData => sData.Reference > 0);
        if (sDictionary.ContainsKey(nameof(NWDProjectSubObject.Project)))
        {
            ulong tProjectReference = ulong.Parse(sDictionary[nameof(NWDProjectSubObject.Project)]);
            tTest =(sData => sData.Project == tProjectReference);
        }
        return (Func<T, bool>)tTest;
    }
    public override void NewAddon(NWDStudioClassConstruction sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
    {
        if (sValues.ContainsKey(nameof(NWDProjectSubObject.Project)))
        {
            ulong tProjectReference = ulong.Parse(sValues[nameof(NWDProjectSubObject.Project)]);
            sObject.Project = tProjectReference;
            sObject.Publish = NWDProjectPartStatus.New;
            // NWDProject? tProject = NWDWebDataManager.GetDataByReference<NWDProject>(sHttpContext, tProjectReference);
            // tProject.NeedToBePublish = true;
            // AddJavascript( "ShowOrHiddeContent('ProjectNeedBeUpdate', true);");
            // AddJavascript( "ShowOrHiddeContent('ProjectNotBeUpdate', false);");
            // NWDWebDataManager.SaveData(sHttpContext, tProject);
        }
    }

    public override void UpdateAddon(NWDStudioClassConstruction sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
    {
        if (sValues.ContainsKey(nameof(NWDProjectSubObject.Project)))
        {
            ulong tProjectReference = ulong.Parse(sValues[nameof(NWDProjectSubObject.Project)]);
            sObject.Project = tProjectReference;
            sObject.Publish = NWDProjectPartStatus.Upgrading;
            
            NWDProject? tProject = NWDWebDataManager.GetDataByReference<NWDProject>(sHttpContext, tProjectReference);
            NWDStudioClassConstruction? tOldObject = NWDWebDataManager.GetDataByReference<NWDStudioClassConstruction>(sHttpContext, sObject.Reference);
            string tOldJson = JsonConvert.SerializeObject(tOldObject);
            string tNewJson = JsonConvert.SerializeObject(sObject);
            if (tOldJson != tNewJson)
            {
                if (sObject.Publish != NWDProjectPartStatus.New)
                {
                    sObject.Publish = NWDProjectPartStatus.Upgrading;
                }
                tProject.NeedToBePublish = true;
                AddJavascript( "ShowOrHiddeContent('ProjectNeedBeUpdate', true);");
                AddJavascript( "ShowOrHiddeContent('ProjectNotBeUpdate', false);");
                NWDWebDataManager.SaveData(sHttpContext, tProject);
            }
        }
    }

    public override void DeleteAddon(NWDStudioClassConstruction sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
    {
        if (sValues.ContainsKey(nameof(NWDProjectSubObject.Project)))
        {
            ulong tProjectReference = ulong.Parse(sValues[nameof(NWDProjectSubObject.Project)]);
            sObject.Project = tProjectReference;
            sObject.Publish = NWDProjectPartStatus.Inactive;
            NWDProject? tProject = NWDWebDataManager.GetDataByReference<NWDProject>(sHttpContext, tProjectReference);
            sObject.Publish = NWDProjectPartStatus.Upgrading;
            tProject.NeedToBePublish = true;
            AddJavascript( "ShowOrHiddeContent('ProjectNeedBeUpdate', true);");
            AddJavascript( "ShowOrHiddeContent('ProjectNotBeUpdate', false);");
            NWDWebDataManager.SaveData(sHttpContext, tProject);
        }
    }
    #endregion
}