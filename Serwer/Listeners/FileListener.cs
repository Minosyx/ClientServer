using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serwer.Communicators;

namespace Serwer.Listeners
{
    public class FileListener : IListener
    {
        private readonly string[] _paths;
        private CommunicatorD? _onConnect;
        private FileSystemWatcher[] _watchers;
        private Thread _thread;

        public FileListener(params string[] paths)
        {
            _paths = paths;


        }

        public void Start(CommunicatorD? onConnect)
        {
            _onConnect = onConnect;

            _watchers = new FileSystemWatcher[_paths.Length];
            for (int i = 0; i < _paths.Length; i++)
            {
                _watchers[i] = new FileSystemWatcher(_paths[i]);
                _watchers[i].NotifyFilter =
                    NotifyFilters.LastWrite;
                _watchers[i].Changed += OnChanged;
                _watchers[i].Created += OnChanged;
                _watchers[i].Renamed += OnChanged;
                _watchers[i].Error += OnError;
                _watchers[i].Filter = "*.in";
                _watchers[i].IncludeSubdirectories = true;
                _watchers[i].EnableRaisingEvents = true;
            }

            new AutoResetEvent(false).WaitOne();
        }

        public void Stop()
        {
            foreach (var watcher in _watchers)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.GetException().Message);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            FileCommunicator communicator = new(e.FullPath);
            _onConnect?.Invoke(communicator);
        }
    }
}
