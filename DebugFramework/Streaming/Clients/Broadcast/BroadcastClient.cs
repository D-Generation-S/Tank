using System.Net;

namespace DebugFramework.Streaming.Clients.Broadcast
{
    public abstract class BroadcastClient : BaseUdpClient
    {
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
