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
        private readonly Dictionary<string, Dictionary<int, string>> _packets = [];
        private int? _totalPackets;
        public IEnumerable<byte[]> SplitPacket(string data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            int totalPackets = (int)Math.Ceiling((double)bytes.Length / MaxPacketSize);
            List<byte[]> packets = [];
            int bytesLeft = bytes.Length;

            for (int i = 0; i < totalPackets; i++)
            {
                int packetSize = Math.Min(MaxPacketSize, bytesLeft);
                byte[] packet = new byte[packetSize + 8];
                Array.Copy(BitConverter.GetBytes(i), 0, packet, 0, 4);
                Array.Copy(BitConverter.GetBytes(totalPackets), 0, packet, 4, 4);
                Array.Copy(bytes, i * MaxPacketSize, packet, 8, packetSize);
                
                packets.Add(packet);
                bytesLeft -= MaxPacketSize;
            }
            return packets;
        }

        public string? ReassemblePacket(byte[] packet, string id)
        {
            int sequenceNumber = BitConverter.ToInt32(packet, 0);
            _totalPackets ??= BitConverter.ToInt32(packet, 4);

            if (!_packets.TryGetValue(id, out var value))
            {
                value = new Dictionary<int, string>(_totalPackets.Value);
                _packets[id] = value;
            }

            string data = Encoding.ASCII.GetString(packet, 8, packet.Length - 8);
            value[sequenceNumber] = data;

            if (value.Count != _totalPackets) return null;
            StringBuilder sb = new();
            for (int i = 0; i < _totalPackets; i++)
            {
                sb.Append(value[i]);
            }
            _packets.Remove(id);
            _totalPackets = null;
            return sb.ToString();
        }
    }
}
