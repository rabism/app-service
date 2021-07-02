using CompaniesAPI.Models;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompaniesAPI.Services
{
    public class MessageReceiver : BackgroundService
    {
        ConsumerConfig consumerConfig;
        IServiceScopeFactory _scopefactory;
        public MessageReceiver(IServiceScopeFactory scopefactory, ConsumerConfig config)
        {
            _scopefactory = scopefactory;
            consumerConfig = config;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() => StartConsumer(stoppingToken));
            return Task.CompletedTask;
        }

        private async Task StartConsumer(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build())
                    {
                        consumer.Subscribe("StockInfo");
                        var consumeResult = consumer.Consume();
                        if (consumeResult != null)
                        {
                            System.Console.WriteLine("Result: {0}", consumeResult.Message.Value);
                            using (var scope = _scopefactory.CreateScope())
                            {
                                ICompanyService service = scope.ServiceProvider.GetRequiredService<ICompanyService>();
                                await Task.Run(() =>
                                {
                                    service.UpdateCompanyStock(JsonConvert.DeserializeObject<Stock>(consumeResult.Message.Value));
                                    System.Console.WriteLine("Message consumed from Kafka");
                                });
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Error in Comsumption: {0}", ex.Message);
            }
        }
    }
}
