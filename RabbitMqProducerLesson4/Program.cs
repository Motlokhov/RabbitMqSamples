using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace RabbitMqProducerLesson4
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
                    string logFileQueueName = "log_fileSave";
                    string logConsoleQueueName = "log_console";
                    string exchangeName = "direct_logs";

                    channel.QueueDeclare(logFileQueueName, true, false, false);
                    channel.QueueDeclare(logConsoleQueueName, true, false, false);
                    channel.ExchangeDeclare(exchangeName, type: ExchangeType.Direct);
                    //to file
                    channel.QueueBind(logFileQueueName, exchangeName, "error");
                    //to console
                    channel.QueueBind(logConsoleQueueName, exchangeName, "error");
                    channel.QueueBind(logConsoleQueueName, exchangeName, "info");
                    channel.QueueBind(logConsoleQueueName, exchangeName, "warning");

                    int i = 1;
                    while(true)
                    {
                        string message = $"Message {i++} is ";
                        string routeKey = "info";
                        if(i % 10 == 0)
                            routeKey = "error";
                        else if
                            (i % 3 == 0)
                            routeKey = "warning";

                        message += routeKey;

                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchangeName, routeKey, null, body);
                        Console.WriteLine($"[x] {message}");
                        Thread.Sleep(1000);
                    }
                }
            }
        }
    }
}
