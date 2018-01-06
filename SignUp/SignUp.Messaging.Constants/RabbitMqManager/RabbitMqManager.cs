using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace SignUp.Messaging.Constants.RabbitMqManager
{
    public class RabbitMqManager<T> where T: class
    {

        public void Send(T evt)
        {
            ConnectionFactory factory = new ConnectionFactory {Uri = new Uri(RabbitMqConstants.RabbitMqUri)};
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

        public T Consume()
        {
            ConnectionFactory factory = new ConnectionFactory { Uri = new Uri(RabbitMqConstants.RabbitMqUri) };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                BasicGetResult result = channel.BasicGet(RabbitMqConstants.RegisterQueue, true);
                if (result != null)
                {
                    string data =
                        Encoding.UTF8.GetString(result.Body);
                    return JsonConvert.DeserializeObject<T>(data);
                }
                return null;

            }
        }

    }
}
