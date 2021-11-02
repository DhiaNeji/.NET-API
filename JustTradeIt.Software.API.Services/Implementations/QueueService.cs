using System;
using JustTradeIt.Software.API.Services.Interfaces;

namespace JustTradeIt.Software.API.Services.Implementations
{
    public class QueueService : IQueueService, IDisposable
    {
        public void Dispose()
        {
            // TODO: Dispose the connection and channel
            throw new NotImplementedException();
        }

        public void PublishMessage(string routingKey, object body)
        {
            throw new NotImplementedException();
        }
    }
}