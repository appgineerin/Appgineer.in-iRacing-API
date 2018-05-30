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

namespace AiRAPI.SDK.Header
{
    //144 bytes
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct VarHeader
    {
        //16 bytes: offset = 0
        private readonly int type;
        //offset = 4
        private readonly int offset;
        //offset = 8
        private readonly int count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private readonly int[] pad;

        //32 bytes: offset = 16
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Defines.MaxString)]
        private readonly string name;
        //64 bytes: offset = 48
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Defines.MaxDesc)]
        private readonly string desc;
        //32 bytes: offset = 112
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Defines.MaxString)]
        private readonly string unit;
    }
}
