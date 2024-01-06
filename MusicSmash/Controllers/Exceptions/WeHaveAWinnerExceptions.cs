using MusicSmash.Models;

namespace MusicSmash.Controllers.Exceptions
{
    public class WeHaveAWinnerExceptions(Album winner) : System.Exception
    {
        public Album Winner { get; } = winner;
   
    }
}
