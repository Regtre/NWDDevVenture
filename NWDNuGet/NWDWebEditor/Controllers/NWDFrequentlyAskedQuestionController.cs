using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NWDFoundation.Models;
using NWDWebRuntime.Managers;
using NWDWebStandard.Controllers;

namespace NWDWebEditor.Controllers;

public class NWDFrequentlyAskedQuestionController : NWDWebDBEditionAsyncController<NWDFrequentlyAskedQuestion>
{
    
    // public override string PathToIndexView()
    // {
    //     return "/Views/NWDFrequentlyAskedQuestion/" + nameof(Index) + ".cshtml";
    // }

    [HttpGet]
    [NWDAuthorizeAdminOnly()]
    public IActionResult NewHere(string sController, string sAction)
    {
        Dictionary<string, string> tDefaultValues = new Dictionary<string, string>()
        {
            {nameof(NWDFrequentlyAskedQuestion.Domain), sController},
            {nameof(NWDFrequentlyAskedQuestion.SubDomain), sAction},
        };
        return RedirectToAction("New",new {sDefaultValues = JsonConvert.SerializeObject(tDefaultValues)});
    }

    public override void NewAddon(NWDFrequentlyAskedQuestion sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
    {
        base.NewAddon(sObject, sValues, sHttpContext);
        if (sValues.ContainsKey(nameof(NWDFrequentlyAskedQuestion.Domain)))
        {
            sObject.Domain = sValues[nameof(NWDFrequentlyAskedQuestion.Domain)];
        }
        if (sValues.ContainsKey(nameof(NWDFrequentlyAskedQuestion.SubDomain)))
        {
            sObject.SubDomain = sValues[nameof(NWDFrequentlyAskedQuestion.SubDomain)];
        }
    }
}