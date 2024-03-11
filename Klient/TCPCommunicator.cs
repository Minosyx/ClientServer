using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Klient
{
    public class TCPCommunicator : ClientCommunicator
    {
        TcpClient _client;

        public TCPCommunicator(string hostname, int port)
        {
            _client = new TcpClient(hostname, port);
        }

        public override string QA(string question)
        {
            WriteLine(question);
            StringBuilder sb = new();
            while (true)
            {
                string line = ReadLine();
                if (line.LastIndexOf('\n') == -1) break;
                sb.Append(line);
            }
            return sb.ToString();
        }

        public void WriteLine(string line)
        {
            byte[] data = Encoding.ASCII.GetBytes(line);
            NetworkStream stream = _client.GetStream();
            stream.Write(data, 0, data.Length);
        }

        public string ReadLine()
        {
            byte[] data = new byte[1024];
            NetworkStream stream = _client.GetStream();
            int len = stream.Read(data, 0, data.Length);
            return Encoding.ASCII.GetString(data, 0, len);
        }
    }
}
