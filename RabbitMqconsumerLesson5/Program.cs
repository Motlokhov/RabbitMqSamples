using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMqconsumerLesson5
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

                    string queue1 = "speed.orange.species";
                    string queue2 = "speed.colour.rabbit";
                    string queue3 = "quick.colour.swine";

                    EventingBasicConsumer consumer = new(channel);
                    consumer.Received += (sender, e) =>
                    {
                        string message = Encoding.UTF8.GetString(e.Body.ToArray());
                        Console.WriteLine($"[x] Message {message} from {queue1}({e.RoutingKey},{e.DeliveryTag},{e.ConsumerTag})");
                        ((EventingBasicConsumer)sender).Model.BasicAck(e.DeliveryTag, false);
                    };
                    channel.BasicConsume(queue1, false, consumer);

                    EventingBasicConsumer consumer2 = new(channel);
                    consumer2.Received += (sender, e) =>
                    {
                        string message = Encoding.UTF8.GetString(e.Body.ToArray());
                        Console.WriteLine($"[x] Message {message} from {queue2}({e.RoutingKey},{e.DeliveryTag},{e.ConsumerTag})");
                        ((EventingBasicConsumer)sender).Model.BasicAck(e.DeliveryTag, false);
                    };
                    channel.BasicConsume(queue2, false, consumer2);

                    EventingBasicConsumer consumer3 = new(channel);
                    consumer3.Received += (sender, e) =>
                    {
                        string message = Encoding.UTF8.GetString(e.Body.ToArray());
                        Console.WriteLine($"[x] Message {message} from {queue3}({e.RoutingKey},{e.DeliveryTag},{e.ConsumerTag})");
                        ((EventingBasicConsumer)sender).Model.BasicAck(e.DeliveryTag, false);
                    };
                    channel.BasicConsume(queue3, false, consumer3);

                    Console.ReadKey();
                }
            }
        }
    }
}
