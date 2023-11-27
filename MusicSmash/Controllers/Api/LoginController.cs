using Microsoft.AspNetCore.Mvc;

namespace MusicSmash.Controllers.Api
{
    [ApiController]
    public class LoginController : Controller
    {


        [Route("/callback")]
        [HttpGet]
        public void Callback(
                            [FromRoute] string state,
                            [FromRoute] string code,
                            [FromRoute] string error)
        {
            if (state is null || error is not null)
                return;

            //Get token
            
            // set token on local
            Redirect("/");
        }

    }
}
