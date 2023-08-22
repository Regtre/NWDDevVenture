using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;
using NWDHub.Configuration;

namespace UnitTest.NWDServer.Sign
{
    public class NWDTestSign : NWDTestSetup
    {
        private static uint _Counter;
        public void ShowAccountSignActionDebug()
        {
            foreach (NWDAccountSignAction tTry in (NWDAccountSignAction[])Enum.GetValues(typeof(NWDAccountSignAction)))
            {
                NWDLogger.Trace(tTry.ToString() + " = " + ((int)tTry).ToString());
            }
        }
        public NWDAccountSign NewFakeAccountSignWithLoginEmail(string sLogin, string sEmail)
        {
            string tPassword = NWDRandom.RandomStringBase64(28);
            return NWDAccountSign.CreateLoginEmailPassword(sLogin, sEmail, tPassword, NWDHubConfiguration.KConfig.GetCrucialProjectId());
        }
        public NWDAccountSign NewFakeAccountSignWithEmail(string sEmail)
        {
            string tPassword = NWDRandom.RandomStringBase64(28);
            return NWDAccountSign.CreateEmailPassword(sEmail, tPassword, NWDHubConfiguration.KConfig.GetCrucialProjectId());
        }
        public NWDAccountSign NewFakeAccountSign()
        {
            string tLoginHash = NWDRandom.RandomStringBase64(28) + (_Counter++).ToString("0000");
            string tSignHash = NWDRandom.RandomStringBase64(28) + (_Counter++).ToString("0000");
            string tRescueHash = NWDRandom.RandomStringBase64(28) + (_Counter++).ToString("0000");
            NWDLogger.Trace(nameof(NewFakeAccountSign) + " create with : " +
                            " SignHash:" + tSignHash + " " +
                            " RescueEmail:" + tRescueHash + " " +
                            " LoginHash:" + tLoginHash + " " +
                            " for project " + NWDHubConfiguration.KConfig.GetCrucialProjectId());
            return new NWDAccountSign()
            {
                ProjectId = NWDHubConfiguration.KConfig.GetCrucialProjectId(),
                LoginHash = tLoginHash,
                SignHash = tSignHash,
                RescueHash = tRescueHash
            };
        }
        [OneTimeTearDown]
        public void TestSignClean()
        {
        }
    }
}