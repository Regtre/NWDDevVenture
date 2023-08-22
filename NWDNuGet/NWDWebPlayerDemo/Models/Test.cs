using System.Collections;
using ChartJSCore.Models;
using NWDFoundation.Models;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;
using NWDWebPlayerDemo.Managers;
using NWDWebPlayerDemo.Models.CV;

namespace NWDWebPlayerDemo.Models;

public class TestWebModel : NWDDatabaseWebBasicModel
{
    [NWDWebPropertyDescription("Title", NWDWebEditionStyle.Text,false, "", "my Title", "my Placeholder", sUseAsSortBy: true, sUseAsColumn: false, sIsPrimaryColumn: true)]
    public string Name { set; get; } = string.Empty;
    [NWDWebPropertyDescription("Desc", NWDWebEditionStyle.RichText,false, "", "my Title", "my Placeholder", sUseAsSortBy: true, sUseAsColumn: false, sIsPrimaryColumn: true)]
    public string Desc { set; get; } = string.Empty;
    [NWDWebPropertyDescription("Color", NWDWebEditionStyle.Color,false, "", "my Description", "my Description Placeholder")]
    public string Color { set; get; } = string.Empty;
    [NWDWebPropertyDescription("Date", NWDWebEditionStyle.Date,false, "", "my Description", "my Description Placeholder")]
    public DateTime Date { set; get; } = DateTime.Now;
    [NWDWebPropertyDescription(
        "ChartType", 
        NWDWebEditionStyle.Dropdown, 
        true,
        "", 
        "my Description", 
        "my Description Placeholder",new string[]{"Line","Bar","AHAHA"})]
    public Enums.ChartType ChartType { set; get; } = Enums.ChartType.Line;
    
    [NWDWebPropertyDescription("Test2", NWDWebEditionStyle.Object,false, "", "my Description", "my Description Placeholder")]
    public Test2 Test2 { set; get; } = new Test2();
    [NWDWebPropertyDescription("Test2", NWDWebEditionStyle.ReferenceArray, true, "", "my Description", "my Description Placeholder", typeof(DataRetrieverTest2),typeof(Test2))]
    public NWDReferencesArray<Test2> Test2ArrayRef { set; get; } = new NWDReferencesArray<Test2>();
    
    [NWDWebPropertyDescription("Test2", NWDWebEditionStyle.List, true, "", "my Description", "my Description Placeholder", typeof(DataRetrieverTest2),typeof(Test2))]
    public IList<Test2> Test2List { set; get; } = new List<Test2>()
    {
        new Test2()
        {
            Reference = 6773094712647226,
        },
        new Test2()
        {
            Reference = 8913658269432523,
        },
    };
    
    public IList<string> Test2ListString { set; get; }
    public IDictionary<string,string> Test2DictionaryString { set; get; }

    /*[NWDWebPropertyDescription("Test28", NWDWebEditionStyle.List, "", "my Description", "my Description Placeholder", typeof(NWDWebPlayerDemo.Managers.DataRetrieverTest2),typeof(Test2))]
    public List<Test2> Test2ListObject { set; get; } = new List<Test2>();*/
}           
