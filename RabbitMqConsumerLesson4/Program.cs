using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMqConsumerLesson4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };
            using(IConnection connection = factory.CreateConnection())
            {
                using(IModel channel = connection.CreateModel())
                {
                    string queueName = "log_console";

                    EventingBasicConsumer consumer = new(channel);
                    consumer.Received += (sender, e) =>
                    {
                        byte[] body = e.Body.ToArray();
                        string message = Encoding.UTF8.GetString(body);
                        ((EventingBasicConsumer)sender).Model.BasicAck(deliveryTag: e.DeliveryTag,multiple:false);
                        Console.WriteLine($"[x] {message} write to console. Route key:{e.RoutingKey}");
                    };
                    channel.BasicConsume(queueName, false,consumer);

                    string queueName1 = "log_fileSave";

                    EventingBasicConsumer consumer1 = new(channel);
                    consumer1.Received += (sender, e) =>
                    {
                        byte[] body = e.Body.ToArray();
                        string message = Encoding.UTF8.GetString(body);
                        ((EventingBasicConsumer)sender).Model.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
                        Console.WriteLine($"[x] {message} write to file. Route key:{e.RoutingKey}");
                    };
                    channel.BasicConsume(queueName1, false, consumer1);

                    Console.ReadKey();
                }
            }
        }
    }
}
