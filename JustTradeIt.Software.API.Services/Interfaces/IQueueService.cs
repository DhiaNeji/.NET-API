namespace JustTradeIt.Software.API.Services.Interfaces
{
    public interface IQueueService
    {
        void PublishMessage(string routingKey, byte[] body);
    }
}