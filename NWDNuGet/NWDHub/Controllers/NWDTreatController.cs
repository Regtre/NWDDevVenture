using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NWDFoundation.Exchanges;
using NWDFoundation.Models.Enums;
using NWDRuntime.Exchanges;
using NWDTreat.Exchanges;
using NWDTreat.Facades;

namespace NWDHub.Controllers;

[ApiController]
[Route("[controller]")]
public class NWDTreatController  : ControllerBase
{
    #region properties

    private readonly INWDTreatManager _Manager;

    #endregion

    #region constructor

    public NWDTreatController(INWDTreatManager sManager)
    {
        _Manager = sManager;
    }

    #endregion

    #region methods

    [HttpPost(Name = "Post")]
    public NWDResponseTreat Post([FromBody] NWDRequestTreat sRequestTreat)
    {
        NWDResponseTreat tResponse = _Manager.Process(sRequestTreat);
        return tResponse;
    }

    #endregion
}