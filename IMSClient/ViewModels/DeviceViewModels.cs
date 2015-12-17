using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace IMSClient.ViewModels
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

        public string Status
        {
            get
            {
                var stringBuilder = new StringBuilder("Device type: ");

                stringBuilder.Append(EnumOps.GetDeviceType(DeviceType));
                
                return stringBuilder.ToString();
            }
        }
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

    public static class EnumOps
    {
        public static string GetDeviceType(DeviceType deviceType)
        {
            return Regex.Replace(Enum.GetName(typeof (DeviceType), deviceType), @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1").ToLowerInvariant();
        }
    }


}
