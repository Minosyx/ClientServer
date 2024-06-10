using System.IO.Ports;

namespace Serwer.Communicators
{
    public class RS232Communicator(string port) : ICommunicator
    {
        private CommandD _onCommand;
        private CommunicatorD _onDisconnect;
        private SerialPort _port;
        private Thread _thread;

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            Console.WriteLine($"Starting RS232Communicator for {port}");
            _onCommand = onCommand;
            _onDisconnect = onDisconnect;
            _port = new SerialPort(port);
            _port.Open();
            _thread = new Thread(Communicate);
            _thread.Start();
        }

        public void Stop()
        {
            Console.WriteLine($"Stopping RS232Communicator for {port}\n");
            _port.Close();
            _onDisconnect(this);
        }

        private void Communicate()
        {
            string? data = null;
            int nl;
            try
            {
                while (true)
                {
                    data += _port.ReadExisting();
                    while ((nl = data.IndexOf('\n')) != -1)
                    {
                        string line = data.Substring(0, nl + 1);
                        data = data.Substring(nl + 1);
                        string answer = _onCommand(line);
                        _port.Write(answer);
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
