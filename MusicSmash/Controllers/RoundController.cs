using MusicSmash.Controllers.Exceptions;
using MusicSmash.Models;
using MusicSmash.Services;
using System.Security.Cryptography.Xml;

namespace MusicSmash.Controllers
{
	public class RoundController
	{
        private readonly GameService _gameService;
        private readonly Events _eventsCollection;
        private readonly RoundService _roundService;
        private readonly AlbumService _albumService;

        public RoundController(GameService gameService, RoundService roundService, Events eventsCollection, AlbumService albumService)
        {
            this._gameService = gameService;
            this._eventsCollection = eventsCollection;
            this._roundService = roundService;
            this._albumService = albumService;
        }

        public Round GetRound()
        {
            var previusRound = GetSavedRound();
            var nextRound = GetNextRoundInternal(previusRound);
            _eventsCollection.OnNewRoundLoaded(this, nextRound);
            return nextRound;
        }

        private Round GetNextRoundInternal(Round previusRound)
		{
			try
			{
                var albumsPool = GetAlbumsOfNextRound(previusRound);
                var games = _gameService.GetRandomGameWithCoupledAlbums(albumsPool);
                return new Round()
                {
                    Index = (previusRound?.Index ?? 0) + 1,
                    Games = games.ToArray()
                };
            }
            catch (WeHaveAWinnerExceptions e)
			{
                return new Round()
				{
                    Index = (previusRound?.Index ?? 0) + 1,
                    Games = new Game[] { previusRound.Games[0] }
                };
			}

		}

		private Round GetSavedRound()
		{
			return _roundService.GetSavedRound(null);
		}

		public void SaveRound(Round round)
		{
			_roundService.SaveRound(null, round);
		}

        private List<Album> GetAlbumsOfNextRound(Round round)
        {
            if (round == null) return _albumService.GetAlbums().ToList();

            return round.Games.Where(g => g.IsFinished).Select(g => g.Winner).ToList();
        }

    }
}
