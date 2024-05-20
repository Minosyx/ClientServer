using System.IO.Ports;

namespace Klient.Communicators
{
    public class RS232Communicator : ClientCommunicator
    {
        private readonly SerialPort _port;

        public RS232Communicator(string port)
        {
            _port = new SerialPort(port);
            _port.Open();
        }

        public override string QA(string question)
        {
            _port.Write(question);
            string answer = string.Empty;
            do
            {
                answer += _port.ReadExisting();
            } while (answer.LastIndexOf('\n') == -1);
            return answer;
        }

        public override void Close()
        {
            //_port.Close();
        }
    }
}
