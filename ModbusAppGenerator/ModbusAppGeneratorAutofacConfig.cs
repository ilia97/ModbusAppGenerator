using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using ModbusAppGenerator.Core.Services;
using ModbusAppGenerator.Core.Services.Interfaces;
using ModbusAppGenerator.DataAccess;
using ModbusAppGenerator.DataAccess.Repository;
using ModbusAppGenerator.DataAccess.UnitOfWork;

namespace ModbusAppGenerator
{
    public class ModbusAppGeneratorAutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<ModbusAppGeneratorContext>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            builder.RegisterType<ProjectService>().As<IProjectService>();

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));

            var container = builder.Build();
            
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}