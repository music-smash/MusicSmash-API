using MusicSmash.Controllers.Exceptions;
using MusicSmash.Extensions;
using MusicSmash.Models;
using MusicSmash.Services;

namespace MusicSmash.Controllers
{
	public class VoteController
	{
		public readonly AlbumService _albumService;

		public VoteController(AlbumService albumService)
		{
			_albumService = albumService;
		}

		public List<(Album left, Album right)> GetRandomCoupledAlbums(Round previusRound)
		{
			IEnumerable<(Album left, Album right)> Associate(List<Album> albums)
			{
				for(int i = 0; i < albums.Count; i += 2)
				{
					yield return (albums[i], i + 1 < albums.Count ? albums[i + 1] : Album.NotDefined);
				}
			}

			var albumPool = GetAlbums(previusRound);

			if (albumPool.Length == 1)
				throw new WinnerExceptions(albumPool[0]);

			return Associate(albumPool.Shuffle().ToList()).ToList();
		}

		private Album[] GetAlbums(Round round)
		{
			if (round == null) return _albumService.GetAlbums();

			return round.Games.Select(g => g.Winner).ToArray();
		}
	}
}
