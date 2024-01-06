using MusicSmash.Controllers.Exceptions;
using MusicSmash.Extensions;
using MusicSmash.Models;
using MusicSmash.Services;

namespace MusicSmash.Controllers
{
	public class GameController
	{
		public readonly AlbumService _albumService;
		public readonly RoundService _roundService;

        public GameController(AlbumService albumService, RoundService roundService)
        {
            _albumService = albumService;
            _roundService = roundService;
        }

        public Game GetNextGame(Round currentRound)
        {
            var nextGame = currentRound.Games.FirstOrDefault(g => !g.IsFinished);

            if (nextGame is null)
                throw new RoundFinishedException(currentRound);
            return nextGame;

        }

        public void SetWinner(Game game, Album winner)
        {
            game.Winner = winner;
        }

    }
}
