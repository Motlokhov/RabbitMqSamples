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
                        queue:"my_first_queue",
                        durable: true
                        , exclusive: false
                        , autoDelete: false
                        , arguments: null);

                    string message = "Hello RabbitMq!";
                    byte[] body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: string.Empty
                        ,routingKey: "my_first_queue"
                        , basicProperties: null
                        ,body: body);

                    Console.WriteLine($"[x] Sent message {message}");
                }
            }
            Console.ReadLine();
        }
    }
}
