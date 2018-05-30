// -----------------------------------------------------
//
// Distributed under GNU GPLv3.
//
// -----------------------------------------------------
//
// Copyright (c) 2018, appgineering.com
// All rights reserved.
// 
// This file is part of the Appgineer.in iRacing API.
//
// -----------------------------------------------------

using System.Globalization;
using YamlDotNet.RepresentationModel;

namespace AiRAPI.Impl.Utils
{
    internal static class YamlUtil
    {
        internal static YamlSequenceNode GetList(this YamlMappingNode node, string key)
        {
            return node.GetChild<YamlSequenceNode>(key) ?? new YamlSequenceNode();
        }

        internal static YamlMappingNode GetMap(this YamlMappingNode node, string key)
        {
            return node.GetChild<YamlMappingNode>(key) ?? new YamlMappingNode();
        }

        internal static bool GetBool(this YamlMappingNode node, string key)
        {
            var result = node.GetValue(key);
            if (string.IsNullOrWhiteSpace(result))
                return false;

            return result != "0";
        }

        internal static byte GetByte(this YamlMappingNode node, string key)
        {
            return byte.TryParse(node.GetNumberValue(key), out var result) ? result : default(byte);
        }

        internal static float GetFloat(this YamlMappingNode node, string key)
        {
            return float.TryParse(node.GetNumberValue(key), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var result) ? result : default(float);
        }

        internal static int GetInt(this YamlMappingNode node, string key)
        {
            return int.TryParse(node.GetNumberValue(key), out var result) ? result : default(int);
        }

        internal static string GetString(this YamlMappingNode node, string key)
        {
            return node.GetValue(key);
        }

        private static T GetChild<T>(this YamlNode node, string key) where T : YamlNode
        {
            var keys = key.Split('.');
            var result = node;
            foreach (var s in keys)
            {
                result = ((YamlMappingNode)result).GetNode(s);
                if (result == null)
                    break;
            }
            return result as T;
        }

        private static YamlNode GetNode(this YamlMappingNode node, string key)
        {
            return node.Children.TryGetValue(new YamlScalarNode(key), out var result) ? result : null;
        }

        private static string GetNumberValue(this YamlMappingNode node, string key)
        {
            var value = node.GetValue(key);
            if (value == null)
                return null;

            if (value.StartsWith("\"") || value.EndsWith("\""))
                value = value.Replace("\"", string.Empty);
            return value;
        }

        private static string GetValue(this YamlMappingNode node, string key)
        {
            return (node.GetNode(key) as YamlScalarNode)?.Value;
        }
    }
}
