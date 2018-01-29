namespace ModbusAppGenerator.DataAccess.Enums
{
    public enum StopBits
    {
        //
        // Summary:
        //     No stop bits are used. This value is not supported by the System.IO.Ports.SerialPort.StopBits
        //     property.
        None = 0,
        //
        // Summary:
        //     One stop bit is used.
        One = 1,
        //
        // Summary:
        //     Two stop bits are used.
        Two = 2,
        //
        // Summary:
        //     1.5 stop bits are used.
        OnePointFive = 3
    }
}
