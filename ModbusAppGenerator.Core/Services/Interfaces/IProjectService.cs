using System.Collections.Generic;
using ModbusAppGenerator.Core.Models;
using ModbusAppGenerator.DataAccess.Enums;

namespace ModbusAppGenerator.Core.Services.Interfaces
{
    public interface IProjectService
    {
        int Add(Project project, string userId);

        void Edit(Project project, string userId);

        void Delete(int projectId, string userId);

        Project Get(int projectId, string userId);

        IList<Project> GetUserProjects(string userId);

        void AddAction(int projectId, SlaveAction action, string userId);

        void EditAction(int projectId, SlaveAction action, string userId);

        void DeleteAction(int projectId, int actionId, string userId);

        byte[] DownloadProject(int projectId, ApplicationType type, string userId, string currentDirectoryRoot);

        OperationResult TestProject(int projectId, int cyclesCount, string userId);
    }
}
