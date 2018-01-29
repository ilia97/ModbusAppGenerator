using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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

        public ModbusAppGeneratorContext(DbContextOptions<ModbusAppGeneratorContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
