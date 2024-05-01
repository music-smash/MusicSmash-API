using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicSmash.API.Attributes;
using MusicSmash.API.Extensions;
using MusicSmash.API.Services;
using MusicSmash.Models;
using System.Security.Claims;

namespace MusicSmash.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RoundController(ILogger<RoundController> logger, IRoundService roundService) : ControllerBase
    {
        private readonly IRoundService _roundService = roundService;

        [HttpPut]
        public ActionResult PutRound([FromBody] RoundBase payload)
        {
            var user = this.GetSpotifyUser();
            _roundService.SaveRound(payload, user.Id);
            return Ok();
        }

        [HttpGet]
        public ActionResult<RoundFull> GetRound()
        {
            var user = this.GetSpotifyUser();
            var round = _roundService.GetNextRound(user.Id);

            var result = new RoundFull(round.Index, round.Games.Select(g => new GameFull(
                               new AlbumFull(g.Left.Id, g.Left.Name, g.Left.Cover),
                                              new AlbumFull(g.Right.Id, g.Right.Name, g.Right.Cover),
                                                             g.Winner is null ? null : new AlbumFull(g.Winner.Id, g.Winner.Name, g.Winner.Cover)
                                                                        )).ToArray());
            return Ok(result);
        }

        #region Contracts
        public record RoundBase(int Index, GameBase[] Games);
        public record GameBase(long Left, long Right, long? Winner);

        public record RoundFull(int Index, GameFull[] Games);
        public record GameFull(AlbumFull Left, AlbumFull Right, AlbumFull Winner);
        public record AlbumFull(long Id, string Name, string Cover);
        #endregion
    }
}
