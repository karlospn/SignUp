using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SignUp.Consumer.ElasticSearch.Configuration;
using SignUp.Messaging.Constants;
using SignUp.Messaging.Constants.Events;
using SignUp.Messaging.Constants.RabbitMqManager;

namespace SignUp.Consumer.ElasticSearch.HostedServices
{
    public class PublishToElasticSearchService: IHostedService
    {
        private int _milisecondsDelay = 1000;
        private readonly ESClientProvider _esClientProvider;

        public PublishToElasticSearchService(ESClientProvider esClientProvider)
        {
            _esClientProvider = esClientProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var rabbitManager = new RabbitMqManager<RegisteredUserEvent>();
                    rabbitManager.Consume(RabbitMqConstants.ElasticQueue, SaveMsgIntoElasticAsync);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                await Task.Delay(_milisecondsDelay, cancellationToken);
            }
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            return  Task.CompletedTask;
        }

        private async Task<int> SaveMsgIntoElasticAsync(RegisteredUserEvent arg)
        {
            var res = await _esClientProvider.Client.IndexAsync(arg.User);
            if (!res.IsValid)
            {
                throw new InvalidOperationException(res.DebugInformation);
            }
            return Convert.ToInt32(res.Id);

        }


    }
}
