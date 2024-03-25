using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer.Services
{
    public class FileService : IServiceModule
    {
        private readonly Dictionary<string, Func<string, string>> _actions = new Dictionary<string, Func<string, string>>()
        {
            { "put", PutFile },
            { "get", GetFile },
            { "dir", GetDir }
        };

        public string AnswerCommand(string command)
        {
            return "";
        }

        private string GetActionType(string command)
        {
            var commandParts = command.IndexOf(' ');
            return "";
        }

        private static string PutFile(string command)
        {
            throw new NotImplementedException();
        }

        private static string GetFile(string arg)
        {
            throw new NotImplementedException();
        }

        private static string GetDir(string arg)
        {
            throw new NotImplementedException();
        }
    }
}
