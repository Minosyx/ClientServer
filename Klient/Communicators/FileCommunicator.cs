using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klient.Communicators
{
    public class FileCommunicator(string path) : ClientCommunicator
    {
        private AutoResetEvent waitForFile = new AutoResetEvent(false);

        public override string QA(string question)
        {
            string filename = Guid.NewGuid().ToString();
            File.WriteAllText($@"{path}\{filename}.in", question);
            
            using var watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.Filter = $"{filename}.out";
            watcher.EnableRaisingEvents = true;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += OnChanged;

            bool fileChanged = waitForFile.WaitOne(10000);

            //while (!isFileReady($@"{path}\{filename}.out"))
            //{
            //    Thread.Sleep(100);
            //}

            return File.ReadAllText($@"{path}\{filename}.out");
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                waitForFile.Set();
            }
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

        public override void Close()
        {
        }
    }
}
