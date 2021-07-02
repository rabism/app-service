using Confluent.Kafka;
using Newtonsoft.Json;
using StocksAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksAPI.Services
{
    public class MessageProducerService : IMessageProducerService
    {
        private IProducer<string, string> _producer;
        private ProducerConfig _config;

        public MessageProducerService(ProducerConfig config)
        {
            this._config = config;
            this._producer = new ProducerBuilder<string, string>(this._config).Build();
        }
        public void WriteMessage(string topicName, Stock stock)
        {
            try
            {
                this._producer.Produce(topicName, new Message<string, string>
                {
                    Key = stock.CompanyCode,
                    Value = JsonConvert.SerializeObject(stock)
                }, handler);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Stock push error: {ex.Message}");
                throw;
            }
        }
        public static void handler(DeliveryReport<string, string> deliveryReport)
        {
            //if (deliveryReport.Status == PersistenceStatus.Persisted)
            //    Console.WriteLine($"{deliveryReport.Key} details sent");
            //else
            //    Console.WriteLine("Kafka update: {0}",deliveryReport.Message);
            Console.WriteLine(JsonConvert.SerializeObject(deliveryReport));
        }
    }
}
