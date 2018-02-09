using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
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

        public ProjectService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public int Add(Project project, string userId)
        {
            var projectEntity = _mapper.Map<Project, ProjectEntity>(project);

            foreach(var device in project.Actions)
            {
                var deviceEntity = _mapper.Map<SlaveAction, SlaveActionEntity>(device);

                _unitOfWork.SlaveActionRepository.Insert(deviceEntity);
            }

            if (project.ConnectionSettings != null)
            {
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

            switch (projectEntity.ConnectionType)
            {
                case ConnectionTypes.Com:
                    _unitOfWork.ComConnectionSettingsRepository.Delete(projectEntity.SettingId);
                    break;
                case ConnectionTypes.Ip:
                    _unitOfWork.IpConnectionSettingsRepository.Delete(projectEntity.SettingId);
                    break;
            }

            foreach (var action in projectEntity.Actions)
            {
                foreach (var dataType in action.Types)
                {
                    _unitOfWork.DataTypesRepository.Delete(dataType.Id);
                }

                _unitOfWork.SlaveActionRepository.Delete(action.Id);
            }

            _unitOfWork.ProjectRepository.Delete(projectId);
            _unitOfWork.Save();
        }

        public void Edit(Project project, string userId)
        {
            var projectEntity = _unitOfWork.ProjectRepository.GetById(project.Id);

            if (projectEntity.UserId != userId)
            {
                throw new AccessDeniedException();
            }

            projectEntity.Description = project.Description;
            projectEntity.Name = project.Name;
            projectEntity.IsLoggerEnabled = project.IsLoggerEnabled;
            projectEntity.Period = project.Period;
            projectEntity.StatFlushPeriod = project.StatFlushPeriod;
            projectEntity.Timeout = project.Timeout;

            if (project.ConnectionSettings.GetType() == typeof(IpConnectionSettings))
            {
                if (project.ConnectionSettings.Id == 0)
                {
                    var ipConnectionSettings = _mapper.Map<IpConnectionSettings, IpConnectionSettingsEntity>((IpConnectionSettings)project.ConnectionSettings);

                    _unitOfWork.IpConnectionSettingsRepository.Insert(ipConnectionSettings);
                    _unitOfWork.Save();

                    projectEntity.SettingId = ipConnectionSettings.Id;
                }
                else
                {
                    var ipConnectionSettings = _unitOfWork.IpConnectionSettingsRepository.GetById(project.ConnectionSettings.Id);

                    _mapper.Map((IpConnectionSettings)project.ConnectionSettings, ipConnectionSettings);

                    _unitOfWork.IpConnectionSettingsRepository.Update(ipConnectionSettings);
                    _unitOfWork.Save();

                    projectEntity.SettingId = ipConnectionSettings.Id;
                }
                
                projectEntity.ConnectionType = ConnectionTypes.Ip;
            }
            else if (project.ConnectionSettings.GetType() == typeof(ComConnectionSettings))
            {
                var comConnectionSettings = _mapper.Map<ComConnectionSettings, ComConnectionSettingsEntity>((ComConnectionSettings)project.ConnectionSettings);

                if (project.ConnectionSettings.Id == 0)
                {
                    _unitOfWork.ComConnectionSettingsRepository.Insert(comConnectionSettings);
                }
                else
                {
                    _unitOfWork.ComConnectionSettingsRepository.Update(comConnectionSettings);
                }

                _unitOfWork.Save();

                projectEntity.SettingId = comConnectionSettings.Id;
                projectEntity.ConnectionType = ConnectionTypes.Com;
            }

            _unitOfWork.ProjectRepository.Update(projectEntity);
            _unitOfWork.Save();
        }

        public Project Get(int projectId, string userId)
        {
            var projectEntity = _unitOfWork.ProjectRepository.GetById(projectId);

            if (projectEntity.UserId != userId)
            {
                throw new AccessDeniedException();
            }

            projectEntity.User = _unitOfWork.UserRepository.GetById(projectEntity.UserId);

            var project = _mapper.Map<ProjectEntity, Project>(projectEntity);

            switch (projectEntity.ConnectionType)
            {
                case ConnectionTypes.Ip:
                    var ipConnectionSettings = _unitOfWork.IpConnectionSettingsRepository.GetById(projectEntity.SettingId) ?? new IpConnectionSettingsEntity();

                    project.ConnectionSettings = _mapper.Map<IpConnectionSettingsEntity, IpConnectionSettings>(ipConnectionSettings);

                    break;
                case ConnectionTypes.Com:
                    var comConnectionSettings = _unitOfWork.ComConnectionSettingsRepository.GetById(projectEntity.SettingId) ?? new ComConnectionSettingsEntity();

                    project.ConnectionSettings = _mapper.Map<ComConnectionSettingsEntity, ComConnectionSettings>(comConnectionSettings);

                    break;
            }

            var projectSlaveActionEntities = (List<SlaveActionEntity>)_unitOfWork.SlaveActionRepository.Get(x => x.ProjectId == project.Id);
            project.Actions = _mapper.Map<List<SlaveActionEntity>, List<SlaveAction>>(projectSlaveActionEntities);

            project.Actions.ForEach(action =>
            {
                action.Types = _unitOfWork.DataTypesRepository
                    .Get(dataType => dataType.SlaveActionEntityId == action.Id)
                    .Select(x => x.Type)
                    .ToList();
            });

            return project;
        }

        public IList<Project> GetUserProjects(string userId)
        {
            var projectsList = _unitOfWork.ProjectRepository.Get(x => x.UserId == userId);

            return _mapper.Map<IList<ProjectEntity>, IList<Project>>(projectsList);
        }

        public void UpdateActions(int projectId, List<SlaveAction> actions, string userId)
        {
            var project = _unitOfWork.ProjectRepository.GetById(projectId);

            if (project.UserId != userId)
            {
                throw new AccessDeniedException();
            }

            foreach (var action in actions)
            {
                SlaveActionEntity slaveActionEntity = null;

                if (action.Id == 0)
                {
                    slaveActionEntity = _mapper.Map<SlaveAction, SlaveActionEntity>(action);
                    slaveActionEntity.ProjectId = projectId;

                    _unitOfWork.SlaveActionRepository.Insert(slaveActionEntity);
                }
                else
                {
                    slaveActionEntity = _unitOfWork.SlaveActionRepository.GetById(action.Id);

                    slaveActionEntity.NumberOfRegisters = action.NumberOfRegisters;
                    slaveActionEntity.ProjectId = projectId;
                    slaveActionEntity.SlaveAddress = action.SlaveAddress;
                    slaveActionEntity.StartAddress = action.StartAddress;
                    
                    _unitOfWork.SlaveActionRepository.Update(slaveActionEntity);

                    var dataTypes = _unitOfWork.DataTypesRepository.Get(x => x.SlaveActionEntityId == slaveActionEntity.Id);

                    foreach (var dataType in dataTypes)
                    {
                        _unitOfWork.DataTypesRepository.Delete(dataType.Id);
                    }
                }

                _unitOfWork.Save();

                foreach (var dataType in action.Types)
                {
                    var dataTypeEntity = new DataTypeEntity()
                    {
                        SlaveActionEntityId = slaveActionEntity.Id,
                        Type = dataType
                    };

                    _unitOfWork.DataTypesRepository.Insert(dataTypeEntity);
                }
            }

            _unitOfWork.Save();
        }

        public byte[] DownloadProject(int projectId, string userId)
        {
            var project = Get(projectId, userId);

            // TODO: Add rebuilding a project

            var consoleAppBinFolderRoute = $"{Directory.GetCurrentDirectory()}\\..\\ModbusApp\\ConsoleApp\\bin\\Release";
            var modbusFilesRoutes = new string[]
            {
                "3MBP.exe",
                "3MBP.exe.config",
                "NModbus4.dll",
                "Core.dll",
                "Autofac.dll"
            };

            var tempFolderRoute = $"{Directory.GetCurrentDirectory()}\\Temp";
            var tempAppFolderRoute = $"{tempFolderRoute}\\AppFiles";
            var zipFileRoute = $"{tempFolderRoute}\\3MBP.zip";

            foreach (var modbusFileName in modbusFilesRoutes)
            {
                File.Copy($"{consoleAppBinFolderRoute}\\{modbusFileName}", $"{tempAppFolderRoute}\\{modbusFileName}", true);
            }

            CreateSettingsFile($"{tempAppFolderRoute}\\3MBP.ini", project);

            ZipFile.CreateFromDirectory(tempAppFolderRoute, zipFileRoute);

            var bytes = File.ReadAllBytes(zipFileRoute);

            var tempFolderFileNames = Directory.GetFiles(tempFolderRoute);

            foreach (var fileName in tempFolderFileNames)
            {
                File.Delete(fileName);
            }

            var tempAppFolderFileNames = Directory.GetFiles(tempAppFolderRoute);

            foreach (var fileName in tempAppFolderFileNames)
            {
                File.Delete(fileName);
            }

            return bytes;
        }

        private void CreateSettingsFile(string filePath, Project project)
        {
            var logging = project.IsLoggerEnabled ? "Yes" : "No";
            var port = "";
            var connectionSettings = "";



            if (project.ConnectionSettings.GetType() == typeof(ComConnectionSettings))
            {
                port = "COM";

                var comConnectionSettings = ((ComConnectionSettings)project.ConnectionSettings);
                var parity = "";

                switch (comConnectionSettings.Parity)
                {
                    case Parity.None:
                        parity = "N";
                        break;
                    case Parity.Even:
                        parity = "E";
                        break;
                    case Parity.Mark:
                        parity = "M";
                        break;
                    case Parity.Odd:
                        parity = "O";
                        break;
                    case Parity.Space:
                        parity = "S";
                        break;
                }

                var stopBits = "0";
                switch (comConnectionSettings.StopBits)
                {
                    case StopBits.None:
                        stopBits = "0";
                        break;
                    case StopBits.One:
                        stopBits = "1";
                        break;
                    case StopBits.OnePointFive:
                        stopBits = "1.5";
                        break;
                    case StopBits.Two:
                        stopBits = "2";
                        break;
                }

                connectionSettings = $"COM={comConnectionSettings.PortName};{comConnectionSettings.BaudRate};{comConnectionSettings.DataBits}{parity}{stopBits}";
            }
            else if (project.ConnectionSettings.GetType() == typeof(IpConnectionSettings))
            {
                port = "IP";

                var ipConnectionSettings = ((IpConnectionSettings)project.ConnectionSettings);

                connectionSettings = $"IP={ipConnectionSettings.Host}:{ipConnectionSettings.Port}";
            }
            else
            {
                // TODO: Substitute with more special Exception
                throw new Exception();
            }

            var fileText = 
                $"//comments\r\n" +
                $"[Main]\r\n" +
                $"Logging={logging}\r\n" +
                $"StatFlushPeriod={project.StatFlushPeriod}\r\n" +
                $"Timeout={project.Timeout}\r\n" +
                $"Port={port}\r\n" +
                $"{connectionSettings}\r\n" +
                $"DeviceID={project.Actions[0].SlaveAddress}\r\n" +
                $"Period={project.Period}\r\n" +
                $"[Reading]//Group#=DeviceID;StartingRegister;Number of Registers;Types\r\n";

            for(int i = 0; i < project.Actions.Count; i++)
            {
                var types = string.Join(";", project.Actions[i].Types.Select(x => x.ToString()));

                fileText += $"{i + 1}={project.Actions[i].StartAddress};{project.Actions[i].NumberOfRegisters};{types}\r\n";
            }

            using (FileStream fs = File.Create(filePath))
            {
                var info = new UTF8Encoding(true).GetBytes(fileText);
                fs.Write(info, 0, info.Length);
            }
        }
    }
}
