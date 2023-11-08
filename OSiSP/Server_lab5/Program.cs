using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Server
    {
        static async Task Main(string[] args)
        {
            //PipeConnect();
            await SocketConnect();
        }

        private static void PipeConnect()
        {
            var pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.InOut);

            StreamWriter writer = new StreamWriter(pipeServer);
            StreamReader reader = new StreamReader(pipeServer);

            while (true)
            {
                try
                {
                    if (!pipeServer.IsConnected)
                        pipeServer.WaitForConnection();
                    string clientMessage = reader.ReadLine();
                    Console.WriteLine("Client message: " + clientMessage);

                    Console.WriteLine("Write message to client:");
                    string message = Console.ReadLine();
                    writer.WriteLine(message);
                    writer.Flush();
                    Console.WriteLine();
                }
                catch
                {
                    break;
                }
            }

            if (pipeServer.IsConnected)
                pipeServer.Disconnect();
        }

        private static async Task SocketConnect()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); ;
            IPEndPoint ipEndPoint = new(ipAddress, 7001);

            using Socket listener = new(ipEndPoint.AddressFamily,
                                    SocketType.Stream,
                                    ProtocolType.Tcp);

            listener.Bind(ipEndPoint);
            listener.Listen(100);

            var handler = await listener.AcceptAsync();
            while (true)
            {
                // Receive message.
                var buffer = new byte[1_024];
                var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, received);

                Console.WriteLine($"Сообщение от клиента: \"{response}\"");

                Console.WriteLine("Напишите сообщение клиенту:");
                string ackMessage = Console.ReadLine();
                var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                await handler.SendAsync(echoBytes, 0);
                Console.WriteLine(
                    $"Сервер отправил ответ: \"{ackMessage}\"");

                if (ackMessage == "e")
                    break;
                Console.WriteLine();
            }
        }

    }
}