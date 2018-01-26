using System;
using System.Collections.Generic;
using System.Text;
using ModbusAppGenerator.DataAccess.Entities;
using ModbusAppGenerator.DataAccess.Repository;

namespace ModbusAppGenerator.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<UserEntity> UserRepository { get; }

        IRepository<ProjectEntity> ProjectRepository { get; }

        IRepository<DeviceEntity> DeviceRepository { get; }

        void Save();
    }
}
