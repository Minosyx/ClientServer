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

            return File.ReadAllText($@"{path}\{filename}.out");
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                waitForFile.Set();
            }
        }

        public override void Close()
        {
        }
    }
}
