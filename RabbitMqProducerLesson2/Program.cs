using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace RabbitMqProducerLesson2
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
                    channel.QueueDeclare(
                        queue: "task_queue",
                        durable: true
                        , exclusive: false
                        , autoDelete: false
                        , arguments: null);

                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.Persistent = true;//save message almost 100% probability on server (save for reboot)

                    int i = 1;
                    while(true)
                    {
                        string message = $"[x] Produced message {i++}";
                        byte[] body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(
                            exchange: string.Empty,
                            routingKey: "task_queue",
                            basicProperties: properties,
                            body: body);

                        Thread.Sleep(1000);
                    }
                }
            }
        }
    }
}
