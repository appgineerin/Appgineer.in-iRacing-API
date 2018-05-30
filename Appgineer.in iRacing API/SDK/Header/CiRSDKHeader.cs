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

using System.IO.MemoryMappedFiles;
using AiRAPI.SDK.Buffer;

namespace AiRAPI.SDK.Header
{
    internal class CiRsdkHeader
    {
        private const int HVerOffset = 0;
        private const int HStatusOffset = 4;
        private const int HTickRateOffset = 8;
        private const int HSesInfoUpdateOffset = 12;
        private const int HSesInfoLenOffset = 16;
        private const int HSesInfoOffsetOffset = 20;
        private const int HNumVarsOffset = 24;
        private const int HVarHeaderOffsetOffset = 28;
        private const int HNumBufOffset = 32;
        private const int HBufLenOffset = 36;

        private readonly MemoryMappedViewAccessor _fileMapView;
        private readonly CVarBuf _buffer;

        internal CiRsdkHeader(MemoryMappedViewAccessor mapView)
        {
            _fileMapView = mapView;
            _buffer = new CVarBuf(mapView, this);
        }

        internal int Version => _fileMapView.ReadInt32(HVerOffset);
        internal int Status => _fileMapView.ReadInt32(HStatusOffset);
        internal int TickRate => _fileMapView.ReadInt32(HTickRateOffset);
        public int SessionInfoUpdate => _fileMapView.ReadInt32(HSesInfoUpdateOffset);
        internal int SessionInfoLength => _fileMapView.ReadInt32(HSesInfoLenOffset);
        internal int SessionInfoOffset => _fileMapView.ReadInt32(HSesInfoOffsetOffset);
        internal int VarCount => _fileMapView.ReadInt32(HNumVarsOffset);
        internal int VarHeaderOffset => _fileMapView.ReadInt32(HVarHeaderOffsetOffset);
        internal int BufferCount => _fileMapView.ReadInt32(HNumBufOffset);
        internal int BufferLength => _fileMapView.ReadInt32(HBufLenOffset);
        internal int Buffer => _buffer.OffsetLatest;
    }
}
