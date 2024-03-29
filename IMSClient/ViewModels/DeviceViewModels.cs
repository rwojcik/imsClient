﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

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

        public string DeviceTypeString => EnumOps.GetDeviceType(DeviceType);

        public string Status
        {
            get
            {
                if (Discriminator == "Binary" && BinarySetting.HasValue)
                {
                    if (DeviceType == DeviceType.AutomaticWindow || DeviceType == DeviceType.Door ||
                        DeviceType == DeviceType.Window)
                        return BinarySetting.Value ? "opened" : "closed";
                    else if (DeviceType == DeviceType.Alarm)
                        return BinarySetting.Value ? "set off" : "armed";
                }
                else if (Discriminator == "Continous" && ContinousSetting.HasValue)
                {
                    return $"{ContinousSetting:F1} ℃";
                }

                return string.Empty;
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

        [Required]
        public string Discriminator { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? ContinousSetting { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? BinarySetting { get; set; }
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
            return Regex.Replace(Enum.GetName(typeof(DeviceType), deviceType), @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1").ToLowerInvariant();
        }
    }


}
