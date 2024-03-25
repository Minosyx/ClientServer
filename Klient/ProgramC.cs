using System.Runtime.CompilerServices;
using Klient.Clients;
using Klient.Communicators;

//TCPCommunicator communicator = new TCPCommunicator("localhost", 12345);

//Thread.Sleep(5000);
//FileCommunicator communicator = new FileCommunicator(@"D:\Studia\IS2S3\PROSIKO\Commands");

RS232Communicator communicator = new RS232Communicator("COM1");


PingClient pc = new PingClient(communicator);
double result = pc.Test(10, 1024, 4089);
Console.WriteLine($"Average time: {result}ms");

//FileClient fc = new FileClient(communicator);
////var answer = fc.Put(@"D:\Studia\IS2S3\PROSIKO\FTP\raz.txt");
////string answer = fc.Dir();
//var answer = fc.Get("raz.txt", @"D:\Studia\IS2S3\PROSIKO\FTP");
//Console.WriteLine(answer);

