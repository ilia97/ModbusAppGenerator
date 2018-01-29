using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ModbusAppGenerator.DataAccess.Enums;

namespace ModbusAppGenerator.DataAccess.Entities
{
    public class ProjectEntity
    {
        public ProjectEntity()
        {
            Actions = new List<SlaveActionEntity>();
        }

        [Key]
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }

        public bool IsLoggerEnabled { set; get; }
        
        public int StatFlushPeriod { set; get; }
        
        public int Timeout { set; get; }
        
        public int Period { set; get; }

        [ForeignKey("User")]
        public string UserId { set; get; }
        public UserEntity User { set; get; }

        public virtual List<SlaveActionEntity> Actions { set; get; }

        public int SettingId { set; get; }

        public ConnectionTypes ConnectionType { set; get; }
    }
}
