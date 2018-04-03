using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.Ports;
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
using ModbusAppGenerator.ModbusApp.Core.DataAccess;
using ModbusAppGenerator.ModbusApp.Core.Services;

namespace ModbusAppGenerator.Core.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Add(Project project, string userId)
        {
            var projectEntity = Mapper.Map<Project, ProjectEntity>(project);

            foreach (var device in project.Actions)
            {
                var deviceEntity = Mapper.Map<SlaveAction, SlaveActionEntity>(device);

                _unitOfWork.SlaveActionRepository.Insert(deviceEntity);
            }

            if (project.ConnectionSettings != null)
            {
                if (project.ConnectionSettings.GetType() == typeof(IpConnectionSettings))
                {
                    var ipConnectionSettings = Mapper.Map<IpConnectionSettings, IpConnectionSettingsEntity>((IpConnectionSettings)project.ConnectionSettings);

                    _unitOfWork.IpConnectionSettingsRepository.Insert(ipConnectionSettings);

                    projectEntity.SettingId = ipConnectionSettings.Id;
                    projectEntity.ConnectionType = ConnectionTypes.Ip;
                }
                else if (project.ConnectionSettings.GetType() == typeof(ComConnectionSettings))
                {
                    var comConnectionSettings = Mapper.Map<ComConnectionSettings, ComConnectionSettingsEntity>((ComConnectionSettings)project.ConnectionSettings);

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

            if (projectEntity.SettingId > 0)
            {
                switch (projectEntity.ConnectionType)
                {
                    case ConnectionTypes.Com:
                        _unitOfWork.ComConnectionSettingsRepository.Delete(projectEntity.SettingId);
                        break;
                    case ConnectionTypes.Ip:
                        _unitOfWork.IpConnectionSettingsRepository.Delete(projectEntity.SettingId);
                        break;
                }
            }

            foreach (var action in projectEntity.Actions)
            {
                if (action != null)
                {
                    foreach (var dataType in action.Types)
                    {
                        if (dataType != null)
                        {
                            _unitOfWork.DataTypesRepository.Delete(dataType.Id);
                        }
                    }
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
                IpConnectionSettingsEntity ipConnectionSettings = null;

                if (project.ConnectionSettings.Id == 0)
                {
                    ipConnectionSettings = Mapper.Map<IpConnectionSettings, IpConnectionSettingsEntity>((IpConnectionSettings)project.ConnectionSettings);

                    _unitOfWork.IpConnectionSettingsRepository.Insert(ipConnectionSettings);
                }
                else
                {
                    ipConnectionSettings = _unitOfWork.IpConnectionSettingsRepository.GetById(project.ConnectionSettings.Id);

                    Mapper.Map((IpConnectionSettings)project.ConnectionSettings, ipConnectionSettings);

                    _unitOfWork.IpConnectionSettingsRepository.Update(ipConnectionSettings);
                }

                _unitOfWork.Save();

                projectEntity.SettingId = ipConnectionSettings.Id;
                projectEntity.ConnectionType = ConnectionTypes.Ip;
            }
            else if (project.ConnectionSettings.GetType() == typeof(ComConnectionSettings))
            {
                ComConnectionSettingsEntity comConnectionSettings = null;

                if (project.ConnectionSettings.Id == 0)
                {
                    comConnectionSettings = Mapper.Map<ComConnectionSettings, ComConnectionSettingsEntity>((ComConnectionSettings)project.ConnectionSettings);

                    _unitOfWork.ComConnectionSettingsRepository.Insert(comConnectionSettings);
                }
                else
                {
                    comConnectionSettings = _unitOfWork.ComConnectionSettingsRepository.GetById(project.ConnectionSettings.Id);

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

            var project = Mapper.Map<ProjectEntity, Project>(projectEntity);

            switch (projectEntity.ConnectionType)
            {
                case ConnectionTypes.Ip:
                    var ipConnectionSettings = _unitOfWork.IpConnectionSettingsRepository.GetById(projectEntity.SettingId) ?? new IpConnectionSettingsEntity();

                    project.ConnectionSettings = Mapper.Map<IpConnectionSettingsEntity, IpConnectionSettings>(ipConnectionSettings);

                    break;
                case ConnectionTypes.Com:
                    var comConnectionSettings = _unitOfWork.ComConnectionSettingsRepository.GetById(projectEntity.SettingId) ?? new ComConnectionSettingsEntity();

                    project.ConnectionSettings = Mapper.Map<ComConnectionSettingsEntity, ComConnectionSettings>(comConnectionSettings);

                    break;
            }

            var projectSlaveActionEntities = (List<SlaveActionEntity>)_unitOfWork.SlaveActionRepository.Get(x => x.ProjectId == project.Id);
            project.Actions = Mapper.Map<List<SlaveActionEntity>, List<SlaveAction>>(projectSlaveActionEntities);

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

            return Mapper.Map<IList<ProjectEntity>, IList<Project>>(projectsList);
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
                    slaveActionEntity = Mapper.Map<SlaveAction, SlaveActionEntity>(action);
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

        public byte[] DownloadProject(int projectId, ApplicationType type, string userId, string currentDirectoryRoot)
        {
            var project = Get(projectId, userId);

            var consoleAppBinFolderRoute = "";
            var modbusFilesRoutes = new List<string>()
            {
                "NModbus4.dll",
                "ModbusAppGenerator.ModbusApp.Core.dll",
                "Autofac.dll"
            };

            switch (type)
            {
                case ApplicationType.Console:
                    consoleAppBinFolderRoute = $"{currentDirectoryRoot}\\..\\ModbusAppGenerator.ModbusApp.Console\\bin\\Release";

                    modbusFilesRoutes.AddRange(new string[]
                    {
                        "ModbusAppGenerator.ModbusApp.Console.exe",
                        "ModbusAppGenerator.ModbusApp.Console.exe.config",
                        
                    });

                    break;
                case ApplicationType.Service:
                    consoleAppBinFolderRoute = $"{currentDirectoryRoot}\\..\\ModbusAppGenerator.ModbusApp.Service\\bin\\Release";

                    modbusFilesRoutes.AddRange(new string[]
                    {
                        "ModbusAppGenerator.ModbusApp.Service.exe",
                        "ModbusAppGenerator.ModbusApp.Service.exe.config"
                    });

                    break;
            }

            var tempFolderRoute = $"{currentDirectoryRoot}\\Temp";
            var tempAppFolderRoute = $"{tempFolderRoute}\\AppFiles";

            Directory.CreateDirectory(tempAppFolderRoute);

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

        public Dictionary<int, string> TestProject(int projectId, string userId)
        {
            var project = this.Get(projectId, userId);

            var modbusService = new ModbusService(new ModbusSlavesRepository());

            var slaveSettings = new List<ModbusApp.Core.Models.GroupSettings>();

            foreach (var action in project.Actions)
            {
                slaveSettings.Add(new ModbusApp.Core.Models.GroupSettings()
                {
                    Id = action.Id,
                    DeviceId = (byte)action.SlaveAddress,
                    StartAddress = (ushort)action.StartAddress,
                    NumberOfRegisters = (ushort)action.NumberOfRegisters,
                    Types = action.Types.Select(type =>
                    {
                        switch (type)
                        {
                            case ModbusDataType.Hex:
                                return Tuple.Create(2, ModbusApp.Core.Misc.Enums.ModbusDataType.Hex);
                            case ModbusDataType.SInt16:
                                return Tuple.Create(2, ModbusApp.Core.Misc.Enums.ModbusDataType.SInt16);
                            case ModbusDataType.SInt32:
                                return Tuple.Create(4, ModbusApp.Core.Misc.Enums.ModbusDataType.SInt32);
                            case ModbusDataType.UInt16:
                                return Tuple.Create(2, ModbusApp.Core.Misc.Enums.ModbusDataType.UInt16);
                            case ModbusDataType.UInt32:
                                return Tuple.Create(4, ModbusApp.Core.Misc.Enums.ModbusDataType.UInt32);
                            case ModbusDataType.UtcTimestamp:
                                return Tuple.Create(4, ModbusApp.Core.Misc.Enums.ModbusDataType.UtcTimestamp);
                            case ModbusDataType.String:
                                return null;
                        }

                        return null;
                    }).ToList()
                });
            }

            if (project.ConnectionSettings is IpConnectionSettings)
            {
                var connectionSettings = project.ConnectionSettings as IpConnectionSettings;

                return modbusService.GetDataFromSlaves(new ModbusApp.Core.Models.MasterSettingsIp()
                {
                    Host = connectionSettings.Host,
                    IsLoggerEnabled = project.IsLoggerEnabled,
                    Period = project.Period,
                    Port = connectionSettings.Port,
                    SlaveSettings = slaveSettings,
                    StatFlushPeriod = project.StatFlushPeriod,
                    Timeout = project.Timeout
                });
            }
            else if (project.ConnectionSettings is ComConnectionSettings)
            {
                var connectionSettings = project.ConnectionSettings as ComConnectionSettings;

                return modbusService.GetDataFromSlaves(new ModbusApp.Core.Models.MasterSettingsCom()
                {
                    IsLoggerEnabled = project.IsLoggerEnabled,
                    Period = project.Period,
                    SlaveSettings = slaveSettings,
                    StatFlushPeriod = project.StatFlushPeriod,
                    Timeout = project.Timeout,
                    BaudRate = connectionSettings.BaudRate,
                    DataBits = connectionSettings.DataBits,
                    Parity = connectionSettings.Parity,
                    PortName = connectionSettings.PortName,
                    StopBits = connectionSettings.StopBits
                });
            }

            throw new Exception();
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
                $"Period={project.Period}\r\n" +
                $"[Reading]//Group#=DeviceID;StartingRegister;Number of Registers;Types\r\n";

            for (int i = 0; i < project.Actions.Count; i++)
            {
                var types = string.Join(";", project.Actions[i].Types.Select(x => x.ToString()));

                fileText += $"{i + 1}={project.Actions[i].SlaveAddress};{project.Actions[i].StartAddress};{project.Actions[i].NumberOfRegisters};{types}\r\n";
            }

            using (FileStream fs = File.Create(filePath))
            {
                var info = new UTF8Encoding(true).GetBytes(fileText);
                fs.Write(info, 0, info.Length);
            }
        }
    }
}
