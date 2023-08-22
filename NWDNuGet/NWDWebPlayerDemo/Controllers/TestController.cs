using Microsoft.AspNetCore.Mvc;
using   NWDWebEditor.Controllers;
using NWDWebEditor.Managers;
using NWDWebPlayerDemo.Models;
using NWDWebStandard.Controllers;

namespace NWDWebPlayerDemo.Controllers;

public class TestController : NWDWebDBEditionAsyncController<TestWebModel>
{
    public IActionResult TestSave()
    {
        TestWebModel tTestWebModel = new TestWebModel();
        tTestWebModel.Test2ListString = new List<string>()
        {
            "A",
            "B",
            "C",
        };
        tTestWebModel.Test2DictionaryString = new Dictionary<string, string>()
        {
            {"A","A1"},
            {"B","B1"},
            {"C","C1"},
        };

        NWDWebDBEditionDataManager<TestWebModel> manager = new NWDWebDBEditionDataManager<TestWebModel>();
        manager.Add(tTestWebModel);
        return RedirectToAction("Index");
    }
}