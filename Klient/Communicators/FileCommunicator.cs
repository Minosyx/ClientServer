using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klient.Communicators
{
    public class FileCommunicator(string path) : ClientCommunicator
    {
        public override string QA(string question)
        {
            string filename = Guid.NewGuid().ToString();
            File.WriteAllText($@"{path}\{filename}.in", question);
            
            using var watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.Filter = $"{filename}.out";
            watcher.EnableRaisingEvents = true;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.WaitForChanged(WatcherChangeTypes.Changed);

            return File.ReadAllText($@"{path}\{filename}.out");
        }

        public override void Close()
        {
        }
    }
}
