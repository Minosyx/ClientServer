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
        private UDPSplitter _splitter = new();

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
            int nl;
            try
            {
                while (true)
                {
                    byte[] bytes = client.Receive(ref _endPoint);
                    string id = _endPoint.ToString();
                    string? packet = _splitter.ReassemblePacket(bytes, id);

                    if (packet == null) continue;
                    data += packet;

                    while ((nl = data.IndexOf('\n')) != -1)
                    {
                        string line = data.Substring(0, nl + 1);
                        data = data.Substring(nl + 1);
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
            foreach (byte[] packet in _splitter.SplitPacket(message))
            {
                client.Send(packet, packet.Length, _endPoint);
                Thread.Sleep(1);
            }
        }
    }
}
