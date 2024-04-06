using MusicSmash.API.Attributes;
using MusicSmash.API.Controllers;
using MusicSmash.API.Extensions;

using MusicSmash.Database.Interfaces;
using MusicSmash.Models;

namespace MusicSmash.API.Services.Implementations
{
    public class RoundService(ILogger<RoundService> logger, IConnection connection) 
        : IRoundService
    {
        private readonly ILogger<RoundService> _logger = logger;
        private readonly IRepository<Round.RoundDB> _roundRepository = connection.Detach<Round.RoundDB>();
        private readonly IRepository<Album> _albumRepository = connection.Detach<Album>();

        public void SaveRound(RoundController.RoundBase payload)
        {
            try
            {
                var round = new Round()
                {
                    Index = payload.Index,
                    Games = payload.Games.Select(g => new Game()
                    {
                        Left = _albumRepository.Get(g.Left),
                        Right = _albumRepository.Get(g.Right),
                        Winner = _albumRepository.Get(g.Winner)
                    }).ToArray()
                };
                //_roundRepository.Upsert(round);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error saving round");
                throw;
            }
        }

        public Round GetNextRound(string userId)
        {
            try
            {
                var playedRounds = _roundRepository.GetAll();

                if (!playedRounds.Any())
                    return BuildNewRound();

                var lastRound = playedRounds.OrderByDescending(r => r.Index).First();

                IEnumerable<Album> albumsPool = null;//lastRound.Games.Select(g => g.Winner);

                return BuildRound(albumsPool, lastRound.Index + 1);
            }
            catch (Exception)
            {
                _logger.LogError($"Error getting next round for {userId}");
                throw;
            }
        }

        private Round BuildNewRound()
        {
            var albumPool = _albumRepository.GetAll();

            return BuildRound(albumPool, 0);
        }

        private Round BuildRound(IEnumerable<Album> albums, int roundIndex)
        {
            IEnumerable<(Album left, Album right)> Associate(List<Album> albums)
            {
                for (int i = 0; i < albums.Count; i += 2)
                {
                    yield return (albums[i], i + 1 < albums.Count ? albums[i + 1] : Album.NotDefined);
                }
            }

            return new Round()
            {
                Index = roundIndex,
                Games = Associate(albums.ToList().Shuffle())
                    .Select(couple => new Game()
                    {
                        Left = couple.left,
                        Right = couple.right,
                        Winner = null
                    }).ToArray()
            };
        }


    }
}
