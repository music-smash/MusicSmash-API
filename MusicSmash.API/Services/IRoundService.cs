using MusicSmash.API.Controllers;
using MusicSmash.Models;

namespace MusicSmash.API.Services
{
    public interface IRoundService
    {
        Round GetNextRound(string userId);
        void SaveRound(RoundController.RoundBase payload);
    }
}
