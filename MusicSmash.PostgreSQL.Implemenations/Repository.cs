using MusicSmash.Database.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSmash.PostgreSQL.Implemenations
{
    internal class Repository : IRepository
    {
        private readonly NpgsqlDataSource _dataSource;

        public Repository(Repository repository) : this(repository._dataSource)
        {
            
        }

        public Repository(NpgsqlDataSource dbConnection)
        {
            _dataSource = dbConnection;
        }

        public IRepository<T, J, Y> Init<T, J, Y>()
                where T : Entity<J, Y>
                where J : DBEntity<Y>
        {
            return ConnectionFactory.GetRepository<T, J, Y>(this);

        }

        internal IEnumerable<IDictionary<string, object>> ExecuteQueryWithResults(string query)
        {
            using(var conn = _dataSource.OpenConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = query;
                var reader = cmd.ExecuteReader();
                var columns = reader.GetColumnSchema().Select(s => s.ColumnName).ToArray();
                while (reader.Read())
                {
                    var row = new object[columns.Length];
                    reader.GetValues(row);
                    yield return columns.Zip(row).ToDictionary();
                }
                reader.Close();
            }
        }

        internal int ExecuteQuery(string query)
        {
            using (var conn = _dataSource.OpenConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = query;
                return cmd.ExecuteNonQuery();
            }

        }
    }
}
