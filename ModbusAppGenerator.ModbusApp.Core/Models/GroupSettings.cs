using System;
using System.Collections.Generic;
using ModbusAppGenerator.ModbusApp.Core.Misc.Enums;

namespace ModbusAppGenerator.ModbusApp.Core.Models
{
    public class GroupSettings
    {
        public int Id { set; get; }

        public byte DeviceId { set; get; }

        public ushort StartAddress { set; get; }
        
        public ushort NumberOfRegisters { set; get; }
        
        public List<Tuple<int, ModbusDataType>> Types { set; get; }
    }
}
