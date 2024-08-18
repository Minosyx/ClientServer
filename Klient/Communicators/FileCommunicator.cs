using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klient.Communicators
{
    public class FileCommunicator : ClientCommunicator
    {
        private readonly AutoResetEvent _waitForFile = new(false);
        private readonly string _path;
        private FileSystemWatcher _watcher;

        public FileCommunicator(string path)
        {
            _path = path;
            CreateWatcher();
        }

        public override string QA(string question)
        {
            string filename = GenerateFilename();
            SetWatcher(filename);

            Write(question, filename);
            bool fileChanged = _waitForFile.WaitOne(10000);

            SetWatcher(enableRaisingEvents: false);
            _waitForFile.Reset();

            string answer = "No response";
            if (!fileChanged) return answer;
            answer = File.ReadAllText($@"{_path}\{filename}.out");
            RemoveFiles(filename);

            return answer;
        }

        private bool RemoveFiles(string filename)
        {
            try
            {
                File.Delete($@"{_path}\{filename}.out");
                File.Delete($@"{_path}\{filename}.in");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private void SetWatcher(string? filter = null, bool enableRaisingEvents = true)
        {
            if (filter != null)
            {
                _watcher.Filter = $"{filter}.out";
            }
            _watcher.EnableRaisingEvents = enableRaisingEvents;
        }

        private void CreateWatcher()
        {
            _watcher = new FileSystemWatcher(_path)
            {
                NotifyFilter = NotifyFilters.LastWrite
            };
            _watcher.Changed += OnChanged;
            _watcher.EnableRaisingEvents = false;
        }

        private void Write(string question, string filename)
        {
            //Console.WriteLine($"Writing to {_path}\\{filename}.in");
            File.WriteAllText($@"{_path}\{filename}.in", question);
        }

        private static string GenerateFilename()
        {
            return Guid.NewGuid().ToString();
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                _waitForFile.Set();
            }
        }

        public override void Close()
        {
            _watcher.Dispose();
        }
    }
}
