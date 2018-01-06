using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignUp.Messaging.Constants.Events;
using SignUp.Messaging.Constants.RabbitMqManager;
using UserContext = SignUp.Consumers.Context.UserContext;

namespace SignUp.Consumers.HostedServices
{
    public class MySqlInsertService : IHostedService
    {
        private int _milisecondsDelay = 1000;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MySqlInsertService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var rabbitManager = new RabbitMqManager<RegisteredUserEvent>();
                var msg = rabbitManager.Consume();
                if (msg != null)
                {
                    await SaveMsgIntoDatabaseAsync(msg);
                }

                await Task.Delay(_milisecondsDelay, cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
           return Task.CompletedTask;
        }


        private async Task SaveMsgIntoDatabaseAsync(RegisteredUserEvent msg)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetService<UserContext>();
                ctx.Users.Add(msg.User);
                await ctx.SaveChangesAsync();
            }
        }
    }
}
