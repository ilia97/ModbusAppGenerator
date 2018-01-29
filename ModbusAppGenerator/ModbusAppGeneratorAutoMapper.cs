using AutoMapper;
using ModbusAppGenerator.Core.Models;
using ModbusAppGenerator.DataAccess.Entities;
using ModbusAppGenerator.ViewModels.ManageViewModels;

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

            CreateMap<SlaveAction, SlaveActionEntity>();
            CreateMap<SlaveActionEntity, SlaveAction>();

            CreateMap<IpConnectionSettings, IpConnectionSettingsEntity>();
            CreateMap<IpConnectionSettingsEntity, IpConnectionSettings>();

            CreateMap<ComConnectionSettings, ComConnectionSettingsEntity>();
            CreateMap<ComConnectionSettingsEntity, ComConnectionSettings>();

            CreateMap<IndexViewModel, UserEntity>();
            CreateMap<UserEntity, IndexViewModel>();
        }
    }
}
