using RabbitMQ.Client;
using System.Text.Json;

namespace MusicSmash.RabbitMQ.Implementations
{

    //Basic monad for Ack/Nack
    public interface IMessage<T> where T : class
    {
        bool Ack();
        bool Nack();
        T Get();
    }

    public class Message<T> : IMessage<T> where T : class
    {
        private readonly QueueConnection _connection;
        private readonly BasicGetResult _basicGetResult;
        internal Message(QueueConnection connection, BasicGetResult basicGetResult)
        {
            _connection = connection;
            _basicGetResult = basicGetResult;
        }

        public bool Ack()
        {
            _connection._model.BasicAck(_basicGetResult.DeliveryTag, false);
            return true;
        }

        public bool Nack()
        {
            _connection._model.BasicNack(_basicGetResult.DeliveryTag, false, true);
            return true;
        }

        public T Get()
        {
            return JsonSerializer.Deserialize<T>(_basicGetResult.Body.ToString());
        }

    }
}
