using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klient.Communicators
{
    public class FileCommunicator : ClientCommunicator
    {
        private AutoResetEvent waitForFile = new AutoResetEvent(false);
        private string path;
        private FileSystemWatcher watcher;

        public FileCommunicator(string path)
        {
            this.path = path;
            CreateWatcher();
        }

        public override string QA(string question)
        {
            string filename = Write(question);
            
            SetWatcher(filename);
            bool fileChanged = waitForFile.WaitOne(10000);
            SetWatcher(enableRaisingEvents: false);
            waitForFile.Reset();

            return File.ReadAllText($@"{path}\{filename}.out");
        }

        private void SetWatcher(string? filter = null, bool enableRaisingEvents = true)
        {
            if (filter != null)
            {
                watcher.Filter = $"{filter}.out";
            }
            watcher.EnableRaisingEvents = enableRaisingEvents;
        }

        private void CreateWatcher()
        {
            watcher = new(path);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += OnChanged;
            watcher.EnableRaisingEvents = false;
        }

        private string Write(string question)
        {
            string filename = Guid.NewGuid().ToString();
            File.WriteAllText($@"{path}\{filename}.in", question);
            return filename;
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
