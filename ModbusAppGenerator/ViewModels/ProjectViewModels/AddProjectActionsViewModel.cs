using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModbusAppGenerator.ViewModels.ProjectViewModels
{
    public class AddProjectActionsViewModel
    {
        public int Id { set; get; }

        public List<ActionViewModel> Actions { set; get; }
    }
}
