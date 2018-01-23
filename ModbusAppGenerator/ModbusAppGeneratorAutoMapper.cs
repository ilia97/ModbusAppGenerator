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

            CreateMap<IndexViewModel, UserEntity>();
            CreateMap<UserEntity, IndexViewModel>();
        }
    }
}
