using System.Linq;
using AutoMapper;
using ModbusAppGenerator.Core.Models;
using ModbusAppGenerator.DataAccess.Entities;
using ModbusAppGenerator.ViewModels.ManageViewModels;
using ModbusAppGenerator.ViewModels.ProjectViewModels;

namespace ModbusAppGenerator
{
    public class ModbusAppGeneratorAutoMapper : Profile
    {
        public ModbusAppGeneratorAutoMapper()
        {
            CreateMap<User, UserEntity>();
            CreateMap<UserEntity, User>();

            CreateMap<Project, ProjectEntity>();
            CreateMap<ProjectEntity, Project>();

            CreateMap<SlaveAction, SlaveActionEntity>()
                .ForMember("Types", opt => opt.MapFrom(action =>
                action.Types.Select(type => new DataTypeEntity()
                {
                    SlaveActionEntityId = action.Id,
                    Type = type
                }))) ;
            CreateMap<SlaveActionEntity, SlaveAction>()
                .ForMember("Types", opt => opt.MapFrom(action =>
                action.Types.Select(type => type.Type))); ;

            CreateMap<IpConnectionSettings, IpConnectionSettingsEntity>();
            CreateMap<IpConnectionSettingsEntity, IpConnectionSettings>();

            CreateMap<ComConnectionSettings, ComConnectionSettingsEntity>();
            CreateMap<ComConnectionSettingsEntity, ComConnectionSettings>();

            CreateMap<IndexViewModel, UserEntity>();
            CreateMap<UserEntity, IndexViewModel>();

            CreateMap<CreateProjectViewModel, CreateIpProjectViewModel>();
            CreateMap<CreateIpProjectViewModel, CreateProjectViewModel>();

            CreateMap<CreateProjectViewModel, CreateComProjectViewModel>();
            CreateMap<CreateComProjectViewModel, CreateProjectViewModel>();

            CreateMap<Project, CreateIpProjectViewModel>();
            CreateMap<CreateIpProjectViewModel, Project>();

            CreateMap<Project, CreateComProjectViewModel>();
            CreateMap<CreateComProjectViewModel, Project>();

            CreateMap<Project, CreateProjectViewModel>();
            CreateMap<CreateProjectViewModel, Project>();

            CreateMap<Project, EditIpProjectViewModel>()
                .ForMember("Host", opt => opt.MapFrom(project =>
                ((IpConnectionSettings)project.ConnectionSettings).Host))
                .ForMember("Port", opt => opt.MapFrom(project =>
                ((IpConnectionSettings)project.ConnectionSettings).Port));
            CreateMap<EditIpProjectViewModel, Project>();

            CreateMap<Project, EditComProjectViewModel>()
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
            CreateMap<EditComProjectViewModel, Project>();

            CreateMap<Project, AddProjectActionsViewModel>();
            CreateMap<AddProjectActionsViewModel, Project>();

            CreateMap<ActionViewModel, SlaveAction>()
                .ForMember("Types", opt => opt.MapFrom(action => 
                action.Types.Split(';', System.StringSplitOptions.RemoveEmptyEntries).ToList()));
            CreateMap<SlaveAction, ActionViewModel>()
                .ForMember("Types", opt => opt.MapFrom(action => 
                string.Join(';', action.Types.Select(type => type.ToString()))));

            CreateMap<Project, DetailsViewModel>();
            CreateMap<DetailsViewModel, Project>();
        }
    }
}
