using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoMapper;
using ModbusAppGenerator.Core.Exceptions;
using ModbusAppGenerator.Core.Models;
using ModbusAppGenerator.Core.Services.Interfaces;
using ModbusAppGenerator.DataAccess;
using ModbusAppGenerator.DataAccess.Entities;
using ModbusAppGenerator.DataAccess.Enums;
using ModbusAppGenerator.DataAccess.UnitOfWork;

namespace ModbusAppGenerator.Core.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(Project project, string userId)
        {
            var projectEntity = _mapper.Map<Project, ProjectEntity>(project);

            foreach(var device in project.Devices)
            {
                var deviceEntity = _mapper.Map<SlaveAction, SlaveActionEntity>(device);

                _unitOfWork.SlaveActionRepository.Insert(deviceEntity);
            }

            if (project.ConnectionSettings.GetType() == typeof(IpConnectionSettings))
            {
                var ipConnectionSettings = _mapper.Map<IpConnectionSettings, IpConnectionSettingsEntity>((IpConnectionSettings)project.ConnectionSettings);

                _unitOfWork.IpConnectionSettingsRepository.Insert(ipConnectionSettings);

                projectEntity.SettingId = ipConnectionSettings.Id;
                projectEntity.ConnectionType = ConnectionTypes.Ip;
            }
            else if (project.ConnectionSettings.GetType() == typeof(ComConnectionSettings))
            {
                var comConnectionSettings = _mapper.Map<ComConnectionSettings, ComConnectionSettingsEntity>((ComConnectionSettings)project.ConnectionSettings);

                _unitOfWork.ComConnectionSettingsRepository.Insert(comConnectionSettings);

                projectEntity.SettingId = comConnectionSettings.Id;
                projectEntity.ConnectionType = ConnectionTypes.Com;
            }

            projectEntity.UserId = userId;

            _unitOfWork.ProjectRepository.Insert(projectEntity);
            _unitOfWork.Save();

            return projectEntity.Id;
        }

        public void Delete(int projectId, string userId)
        {
            var projectEntity = _unitOfWork.ProjectRepository.GetById(projectId);

            if (projectEntity.UserId != userId)
            {
                throw new AccessDeniedException();
            }

            _unitOfWork.ProjectRepository.Delete(projectId);
            _unitOfWork.Save();
        }

        public Stream DownloadProject(int projectId, string userId)
        {
            var projectEntity = _unitOfWork.ProjectRepository.GetById(projectId);

            if (projectEntity.UserId != userId)
            {
                throw new AccessDeniedException();
            }

            throw new NotImplementedException();
        }

        public void Edit(Project project, string userId)
        {
            var projectEntity = _unitOfWork.ProjectRepository.GetById(project.Id);

            if (projectEntity.UserId != userId)
            {
                throw new AccessDeniedException();
            }

            var mappedProject = _mapper.Map<Project, ProjectEntity> (project);

            _unitOfWork.ProjectRepository.Update(mappedProject);
            _unitOfWork.Save();
        }

        public Project Get(int projectId, string userId)
        {
            var projectEntity = _unitOfWork.ProjectRepository.GetById(projectId);

            if (projectEntity.UserId != userId)
            {
                throw new AccessDeniedException();
            }

            var project = _mapper.Map<ProjectEntity, Project>(projectEntity);

            switch(projectEntity.ConnectionType)
            {
                case DataAccess.Enums.ConnectionTypes.Ip:
                    var ipConnectionSettings = _unitOfWork.IpConnectionSettingsRepository.GetById(projectEntity.SettingId);

                    project.ConnectionSettings = _mapper.Map<IpConnectionSettingsEntity, IpConnectionSettings>(ipConnectionSettings);

                    break;
                case DataAccess.Enums.ConnectionTypes.Com:
                    var comConnectionSettings = _unitOfWork.ComConnectionSettingsRepository.GetById(projectEntity.SettingId);

                    project.ConnectionSettings = _mapper.Map<ComConnectionSettingsEntity, ComConnectionSettings>(comConnectionSettings);

                    break;
            }

            return project;
        }

        public IList<Project> GetUserProjects(string userId)
        {
            var projectsList = _unitOfWork.ProjectRepository.Get(x => x.UserId == userId);

            return _mapper.Map<IList<ProjectEntity>, IList<Project>>(projectsList);
        }
    }
}
