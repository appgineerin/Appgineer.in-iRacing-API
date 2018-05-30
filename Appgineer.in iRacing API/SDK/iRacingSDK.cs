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
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using AiRAPI.Data.Enums;
using AiRAPI.SDK.Header;

namespace AiRAPI.SDK
{
    // ReSharper disable once InconsistentNaming
    internal sealed class iRacingSDK
    {
        //VarHeader offsets
        private const int VarOffsetOffset = 4;
        private const int VarCountOffset = 8;
        private const int VarNameOffset = 16;
        private const int VarDescOffset = 48;
        private const int VarUnitOffset = 112;
        private int _varHeaderSize = 144;

        public bool IsInitialized;

        private MemoryMappedFile _iRacingFile;
        private MemoryMappedViewAccessor _fileMapView;

        public CiRsdkHeader Header;
        public readonly Dictionary<string, CVarHeader> VarHeaders = new Dictionary<string, CVarHeader>();

        public void Startup()
        {
            try
            {
                _iRacingFile = MemoryMappedFile.OpenExisting(Defines.MemMapFileName);
                _fileMapView = _iRacingFile.CreateViewAccessor();
                _varHeaderSize = Marshal.SizeOf(typeof(VarHeader));

                Header = new CiRsdkHeader(_fileMapView);
                GetVarHeaders();

                IsInitialized = true;
            }
            catch
            {
                // ignored
            }
        }

        private void GetVarHeaders()
        {
            VarHeaders.Clear();
            for (var i = 0; i < Header.VarCount; i++)
            {
                var type = _fileMapView.ReadInt32(Header.VarHeaderOffset + i * _varHeaderSize);
                var offset = _fileMapView.ReadInt32(Header.VarHeaderOffset + i * _varHeaderSize + VarOffsetOffset);
                var count = _fileMapView.ReadInt32(Header.VarHeaderOffset + i * _varHeaderSize + VarCountOffset);

                var name = new byte[Defines.MaxString];
                var desc = new byte[Defines.MaxDesc];
                var unit = new byte[Defines.MaxString];

                _fileMapView.ReadArray(Header.VarHeaderOffset + i * _varHeaderSize + VarNameOffset, name, 0, Defines.MaxString);
                _fileMapView.ReadArray(Header.VarHeaderOffset + i * _varHeaderSize + VarDescOffset, desc, 0, Defines.MaxDesc);
                _fileMapView.ReadArray(Header.VarHeaderOffset + i * _varHeaderSize + VarUnitOffset, unit, 0, Defines.MaxString);

                var nameStr = Encoding.Default.GetString(name).TrimEnd('\0');
                var descStr = Encoding.Default.GetString(desc).TrimEnd('\0');
                var unitStr = Encoding.Default.GetString(unit).TrimEnd('\0');

                VarHeaders[nameStr] = new CVarHeader(type, offset, count, nameStr, descStr, unitStr);
            }
        }

        public object GetData(string name)
        {
            if (!IsInitialized || Header == null)
                return null;

            if (!VarHeaders.ContainsKey(name))
                return null;

            var varOffset = VarHeaders[name].Offset;
            var count = VarHeaders[name].Count;
            switch (VarHeaders[name].Type)
            {
                case CVarHeader.VarType.IrChar:
                    var bytes = new byte[count];
                    _fileMapView.ReadArray(Header.Buffer + varOffset, bytes, 0, count);
                    return Encoding.Default.GetString(bytes).TrimEnd('\0');
                case CVarHeader.VarType.IrBool:
                    if (count > 1)
                    {
                        var data = new bool[count];
                        _fileMapView.ReadArray(Header.Buffer + varOffset, data, 0, count);
                        return data;
                    }

                    return _fileMapView.ReadBoolean(Header.Buffer + varOffset);
                case CVarHeader.VarType.IrInt:
                case CVarHeader.VarType.IrBitField:
                    if (count > 1)
                    {
                        var data = new int[count];
                        _fileMapView.ReadArray(Header.Buffer + varOffset, data, 0, count);
                        return data;
                    }

                    return _fileMapView.ReadInt32(Header.Buffer + varOffset);
                case CVarHeader.VarType.IrFloat:
                    if (count > 1)
                    {
                        var data = new float[count];
                        _fileMapView.ReadArray(Header.Buffer + varOffset, data, 0, count);
                        return data;
                    }

                    return _fileMapView.ReadSingle(Header.Buffer + varOffset);
                case CVarHeader.VarType.IrDouble:
                    if (count > 1)
                    {
                        var data = new double[count];
                        _fileMapView.ReadArray(Header.Buffer + varOffset, data, 0, count);
                        return data;
                    }

                    return _fileMapView.ReadDouble(Header.Buffer + varOffset);
                default:
                    return null;
            }
        }

        public string GetSessionInfo()
        {
            if (!IsInitialized || Header == null)
                return null;

            var data = new byte[Header.SessionInfoLength];
            _fileMapView.ReadArray(Header.SessionInfoOffset, data, 0, Header.SessionInfoLength);
            return Encoding.Default.GetString(data).TrimEnd('\0');
        }

        public bool IsConnected()
        {
            if (IsInitialized && Header != null)
                return (Header.Status & 1) > 0;

            return false;
        }

        public void Shutdown()
        {
            IsInitialized = false;
            Header = null;
        }

        private static IntPtr GetBroadcastMessageId()
        {
            return NativeMethods.RegisterWindowMessage(Defines.BroadcastMessageName);
        }

        public static void BroadcastMessage(BroadcastMessageType msg, int var1, int var2, int var3)
        {
            BroadcastMessage(msg, var1, MakeLong((short) var2, (short) var3));
        }

        public static void BroadcastMessage(BroadcastMessageType msg, int var1, int var2)
        {
            var msgId = GetBroadcastMessageId();
            var hwndBroadcast = IntPtr.Add(IntPtr.Zero, 0xffff);

            if (msgId != IntPtr.Zero)
                NativeMethods.SendNotifyMessage(hwndBroadcast, msgId.ToInt32(), MakeLong((short) msg, (short) var1), var2);
        }

        private static int MakeLong(short lowPart, short highPart)
        {
            return (int) ((ushort) lowPart | (uint) (highPart << 16));
        }
    }
}
