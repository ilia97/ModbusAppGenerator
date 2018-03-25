using System.Linq;
using AutoMapper;
using ModbusAppGenerator.Core.Models;
using ModbusAppGenerator.DataAccess.Entities;
using ModbusAppGenerator.ViewModels.ManageViewModels;
using ModbusAppGenerator.ViewModels.ProjectViewModels;

namespace ModbusAppGenerator
{
    public class ModbusAppGeneratorAutoMapper
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg=>
            {
                cfg.CreateMap<User, UserEntity>();
                cfg.CreateMap<UserEntity, User>();

                cfg.CreateMap<Project, ProjectEntity>();
                cfg.CreateMap<ProjectEntity, Project>();

                cfg.CreateMap<SlaveAction, SlaveActionEntity>()
                .ForMember("Types", opt => opt.MapFrom(action =>
                action.Types.Select(type => new DataTypeEntity()
                {
                    SlaveActionEntityId = action.Id,
                    Type = type
                })));
                cfg.CreateMap<SlaveActionEntity, SlaveAction>()
                .ForMember("Types", opt => opt.MapFrom(action =>
                action.Types.Select(type => type.Type)));

                cfg.CreateMap<IpConnectionSettings, IpConnectionSettingsEntity>();
                cfg.CreateMap<IpConnectionSettingsEntity, IpConnectionSettings>();

                cfg.CreateMap<ComConnectionSettings, ComConnectionSettingsEntity>();
                cfg.CreateMap<ComConnectionSettingsEntity, ComConnectionSettings>();

                cfg.CreateMap<IndexViewModel, UserEntity>();
                cfg.CreateMap<UserEntity, IndexViewModel>();

                cfg.CreateMap<CreateProjectViewModel, CreateIpProjectViewModel>();
                cfg.CreateMap<CreateIpProjectViewModel, CreateProjectViewModel>();

                cfg.CreateMap<CreateProjectViewModel, CreateComProjectViewModel>();
                cfg.CreateMap<CreateComProjectViewModel, CreateProjectViewModel>();

                cfg.CreateMap<Project, CreateIpProjectViewModel>();
                cfg.CreateMap<CreateIpProjectViewModel, Project>();

                cfg.CreateMap<Project, CreateComProjectViewModel>();
                cfg.CreateMap<CreateComProjectViewModel, Project>();

                cfg.CreateMap<Project, CreateProjectViewModel>();
                cfg.CreateMap<CreateProjectViewModel, Project>();

                cfg.CreateMap<Project, EditIpProjectViewModel>()
                .ForMember("Host", opt => opt.MapFrom(project =>
                ((IpConnectionSettings)project.ConnectionSettings).Host))
                .ForMember("Port", opt => opt.MapFrom(project =>
                ((IpConnectionSettings)project.ConnectionSettings).Port));
                cfg.CreateMap<EditIpProjectViewModel, Project>();

                cfg.CreateMap<Project, EditComProjectViewModel>()
                .ForMember("PortName", opt => opt.MapFrom(project =>
                ((ComConnectionSettings)project.ConnectionSettings).PortName))
                .ForMember("BaudRate", opt => opt.MapFrom(project =>
                ((ComConnectionSettings)project.ConnectionSettings).BaudRate))
                .ForMember("DataBits", opt => opt.MapFrom(project =>
                ((ComConnectionSettings)project.ConnectionSettings).DataBits))
                .ForMember("Parity", opt => opt.MapFrom(project =>
                ((ComConnectionSettings)project.ConnectionSettings).Parity))
                .ForMember("StopBits", opt => opt.MapFrom(project =>
                ((ComConnectionSettings)project.ConnectionSettings).StopBits));
                cfg.CreateMap<EditComProjectViewModel, Project>();

                cfg.CreateMap<Project, AddProjectActionsViewModel>();
                cfg.CreateMap<AddProjectActionsViewModel, Project>();

                cfg.CreateMap<ActionViewModel, SlaveAction>()
                .ForMember("Types", opt => opt.MapFrom(action =>
                action.Types.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries).ToList()));
                cfg.CreateMap<SlaveAction, ActionViewModel>()
                    .ForMember("Types", opt => opt.MapFrom(action =>
                    string.Join(";", action.Types.Select(type => type.ToString()))));

                cfg.CreateMap<Project, DetailsViewModel>();
                cfg.CreateMap<DetailsViewModel, Project>();
            });
        }
    }
}