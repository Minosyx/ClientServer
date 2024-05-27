using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wspólne
{
    
    public class UDPSplitter
    {
        private const int MaxPacketSize = 65499;
        private readonly Dictionary<string, Dictionary<int, string>> _packets = new();
        private int? totalPackets = null;
        public IEnumerable<byte[]> SplitPacket(string data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            int totalPackets = (int)Math.Ceiling((double)bytes.Length / MaxPacketSize);
            List<byte[]> packets = new();

            for (int i = 0; i < totalPackets; i++)
            {
                int packetSize = Math.Min(MaxPacketSize, bytes.Length - i * MaxPacketSize);
                byte[] packet = new byte[packetSize + 8];
                Array.Copy(BitConverter.GetBytes(i), 0, packet, 0, 4);
                Array.Copy(BitConverter.GetBytes(totalPackets), 0, packet, 4, 4);
                Array.Copy(bytes, i * MaxPacketSize, packet, 8, packetSize);
                
                packets.Add(packet);
            }
            return packets;
        }

        public string? ReassemblePacket(byte[] packet, string id)
        {
            int sequenceNumber = BitConverter.ToInt32(packet, 0);
            totalPackets ??= BitConverter.ToInt32(packet, 4);

            if (!_packets.ContainsKey(id))
            {
                _packets[id] = new Dictionary<int, string>(totalPackets.Value);
            }

            string data = Encoding.ASCII.GetString(packet, 8, packet.Length - 8);

            _packets[id][sequenceNumber] = data;

            if (_packets[id].Count != totalPackets) return null;
            StringBuilder sb = new();
            for (int i = 0; i < totalPackets; i++)
            {
                sb.Append(_packets[id][i]);
            }
            _packets.Remove(id);
            totalPackets = null;
            return sb.ToString();
        }
    }
}
