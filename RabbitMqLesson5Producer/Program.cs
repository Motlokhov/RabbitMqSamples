using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMqProducerLesson5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };
            using(IConnection connection = factory.CreateConnection())
            {
                using(IModel channel = connection.CreateModel())
                {
                    string exchangeName = "topic_exchange_example";
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true);

                    string queue1 = "speed.orange.species";
                    string queue2 = "speed.colour.rabbit";
                    string queue3 = "quick.colour.swine";
                    channel.QueueDeclare(queue1, true,false,false);
                    channel.QueueDeclare(queue2, true, false, false);
                    channel.QueueDeclare(queue3, true, false, false);

                    channel.QueueBind(queue1, exchangeName, "*.orange.*");
                    channel.QueueBind(queue2, exchangeName, "*.*.rabbit");
                    channel.QueueBind(queue2, exchangeName, "lazy.#");
                    channel.QueueBind(queue3, exchangeName, "quick.*.swine");

                    string[] messages =
                    {
                        "quick.orange.rabbit",// q1,q2
                        "lazy.orange.elephant", //q1,q2
                        "quick.orange.fox", //q1
                        "lazy.brown.fox", //q2
                        "lazy.pink.rabbit", //q2 (matching 2 times, but will be send once)
                        "quick.brown.fox", //no matching
                        "lazy.orange.male.rabbit", //q2
                        "quick.brown.swine" //q3
                    };

                    foreach(string message in messages)
                    {
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchangeName, message, null, body);
                        Console.WriteLine($"[x] Message {message} is sent.");
                    }
                    Console.ReadKey();
                }
            }
        }
    }
}
