using Microsoft.AspNetCore.Mvc;
using MusicSmash.Models;

namespace MusicSmash.API.Extensions
{
    public static class ControllerExtension
    {

        public static User GetSpotifyUser(this ControllerBase controller)
        {
            return controller.HttpContext.Items["User"] as User;
        }

    }
}
