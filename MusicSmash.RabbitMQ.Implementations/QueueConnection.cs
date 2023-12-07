using RabbitMQ.Client;
using System.Text.Json;

namespace MusicSmash.RabbitMQ.Implementations
{
    public class QueueConnection : IDisposable
    {
        private const string Exchange = "MusicSmash";
        internal readonly IModel _model;

        internal QueueConnection(IModel model)
        {
            _model = model;
        }

        public void Dispose()
        {
            _model.Dispose();
        }

        public void Publish<T>(T item) where T:class
        {
            _model.BasicPublish(Exchange, "", _model.CreateBasicProperties()
                , JsonSerializer.SerializeToUtf8Bytes(item));
        }

        public IMessage<T> DeQueue<T>() where T:class
        {
            var result = _model.BasicGet(Exchange, false);
            if(result is null)
                _model.BasicNack(result.DeliveryTag, false, true);
            return new Message<T>(this, result);
        }
    }


}
