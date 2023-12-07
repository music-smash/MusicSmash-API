using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSmash.PostgreSQL.Implemenations
{
    public class ConnectionFactory(string connectionString)
    {
        private readonly string _connectionString = connectionString;

        public Connection GetConnection()
        {
            return new Connection(NpgsqlDataSource.Create(_connectionString));
        }

    }
}
