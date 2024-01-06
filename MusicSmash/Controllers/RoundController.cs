using MusicSmash.Controllers.Exceptions;
using MusicSmash.Models;
using MusicSmash.Services;
using System.Security.Cryptography.Xml;

namespace MusicSmash.Controllers
{
	public class RoundController
	{
        private readonly VoteController _voteController;
        private readonly Events _eventsCollection;
        private readonly RoundService _roundService;

		public RoundController(VoteController voteController, RoundService roundService, Events eventsCollection)
		{
            this._voteController = voteController;
            this._eventsCollection = eventsCollection;
            this._roundService = roundService;
		}

        public Round GetNextRound(Round previusRound)
        {
            var nextRound = GetNextRoundInternal(previusRound);
            _eventsCollection.OnNewRoundLoaded(this, nextRound);
            return nextRound;
        }

        private Round GetNextRoundInternal(Round previusRound)
		{
			try
			{
                var albumPool = _voteController.GetRandomCoupledAlbums(previusRound);
                return new Round()
                {
                    Index = (previusRound?.Index ?? 0) + 1,
                    Games = albumPool
                        .Select((couple) => new Game()
                        {
                            Left = couple.left,
                            Right = couple.right
                        }).ToArray()
                };
            }
            catch (WinnerExceptions e)
			{
                return new Round()
				{
                    Index = (previusRound?.Index ?? 0) + 1,
                    Games = new Game[] { FinishedGame.FromWinner(e.Winner) }
                };
			}

		}

		public Round GetOldRound()
		{
			return _roundService.GetRound(null);
		}

		public void SaveRound(Round round)
		{
			_roundService.SaveRound(null, round);
		}
	}
}
