using System.Net;
using System.Net.Sockets;
using System.Text;
using Serwer.Attributes;
using Serwer.Communicators;

namespace Serwer.Listeners
{
    [Medium("TCP")]
    public class TCPListener : IListener
    {
        private readonly Thread _thread;
        private readonly int _portNo;
        private CommunicatorD? _onConnect;
        private TcpListener _server;
        private bool _shouldTerminate;

        public TCPListener(int portNo)
        {
            _portNo = portNo;
            _thread = new Thread(Listen);
        }

        public TCPListener(string portNo)
        {
            try
            {
                _portNo = int.Parse(portNo);
                _thread = new Thread(Listen);
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
            _server.Stop();
        }

        private void Listen()
        {
            _server = new TcpListener(IPAddress.Any, _portNo);
            _server.Start();
            while (!_shouldTerminate)
            {
                TcpClient client = _server.AcceptTcpClient();
                if (!client.Connected) continue;
                TCPCommunicator communicator = new(client);
                _onConnect?.Invoke(communicator);
            }
        }
    }
}

