using MusicSmash.Controllers.Exceptions;
using MusicSmash.Extensions;
using MusicSmash.Models;

namespace MusicSmash.Services
{
    public class GameService
    {
        public List<Game> GetRandomGameWithCoupledAlbums(List<Album> albumsPool)
        {
            IEnumerable<(Album left, Album right)> Associate(List<Album> albums)
            {
                for (int i = 0; i < albums.Count; i += 2)
                {
                    yield return (albums[i], i + 1 < albums.Count ? albums[i + 1] : Album.NotDefined);
                }
            }

            if (albumsPool.Count == 1)
                throw new WeHaveAWinnerExceptions(albumsPool.First());

            return Associate(albumsPool.Shuffle().ToList())
                .Select(couple => new Game()
                {
                    Left = couple.left,
                    Right = couple.right,
                    Winner = null
                }).ToList();
        }
    }
}
