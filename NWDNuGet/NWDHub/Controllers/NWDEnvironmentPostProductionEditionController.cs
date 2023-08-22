using Microsoft.AspNetCore.Http;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDHub.Models;
using NWDWebStandard.Controllers;
using NWDCrucial.Models;
using NWDWebEditor.Controllers;

namespace NWDHub.Controllers;

public class NWDEnvironmentPostProductionEditionController : NWDModelEditionAsyncController<NWDProjectPublishDataTrack>
{
    private const NWDEnvironmentKind _ENVIRONMENT_KIND = NWDEnvironmentKind.PostProduction;
    protected override Func<T, bool> Filter<T>(Dictionary<string, string> sDictionary)
    {
        Func<NWDProjectPublishDataTrack, bool> tTest = (sData => sData.Reference > 0);
        if (sDictionary.ContainsKey(nameof(NWDProjectSubObject.Project)))
        {
            ulong tProject = ulong.Parse(sDictionary[nameof(NWDProjectSubObject.Project)]);
            tTest = (sData => sData.Project == tProject && sData.Kind == _ENVIRONMENT_KIND);
        }
        return (Func<T, bool>)tTest;
    }
}