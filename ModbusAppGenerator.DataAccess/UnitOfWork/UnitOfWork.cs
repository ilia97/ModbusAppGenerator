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
        private IRepository<UserEntity> userRepository;
        private IRepository<ProjectEntity> projectRepository;
        private IRepository<DeviceEntity> deviceRepository;

        public IRepository<UserEntity> UserRepository
        {
            get
            {
                return this.userRepository;
            }
        }

        public IRepository<ProjectEntity> ProjectRepository
        {
            get
            {
                return this.projectRepository;
            }
        }

        public IRepository<DeviceEntity> DeviceRepository
        {
            get
            {
                return this.deviceRepository;
            }
        }

        public UnitOfWork(ModbusAppGeneratorContext context,
            IRepository<UserEntity> userRepository,
            IRepository<ProjectEntity> projectRepository,
            IRepository<DeviceEntity> deviceRepository)
        {
            this.context = context;

            this.userRepository = userRepository;
            this.projectRepository = projectRepository;
            this.deviceRepository = deviceRepository;
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
