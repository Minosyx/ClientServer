using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer.Communicators
{
    public class FileCommunicator(string path) : ICommunicator
    {
        private CommandD _onCommand;
        private CommunicatorD _onDisconnect;
        private Thread _thread;

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            Console.WriteLine($"Starting FileCommunicator for {path}");
            _onCommand = onCommand;
            _onDisconnect = onDisconnect;
            _thread = new Thread(Watch);
            _thread.Start();
        }

        public void Stop()
        {
            Console.WriteLine($"Stopping FileCommunicator for {path}\n");
            _onDisconnect(this);
        }

        private void Watch()
        {
            FileSystemWatcher watcher = new(path);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Renamed += OnChanged;
            watcher.Error += OnError;
            watcher.Filter = "*.in";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
            new AutoResetEvent(false).WaitOne();
        }

        private void Communicate(string filepath)
        {
            string fileContent = File.ReadAllText(filepath);
            string newFile = filepath.Replace(".in", ".out");
            string[] lines = fileContent.Trim().Split('\n');
            string answer = "";
            foreach (string line in lines)
            { 
                answer += _onCommand(line);
            }
            File.WriteAllText(newFile, answer);
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.GetException().Message);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Communicate(e.FullPath);
        }
    }
}
