using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ModbusAppGenerator.ViewModels.ProjectViewModels
{
    public class ProjectListItemViewModel
    {
        [HiddenInput]
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }
    }
}
