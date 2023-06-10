using HidApi;

namespace BuzzSharp
{
    public static class BuzzHidFinder
    {
        private const ushort VID = 0x054c, PID = 0x1000;
        
        public static IReadOnlyCollection<BuzzHid> FindHandsets()
        {
            var devices = new List<BuzzHid>();
            
            var deviceInfos = Hid.Enumerate(VID, PID);
            foreach (var deviceInfo in deviceInfos)
            {
                try
                {
                    var hidDevice = deviceInfo.ConnectToDevice();
                    devices.Add(new BuzzHid(hidDevice));
                }
                catch (System.Exception)
                {
                    Console.WriteLine($"Failed to open Buzz device!");
                }
            }

            return devices.AsReadOnly();
        }
    }
}