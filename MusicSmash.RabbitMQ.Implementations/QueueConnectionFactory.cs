using RabbitMQ.Client;
using System.Text.Json.Serialization;

namespace MusicSmash.RabbitMQ.Implementations
{
    public static class QueueConnectionFactory
    {
        private static readonly ConnectionFactory _connectionFactory = new ConnectionFactory();

        private static IConnection GetConnection()
        {
            _connectionFactory.HostName = "localhost";
            _connectionFactory.UserName = "guest";
            _connectionFactory.Password = "guest";
            _connectionFactory.Port = 5672;
            return _connectionFactory.CreateConnection();
        }

        public static QueueConnection GetModel()
        {
            return new QueueConnection(GetConnection().CreateModel());
        }
    }


}
