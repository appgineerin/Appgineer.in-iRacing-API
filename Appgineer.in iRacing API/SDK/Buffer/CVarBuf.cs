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
using System.Runtime.InteropServices;
using AiRAPI.SDK.Header;

namespace AiRAPI.SDK.Buffer
{
    internal class CVarBuf
    {
        private const int VarBufOffset = 48;
        private const int VarTickCountOffset = 0;
        private const int VarBufOffsetOffset = 4;

        private readonly int _varBufSize;
        private readonly MemoryMappedViewAccessor _fileMapView;
        private readonly CiRsdkHeader _header;

        internal CVarBuf(MemoryMappedViewAccessor mapView, CiRsdkHeader header)
        {
            _fileMapView = mapView;
            _header = header;
            _varBufSize = Marshal.SizeOf(typeof(VarBuf));
        }

        internal int OffsetLatest
        {
            get
            {
                var bufCount = _header.BufferCount;
                var ticks = new int[_header.BufferCount];
                for (var i = 0; i < bufCount; i++)
                    ticks[i] = _fileMapView.ReadInt32(VarBufOffset + i * _varBufSize + VarTickCountOffset);

                var latestTick = ticks[0];
                var latest = 0;
                for (var i = 0; i < bufCount; i++)
                {
                    if (latestTick < ticks[i])
                        latest = i;
                }

                return _fileMapView.ReadInt32(VarBufOffset + latest * _varBufSize + VarBufOffsetOffset);
            }
        }
    }
}
