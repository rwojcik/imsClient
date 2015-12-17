using System;

namespace IMSClient.Signal
{

    public class DeviceSetting : EventArgs
    {
        public long DeviceId { get; }

        public double ContinousSetting { get; }

        public bool BinarySetting { get; }

        public string Discriminator { get; }

        public bool Success { get; }

        public DeviceSetting(long deviceId, string discriminator, bool success = default (bool), double continousSetting = default(double), bool binaryStetting = default(bool))
        {
            DeviceId = deviceId;
            ContinousSetting = continousSetting;
            BinarySetting = binaryStetting;
            Discriminator = discriminator;
            Success = success;
        }
    }

    public delegate void DeviceUpdate(object sender, DeviceSetting deviceSetting);

    public delegate void DeviceUpdated(object sender, DeviceSetting deviceSetting);
}