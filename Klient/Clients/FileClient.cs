using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klient.Communicators;

namespace Klient.Clients
{
    public class FileClient(ClientCommunicator communicator) : QAClient(communicator)
    {
        private readonly ClientCommunicator _communicator = communicator;

        public string Get(string filename)
        {
            return _communicator.QA($"ftp get {filename}");
        }

        public void Put(string filepath)
        {
            var content = Convert.ToBase64String(File.ReadAllBytes(filepath));
            var filename = filepath.Split('\\').Last();
            _communicator.QA($"ftp put {filename} {content}");
        }

        public void Dir()
        {
            _communicator.QA("ftp dir");
        }
    }
}
