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
			return new Round()
			{
				Index = (previusRound?.Index ?? 0) + 1,
				Games = _voteController.GetRandomCoupledAlbums(previusRound)
					.Select((couple) => new Game()
					{
						Left = couple.left,
						Right = couple.right
					}).ToArray()
			};
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
