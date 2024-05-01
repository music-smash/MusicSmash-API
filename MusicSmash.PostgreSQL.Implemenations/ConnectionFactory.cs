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
using static MusicSmash.Models.Album;
using static MusicSmash.Models.Game;
using static MusicSmash.Models.Round;

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
            Bind<RoundRepository, Round, RoundDB, long>();
            Bind<AlbumRepository, Album, AlbumDB, long>();
            Bind<GameRepository, Game, GameDB, long>();
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
        private static void Bind<TRepo, T, J, Y>() 
            where TRepo : IRepository<T, J, Y>
            where T : Entity<J, Y>
            where J : DBEntity<Y>
        {
            keyValuePairs[typeof(T)] = (r) => Activator.CreateInstance(typeof(TRepo), r) as IRepository;
        }

        public static IRepository<T, J, Y> GetRepository<T, J, Y>(IRepository repository)
            where T : Entity<J, Y>
            where J : DBEntity<Y>
        {
            return keyValuePairs[typeof(T)](repository) as IRepository<T, J, Y>;
        }
    }
}
