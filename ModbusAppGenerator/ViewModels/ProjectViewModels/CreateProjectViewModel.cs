using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ModbusAppGenerator.Core.Enums;

namespace ModbusAppGenerator.ViewModels.ProjectViewModels
{
    public class CreateProjectViewModel
    {
        public string Name { set; get; }

        public string Description { set; get; }
    }
}
