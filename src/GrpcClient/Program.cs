using System;
using System.Threading.Tasks;
using Greet;
using Grpc.Core;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // The port number here must match the port of the gRPC server
            Channel channel = new Channel("localhost:50051", ChannelCredentials.Insecure);
            Greeter.GreeterClient client = new Greeter.GreeterClient(channel);

            while (true)
            {
                Console.Write("Greet Name: ");
                string name = Console.ReadLine();
                if (string.IsNullOrEmpty(name))
                {
                    break;
                }

                HelloReply reply = await client.SayHelloAsync(new HelloRequest { Name = name });
                Console.WriteLine(Environment.NewLine + "Greeting: " + reply.Message);
            }

            await channel.ShutdownAsync();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
