using System;
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

        public int StartAddress { set; get; }

        public int NumberOfRegisters { set; get; }

        public List<ModbusDataType> Types { set; get; }
    }
}
