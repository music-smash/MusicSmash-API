
using MusicSmash.Database.Interfaces;
using static MusicSmash.Models.Game;

namespace MusicSmash.Models
{
	public class Game : Entity<GameDB, long>
	{

		public required Album Left { get; set; }
		public required Album Right { get; set; }
		public required Album Winner { get; set; }

		public bool IsFinished => Winner != null;

		public class GameDB : DBEntity<long>
		{
            public required long Left { get; set; }
            public required long Right { get; set; }
            public required long? Winner { get; set; }
        }
	}
}