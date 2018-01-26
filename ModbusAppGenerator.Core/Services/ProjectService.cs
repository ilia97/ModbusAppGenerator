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

        public int Add(Project project)
        {
            var projectEntity = _mapper.Map<Project, ProjectEntity>(project);

            foreach(var device in project.Devices)
            {
                var deviceEntity = _mapper.Map<Device, DeviceEntity>(device);

                _unitOfWork.DeviceRepository.Insert(deviceEntity);
            }

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

            return _mapper.Map<ProjectEntity, Project>(projectEntity);
        }

        public IList<Project> GetUserProjects(string userId)
        {
            var projectsList = _unitOfWork.ProjectRepository.Get(x => x.UserId == userId);

            return _mapper.Map<IList<ProjectEntity>, IList<Project>>(projectsList);
        }
    }
}
