using MusicSmash.API.Attributes;
using MusicSmash.API.Controllers;
using MusicSmash.API.Extensions;

using MusicSmash.Database.Interfaces;
using MusicSmash.Models;
using MusicSmash.PostgreSQL.Implemenations.Mediators;
using static MusicSmash.Models.Album;
using static MusicSmash.Models.Game;
using static MusicSmash.Models.Round;

namespace MusicSmash.API.Services.Implementations
{
    public class RoundService(ILogger<RoundService> logger, IConnection connection) 
        : IRoundService
    {
        private readonly ILogger<RoundService> _logger = logger;
        private readonly IRepository<Round, RoundDB, long> _roundRepository = connection.Detach<Round, RoundDB, long>();
        private readonly IRepository<Album, AlbumDB, long> _albumRepository = connection.Detach<Album, AlbumDB, long>();
        private readonly IRepository<Game, GameDB, long> _gameRepository = connection.Detach<Game, GameDB, long>();

        public void SaveRound(RoundController.RoundBase payload, string userId)
        {
            CheckDataIntegrity(payload, userId);
            try
            {
                var round = new Round()
                {
                    Id = -1,
                    Index = payload.Index,
                    Games = payload.Games.Select(g => new Game()
                    {
                        Id = -1,
                        Left = ToObject(_albumRepository.Get(g.Left)),
                        Right = ToObject(_albumRepository.Get(g.Right)),
                        Winner = g.Winner is null ? null : ToObject(_albumRepository.Get(g.Winner.Value))
                    }).ToArray(),
                    Owner = new User() { Id = userId }
                };
                for(int i = 0; i < round.Games.Length; i++)
                {
                    round.Games[i] = ToObject(_gameRepository.Upsert(round.Games[i]));
                }
                _roundRepository.Upsert(round);
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
                var lastRound = GetLastPlayedRound(userId);

                if (lastRound is null)
                    return BuildNewRound(userId);

                var round = ToObject(lastRound);

                IEnumerable<Album> albumsPool = round.Games.Select(g => g.Winner);

                return BuildRound(albumsPool, lastRound.Index + 1, userId);
            }
            catch (Exception)
            {
                _logger.LogError($"Error getting next round for {userId}");
                throw;
            }
        }

        private RoundDB GetLastPlayedRound(string userId)
        {
            var playedRounds = _roundRepository.GetAll().Where(r => r.userId.Equals(userId));

            if (!playedRounds.Any())
                return null;

            return playedRounds.OrderByDescending(r => r.Index).First();
        }

        private void CheckDataIntegrity(RoundController.RoundBase payload, string userId)
        {
            if (payload.Games.Length == 0)
            {
                throw new Exception("Round must have at least one game");
            }
            if (payload.Games.Any(g => g.Winner is null))
            {
                throw new Exception("All games must have a winner");
            }
            if (payload.Games.Any(g => g.Left == g.Right))
            {
                throw new Exception("A game cannot have the same album on both sides");
            }
            if (!payload.Games.All(g => g.Winner == g.Left || g.Winner == g.Right))
            {
                throw new Exception("Winner must be one of the albums in the game");
            }
            var lastPlayedRound = GetLastPlayedRound(userId);

            if (lastPlayedRound == null && payload.Index != 0)
            {
                throw new Exception("Round index must be 0");
            }
            if (lastPlayedRound != null && payload.Index != lastPlayedRound.Index + 1)
            {
                throw new Exception("Round index must be the next one in sequence");
            }
            if(lastPlayedRound == null)
            {
                return;
            }

            var lastWinners = ToObject(lastPlayedRound).Games.Select(g => g.Winner).Distinct();
            if (!lastWinners.All(w => payload.Games.Any(g => g.Left == w.Id || g.Right == w.Id)))
            {
                throw new Exception("All winners from the last round must be present in the current round");
            }   

        }

        private Round BuildNewRound(string userId)
        {
            var albumPool = _albumRepository.GetAll();

            return BuildRound(albumPool.Select(a => ToObject(a)), 0, userId);
        }

        private Round BuildRound(IEnumerable<Album> albums, int roundIndex, string userId)
        {
            if(albums.Count() == 1)
            {
                throw new Exception("We already have a winner.");
            }

            IEnumerable<(Album left, Album right)> Associate(List<Album> albums)
            {
                for (int i = 0; i < albums.Count; i += 2)
                {
                    yield return (albums[i], i + 1 < albums.Count ? albums[i + 1] : Album.NotDefined);
                }
            }

            return new Round()
            {
                Id = -1,
                Index = roundIndex,
                Games = Associate(albums.ToList().Shuffle())
                    .Select(couple => new Game()
                    {
                        Id = -1,
                        Left = couple.left,
                        Right = couple.right,
                        Winner = null
                    }).ToArray(),
                Owner = new User() { Id = userId }
            };
        }

        private Round ToObject(RoundDB db)
        {
            return new RoundMediator(connection).Mediate(db);
        }

        private Album ToObject(AlbumDB db)
        {
            return new AlbumMediator(connection).Mediate(db);
        }
        private Game ToObject(GameDB db)
        {
            return new GameMediator(connection).Mediate(db);
        }
    }
}
