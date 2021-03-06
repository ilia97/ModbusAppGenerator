﻿using System.Collections.Generic;

namespace ModbusAppGenerator.Core.Models
{
    public class Project
    {
        public Project()
        {
            Actions = new List<SlaveAction>();
        }

        public int Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }

        public bool IsLoggerEnabled { set; get; }

        public int StatFlushPeriod { set; get; }

        public int Timeout { set; get; }

        public int Period { set; get; }

        public virtual List<SlaveAction> Actions { set; get; }

        public ConnectionSettings ConnectionSettings { set; get; }

        public User User { set; get; }
    }
}
