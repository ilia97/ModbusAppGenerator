using Autofac;
using ModbusAppGenerator.ModbusApp.Core.DataAccess;
using ModbusAppGenerator.ModbusApp.Core.DataAccess.Interfaces;
using ModbusAppGenerator.ModbusApp.Core.Services;
using ModbusAppGenerator.ModbusApp.Core.Services.Interfaces;

namespace ModbusAppGenerator.ModbusApp.Service
{
    public class AutofacConfig
    {
        public static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ModbusMasterInitializer>().As<IModbusMasterInitializer>();
            builder.RegisterType<ModbusSlavesRepository>().As<IModbusSlavesRepository>();
            builder.RegisterType<ModbusService>().As<IModbusService>();

            return builder.Build();
        }
    }
}
