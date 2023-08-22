using NWDUnityShared.Tools;
using System;
using UnityEngine.Device;

namespace NWDUnityEditor.Device
{
    [Serializable]
    public class NWDFakeDevice
    {
        static public NWDFakeDevice CreateFakeDevice()
        {
            string tDevice = NWDToolbox.RandomStringAlpha(32);
            return new NWDFakeDevice
            {
                DeviceID = tDevice,
                InternalName = "Device " + tDevice,
                Language = "en",
                Country = "US",
                NetworkQuality = NWDDeviceNetworkQuality.Good
            };
        }

        static public NWDFakeDevice CreateDefaultFakeDevice()
        {
            return new NWDFakeDevice
            {
                DeviceID = SystemInfo.deviceUniqueIdentifier + "Fake",
                InternalName = "Default fake device",
                Language = "en",
                Country = "US",
                NetworkQuality = NWDDeviceNetworkQuality.Good
            };
        }

        public string DeviceID;
        public string InternalName;
        public NWDDeviceNetworkQuality NetworkQuality;
        public string Language;
        public string Country;
    }
}
