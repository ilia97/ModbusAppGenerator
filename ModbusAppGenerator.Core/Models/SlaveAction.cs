using System.Collections.Generic;
using ModbusAppGenerator.DataAccess.Enums;

namespace ModbusAppGenerator.Core.Models
{
    public class SlaveAction
    {
        public SlaveAction()
        {
            Types = new List<ModbusDataType>();
        }

        public int Id { set; get; }
        
        public Project Project { set; get; }

        public int SlaveAddress { set; get; }

        public ushort StartAddress { set; get; }

        public ushort NumberOfRegisters { set; get; }

        public List<ModbusDataType> Types { set; get; }
    }
}
