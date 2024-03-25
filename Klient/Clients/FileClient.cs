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
        private void Close()
        {
            communicator.Close();
        }

        public bool Get(string filename, string path)
        {
            string answer = communicator.QA($"ftp get {filename}\n");
            Close();
            if (answer == "File not found")
            {
                return false;
            }
            byte[] content = Convert.FromBase64String(answer);
            File.WriteAllBytes($@"{path}\{filename}", content);
            return true;
        }

        public string Put(string filepath)
        {
            var content = Convert.ToBase64String(File.ReadAllBytes(filepath));
            var filename = filepath.Split('\\').Last();
            string answer = communicator.QA($"ftp put {filename} {content}\n");
            Close();
            return answer.Trim();
        }

        public string Dir()
        {
            string answer = communicator.QA("ftp dir\n");
            Close();
            return answer.Trim();
        }
    }
}
