﻿using System.ComponentModel;
using System.Reflection;

namespace QuixelBridgeDownloader.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString())!;
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>()!;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
