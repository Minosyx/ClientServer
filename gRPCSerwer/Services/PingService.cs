using System.Text;
using Serwer.Attributes;
using Wspólne;

namespace Serwer.Services
{
    [Service("PING")]
    public class PingService : IServiceModule
    {
        public string AnswerCommand(string command)
        {
            var answerLength = GetAnswerLength(command);
            return "ping " + CommonTools.GenerateRandomAnswer(answerLength - 6) + '\n';
        }

        private static int GetAnswerLength(string command)
        {
            var i = command.IndexOf(' ');
            var j = command.IndexOf(' ', i + 1);
            return int.Parse(command.Substring(i + 1, j - i - 1));
        }
    }
}

