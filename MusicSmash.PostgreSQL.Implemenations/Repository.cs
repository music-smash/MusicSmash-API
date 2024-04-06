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
        private readonly NpgsqlConnection _dbConnection;

        public Repository(Repository repository) : this(repository._dbConnection)
        {
            
        }

        public Repository(NpgsqlConnection dbConnection)
        {
            _dbConnection = dbConnection;

            if (_dbConnection.State != System.Data.ConnectionState.Open)
                _dbConnection.Open();
        }

        public IRepository<T> Init<T>()
        {
            return ConnectionFactory.GetRepository<T>(this);
        }

        protected IEnumerable<IDictionary<string, object>> ExecuteQuery(string query)
        {
            var cmd = _dbConnection.CreateCommand();
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

        ~Repository()
        {
            _dbConnection.Close();
        }
    }
}
