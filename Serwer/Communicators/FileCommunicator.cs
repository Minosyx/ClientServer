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
            //Watch();
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
            watcher.Error += OnError;
            watcher.Filter = "*.in";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            Thread.Sleep(Timeout.Infinite);
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

        private bool isFileReady(string filename)
        {
            try
            {
                using FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.GetException().Message);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            //if (isFileReady(e.FullPath))
                Communicate(e.FullPath);
            //Thread.Sleep(1000);
        }
    }
}
