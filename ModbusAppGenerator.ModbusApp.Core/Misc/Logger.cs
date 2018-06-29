using System;
using System.Configuration;
using System.IO;

namespace ModbusAppGenerator.ModbusApp.Core.Misc
{
    public static class Logger
    {
        public static bool WriteLogsToConsole;

        private static string logFileName = "3MBP.log";
        private static string dataFolderName = "DATA";

        public static void Write(string error)
        {
            if (!Directory.Exists(dataFolderName))
            {
                Directory.CreateDirectory(dataFolderName);
            }
            
            var filePath = Path.Combine(dataFolderName, logFileName);

            File.AppendAllText(filePath, $"{DateTime.Now:yyyy:MM:dd HH:mm:ss} {error}\r\n");
            
            if (WriteLogsToConsole)
            {
                Console.WriteLine(error);
            }
        }

        public static void WriteDebug(string text)
        {
            var dataFolderName = ConfigurationManager.AppSettings["DataFolderName"];

            if (!Directory.Exists(dataFolderName))
            {
                Directory.CreateDirectory(dataFolderName);
            }
            
            var fileName = $"3MBP_{DateTime.Now:yyyy-MM-dd}.dbg";
            
            var filePath = Path.Combine(dataFolderName, fileName);
            
            File.AppendAllText(filePath, $"{DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc):HH:mm:ss} {text}\r\n");

            if (WriteLogsToConsole)
            {
                Console.WriteLine($"{DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc):HH:mm:ss} {text}\r\n");
            }
        }
    }
}
