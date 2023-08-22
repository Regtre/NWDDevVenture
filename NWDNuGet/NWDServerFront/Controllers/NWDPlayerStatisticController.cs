using Microsoft.AspNetCore.Mvc;
using NWDPlayerStatistic.Exchanges;
using NWDPlayerStatistic.Facades;
using NWDServerMiddle.Managers;

namespace NWDServerFront.Controllers;

[ApiController]
[Route("[controller]")]
public class NWDPlayerStatisticController : ControllerBase
{
    #region properties

    private readonly INWDPlayerStatisticManager _Manager;

    #endregion

    #region constructor

    public NWDPlayerStatisticController(NWDPlayerStatisticManager sManager)
    {
        _Manager = sManager;
    }

    #endregion

    #region methods

    [HttpPost(Name = "PostStatistic")]

    public NWDPlayerStatisticResponse Post([FromBody] NWDPlayerStatisticRequest sRequest)
    {
        NWDPlayerStatisticResponse tResponse = _Manager.Process(sRequest);
        return tResponse;
    }
    #endregion

}