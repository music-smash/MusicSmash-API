using Npgsql;

namespace MusicSmash.PostgreSQL.Implemenations
{
    public class Connection : IDisposable
    {
        private readonly NpgsqlDataSource _npgsqlDataSource;

        internal Connection(NpgsqlDataSource npgsqlDataSource)
        {
            this._npgsqlDataSource = npgsqlDataSource;
        }

        public async Task<TResult> Execute<TResult>(ICommand<TResult> command)
        {
            await using (var sqlCommand = _npgsqlDataSource.CreateCommand(command.Query))
            {
                if (command.Parameters.Any())
                    foreach (var parameter in command.Parameters)
                    {
                        sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }

                await using(var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    return command.Map(reader);
                }
            }
        }

        public void Dispose()
        {
            _npgsqlDataSource.Dispose();
        }
    }
}
