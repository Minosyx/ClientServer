using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serwer.Services
{
    public class FileService : IServiceModule
    {
        private static readonly string RootDir = @"C:\Users\Patryk\Desktop\Files";

        private readonly Dictionary<string, Func<string, string>> _actions = new()
        {
            { "put", PutFile },
            { "get", GetFile },
            { "dir", GetDir }
        };

        public string AnswerCommand(string command)
        {
            command = command.Trim();
            var (action, data) = ExtractParams(command);
            return _actions[action](data);
        }

        private (string action, string data) ExtractParams(string command)
        {
            var commandPartIndex = command.IndexOf(' ');
            if (commandPartIndex == -1)
            {
                throw new ArgumentException("Invalid command");
            }
            var actionPartIndex = command.IndexOf(' ', commandPartIndex + 1);
            if (actionPartIndex == -1)
            {
                throw new ArgumentException("Invalid command");
            }
            return (command[(commandPartIndex + 1)..actionPartIndex], command[(actionPartIndex + 1)..]);
        }

        private static string PutFile(string data)
        {
            var i = data.IndexOf(' ');
            var fileName = data[..i];
            data = data[(i + 1)..];
            var fileContent = Convert.FromBase64String(data);
            var path = $@"{RootDir}\{fileName}";
            File.WriteAllBytes(path, fileContent);
            return "File saved\n";
        }

        private static string GetFile(string data)
        {
            var path = $@"{RootDir}\{data}";
            if (!File.Exists(path))
            {
                return "File not found\n";
            }
            var b64 = Convert.ToBase64String(File.ReadAllBytes(path));
            return $"{b64}\n";
        }

        private static string GetDir(string data)
        {
            var files = Directory.GetFiles($@"{RootDir}\{data}");
            return string.Join('\n', files.Select(f => f[(RootDir.Length + 1)..])) + '\n';
        }
    }
}
