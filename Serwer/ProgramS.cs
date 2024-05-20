using System.Diagnostics;
using Serwer;
using Serwer.Listeners;
using Serwer.Services;


Server server = Server.Instance;


//server.AddServiceModule("ping", new PingService());
//server.AddServiceModule("ftp", new FileService(@"E:\Studia\IS2S3\FTPServer"));
//server.AddServiceModule("chat", new MessageService());

server.AddServiceModule("config", new ConfigurationService());
server.AddListener("tcp", new TCPListener(12345));
//server.AddListener("file", new FileListener(@"D:\Studia\IS2S3\PROSIKO\Commands", @"D:\Studia\IS2S3\PROSIKO\Commands2"));

//server.AddListener(new RS232Listener("COM2"));
//server.AddListener("udp", new UDPListener(12345));

server.Start();

//Process.GetCurrentProcess().WaitForExit();