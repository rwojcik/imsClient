using System;
using System.Collections.Generic;

namespace IMSPrototyper.ViewModels
{
    public class DeviceViewModel
    {
        public long DeviceId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long GroupId { get; set; }

        public IEnumerable<DeviceHistoryViewModel> History { get; set; }

        public DeviceType DeviceType { get; set; }

        public string Discriminator { get; set; }

        public double? ContinousSetting { get; set; }

        public bool? BinarySetting { get; set; }
    }

    public class AddDeviceViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long GroupId { get; set; }
        public DeviceType DeviceType { get; set; }
        
        public string Discriminator { get; set; }

        public double? ContinousSetting { get; set; }

        public bool? BinarySetting { get; set; }

    }

    public class UpdateDeviceViewModel
    {
        public long DeviceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long GroupId { get; set; }
    }

    public class DeviceHistoryViewModel
    {
        public string ChangedBy { get; set; }

        public DateTime RecordedAt { get; set; }
        
        public string Discriminator { get; set; }

        public double? ContinousSetting { get; set; }

        public bool? BinarySetting { get; set; }
    }
    public enum DeviceType
    {
        AutomaticWindow = 1,
        Window = 2,
        Thermometer = 3,
        Thermostat = 4,
        Door = 5,
        Alarm = 6,
    }
}
