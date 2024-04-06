using MusicSmash.Database.Interfaces;
using MusicSmash.Models;
using MusicSmash.PostgreSQL.Implemenations.Repositories;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MusicSmash.PostgreSQL.Implemenations
{
    public static class ConnectionFactory
    {
        static ConnectionFactory()
        {
            BindRepositories();
        }

        private static void BindRepositories()
        {
            Bind<RoundRepository, Round.RoundDB>();
            Bind<AlbumRepository, Album>();
            Bind<GameRepository, Game.GameDB>();
        }

        public static Connection GetConnection(string connectionString)
        {
            return new Connection(NpgsqlDataSource.Create(MapCockroachDbConnectionString(connectionString)));
        }

        public static string MapCockroachDbConnectionString(string connectionString)
        {
            var connStringBuilder = new NpgsqlConnectionStringBuilder();
            connStringBuilder.SslMode = SslMode.VerifyFull;
            Uri databaseUrl = new Uri(connectionString);
            connStringBuilder.Host = databaseUrl.Host;
            connStringBuilder.Port = databaseUrl.Port;
            var items = databaseUrl.UserInfo.Split(new[] { ':' });
            if (items.Length > 0) connStringBuilder.Username = items[0];
            if (items.Length > 1) connStringBuilder.Password = items[1];
            connStringBuilder.Database = databaseUrl.AbsolutePath.Substring(1); // removing backslash
            return connStringBuilder.ConnectionString;
        }

        private static IDictionary<Type, Func<IRepository, IRepository>> keyValuePairs = new Dictionary<Type, Func<IRepository, IRepository>>();
        private static void Bind<TRepo, T>() where TRepo : IRepository<T>
        {
            keyValuePairs[typeof(T)] = (r) => Activator.CreateInstance(typeof(TRepo), r) as IRepository;
        }

        public static IRepository<T> GetRepository<T>(IRepository repository)
        {
            return keyValuePairs[typeof(T)](repository) as IRepository<T>;
        }
    }
}
