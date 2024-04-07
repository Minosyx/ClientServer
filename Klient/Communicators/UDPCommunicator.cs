using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Klient.Communicators
{
    public class UDPCommunicator : ClientCommunicator
    {
        private readonly UdpClient _client = new();
        private IPEndPoint _endPoint;

        public UDPCommunicator(string ip, int port)
        {
            _client.Connect(ip, port);
            _endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }

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
            _client.Close();
        }

        public void WriteLine(string line)
        {
            byte[] data = Encoding.ASCII.GetBytes(line);
            _client.Send(data, data.Length);
        }

        public string ReadLine()
        {
            byte[] data = _client.Receive(ref _endPoint);
            return Encoding.ASCII.GetString(data);
        }
    }
}
