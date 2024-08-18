using System.Diagnostics;
using Serwer;
using Serwer.Listeners;
using Serwer.Services;


Server server = Server.Instance;

#region Services

//server.AddServiceModule("ping", new PingService());
//server.AddServiceModule("ftp", new FileService(@"E:\Studia\IS2S3\FTPServer"));
//server.AddServiceModule("chat", new MessageService());
server.AddServiceModule("config", new ConfigurationService(server.AddListener, server.RemoveListener, server.AddServiceModule, server.RemoveServiceModule, server.AddCommunicator));

#endregion

#region Listeners

server.AddListener("tcp", new TCPListener(12345));
//server.AddListener("file", new FileListener(@"E:\Studia\IS2S3\Commands", @"E:\Studia\IS2S3\Commands2"));
//server.AddListener("rs", new RS232Listener("COM2"));
//server.AddListener("udp", new UDPListener(12346));
//server.AddListener("grpc", new gRPCListener());

#endregion

server.Start();

Process.GetCurrentProcess().WaitForExit();