using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ModbusAppGenerator.DataAccess.Entities;

namespace ModbusAppGenerator.DataAccess
{
    public class ModbusAppGeneratorContext : IdentityDbContext<UserEntity>
    {
        public DbSet<ProjectEntity> Projects { get; set; }

        public DbSet<SlaveActionEntity> SlaveActions { get; set; }

        public DbSet<IpConnectionSettingsEntity> IpConnectionSettings { get; set; }

        public DbSet<ComConnectionSettingsEntity> ComConnectionSettings { get; set; }

        public DbSet<DataTypeEntity> DataTypes { get; set; }

        public ModbusAppGeneratorContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ModbusAppGeneratorContext Create()
        {
            return new ModbusAppGeneratorContext();
        }
    }
}
