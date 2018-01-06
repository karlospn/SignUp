using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignUp.Messaging.Constants;
using SignUp.Messaging.Constants.Events;
using SignUp.Messaging.Constants.RabbitMqManager;
using UserContext = SignUp.Consumer.MySql.Context.UserContext;

namespace SignUp.Consumer.MySql.HostedServices
{
    public class PublishToMySqlService : IHostedService
    {
        private int _milisecondsDelay = 1000;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PublishToMySqlService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var rabbitManager = new RabbitMqManager<RegisteredUserEvent>();
                    rabbitManager.Consume(RabbitMqConstants.RegisterQueue, SaveMsgIntoDatabaseAsync);
                                  
                }catch (Exception e)
                {
                    Console.WriteLine(e);
                }             
                await Task.Delay(_milisecondsDelay, cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
           return Task.CompletedTask;
        }


        private async Task<int> SaveMsgIntoDatabaseAsync(RegisteredUserEvent msg)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetService<UserContext>();
                ctx.Users.Add(msg.User);
                return await ctx.SaveChangesAsync();
            }
        }
    }
}
