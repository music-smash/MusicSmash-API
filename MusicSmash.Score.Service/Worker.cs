using MusicSmash.Database.Interfaces;
using MusicSmash.Models;
using MusicSmash.PostgreSQL.Implemenations.Mediators;
using MusicSmash.RabbitMQ.Implementations;
using MusicSmash.Score.Engine;
using static MusicSmash.Models.Album;

namespace MusicSmash.Score.Service
{
    public class Worker(ILogger<Worker> logger, IConnection _dbconnection) : BackgroundService
    {
        private readonly QueueConnection _connection = QueueConnectionFactory.GetModel();
        private readonly ScoreEngine _scoreEngine = new ScoreEngine();
        private readonly IRepository<Album, AlbumDB, long> _albumRepository = _dbconnection.Detach<Album, AlbumDB, long>();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                var roundMonad = _connection.DeQueue<Round>();
                var round = roundMonad.Get();

                try
                {
                    var albumsScoreDeltas = _scoreEngine.GetScoreDeltaForAlbumsInRound(round);
                    UpdateAlbumScores(albumsScoreDeltas);
                    roundMonad.Ack();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing round {round}", round.Index);
                    roundMonad.Nack();
                }
            }
        }

        private void UpdateAlbumScores(IDictionary<Album, int> albumsScoreDeltas)
        {
            var mediator = new AlbumMediator(_dbconnection);
            foreach (var (album, scoreDelta) in albumsScoreDeltas)
            {
                var albumDbCopy = _albumRepository.Get(album.Id);
                albumDbCopy.Score += scoreDelta;
                _albumRepository.Upsert(mediator.Mediate(albumDbCopy));
            }
        }
    }
}
