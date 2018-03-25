using ModbusAppGenerator.ModbusApp.Core.Models;

namespace ModbusAppGenerator.ModbusApp.Core.DataAccess.Interfaces
{
    public interface IModbusMasterInitializer
    {
        MasterSettings GetMasterSettings();
    }
}
