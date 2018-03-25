using System.Web.Mvc;

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
