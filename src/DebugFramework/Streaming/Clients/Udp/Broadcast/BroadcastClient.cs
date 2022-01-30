using System.Net;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Udp.Broadcast
{
    public abstract class BroadcastClient : BaseNetworkClient
    {
        protected async Task<IPAddress> GetBroadcastAddressAsync(IPAddress ipAddress, IPAddress subnetMask)
        {
            return await Task.Run(() => GetBroadcastAddress(ipAddress, subnetMask));
        }

        protected IPAddress GetBroadcastAddress(IPAddress ipAddress, IPAddress subnetMask)
        {
            byte[] ipAddressBytes = ipAddress.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAddressBytes.Length != subnetMaskBytes.Length)
            {
                return null;
            }

            byte[] broadcastAddress = new byte[ipAddressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAddressBytes[i] | subnetMaskBytes[i] ^ 255);
            }
            return new IPAddress(broadcastAddress);
        }
    }
}
