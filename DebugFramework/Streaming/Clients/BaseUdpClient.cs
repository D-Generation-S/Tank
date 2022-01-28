using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients
{
    public class BaseUdpClient
    {
        public List<int> GetFreePort(int startRange, int endRange, int number)
        {
            List<int> returnPorts = new List<int>();
            for (int i = 0; i < number; i++)
            {
                returnPorts.Add(GetFreePort(startRange, endRange, returnPorts));
            }
            return returnPorts;
        }

        public async Task<List<int>> GetFreePortAsync(int startRange, int endRange, int number)
        {
            return await Task.Run(() => GetFreePort(startRange, endRange, number));
        }

        public int GetFreePort(int startRange, int endRange) => GetFreePort(startRange, endRange, new List<int>());

        public int GetFreePort(int startRange, int endRange, List<int> forbiddenPorts)
        {
            int returnInt = 0;

            startRange = startRange == 0 ? 1 : startRange;
            forbiddenPorts = forbiddenPorts ?? new List<int>();

            List<int> portArray = new List<int>();
            portArray.AddRange(forbiddenPorts);
            portArray.AddRange(GetBlockedPorts(startRange, endRange));
            portArray.Sort();

            for (int i = startRange; i < endRange; i++)
            {
                if (!portArray.Contains(i))
                {
                    returnInt = i;
                }
            }

            return returnInt;
        }

        public List<int> GetBlockedPorts() => GetBlockedPorts(0, 49151);

        public List<int> GetBlockedPorts(int startRange, int endRange)
        {
            List<int> blockedPorts = new List<int>();

            IPGlobalProperties iPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            blockedPorts.AddRange(iPGlobalProperties.GetActiveTcpConnections()
                                                 .Where(x => x.LocalEndPoint.Port >= startRange)
                                                 .Where(x => x.LocalEndPoint.Port <= endRange)
                                                 .Select(x => x.LocalEndPoint.Port));
            blockedPorts.AddRange(iPGlobalProperties.GetActiveTcpListeners()
                                     .Where(x => x.Port >= startRange)
                                     .Where(x => x.Port <= endRange)
                                     .Select(x => x.Port));
            blockedPorts.AddRange(iPGlobalProperties.GetActiveUdpListeners()
                                     .Where(x => x.Port >= startRange)
                                     .Where(x => x.Port <= endRange)
                                     .Select(x => x.Port));

            return blockedPorts;
        }

        public async Task<int> GetFreePortAsync(int startRange, int endRange, List<int> forbiddenPorts)
        {
            return await Task.Run(() => GetFreePort(startRange, endRange, forbiddenPorts));
        }

        public async Task<int> GetFreePortAsync(int startRange, int endRange) => await GetFreePortAsync(startRange, endRange, new List<int>());

        public IPAddress GetClientIp()
        {
            UnicastIPAddressInformation unicastIPAddressInformation = GetUnicastAdressInformation();
            return unicastIPAddressInformation?.Address;
        }

        public async Task<IPAddress> GetClientIpAsync()
        {
            UnicastIPAddressInformation unicastIPAddressInformation = await GetUnicastAdressInformationAsync();
            return unicastIPAddressInformation?.Address;
        }

        public UnicastIPAddressInformation GetUnicastAdressInformation()
        {

            NetworkInterface fastesInterface = NetworkInterface.GetAllNetworkInterfaces()
                                                   .ToList()
                                                   .Where(nic => nic.OperationalStatus == OperationalStatus.Up)
                                                   .Where(nic => nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet || nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                                                   .OrderBy(nic => nic.Speed)
                                                   .FirstOrDefault();
            return fastesInterface.GetIPProperties().UnicastAddresses.FirstOrDefault(unicast => unicast.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
        }

        public async Task<UnicastIPAddressInformation> GetUnicastAdressInformationAsync()
        {
            return await Task.Run(() => GetUnicastAdressInformation());

        }


    }
}
