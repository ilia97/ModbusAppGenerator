﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Ports;
using System.Linq;
using Core.DataAccess.Interfaces;
using Core.Misc.Enums;
using Core.Models;
using Core.Misc.Exceptions;

namespace Core.DataAccess
{
    public class ModbusMasterInitializer : IModbusMasterInitializer
    {
        public MasterSettings GetMasterSettings()
        {
            // Получаем имя файла настроек из файла конфигураций.
            var initFileName = ConfigurationManager.AppSettings["InitFileName"];

            List<string> fileLines;
            try
            {
                // Считываем все строки из файла конфигураций.
                fileLines = File.ReadAllLines(initFileName).ToList();
            }
            catch(Exception)
            {
                throw new FileNotFoundException($"Settings file {initFileName} wasn't found.");
            }

            // Удаляем все комментарии, чтобы не учитывать их при считывании файла.
            // Если в строке содержится символ комментария, то обрезаем строку до этого символа.
            fileLines =
                fileLines.Select(
                    fileLine =>
                        fileLine.IndexOf("//", StringComparison.Ordinal) > -1
                            ? fileLine.Substring(0, fileLine.IndexOf("//", StringComparison.Ordinal)).Trim()
                            : fileLine.Trim()).ToList();

            // Удаляем все пустуе строки, которые образовались в результате предыдущего шага.
            fileLines.RemoveAll(string.IsNullOrWhiteSpace);

            // На первой строке написана строка "[Main]" просто для удобства чтения, её игнорируем.
            // На второй строке после знака равно должна располагаться настройка, отвечающая за то, включено логирование или нет.
            if (fileLines[1].Split('=')[0].Trim().ToLower() != "logging")
            {
                throw new InvalidSettingsException($"Exception occured when getting application settings from \"{initFileName}\".\r\nLogger settings must be placed on the line 2. For example:\r\nLogging=Yes");
            }

            var isLoggerEnabledString = fileLines[1].Split('=')[1].Trim().ToLower();
            var isLoggerEnabled = true;
            switch (isLoggerEnabledString)
            {
                case "yes":
                    break;
                case "no":
                    isLoggerEnabled = false;
                    break;
                default:
                    throw new InvalidSettingsException($"Exception occured when getting application settings from \"{initFileName}\".\r\nLogger value must be equal to \"Yes\" or \"No\" (line 2). For example:\r\nLogging=Yes");
            }

            // На первой строке написана строка "[Main]" просто для удобства чтения, её игнорируем.
            // На второй строке после знака равно должна располагаться настройка, отвечающая за то, включено логирование или нет.
            if (fileLines[2].Split('=')[0].Trim().ToLower() != "statflushperiod")
            {
                throw new InvalidSettingsException($"Exception occured when getting application settings from \"{initFileName}\".\r\nStatFlushPeriod settings must be placed on the line 3. For example:\r\nStatFlushPeriod=1");
            }

            int statFlushPeriod;
            var statFlushPeriodString = fileLines[2].Split('=')[1].Trim().ToLower();
            if (!int.TryParse(statFlushPeriodString, out statFlushPeriod))
            {
                statFlushPeriod = 0;
            }

            // На третьей строке после знака равно располагается настройка отвечающая за величину таймаута запроса.
            if (fileLines[3].Split('=')[0].Trim().ToLower() != "timeout")
            {
                throw new InvalidSettingsException($"Exception occured when getting application settings from \"{initFileName}\".\r\nTimeout settings must be placed on the line 3. For example:\r\nTimeout=1000");
            }

            int timeout;
            try
            {
                // Преобразуем к целому числу. 
                timeout = Convert.ToInt32(fileLines[3].Split('=')[1].Trim());
            }
            catch (Exception)
            {
                throw new InvalidSettingsException($"Exception occured when getting application settings from \"{initFileName}\".\r\nTimeout must be a positive integer number (line 3). For example:\r\nTimeout=1000");
            }

            // На четвёртой строке после знака равно располагается тип соединения (COM или IP).
            if (fileLines[4].Split('=')[0].Trim().ToLower() != "port")
            {
                throw new InvalidSettingsException($"Exception occured when getting application settings from \"{initFileName}\".\r\nPort settings must be placed on the line 4. For example:\r\nPort=IP");
            }

            var portTypeString = fileLines[4].Split('=')[1].Trim();
            var portType = PortType.IP;
            switch (portTypeString.ToLower())
            {
                case "ip":
                    break;
                case "com":
                    portType = PortType.COM;
                    break;
                default:
                    throw new InvalidSettingsException($"Exception occured when getting application settings from \"{initFileName}\".\r\nPort must be a equal to \"IP\" or \"COM\" (line 4). For example:\r\nPort=IP");
            }

            // На шестой строке после знака равно располагается идентификатор устройства.
            if (fileLines[6].Split('=')[0].Trim().ToLower() != "deviceid")
            {
                throw new InvalidSettingsException($"Exception occured when getting application settings from \"{initFileName}\".\r\nDeviceID settings must be placed on the line 6. For example:\r\nDeviceID=10");
            }

            byte deviceId;
            try
            {
                // Преобразуем к целому однобайтовому числу. 
                deviceId = Convert.ToByte(fileLines[6].Split('=')[1]);
            }
            catch (Exception)
            {
                throw new InvalidSettingsException($"Exception occured when getting application settings from \"{initFileName}\".\r\nDeviceId must be a positive byte number (line 6). For example:\r\nDeviceID=10");
            }

            // На седьмой строке после знака равно располагается интервал опроса контроллеров.
            if (fileLines[7].Split('=')[0].Trim().ToLower() != "period")
            {
                throw new InvalidSettingsException($"Exception occured when getting application settings from \"{initFileName}\".\r\nPeriod settings must be placed on the line 7. For example:\r\nPeriod=10");
            }

            int period;
            try
            {
                // Преобразуем к целому однобайтовому числу. 
                period = Convert.ToInt32(fileLines[7].Split('=')[1]);
            }
            catch (Exception)
            {
                throw new InvalidSettingsException($"Exception occured when getting application settings from \"{initFileName}\".\r\nPeriod must be a positive integer number (line 6). For example:\r\nPeriod=10");
            }

            // На восьмой строке написана строка "[Reading]" просто для удобства чтения, её игнорируем.
            // На строках, начиная с девятой расположена информация о группах контроллеров.
            var groups = new List<GroupSettings>();
            for (var i = 9; i < fileLines.Count; i++)
            {
                try
                {
                    // До знака равно находится номер группы, после знака равно информация о принимаемых данных для этой группы.
                    var slaveDetails = fileLines[i].Split('=');

                    // Все данные для группы расположены после знака "=". 
                    var slaveSettings = slaveDetails[1];

                    // До первой точки с запятой располагается номер первого регистра, заполненного данными.
                    // Обрезаем строку по этому символу.
                    var startAddress = slaveSettings.Substring(0, slaveSettings.IndexOf(";", StringComparison.Ordinal));
                    slaveSettings = slaveSettings.Substring(slaveSettings.IndexOf(";", StringComparison.Ordinal) + 1);

                    // Между первой и второй точкой с запятой располагается количество регистров, заполненных данными.
                    var registersCount = slaveSettings.Substring(0, slaveSettings.IndexOf(";", StringComparison.Ordinal));
                    slaveSettings = slaveSettings.Substring(slaveSettings.IndexOf(";", StringComparison.Ordinal) + 1);

                    // Остальная часть строки содержит типы данных, перечисленные черезточку с запятой.
                    var dataTypes = slaveSettings.Split(';');

                    // Добавляем в список объектов новый объект, содержащий всю вышеперечисленную информацию.
                    groups.Add(new GroupSettings()
                    {
                        Id = Convert.ToInt32(slaveDetails[0]),
                        StartAddress = Convert.ToUInt16(startAddress),
                        NumberOfRegisters = Convert.ToUInt16(registersCount),
                        Types = dataTypes.Select(x =>
                        {
                            switch (x.ToLower())
                            {
                                case "utc_timestamp":
                                    return Tuple.Create(4, ModbusDataType.UtcTimestamp);
                                case "sint16":
                                    return Tuple.Create(2, ModbusDataType.SInt16);
                                case "uint16":
                                    return Tuple.Create(2, ModbusDataType.UInt16);
                                case "sint32":
                                    return Tuple.Create(4, ModbusDataType.SInt32);
                                case "uint32":
                                    return Tuple.Create(4, ModbusDataType.UInt32);
                                case "hex":
                                    return Tuple.Create(2, ModbusDataType.Hex);
                                default:
                                    if (x.ToLower().StartsWith("string8_"))
                                    {
                                        try
                                        {
                                            // Количество байт записано после части "string8_" (8 символов).
                                            var bytesCount = Convert.ToInt32(x.ToLower().Substring(8));

                                            return Tuple.Create(bytesCount, ModbusDataType.String);
                                        }
                                        catch
                                        {
                                            throw new InvalidTypeException(
                                                $"Exception occured when getting application settings from \"{initFileName}\".\r\n String data type has an incorrect format (line {i + 1}).\r\n Correct string data type format: String8_[number of chars in string].");
                                        }
                                    }
                                    else
                                    {
                                        throw new InvalidTypeException(
                                            $"Exception occured when getting application settings from \"{initFileName}\".\r\n Data type has an incorrect format (line {i + 1}).\r\n Correct data types are: UInt16, SInt16, UInt32, SInt32, String8_[number of chars in string], UtcTimestamp, Hex.");
                                    }
                            }
                        }).ToList()
                    });
                }
                catch (Exception)
                {
                    throw new InvalidSettingsException($"Exception occured when getting application settings from \"{initFileName}\".\r\nGroup declaration has an incorrect format (line {i + 1}).\r\n The correct declaration is: [Group number]=[StartingRegister];[Number of Registers];[Types splitted with \";\"] Example of group declaration:\r\n2=2050;4;Uint32;Uint32");
                }
            }

            // В зависимости от того, какой тип соединения прописан на пятой строке файла,
            // мы создаём разные типы объектов, содержащие полную информацию о соединении.
            switch (portType)
            {
                case PortType.IP:
                    try
                    {
                        if (fileLines[5].Split('=')[0].Trim().ToLower() != "ip")
                        {
                            throw new InvalidSettingsException($"Exception occured when getting application settings from \"{initFileName}\".\r\nIP connection settings must be placed on the line 5. For example:\r\nIP=127.0.0.1:502");
                        }

                        var ipAddress = fileLines[5].Split('=')[1];

                        return new MasterSettingsIp
                        {
                            Host = ipAddress.Split(':')[0],
                            IsLoggerEnabled = isLoggerEnabled,
                            Period = period,
                            DeviceId = deviceId,
                            Port = Convert.ToInt32(ipAddress.Split(':')[1]),
                            SlaveSettings = groups,
                            StatFlushPeriod = statFlushPeriod,
                            Timeout = timeout
                        };
                    }
                    catch (Exception)
                    {
                        throw new InvalidSettingsException($"Exception occured when getting application settings from \"{initFileName}\".\r\nIP connection type has an incorrect format (line 5).\r\n The correct declaration is: IP=[Ip address]:[Port]. Example of connection declaration:\r\nIP=127.0.0.1:502");
                    }
                case PortType.COM:
                    try
                    {
                        if (fileLines[5].Split('=')[0].Trim().ToLower() != "com")
                        {
                            throw new InvalidSettingsException($"Exception occured when getting application settings from \"{initFileName}\".\r\nCOM connection settings must be placed on the line 5. For example:\r\nCOM=COM9;9600;8N1");
                        }
                        var comSettings = fileLines[5].Split('=')[1].Split(';');

                        var portName = comSettings[0];
                        var baudRate = Convert.ToInt32(comSettings[1]);
                        var dataBits = Convert.ToInt32(comSettings[2][0].ToString());

                        var parity = Parity.Even;
                        switch (comSettings[2][1])
                        {
                            case 'N':
                                parity = Parity.None;
                                break;
                            case 'E':
                                parity = Parity.Even;
                                break;
                            case 'M':
                                parity = Parity.Mark;
                                break;
                            case 'O':
                                parity = Parity.Odd;
                                break;
                            case 'S':
                                parity = Parity.Space;
                                break;
                        }

                        var stopBits = StopBits.None;
                        switch (comSettings[2].Substring(2))
                        {
                            case "0":
                                stopBits = StopBits.None;
                                break;
                            case "1":
                                stopBits = StopBits.One;
                                break;
                            case "1.5":
                                stopBits = StopBits.OnePointFive;
                                break;
                            case "2":
                                stopBits = StopBits.Two;
                                break;
                        }

                        return new MasterSettingsCom
                        {
                            PortName = portName,
                            BaudRate = baudRate,
                            DataBits = dataBits,
                            StopBits = stopBits,
                            Parity = parity,
                            IsLoggerEnabled = isLoggerEnabled,
                            Period = period,
                            DeviceId = deviceId,
                            SlaveSettings = groups,
                            StatFlushPeriod = statFlushPeriod,
                            Timeout = timeout
                        };
                    }
                    catch (Exception)
                    {
                        throw new InvalidSettingsException($"Exception occured when getting application settings from \"{initFileName}\".\r\nCOM connection type has an incorrect format (line 5).\r\n The correct declaration is: COM=[Port name];[Baud rate];[Data Bits][Parity][Stop Bits] Example of connection declaration:\r\nCOM=COM9;9600;8N1");
                    }
                default:
                    throw new InvalidSettingsException($"Exception occured when getting application settings from \"{initFileName}\".\r\nPort must be a equal to \"IP\" or \"COM\" (line 4). For example:\r\nPort=IP");
            }
        }
    }
}
