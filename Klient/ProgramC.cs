using Klient.Clients;
using Klient.Communicators;

Thread.Sleep(500);

#region Communicators

TCPCommunicator tcpCommunicator = new TCPCommunicator("localhost", 12345);
UDPCommunicator udpCommunicator = new UDPCommunicator("127.0.0.1", 12346);
FileCommunicator fileCommunicator = new FileCommunicator(@"D:\Commands");
FileCommunicator fileCommunicator2 = new FileCommunicator(@"E:\Studia\IS2S3\Commands2");
RS232Communicator rs232Communicator = new RS232Communicator("COM1");
gRPCCommunicator gRpcCommunicator = new gRPCCommunicator("https://localhost:7032");

#endregion

#region Clients

ConfigurationClient cc = new ConfigurationClient(tcpCommunicator);

PingClient pc = new PingClient(tcpCommunicator);
FileClient fc = new FileClient(fileCommunicator);
MessageClient mc = new MessageClient(tcpCommunicator);

#endregion

#region Configuration Tests

string answer = cc.StartService("ping", "ping");
Console.WriteLine(answer);

answer = cc.StartService("ftp", "ftp", @"E:\Studia\IS2S3\FTPServer");
Console.WriteLine(answer);

answer = cc.StartService("chat", "chat");
Console.WriteLine(answer);



answer = cc.StartMedium("udp", "udp", "12346");
Console.WriteLine(answer);

answer = cc.StartMedium("file", "file", @"D:\Commands", @"E:\Studia\IS2S3\Commands2");
Console.WriteLine(answer);

answer = cc.StartMedium("rs", "rs232", "COM2");
Console.WriteLine(answer);

answer = cc.StartMedium("grpc", "grpc");
Console.WriteLine(answer);

#endregion


#region Ping Tests

//double result = pc.Test(1000, 1024, 4089);
//Console.WriteLine($"Average time: {result}ms");

//result = pc.Test(10000, 1024, 4089);
//Console.WriteLine($"Average time: {result}ms");

#endregion

#region File Tests

answer = fc.Dir();
Console.WriteLine(answer);
double milliseconds = 0;
for (int i = 0; i < 10; i++)
{
    File.Delete(@"E:\Studia\IS2S3\FTPClient\IMG_0248.jpg");
    DateTime now = DateTime.Now;
    fc.Get("IMG_0248.jpg", @"E:\Studia\IS2S3\FTPClient");
    TimeSpan diff = DateTime.Now - now;
    milliseconds += diff.TotalMilliseconds;
}

Console.WriteLine($"Average time: {milliseconds / 10}ms");

//answer = fc.Put(@"E:\Studia\IS2S3\FTPClient\IMG_0248.jpg");
//Console.WriteLine(answer);

#endregion

#region Message Tests

//bool ret = mc.Send(["Patryk", "Alice"], "John", "Hello. Nice to meet you");
//ret = mc.Send(["Patryk"], "Jack", "How's it going?");

//answer = mc.Receive("Patryk");

//Console.WriteLine(answer);

//string[] users = mc.GetUsers();

//foreach (var user in users)
//{
//    Console.WriteLine(user);
//}

#endregion
