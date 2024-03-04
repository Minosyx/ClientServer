using System.Net;
using System.Net.Sockets;


namespace Serwer
{
    internal class TCPListener : IListener
    {
        private readonly Thread _thread;
        private int _portNo;
        private CommunicatorD onConnect;

        public TCPListener(int portNo)
        {
            _portNo = portNo;
            _thread = new Thread(Listen);
        }

        public void Start(CommunicatorD onConnect)
        {
            onConnect = onConnect;
            _thread.Start();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        private void Listen()
        {
            var server = new TcpListener(IPAddress.Any, _portNo);
            server.Start();
            while (true)
            {
                var client = server.AcceptTcpClient();
                var communicator = new TCPCommunicator(client);
                onConnect(communicator);
            }
        }
    }
}

