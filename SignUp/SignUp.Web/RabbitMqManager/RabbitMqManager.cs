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
            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new Uri(RabbitMqConstants.RabbitMqUri);
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: RabbitMqConstants.RegisterExchange,
                    type: ExchangeType.Direct);
                string message = JsonConvert.SerializeObject(evt);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: RabbitMqConstants.RegisterExchange,
                    routingKey: "",
                    basicProperties: null,
                    body: body);
            }
        }

    }
}
