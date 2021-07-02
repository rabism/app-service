using StocksAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksAPI.Services
{
    public interface IMessageProducerService
    {
        void WriteMessage(string topicName, Stock stock);
    }
}
