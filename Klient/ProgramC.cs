using System.Runtime.CompilerServices;
using Klient.Clients;
using Klient.Communicators;

//TCPCommunicator communicator = new TCPCommunicator("localhost", 12345);

Thread.Sleep(5000);
FileCommunicator communicator = new FileCommunicator(@"D:\Studia\IS2S3\PROSIKO\Commands");
FileCommunicator communicator2 = new FileCommunicator(@"D:\Studia\IS2S3\PROSIKO\Commands2");

//RS232Communicator communicator = new RS232Communicator("COM1");
//UDPCommunicator communicator = new UDPCommunicator("127.0.0.1", 12345);

//ConfigurationClient cc = new ConfigurationClient(communicator);
//string answer = cc.StartMedium("udp", "udp", "12345");
//Console.WriteLine(answer);

//communicator = new TCPCommunicator("localhost", 12345);

//cc = new ConfigurationClient(communicator);
//answer = cc.StartService("ftp", "file", @"D:\Studia\IS2S3\PROSIKO\FTPServer");
//Console.WriteLine(answer);

//UDPCommunicator u_communicator = new UDPCommunicator("127.0.0.1", 12345);


PingClient pc = new PingClient(communicator);
double result = pc.Test(100, 1024, 4089);
Console.WriteLine($"Average time: {result}ms");
//Console.WriteLine($"Average time: {result}ms");

//communicator = new TCPCommunicator("localhost", 12345);

//cc = new ConfigurationClient(communicator);
//answer = cc.StopService("ping");
//Console.WriteLine(answer);

//communicator = new TCPCommunicator("localhost", 12345);
PingClient pc2 = new PingClient(communicator);
double result2 = pc2.Test(100, 1024, 4089);
Console.WriteLine($"Average time: {result2}ms");
//Console.WriteLine($"Average time: {result2}ms");

//PingClient pc2 = new PingClient(communicator2);
//double result2 = pc2.Test(10, 1024, 4089);
//Console.WriteLine($"Average time: {result2}ms");
//FileClient fc = new FileClient(u_communicator);
////answer = fc.Put(@"D:\Studia\IS2S3\PROSIKO\FTP\raz.txt");
//answer = fc.Dir();
//Console.WriteLine(answer);

//u_communicator = new UDPCommunicator("127.0.0.1", 12345);
//Console.WriteLine(boolAnswer);
//Console.WriteLine(boolAnswer);
//FileClient fc = new FileClient(communicator);
////string answer = fc.Dir();
////Console.WriteLine(answer);
//fc.Get("IMG_0248.jpg", @"E:\Studia\IS2S3\FTPClient");
////fc.Put(@"E:\Studia\IS2S3\FTPClient\IMG_0248.jpg");
////fc.Put(@"E:\Studia\IS2S3\FTPClient\IMG_0248.jpg");

//FileClient anotherFileClient = new FileClient(communicator);
//fc.Dir();
//fc.Dir();
//MessageClient mc = new MessageClient(communicator);
//bool ret = mc.Send(["Patryk", "Alice"], "John", "Witam witam wszystkich. Siemanko");

//string answer = mc.Receive("Patryk");

//Console.WriteLine(answer);

//string[] users = mc.GetUsers();

//foreach (var user in users)
//{
//    Console.WriteLine(user);
//}