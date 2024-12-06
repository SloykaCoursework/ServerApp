using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Handler
{
    internal class RabbitMQHandler
    {

        public static async void Send(string token, string message)
        {

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: token, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: token, body: body);
            Logger.LogInformation($"The result was sent for {token}");

        }

    }
}
