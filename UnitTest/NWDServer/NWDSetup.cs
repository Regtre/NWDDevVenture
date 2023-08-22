using Microsoft.Extensions.Configuration;
using NWDFoundation.Configuration;
using NWDFoundation.Configuration.Databases;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Exchanges;
using NWDFoundation.Logger;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;
using NWDHub.Configuration;
using NWDHub.Models;
using NWDServerFront.Configuration;
using NWDServerMiddle.Configuration;
using NWDServerMiddle.Managers;
using NWDServerMiddle.Managers.ModelManagers;
using NWDServerShared.Configuration;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models;

namespace UnitTest.NWDServer;

[SetUpFixture]
public class NWDSetup
{
    #region Static properties
    /// <summary>
    /// Use for generate unique player token by test
    /// </summary>
    public static ulong PlayerCounter = 1;

    public static readonly uint FalseProjectId = 666;
    static NWDSetup()
    {
    }

    #endregion

    #region Static methods

    public static NWDRequestPlayerToken GetPlayerToken(NWDExchangeOrigin sExchangeOrigin = NWDExchangeOrigin.Web, NWDEnvironmentKind sEnvironmentKind = NWDEnvironmentKind.Dev)
    {
        string tPayload = NWDRandom.RandomStringAlpha(16);
        ushort tRange = (ushort)NWDRandom.IntNumeric(1);
        ulong tPlayerReference = NWDRandom.UnsignedLongNumeric(8) * 1000000000 + PlayerCounter++;
        ulong tProjectId = NWDHubConfiguration.KConfig.GetCrucialProjectId();
        string tToken = NWDRandom.RandomStringAlpha(16);
        NWDRequestPlayerToken rPlayerToken = new NWDRequestPlayerToken(tProjectId,sEnvironmentKind )
        {
            AccountRange = tRange,
            PlayerReference = tPlayerReference,
            Token = tToken,
            EnvironmentKind = sEnvironmentKind,
            ProjectId = tProjectId,
            ExchangeOrigin = sExchangeOrigin,
        };
        return rPlayerToken;
    }

    /// <summary>
    /// Return configuration from file
    /// </summary>
    /// <param name="sOutputPath"></param>
    /// <returns></returns>
    private static IConfigurationRoot GetIConfigurationRoot(string sOutputPath)
    {
        return new ConfigurationBuilder()
            .SetBasePath(sOutputPath)
            .AddJsonFile("appsettings.Test.json", optional: true)
            //.AddUserSecrets("e3dfcccf-0cb3-423a-b302-e3e92e95c128")
            .AddEnvironmentVariables()
            .Build();
    }

    #endregion

    #region Instance methods

    /// <summary>
    /// Set up the test, init by configuration, create managers and prepare the project to test 
    /// </summary>
    [OneTimeSetUp]
    public void Setup()
    {
        // Get Configuration
        NWDLogger.Trace("Launch Configuration for test");
        var tIConfig = GetIConfigurationRoot(TestContext.CurrentContext.TestDirectory);
        NWDServerConfiguration.KConfig.LoadConfig(tIConfig);
        NWDConfigurationDatabase.KConfig.LoadSection(tIConfig); 
        NWDServerMiddleConfiguration.KConfig.LoadConfig(tIConfig);
        NWDWebRuntimeConfiguration.KConfig.LoadConfig(tIConfig);
        NWDDatabaseConnectorConfiguration.KConfig.LoadConfig(tIConfig);
        NWDHubConfiguration.KConfig.LoadConfig(tIConfig);
        //NWDEditorKeyManager.EditorManager = new NWDServerTreatKeyManager(NWDAdminConfiguration.KConfig.MonitoringSecretKey);
        // Prepare manager and DAO from Configuration
        NWDLogger.Trace("Prepare manager for test");

        NWDServerFrontConfiguration.KConfig.PrepareAfterConfiguration();
        NWDWebDBDataManager.CreateAllTables();
        
        NWDServerConfiguration.KConfig.Status = NWDServerStatus.Active;
        NWDLogger.Trace("Server is active");
        
        
        
    }
    #endregion
}