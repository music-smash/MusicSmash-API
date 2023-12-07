using Npgsql;

namespace MusicSmash.PostgreSQL.Implemenations
{
    public interface ICommand<TResult>
    {
        string? Query { get; }
        IEnumerable<KeyValuePair<string, object>> Parameters { get; }
        TResult Map(NpgsqlDataReader reader);
    }
}
