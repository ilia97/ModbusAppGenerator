﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ModbusAppGenerator.DataAccess.Enums;

namespace ModbusAppGenerator.DataAccess.Entities
{
    public class SlaveActionEntity
    {
        public SlaveActionEntity()
        {
            Types = new List<DataTypeEntity>();
        }

        [Key]
        public int Id { set; get; }

        [ForeignKey("Project")]
        public int ProjectId { set; get; }
        public ProjectEntity Project { set; get; }

        public int SlaveAddress { set; get; }

        public int StartAddress { set; get; }

        public int NumberOfRegisters { set; get; }

        public ActionTypes ActionType { set; get; }

        public string Formula { set; get; }

        public virtual List<DataTypeEntity> Types { set; get; }
    }
}
