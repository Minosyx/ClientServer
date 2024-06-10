using Grpc.Core;
using gRPCSerwer.Protos;

namespace gRPCSerwer.gRPCServices
{
    public class MessengerService : Messenger.MessengerBase
    {
        private readonly ILogger<MessengerService> _logger;

        public MessengerService(ILogger<MessengerService> logger)
        {
            _logger = logger;
        }

        public override Task<MessageResponse> SendMessage(MessageRequest request, ServerCallContext context)
        {
            return Task.FromResult(new MessageResponse
            {
                Message = "Message sent: " + request.Message
            });
        }
    }
}
