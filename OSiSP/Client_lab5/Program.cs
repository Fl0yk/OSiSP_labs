using System;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;

namespace Client
{
    class Client
    {
        static async Task Main(string[] args)
        {
            //PipeConnect();
            await SocketConnect();
        }

        private static void PipeConnect()
        {
            var pipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.InOut);

            StreamWriter writer = new StreamWriter(pipeClient);
            StreamReader reader = new StreamReader(pipeClient);

            while (true)
            {

                if (!pipeClient.IsConnected)
                    pipeClient.Connect();

                Console.WriteLine("Write message to server:");
                string message = Console.ReadLine();
                if (string.IsNullOrEmpty(message))
                    break;

                writer.WriteLine(message);
                writer.Flush();

                //writer = new StreamWriter(pipeClient);
                if (!pipeClient.IsConnected)
                    pipeClient.Connect();
                string clientMessage = reader.ReadLine();
                Console.WriteLine("Server message: " + clientMessage);
                Console.WriteLine();
            }

            if (pipeClient.IsConnected)
                pipeClient.Close();
        }

        private static async Task SocketConnect()
        {
            //IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("host.contoso.com");
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new(ipAddress, 7001);

            using Socket client = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            await client.ConnectAsync(ipEndPoint);
            while (true)
            {
                // Send message.
                Console.WriteLine("Напишите сообщение серверу:");
                string message = Console.ReadLine();
                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = await client.SendAsync(messageBytes, SocketFlags.None);
                Console.WriteLine($"Сообщение на сервер: \"{message}\"");

                // Receive ack.
                var buffer = new byte[1_024];
                var received = await client.ReceiveAsync(buffer, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, received);

                Console.WriteLine($"Сообщение от сервера: \"{response}\"");
                if (response == "e")
                {
                    break;
                }

                Console.WriteLine();
            }

            client.Shutdown(SocketShutdown.Both);
        }
    }
}