using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Klient.Communicators;

namespace Klient.Clients
{
    public class FileClient(ClientCommunicator communicator) : QAClient(communicator)
    {
        private readonly ClientCommunicator _communicator = communicator;

        public bool Get(string filename, string path, string? newFilename = null)
        {
            string answer = _communicator.QA($"ftp get {filename}\n");
            if (answer == "File not found")
            {
                return false;
            }
            byte[] content = Convert.FromBase64String(answer);
            File.WriteAllBytes($@"{path}\{newFilename ?? filename}", content);
            return true;
        }

        public string Put(string filepath, string filename)
        {
            var content = Convert.ToBase64String(File.ReadAllBytes(filepath));
            string answer = _communicator.QA($"ftp put {filename} {content}\n");
            return answer.Trim();
        }

        public string Dir()
        {
            string answer = _communicator.QA("ftp dir\n");
            return answer.Trim();
        }
    }
}
