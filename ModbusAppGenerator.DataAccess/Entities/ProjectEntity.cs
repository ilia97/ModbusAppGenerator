using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModbusAppGenerator.DataAccess.Entities
{
    public class ProjectEntity
    {
        [Key]
        public int Id { set; get; }
        
        [ForeignKey("User")]
        public string UserId { set; get; }

        public UserEntity User { set; get; }
    }
}
