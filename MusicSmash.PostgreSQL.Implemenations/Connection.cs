using MusicSmash.Database.Interfaces;
using MusicSmash.Models;
using Npgsql;

namespace MusicSmash.PostgreSQL.Implemenations
{
    public class Connection : IConnection
    {
        private readonly NpgsqlDataSource _npgsqlDataSource;

        internal Connection(NpgsqlDataSource npgsqlDataSource) => this._npgsqlDataSource = npgsqlDataSource;

        public IRepository<T> Detach<T>()
        {
            return new Repository(_npgsqlDataSource.OpenConnection()).Init<T>();
        }

        public void Dispose()
        {
            _npgsqlDataSource.Dispose();
        }
    }
}
