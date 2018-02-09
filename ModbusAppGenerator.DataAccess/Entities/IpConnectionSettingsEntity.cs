using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModbusAppGenerator.DataAccess.Entities
{
    public class IpConnectionSettingsEntity
    {
        public int Id { set; get; }

        public string Host { set; get; }

        public int Port { set; get; }
    }
}
