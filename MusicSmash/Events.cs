using MusicSmash.Models;

namespace MusicSmash
{
    public class Events
    {
        public event EventHandler<Round> NewRoundLoaded;
        public void OnNewRoundLoaded(object caller, Round round)
        {
            NewRoundLoaded?.Invoke(caller, round);
        }
    }
}
