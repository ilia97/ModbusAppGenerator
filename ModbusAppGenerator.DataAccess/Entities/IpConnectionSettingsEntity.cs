using System.ComponentModel.DataAnnotations;

namespace ModbusAppGenerator.DataAccess.Entities
{
    public class IpConnectionSettingsEntity
    {
        [Key]
        public int Id { set; get; }

        public string Host { set; get; }

        public int Port { set; get; }
    }
}