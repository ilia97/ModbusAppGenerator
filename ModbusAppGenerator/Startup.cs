using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModbusAppGenerator.Core.Services.Interfaces;
using ModbusAppGenerator.Core.Services;
using ModbusAppGenerator.DataAccess;
using ModbusAppGenerator.DataAccess.Entities;
using ModbusAppGenerator.DataAccess.UnitOfWork;
using ModbusAppGenerator.DataAccess.Repository;

namespace ModbusAppGenerator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ModbusAppGeneratorContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<UserEntity, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
            })
                .AddEntityFrameworkStores<ModbusAppGeneratorContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IProjectService, ProjectService>();

            services.AddTransient<IRepository<UserEntity>, Repository<UserEntity>>();
            services.AddTransient<IRepository<ProjectEntity>, Repository<ProjectEntity>>();
            services.AddTransient<IRepository<SlaveActionEntity>, Repository<SlaveActionEntity>>();
            services.AddTransient<IRepository<IpConnectionSettingsEntity>, Repository<IpConnectionSettingsEntity>>();
            services.AddTransient<IRepository<ComConnectionSettingsEntity>, Repository<ComConnectionSettingsEntity>>();
            services.AddTransient<IRepository<DataTypeEntity>, Repository<DataTypeEntity>>();

            services.AddMvc();

            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
