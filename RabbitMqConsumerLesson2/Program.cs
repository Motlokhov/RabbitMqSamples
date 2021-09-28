using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMqConsumerLesson2
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };
            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();

            channel.BasicQos(prefetchSize: 0, prefetchCount: 10, global: false);

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                byte[] body = e.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"[x] Message: {message}");

                Thread.Sleep(2000);

                Console.WriteLine($"[x] Done. DeliveryTag:{e.DeliveryTag}");
                ((EventingBasicConsumer)sender).Model.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
            };

            channel.BasicConsume("task_queue", false, consumer);
            Console.ReadKey();
        }
    }
}
