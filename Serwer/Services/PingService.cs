using System.Text;
using Wspólne;

namespace Serwer.Services
{
    public class PingService : IServiceModule
    {
        public PingService()
        {
        }

        public string AnswerCommand(string command)
        {
            var answerLength = GetAnswerLength(command);
            return "ping " + CommonTools.GenerateRandomAnswer(answerLength - 6) + '\n';
        }

        private int GetAnswerLength(string command)
        {
            var i = command.IndexOf(' ');
            var j = command.IndexOf(' ', i + 1);
            return int.Parse(command.Substring(i + 1, j - i - 1));
        }
    }
}

