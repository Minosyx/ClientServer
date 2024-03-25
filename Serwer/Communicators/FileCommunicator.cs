using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer.Communicators
{
    public class FileCommunicator(string file) : ICommunicator
    {
        private CommandD _onCommand;
        private CommunicatorD _onDisconnect;
        private Thread _thread;

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            Console.WriteLine($"Starting FileCommunicator for {file}");
            _onCommand = onCommand;
            _onDisconnect = onDisconnect;
            _thread = new Thread(Communicate);
            _thread.Start();
        }

        public void Stop()
        {
            Console.WriteLine($"Stopping FileCommunicator for {file}\n");
            _onDisconnect(this);
        }

        private void Communicate(object? obj)
        {
            string fileContent = File.ReadAllText(file);
            string newFile = file.Replace(".in", ".out");
            string[] lines = fileContent.Trim().Split('\n');
            string answer = "";
            foreach (string line in lines)
            { 
                answer += _onCommand(line);
            }
            File.WriteAllText(newFile, answer);
            Stop();
        }
    }
}
