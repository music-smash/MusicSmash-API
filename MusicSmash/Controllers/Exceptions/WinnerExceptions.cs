using MusicSmash.Models;

namespace MusicSmash.Controllers.Exceptions
{
    public class WinnerExceptions(Album winner) : System.Exception
    {
        public Album Winner { get; } = winner;
   
    }
}
