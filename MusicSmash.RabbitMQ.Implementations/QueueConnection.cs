using RabbitMQ.Client;
using System.Text.Json;

namespace MusicSmash.RabbitMQ.Implementations
{
    public class QueueConnection : IDisposable
    {
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
            _model.BasicPublish("MusicSmash", "", _model.CreateBasicProperties()
                , JsonSerializer.SerializeToUtf8Bytes(item));
        }

        public IMessage<T> DeQueue<T>() where T:class
        {
            var result = _model.BasicGet("MusicSmash", false);
            if(result is null)
                _model.BasicAck(result.DeliveryTag, false);
            return new Message<T>(this, result);
        }
    }


}
