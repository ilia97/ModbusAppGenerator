using System.Collections.Generic;

namespace ModbusAppGenerator.Core.Models
{
    public class OperationResult
    {
        public OperationResult()
        {
            Logs = new List<string>();
            Results = new List<Dictionary<string, string>>();
        }

        public List<string> Logs { get; set; }

        public List<Dictionary<string, string>> Results { get; set; }
    }
}
