using MusicSmash.Database.Interfaces;
using MusicSmash.Models;
using Npgsql;

namespace MusicSmash.PostgreSQL.Implemenations
{
    public class Connection : IConnection
    {
        private readonly NpgsqlDataSource _npgsqlDataSource;

        internal Connection(NpgsqlDataSource npgsqlDataSource) => this._npgsqlDataSource = npgsqlDataSource;

        public IRepository<T, J, Y> Detach<T, J, Y>()
            where T : Entity<J, Y>
            where J : DBEntity<Y>
        {
            return new Repository(_npgsqlDataSource).Init<T, J, Y>();
        }

        public void Dispose()
        {
            _npgsqlDataSource.Dispose();
        }
    }
}
