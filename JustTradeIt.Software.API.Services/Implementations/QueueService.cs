using System;
using JustTradeIt.Software.API.Services.Interfaces;
using RabbitMQ.Client;

namespace JustTradeIt.Software.API.Services.Implementations
{
    public class QueueService : IQueueService, IDisposable
    {
        private IConnection connection;
        private IModel model;
        private IBasicProperties properties;
        public void Dispose()
        {
            var connectionFactory = new ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost"
            };
            this.connection = connectionFactory.CreateConnection();
            this.model = connection.CreateModel();
            this.properties = model.CreateBasicProperties();
            this.properties.Persistent = false;
        }

        public void PublishMessage(string routingKey, byte[] body)
        {
            this.model.BasicPublish("JustTradeItExchange", routingKey, this.properties,body);

        }
    }
}