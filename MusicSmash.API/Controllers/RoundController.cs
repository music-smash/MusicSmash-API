using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicSmash.API.Services;
using MusicSmash.Models;

namespace MusicSmash.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoundController(ILogger<RoundController> logger, IRoundService roundService) : ControllerBase
    {
        private readonly ILogger<RoundController> _logger = logger;
        private readonly IRoundService _roundService = roundService;

        [HttpPut]
        public ActionResult PutRound([FromHeader] string userId, [FromBody] RoundBase payload)
        {
            return Ok();
        }

        [HttpGet]
        public ActionResult<RoundFull> GetRound([FromHeader] string userId)
        {
            var round = _roundService.GetNextRound(userId);
            return Ok(round);
        }



        #region Contracts
        public class RoundBase
        {
            public int LeftAlbum { get; set; }
            public int RightAlbum { get; set; }

            public int Winner { get; set; }
        }

        public class RoundFull
        {
            public Album Left { get; set; }
            public Album Right { get; set;}
        }
        #endregion
    }
}
