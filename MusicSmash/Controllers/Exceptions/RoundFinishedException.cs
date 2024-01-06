using MusicSmash.Models;

namespace MusicSmash.Controllers.Exceptions
{
    public class RoundFinishedException : System.Exception
    {
        public Round Round { get; }

        public RoundFinishedException(Round round)
        {
            Round = round;
        }
    }
}
