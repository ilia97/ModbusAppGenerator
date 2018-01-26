using System;
using System.Collections.Generic;
using System.Text;

namespace ModbusAppGenerator.Core.Models
{
    public class Project
    {
        public int Id { set; get; }

        public string UserId { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }

        public virtual List<Device> Devices { set; get; }

        public ConnectionSettings ConnectionSettings { set; get; }
    }
}
