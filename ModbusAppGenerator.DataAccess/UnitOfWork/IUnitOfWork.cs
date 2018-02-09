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

        IRepository<SlaveActionEntity> SlaveActionRepository { get; }

        IRepository<IpConnectionSettingsEntity> IpConnectionSettingsRepository { get; }

        IRepository<ComConnectionSettingsEntity> ComConnectionSettingsRepository { get; }

        IRepository<DataTypeEntity> DataTypesRepository { get; }

        void Save();
    }
}
