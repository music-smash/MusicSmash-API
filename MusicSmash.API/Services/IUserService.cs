
using MusicSmash.Models;

namespace MusicSmash.API.Services
{
    public interface IUserService
    {
        Task<User> GetMeAsync(string jwtToken);
    }
}
