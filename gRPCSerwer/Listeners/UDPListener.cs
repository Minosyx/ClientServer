using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Serwer.Attributes;
using Serwer.Communicators;

namespace Serwer.Listeners
{
    [Medium("UDP")]
    public class UDPListener : IListener
    {
        private readonly Thread _thread;
        private CommunicatorD? _onConnect;
        private readonly UdpClient _server;

        public UDPListener(int portNo)
        {
            _thread = new Thread(Listen);
            _server = new UdpClient(portNo);
        }

        public UDPListener(string portNo)
        {
            try
            {
                var portNoInt = int.Parse(portNo);
                _thread = new Thread(Listen);
                _server = new UdpClient(portNoInt);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Start(CommunicatorD? onConnect)
        {
            _onConnect = onConnect;
            _thread.Start();
        }

        public void Stop()
        {
            _server.Close();
        }

        private void Listen()
        {
            UDPCommunicator communicator = new(_server);
            _onConnect?.Invoke(communicator);
        }
    }
}
