using System;
using System.Collections.Generic;
using System.Text;
using ModbusAppGenerator.DataAccess.Entities;
using ModbusAppGenerator.DataAccess.Repository;

namespace ModbusAppGenerator.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private ModbusAppGeneratorContext context;
        private IRepository<UserEntity> _userRepository;
        private IRepository<ProjectEntity> _projectRepository;
        private IRepository<SlaveActionEntity> _slaveActionRepository;
        private IRepository<IpConnectionSettingsEntity> _ipConnectionSettingsRepository;
        private IRepository<ComConnectionSettingsEntity> _comConnectionSettingsRepository;
        private IRepository<DataTypeEntity> _dataTypesRepository;

        public IRepository<UserEntity> UserRepository
        {
            get
            {
                return this._userRepository;
            }
        }

        public IRepository<ProjectEntity> ProjectRepository
        {
            get
            {
                return this._projectRepository;
            }
        }

        public IRepository<SlaveActionEntity> SlaveActionRepository
        {
            get
            {
                return this._slaveActionRepository;
            }
        }

        public IRepository<IpConnectionSettingsEntity> IpConnectionSettingsRepository
        {
            get
            {
                return this._ipConnectionSettingsRepository;
            }
        }

        public IRepository<ComConnectionSettingsEntity> ComConnectionSettingsRepository
        {
            get
            {
                return this._comConnectionSettingsRepository;
            }
        }

        public IRepository<DataTypeEntity> DataTypesRepository
        {
            get
            {
                return this._dataTypesRepository;
            }
        }

        public UnitOfWork(ModbusAppGeneratorContext context,
            IRepository<UserEntity> userRepository,
            IRepository<ProjectEntity> projectRepository,
            IRepository<SlaveActionEntity> slaveActionRepository,
            IRepository<IpConnectionSettingsEntity> ipConnectionSettingsRepository,
            IRepository<ComConnectionSettingsEntity> comConnectionSettingsRepository,
            IRepository<DataTypeEntity> dataTypesRepository)
        {
            this.context = context;

            this._userRepository = userRepository;
            this._projectRepository = projectRepository;
            this._slaveActionRepository = slaveActionRepository;
            this._ipConnectionSettingsRepository = ipConnectionSettingsRepository;
            this._comConnectionSettingsRepository = comConnectionSettingsRepository;
            this._dataTypesRepository = dataTypesRepository;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
