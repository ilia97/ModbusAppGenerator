using System.Collections.Generic;
using System.IO;
using ModbusAppGenerator.Core.Models;

namespace ModbusAppGenerator.Core.Services.Interfaces
{
    public interface IProjectService
    {
        int Add(Project project);

        void Edit(Project project, string userId);

        void Delete(int projectId, string userId);

        Project Get(int projectId, string userId);

        IList<Project> GetUserProjects(string userId);

        Stream DownloadProject(int projectId, string userId);
    }
}
