using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace RabbitMqProducerLesson3
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };

            using(IConnection connection = factory.CreateConnection())
            {
                using(IModel channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                    int i = 1;
                    while(true)
                    {
                        string message = $"Message {i++}";
                        byte[] body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "logs", routingKey: string.Empty, body: body);
                        Console.WriteLine($"[x] Sent {message}");
                        Thread.Sleep(1000);
                    }
                }
            }
        }
    }
}
