using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Klient.Communicators
{
    public class TCPCommunicator(string hostname, int port) : ClientCommunicator
    {
        private readonly TcpClient _client = new(hostname, port);

        public override string QA(string question)
        {
            WriteLine(question);
            string answer = string.Empty;
            do
            {
                answer += ReadLine();
            } while (answer.LastIndexOf('\n') == -1);

            return answer;
        }

        public override void Close()
        {
            //_client.Close();
        }

        public void WriteLine(string line)
        {
            byte[] data = Encoding.ASCII.GetBytes(line);
            NetworkStream stream = _client.GetStream();
            stream.Write(data, 0, data.Length);
        }

        public string ReadLine()
        {
            byte[] data = new byte[4096];
            NetworkStream stream = _client.GetStream();
            //stream.ReadTimeout = 10000;
            int len = stream.Read(data, 0, data.Length);
            return Encoding.ASCII.GetString(data, 0, len);
        }
    }
}
