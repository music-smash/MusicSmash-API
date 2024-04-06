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
        private readonly ILogger<RoundController> _logger = logger;
        private readonly IRoundService _roundService = roundService;

        [HttpPut]
        public ActionResult PutRound([FromHeader] string userId, [FromBody] RoundBase payload)
        {
            _roundService.SaveRound(payload);
            return Ok();
        }

        [HttpGet]
        public ActionResult<RoundFull> GetRound()
        {
            var user = this.GetSpotifyUser();
            var round = _roundService.GetNextRound(user.Id);
            return Ok(round);
        }

        #region Contracts
        public record RoundBase(int Index, GameBase[] Games);
        public record GameBase(string Left, string Right, string Winner);

        public record RoundFull(int Index, GameFull[] Games);
        public record GameFull(AlbumFull Left, AlbumFull Right, AlbumFull Winner);
        public record AlbumFull(string Id, string Name, string Cover, string ReleaseDate, int Score);
        #endregion
    }
}
