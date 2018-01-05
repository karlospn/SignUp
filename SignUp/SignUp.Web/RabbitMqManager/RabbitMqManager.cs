using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SignUp.Messaging.Constants;
using SignUp.Messaging.Constants.Events;

namespace SignUp.Web.RabbitMqManager
{
    public class RabbitMqManager
    {

        public void SendUser(RegisteredUserEvent evt)
        {
            var factory = new ConnectionFactory() { Uri = new Uri(RabbitMqConstants.RabbitMqUri) };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(
                    exchange: RabbitMqConstants.RegisterExchange,
                    type: ExchangeType.Direct);

                channel.QueueDeclare(
                    queue: RabbitMqConstants.RegisterQueue,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                channel.QueueBind(
                    queue: RabbitMqConstants.RegisterQueue,
                    exchange: RabbitMqConstants.RegisterExchange,
                    routingKey: "");

                var serializedModel = JsonConvert.SerializeObject(evt);


                var messageProperties = channel.CreateBasicProperties();
                messageProperties.ContentType =
                    RabbitMqConstants.JsonMimeType;

                channel.BasicPublish(
                    exchange: RabbitMqConstants.RegisterExchange,
                    routingKey: "",
                    basicProperties: messageProperties,
                    body: Encoding.UTF8.GetBytes(serializedModel));
            }
        }

    }
}
