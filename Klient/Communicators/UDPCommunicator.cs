using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Wspólne;

namespace Klient.Communicators
{
    public class UDPCommunicator : ClientCommunicator
    {
        private readonly UdpClient _client = new();
        private readonly UDPSplitter _splitter = new();
        private IPEndPoint _endPoint;

        public UDPCommunicator(string ip, int port)
        {
            _client.Connect(ip, port);
            _endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        public override string QA(string question)
        {
            WriteLine(question);
            string answer = string.Empty;
            do
            {
                answer += ReadLine();
            } while (answer.LastIndexOf('\n') == -1);

            return answer;
        }

        public override void Close()
        {
            _client.Close();
        }

        public void WriteLine(string line)
        {
            var packets = _splitter.SplitPacket(line).ToList();
            foreach (byte[] packet in packets)
            {
                _client.Send(packet, packet.Length);
                if (packets.Count > 1)
                {
                    Thread.Sleep(1);
                }
            }
        }

        public string ReadLine()
        {
            string fullMessage = string.Empty;
            while (fullMessage.LastIndexOf('\n') == -1)
            {
                var data = _client.Receive(ref _endPoint);
                string? partialMessage = _splitter.ReassemblePacket(data, _endPoint.ToString());
                if (partialMessage == null) continue;
                fullMessage += partialMessage;
            }
            return fullMessage;
        }
    }
}
