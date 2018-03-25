using System.Collections.Generic;

namespace ModbusAppGenerator.ViewModels.ProjectViewModels
{
    public class AddProjectActionsViewModel
    {
        public int Id { set; get; }

        public List<ActionViewModel> Actions { set; get; }
    }
}
