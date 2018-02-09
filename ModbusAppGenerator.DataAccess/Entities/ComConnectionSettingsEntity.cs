using ModbusAppGenerator.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModbusAppGenerator.DataAccess.Entities
{
    public class ComConnectionSettingsEntity
    {
        public int Id { set; get; }

        public string PortName { set; get; }

        public int BaudRate { set; get; }

        public int DataBits { set; get; }

        public Parity Parity { set; get; }

        public StopBits StopBits { set; get; }
    }
}
