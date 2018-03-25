namespace ModbusAppGenerator.ModbusApp.Core.Misc
{
    public static class PackagesCounter
    {
        public static int RequestedPackagesCount { get; set; }

        public static int RecievedPackagesCount { get; set; }

        public static int LostPackagesCount
        {
            get
            {
                return RequestedPackagesCount - RecievedPackagesCount;
            }
        }
    }
}
