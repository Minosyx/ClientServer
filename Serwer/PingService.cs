using System.Text;

namespace Serwer
{
    internal class PingService : IServiceModule
    {
        public PingService()
        {
        }

        public string AnswerCommand(string command)
        {
            var answerLength = GetAnswerLength(command);
            return "ping " + GenerateRandomAnswer(answerLength - 6) + '\n';
        }

        private int GetAnswerLength(string command)
        {
            var i = command.IndexOf(' ');
            var j = command.IndexOf(' ', i + 1);
            return int.Parse(command.Substring(i + 1, j - i - 1));
        }

        private string GenerateRandomAnswer(int length)
        {
            char[] chars = new char[length];
            int m = 'Z' - 'A' + 1;
            for (int i = 0; i < length; i++)
            {
                chars[i] = (char) ('A' + i % m);
            }
            return new string(chars);
        }
    }
}

