using NWDUnityShared.Facades;
using System;
using System.Security.Cryptography;
using UnityEditor;

namespace NWDUnityTests.Manager
{
    public class NWDUnityTestsDeviceManager : INWDUnityDeviceManager
    {
        static private object _lock = new object();

        private string DeviceId;
        private string DeviceName;

        public string GetDeviceId()
        {
            lock (_lock)
            {
                return DeviceId;
            }
        }

        public string GetDeviceName()
        {
            lock (_lock)
            {
                return DeviceName;
            }
        }

        public NWDUnityTestsDeviceManager()
        {
            RegenerateDevice();
        }

        public NWDUnityTestsDeviceManager(string sDeviceId, string sDeviceName)
        {
            DeviceId = sDeviceId;
            DeviceName = sDeviceName;
        }

        public void RegenerateDevice ()
        {
            GUID tId = GUID.Generate();
            DateTime tDateTime = DateTime.Now;
            ChangeDevice("TEST-DEVICE-ID" + tId.ToString() + tDateTime.ToString(), "TEST-DEVICE-NAME" + tId.ToString() + tDateTime.ToString());
        }

        public void ChangeDevice(string sDeviceId, string sDeviceName)
        {
            lock (_lock)
            {
                DeviceId = sDeviceId;
                DeviceName = sDeviceName;
            }
        }
    }
}
