using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serwer.Communicators
{
    public class UDPCommunicator(UdpClient client) : ICommunicator
    {
        private readonly UdpClient _client = client;
        private IPEndPoint _endPoint;
        private CommunicatorD _onDisconnect;
        private CommandD _onCommand;
        private Thread _thread;

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            //Console.WriteLine("UDP Communicator started");

            _onCommand = onCommand;
            _onDisconnect = onDisconnect;
            _thread = new Thread(Communicate);
            _thread.Start();
            _endPoint = new IPEndPoint(IPAddress.Any, 0);
        }


        public void Stop()
        {
            //Console.WriteLine("UDP Communicator stopped");
            _client.Close();
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
                    byte[] bytes = _client.Receive(ref _endPoint);
                    data += Encoding.ASCII.GetString(bytes);
                    while ((nl = data.IndexOf('\n')) != -1)
                    {
                        string line = data.Substring(0, nl + 1);
                        data = data.Substring(nl + 1);
                        string answer = _onCommand(line);
                        byte[] msg = Encoding.ASCII.GetBytes(answer);
                        _client.Send(msg, msg.Length, _endPoint);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Stop();
        }
    }
}
