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

        protected IEnumerable<object> ExecuteQuery(string query)
        {
            var cmd = _dbConnection.CreateCommand();
            cmd.CommandText = query;
            var reader = cmd.ExecuteReader();
            int columnSize = reader.GetColumnSchema().Count;
            while (reader.Read())
            {
                var row = new object[columnSize];
                reader.GetValues(row);
                yield return row;
            }
            reader.Close();
        }

        ~Repository()
        {
            _dbConnection.Close();
        }
    }
}
