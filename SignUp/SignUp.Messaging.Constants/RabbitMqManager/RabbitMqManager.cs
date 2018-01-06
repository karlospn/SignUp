using System;
using System.Text;
using System.Threading.Tasks;
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
                    type: ExchangeType.Fanout);
                string message = JsonConvert.SerializeObject(evt);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: RabbitMqConstants.RegisterExchange,
                    routingKey: "",
                    basicProperties: null,
                    body: body);
            }
        }

        public void Consume(string queue, Func<T, Task<int>> businessFunc)
        {
            ConnectionFactory factory = new ConnectionFactory { Uri = new Uri(RabbitMqConstants.RabbitMqUri) };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                BasicGetResult result = channel.BasicGet(queue, false);
                if (result != null)
                {
                    var data = Encoding.UTF8.GetString(result.Body);
                    var msg = JsonConvert.DeserializeObject<T>(data);
                    businessFunc.Invoke(msg);
                    channel.BasicAck(result.DeliveryTag, true);
                }
            }
        }

    }
}
