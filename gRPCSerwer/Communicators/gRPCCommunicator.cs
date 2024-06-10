using Grpc.Core;
using Serwer;

namespace gRPCSerwer.Communicators
{
    public class gRPCCommunicator : Messenger.MessengerBase, ICommunicator
    {
        private CommandD _onCommand;
        private CommunicatorD _onDisconnect;

        public void Start(CommandD onCommand, CommunicatorD onDisconnect)
        {
            _onCommand = onCommand;
            _onDisconnect = onDisconnect;
        }

        public override Task<MessageResponse> SendMessage(MessageRequest request, ServerCallContext context)
        {
            return Task.FromResult(new MessageResponse
            {
                Message = _onCommand(request.Message)
            });
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
