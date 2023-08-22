using Newtonsoft.Json;
using NWDUnityEditor.Device;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Tools;
using NWDUnityShared.Facades;
using System;
using System.Collections.Generic;

namespace NWDUnityEditor.Managers
{
    [Serializable]
    public class NWDUnityEditorDeviceManager : INWDUnityDeviceManager
    {
        [JsonProperty]
        private int CurrentDevice = 0;
        [JsonProperty]
        private List<NWDFakeDevice> Devices = new List<NWDFakeDevice>();

        public void AddFakeDevice()
        {
            AddDevice(NWDFakeDevice.CreateFakeDevice());
        }

        private void AddDevice(NWDFakeDevice sDevice)
        {
            Devices.Add(sDevice);
            Save();
        }

        public List<NWDFakeDevice> GetDevices ()
        {
            if (Devices.Count == 0)
            {
                AddDevice(NWDFakeDevice.CreateDefaultFakeDevice ());
            }
            return Devices;
        }

        public NWDFakeDevice GetDevice(int sDevice)
        {
            if (sDevice == 0 && Devices.Count == 0)
            {
                AddDevice(NWDFakeDevice.CreateDefaultFakeDevice());
            }
            return Devices[sDevice];
        }

        public void DeleteFakeDevice(int sDeviceId)
        {
            if (sDeviceId > 0 && sDeviceId < Devices.Count)
            {
                if (CurrentDevice == sDeviceId)
                {
                    CurrentDevice = 0;
                }
                if (CurrentDevice > sDeviceId)
                {
                    CurrentDevice--;
                }

                Devices.RemoveAt(sDeviceId);
                Save();
            }
        }

        public void SetCurrentDevice (int sDeviceId)
        {
            if (sDeviceId >= 0 && sDeviceId < Devices.Count)
            {
                CurrentDevice = sDeviceId;
                Save();
            }
        }

        public void Save()
        {
            NWDUnityEngineEditor.Instance.ThreadManager.UniqueCallOnMainThread(ProcessSave);
        }

        private void ProcessSave ()
        {
            NWDUserSettings.Save(this);
        }

        public string GetDeviceId()
        {
            NWDFakeDevice tDevice = GetDevice(CurrentDevice);
            return tDevice.DeviceID;
        }

        public string GetDeviceName()
        {
            NWDFakeDevice tDevice = GetDevice(CurrentDevice);
            return tDevice.InternalName;
        }
    }
}
