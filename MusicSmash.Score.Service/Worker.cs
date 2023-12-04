using MusicSmash.Models;
using MusicSmash.RabbitMQ.Implementations;
using MusicSmash.Score.Engine;

namespace MusicSmash.Score.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly QueueConnection _connection;
        private readonly ScoreEngine _scoreEngine;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _connection = QueueConnectionFactory.GetModel();
            _scoreEngine = new ScoreEngine();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
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
                    _logger.LogError(ex, "Error processing round {round}", round.Index);
                    roundMonad.Nack();
                }
            }
        }

        private void UpdateAlbumScores(IDictionary<Album, int> albumsScoreDeltas)
        {
            
        }
    }
}
