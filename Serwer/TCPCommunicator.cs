using System.Net.Sockets;
using System.Text;

namespace Serwer
{
    public class TCPCommunicator : ICommunicator
    {
        private TcpClient _client;
        private CommunicatorD _onDisconnect;
        private CommandD _onCommand;
        private Thread _thread;

        public TCPCommunicator(TcpClient client)
        {
            _client = client;
        }

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            _onCommand = onCommand;
            _onDisconnect = onDisconnect;
            _thread = new Thread(Communicate);
            _thread.Start();
        }

        public void Stop()
        {
            _client.Close();
            _onDisconnect(this);
        }

        private void Communicate()
        {
            string? data = null;
            int len, nl;
            byte[] bytes = new byte[1024];
            NetworkStream stream = _client.GetStream();
            while ((len = stream.Read(bytes, 0, bytes.Length)) > 0)
            {
                data += Encoding.ASCII.GetString(bytes, 0, len);
                while ((nl = data.IndexOf('\n')) != -1)
                {
                    string line = data.Substring(0, nl + 1);
                    data = data.Substring(nl + 1);
                    byte[] msg = Encoding.ASCII.GetBytes(_onCommand(line));
                    stream.Write(msg, 0, msg.Length);
                }
            }
            stream.Close();
            Stop();
        }
    }
}