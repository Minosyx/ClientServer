using Serwer.Attributes;
using Serwer.Communicators;

namespace Serwer.Listeners
{
    [Medium("GRPC")]
    public class gRPCListener : IListener
    {
        private CommunicatorD? _onConnect;
        private WebApplication app;

        public void Start(CommunicatorD? onConnect)
        {
            _onConnect = onConnect;
            var communicator = new gRPCCommunicator();
            _onConnect?.Invoke(communicator);

            var builder = WebApplication.CreateBuilder();
            builder.Services.AddGrpc(options =>
            {
                options.MaxReceiveMessageSize = null;
                options.MaxSendMessageSize = null;
            });
            builder.Services.AddSingleton(communicator);
            app = builder.Build();
            app.MapGrpcService<gRPCCommunicator>();
            app.StartAsync();
        }

        public void Stop()
        {
            app.StopAsync();
        }
    }
}
