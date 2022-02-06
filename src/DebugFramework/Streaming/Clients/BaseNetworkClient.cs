using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients
{
    /// <summary>
    /// Base class for network clients
    /// </summary>
    public class BaseNetworkClient
    {
        /// <summary>
        /// Get a number n of free port between a given range
        /// </summary>
        /// <param name="startRange">The start of the range to get ports from</param>
        /// <param name="endRange">The end of the range to get ports from</param>
        /// <param name="number">The number of ports to get</param>
        /// <returns>A number of free ports to use</returns>
        public List<int> GetFreePort(int startRange, int endRange, int number)
        {
            List<int> returnPorts = new List<int>();
            for (int i = 0; i < number; i++)
            {
                returnPorts.Add(GetFreePort(startRange, endRange, returnPorts));
            }
            return returnPorts;
        }

        /// <summary>
        /// Get a number n of free port between a given range async
        /// </summary>
        /// <param name="startRange">The start of the range to get ports from</param>
        /// <param name="endRange">The end of the range to get ports from</param>
        /// <param name="number">The number of ports to get</param>
        /// <returns>A number of free ports to use</returns>
        public async Task<List<int>> GetFreePortAsync(int startRange, int endRange, int number)
        {
            return await Task.Run(() => GetFreePort(startRange, endRange, number));
        }

        /// <summary>
        /// Get a free port between a given range
        /// </summary>
        /// <param name="startRange">The start of the range to get ports from</param>
        /// <param name="endRange">The end of the range to get ports from</param>
        /// <returns>A free port to use</returns>
        public int GetFreePort(int startRange, int endRange) => GetFreePort(startRange, endRange, new List<int>());


        /// <summary>
        /// Get a free port between a given range
        /// </summary>
        /// <param name="startRange">The start of the range to get ports from</param>
        /// <param name="endRange">The end of the range to get ports from</param>
        /// <param name="forbiddenPorts">Ports which should not be used while trying to get free ports</param>
        /// <returns>A free port to use</returns>
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

        /// <summary>
        /// Get a free port between a given range async
        /// </summary>
        /// <param name="startRange">The start of the range to get ports from</param>
        /// <param name="endRange">The end of the range to get ports from</param>
        /// <returns>A free port to use</returns>
        public async Task<int> GetFreePortAsync(int startRange, int endRange) => await GetFreePortAsync(startRange, endRange, new List<int>());

        /// <summary>
        /// Get a free port between a given range async
        /// </summary>
        /// <param name="startRange">The start of the range to get ports from</param>
        /// <param name="endRange">The end of the range to get ports from</param>
        /// <param name="forbiddenPorts">Ports which should not be used while trying to get free ports</param>
        /// <returns>A free port to use</returns>
        public async Task<int> GetFreePortAsync(int startRange, int endRange, List<int> forbiddenPorts) => await Task.Run(() => GetFreePort(startRange, endRange, forbiddenPorts));

        /// <summary>
        /// Get a list with all the ports which are blocked right now
        /// </summary>
        /// <returns>A list with all the blocked ports</returns>
        protected List<int> GetBlockedPorts() => GetBlockedPorts(0, 49151);

        /// <summary>
        /// Get a list with all the ports which are blocked right now
        /// </summary>
        /// <param name="startRange">The start range to search for blocked ports</param>
        /// <param name="endRange">The end range to search for blocked ports</param>
        /// <returns>A list with all the blocked ports</returns>
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

        /// <summary>
        /// Get the local main ip
        /// </summary>
        /// <returns>The local client ip</returns>
        public IPAddress GetLocalIp() => GetUnicastAdressInformation()?.Address;

        /// <summary>
        /// Get the local main ip async
        /// </summary>
        /// <returns>The local client ip</returns>
        public async Task<IPAddress> GetLocalIPAsync()
        {
            UnicastIPAddressInformation unicastIPAddressInformation = await GetUnicastAdressInformationAsync();
            return unicastIPAddressInformation?.Address;
        }

        /// <summary>
        /// Get the unicast ip address information of the fastes currently connected network adapter
        /// </summary>
        /// <returns>The unicast ip address information of the fastes network adapter</returns>
        public UnicastIPAddressInformation GetUnicastAdressInformation()
        {

            return NetworkInterface.GetAllNetworkInterfaces()
                                   .ToList()
                                   .Where(nic => nic.OperationalStatus == OperationalStatus.Up)
                                   .Where(nic => nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet || nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                                   .OrderBy(nic => nic.Speed)
                                   .FirstOrDefault()
                                   ?.GetIPProperties().UnicastAddresses
                                   .FirstOrDefault(unicast => unicast.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
        }

        /// <summary>
        /// Get the unicast ip address information of the fastes currently connected network adapter async
        /// </summary>
        /// <returns>The unicast ip address information of the fastes network adapter</returns>
        public async Task<UnicastIPAddressInformation> GetUnicastAdressInformationAsync() => await Task.Run(() => GetUnicastAdressInformation());
    }
}
