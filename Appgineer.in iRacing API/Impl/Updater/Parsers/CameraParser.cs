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

using System.Linq;
using AiRAPI.Impl.Camera;
using AiRAPI.Impl.Utils;
using YamlDotNet.RepresentationModel;

namespace AiRAPI.Impl.Updater.Parsers
{
    internal sealed class CameraParser : Parser
    {
        internal override void Parse(YamlMappingNode root, Simulation sim)
        {
            var cameraGroups = root.GetList("CameraInfo.Groups");
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var group in cameraGroups.Children.OfType<YamlMappingNode>())
            {
                var id = group.GetByte("GroupNum");
                var name = group.GetString("GroupName");

                var cameraGroup = sim.CameraManager.GetCameraGroup(id);
                if (cameraGroup != null && cameraGroup.Name == name)
                    continue;

                lock (sim.SharedCollectionLock)
                {
                    LoadCameras(cameraGroups, (CameraManager)sim.CameraManager);
                }
                return;
            }
        }

        private static void LoadCameras(YamlSequenceNode cameraGroups, CameraManager cameraManager)
        {
            lock (ApiProvider.Simulation.SharedCollectionLock)
            {
                cameraManager.CameraGroups.Clear();
                foreach (var cameraGroup in cameraGroups.Children.OfType<YamlMappingNode>().Select(g => new CameraGroup { Id = g.GetByte("GroupNum"), Name = g.GetString("GroupName") }))
                {
                    cameraManager.CameraGroups.Add(cameraGroup);
                }
            }
        }
    }
}
