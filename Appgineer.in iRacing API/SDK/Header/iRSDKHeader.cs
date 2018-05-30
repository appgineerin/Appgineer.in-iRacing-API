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

using System.Runtime.InteropServices;
using AiRAPI.SDK.Buffer;

namespace AiRAPI.SDK.Header
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    internal struct iRSDKHeader
    {
        //12 bytes: offset = 0
        private readonly int ver;
        private readonly int status;
        private readonly int tickRate;

        //12 bytes: offset = 12
        private readonly int sessionInfoUpdate;
        private readonly int sessionInfoLen;
        private readonly int sessionInfoOffset;

        //8 bytes: offset = 24
        private readonly int numVars;
        private readonly int varHeaderOffset;

        //16 bytes: offset = 32
        private readonly int numBuf;
        private readonly int bufLen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private readonly int[] pad1;

        //128 bytes: offset = 48
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Defines.MaxBufs)]
        private readonly VarBuf[] varBuf;
    }
}
