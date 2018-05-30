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

namespace AiRAPI.SDK.Header
{
    internal class CVarHeader
    {
        internal enum VarType
        {
            IrChar,
            IrBool,
            IrInt,
            IrBitField,
            IrFloat,
            IrDouble
        }

        internal VarType Type { get; }
        internal int Offset { get; }
        internal int Count { get; }
        public string Name { get; }
        public string Desc { get; }
        public string Unit { get; }

        internal CVarHeader(int type, int offset, int count, string name, string desc, string unit)
        {
            Type = (VarType)type;
            Offset = offset;
            Count = count;
            Name = name;
            Desc = desc;
            Unit = unit;
        }

        private int Bytes
        {
            get
            {
                switch (Type)
                {
                    case VarType.IrChar:
                    case VarType.IrBool:
                        return 1;
                    case VarType.IrInt:
                    case VarType.IrBitField:
                    case VarType.IrFloat:
                        return 4;
                    case VarType.IrDouble:
                        return 8;
                    default:
                        return 0;
                }
            }
        }

        internal int Length => Bytes * Count;
    }
}
