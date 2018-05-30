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

namespace AiRAPI.SDK.Buffer
{
    //32 bytes
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct VarBuf
    {
        private readonly int tickCount;
        private readonly int bufOffset;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private readonly int[] pad;
    }
}
