using System.Collections.Generic;
using ModbusAppGenerator.ModbusApp.Core.Models;

namespace ModbusAppGenerator.ModbusApp.Core.Services.Interfaces
{
    public interface IModbusService
    {
        void GetAndSaveDataFromSlaves(MasterSettings masterSettings);

        Dictionary<int, string> GetDataFromSlaves(MasterSettings masterSettings);

        void SaveResults(Dictionary<int, string> results);
    }
}
