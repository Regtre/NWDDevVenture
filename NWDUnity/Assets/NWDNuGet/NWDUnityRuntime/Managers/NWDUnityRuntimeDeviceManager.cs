using NWDUnityShared.Facades;
using UnityEngine.Device;

namespace NWDUnityRuntime.Managers
{
    public class NWDUnityRuntimeDeviceManager : INWDUnityDeviceManager
    {
        public readonly string DeviceId = SystemInfo.deviceUniqueIdentifier;

        public string GetDeviceId()
        {
            return DeviceId;
        }

        public string GetDeviceName()
        {
            return null;
        }
    }
}
