using MusicSmash.Controllers.Exceptions;
using MusicSmash.Models;
using MusicSmash.Services;
using System.Security.Cryptography.Xml;

namespace MusicSmash.Controllers
{
	public class RoundController
	{
		public readonly VoteController _voteController;
		public readonly RoundService _roundService;

		public RoundController(VoteController voteController, RoundService roundService)
		{
			_voteController = voteController;
			_roundService = roundService;
		}

		public Round GetNextRound(Round previusRound)
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
