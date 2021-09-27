using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMqConsumerLesson3
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
                    string queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName, exchange: "logs", routingKey: string.Empty);

                    Console.WriteLine("[*] Waiting for logs.");

                    EventingBasicConsumer consumer = new(channel);
                    consumer.Received += (sender, e) =>
                    {
                        byte[] body = e.Body.ToArray();
                        string message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"[x] {message}");
                    };

                    channel.BasicConsume(queue: queueName, autoAck: true, consumer);
                    Console.ReadKey();
                }
            }
        }
    }
}
