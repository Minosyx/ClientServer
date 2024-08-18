using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Wspólne;

namespace Serwer.Communicators
{
    public class UDPCommunicator(UdpClient client) : ICommunicator
    {
        private IPEndPoint _endPoint;
        private CommunicatorD _onDisconnect;
        private CommandD _onCommand;
        private Thread _thread;
        private readonly UDPSplitter _splitter = new();

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            _onCommand = onCommand;
            _onDisconnect = onDisconnect;
            _thread = new Thread(Communicate);
            _endPoint = new IPEndPoint(IPAddress.Any, 0);
            _thread.Start();
        }


        public void Stop()
        {
            client.Close();
            _onDisconnect(this);
        }

        private void Communicate()
        {
            string? data = null;
            try
            {
                while (true)
                {
                    byte[] bytes = client.Receive(ref _endPoint);
                    string id = _endPoint.ToString();
                    string? packet = _splitter.ReassemblePacket(bytes, id);

                    if (packet == null) continue;
                    data += packet;

                    int nl;
                    while ((nl = data.IndexOf('\n')) != -1)
                    {
                        string line = data[..(nl + 1)];
                        data = data[(nl + 1)..];
                        string answer = _onCommand(line);

                        SendMessage(answer);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Stop();
        }

        private void SendMessage(string message)
        {
            var packets = _splitter.SplitPacket(message).ToList();
            foreach (byte[] packet in packets)
            {
                client.Send(packet, packet.Length, _endPoint);
                if (packets.Count > 1)
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}
