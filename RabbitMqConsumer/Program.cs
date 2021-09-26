using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMqProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory rabbitFactory = new() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };

            using(IConnection connection = rabbitFactory.CreateConnection())
            {
                using(IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "my_first_queue",
                        durable: true
                        , exclusive: false
                        , autoDelete: false
                        , arguments: null);

                    EventingBasicConsumer consumer = new(channel);

                    consumer.Unregistered += (sender, e) => Console.WriteLine("[x] Consumer unregistered");
                    consumer.Shutdown += (sender, e) => Console.WriteLine("[x] Consumer shutdown");
                    consumer.Registered += (sender, e) => Console.WriteLine("[x] Consumer registered");
                    consumer.ConsumerCancelled += (sender, e) => Console.WriteLine("[x] Consumer canceled.");

                    consumer.Received += (sender, e) =>
                    {
                        byte[] body = e.Body.ToArray();
                        string message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"[x] Received {message}");
                    };

                    channel.BasicConsume(
                        queue: "my_first_queue",
                        autoAck: true,
                        consumer: consumer);
                }
            }
            Console.ReadLine();
        }
    }
}
