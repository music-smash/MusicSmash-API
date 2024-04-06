using MusicSmash.Database.Interfaces;
using MusicSmash.PostgreSQL.Implemenations;

namespace MusicSmash.Score.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            var configuration = builder.Configuration;

            builder.Services.AddScoped<IConnection>(ctx => ConnectionFactory.GetConnection(configuration["db:connection-string"]));
            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}