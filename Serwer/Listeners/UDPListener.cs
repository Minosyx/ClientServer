using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Serwer.Communicators;

namespace Serwer.Listeners
{
    public class UDPListener : IListener
    {
        private readonly Thread _thread;
        private int _portNo;
        private CommunicatorD? _onConnect;
        private UdpClient _server;
        private bool _shouldTerminate = false;

        public UDPListener(int portNo)
        {
            _portNo = portNo;
            _thread = new Thread(Listen);
            _server = new UdpClient(_portNo);
        }

        public UDPListener(string portNo)
        {
            try
            {
                _portNo = int.Parse(portNo);
                _thread = new Thread(Listen);
                _server = new UdpClient(_portNo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Start(CommunicatorD? onConnect)
        {
            _onConnect = onConnect;
            _shouldTerminate = false;
            _thread.Start();
        }

        public void Stop()
        {
            _shouldTerminate = true;
            _server.Close();
        }

        private void Listen()
        {
            UDPCommunicator communicator = new(_server);
            _onConnect?.Invoke(communicator);
        }
    }
}
