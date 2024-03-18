﻿using System.Net.Sockets;
using System.Text;

namespace Serwer.Communicators
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
            Console.WriteLine($"Client Connected {_client.Client.RemoteEndPoint}");
            _onCommand = onCommand;
            _onDisconnect = onDisconnect;
            _thread = new Thread(Communicate);
            _thread.Start();
        }

        public void Stop()
        {
            Console.WriteLine($"Client Closed");
            if (_client.Connected)
            {
                _client.Close();
                _onDisconnect(this);
            }
        }

        private void Communicate()
        {
            string? data = null;
            int len, nl;
            byte[] bytes = new byte[4096];
            NetworkStream stream = _client.GetStream();
            try
            {
                while ((len = stream.Read(bytes, 0, bytes.Length)) > 0)
                {
                    data += Encoding.ASCII.GetString(bytes, 0, len);
                    while ((nl = data.IndexOf('\n')) != -1)
                    {
                        string line = data.Substring(0, nl + 1);
                        data = data.Substring(nl + 1);
                        string answer = _onCommand(line);
                        byte[] msg = Encoding.ASCII.GetBytes(answer);
                        stream.Write(msg, 0, msg.Length);
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