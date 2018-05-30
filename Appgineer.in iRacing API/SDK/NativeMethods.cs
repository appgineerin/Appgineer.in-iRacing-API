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

using System;
using System.Runtime.InteropServices;

namespace AiRAPI.SDK
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr RegisterWindowMessage(string lpProcName);

        [DllImport("user32.dll")]
        internal static extern IntPtr SendNotifyMessage(IntPtr hWnd, int msg, int wParam, int lParam);
    }
}
