using NWDFoundation.Configuration.Environments;
using NWDFoundation.Exchanges;
using NWDHub.Controllers;
using NWDHub.Managers;
using NWDServerFront.Controllers;
using NWDServerMiddle.Managers;

namespace UnitTest.NWDServer;

public class NWDTestSetup
{
    #region Static properties
    
    static protected NWDEditorManager EditorManager = new NWDEditorManager();
    static protected NWDRuntimeManager RuntimeManager = new NWDRuntimeManager();
    static protected NWDEditorController EditorController = new NWDEditorController(EditorManager);
    static protected NWDRuntimeController RuntimeController = new NWDRuntimeController(RuntimeManager);
    
    #endregion
    
    #region Static methods

    protected static NWDRequestPlayerToken GetPlayerToken(NWDExchangeOrigin sExchangeOrigin, NWDEnvironmentKind sEnvironmentKind = NWDEnvironmentKind.Dev)
    {
        return NWDSetup.GetPlayerToken(sExchangeOrigin, sEnvironmentKind);
    }

    #endregion

    #region Instance methods
    
    #endregion
}