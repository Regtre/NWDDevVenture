using Microsoft.AspNetCore.Mvc;
using NWDCrucial.Exchanges;
using NWDCrucial.Facades;
using NWDServerMiddle.Managers;

namespace NWDServerFront.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NWDCrucialController : ControllerBase
    {
        #region properties

        private readonly INWDCrucialManager _Manager;

        #endregion

        #region constructor

        public NWDCrucialController(NWDCrucialManager sManager)
        {
            _Manager = sManager;
        }

        #endregion

        #region methods

        [HttpPost(Name = "PostPublish")]
        public NWDResponseCrucial Post([FromBody] NWDRequestCrucial sRequestCrucial)
        {
            NWDResponseCrucial tResponse = _Manager.Process(sRequestCrucial);
            return tResponse;
        }

        #endregion
    }
}