using Microsoft.AspNetCore.Mvc;
using NWDEditor.Exchanges;
using NWDEditor.Facades;
using NWDHub.Managers;

namespace NWDHub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NWDEditorController : ControllerBase
    {
        #region properties

        private readonly INWDEditorManager _Manager;

        #endregion

        #region constructor

        public NWDEditorController(INWDEditorManager sManager)
        {
            _Manager = sManager;
        }

        #endregion

        #region methods

        [HttpPost(Name = "")]
        public NWDResponseEditor Post([FromBody] NWDRequestEditor sRequestEditor)
        {
            NWDResponseEditor tResponse = _Manager.Process(sRequestEditor);
            return tResponse;
        }

        #endregion
    }
}