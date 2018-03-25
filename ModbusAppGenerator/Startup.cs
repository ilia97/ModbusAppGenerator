using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ModbusAppGenerator.Startup))]
namespace ModbusAppGenerator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
