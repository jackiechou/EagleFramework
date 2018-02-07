using System;
using System.Linq;
using System.Net;

namespace Eagle.Core.Common
{
    public static class IpAddressExtensions
    {
        public static bool IsInRange(this IPAddress address, IPAddress lowerAddress, IPAddress upperAddress)
        {

            if (lowerAddress.AddressFamily != address.AddressFamily || address.AddressFamily != upperAddress.AddressFamily)
            {
                return false;
            }

            var lowerBytes = lowerAddress.GetAddressBytes();
            var upperBytes = upperAddress.GetAddressBytes();
            var addressBytes = address.GetAddressBytes();

            var lowerIpNumber = BitConverter.ToInt32(lowerBytes.Reverse().ToArray(), 0);
            var upperIpNumber = BitConverter.ToInt32(upperBytes.Reverse().ToArray(), 0);
            var addressIpNumber = BitConverter.ToInt32(addressBytes.Reverse().ToArray(), 0);

            var minIpNumber = lowerIpNumber <= upperIpNumber ? lowerIpNumber : upperIpNumber;
            var maxIpNumber = lowerIpNumber <= upperIpNumber ? upperIpNumber : lowerIpNumber;

            return minIpNumber <= addressIpNumber && addressIpNumber <= maxIpNumber;
        }
    }
}
