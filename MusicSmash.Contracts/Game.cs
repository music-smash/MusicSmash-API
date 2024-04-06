namespace MusicSmash.Models
{
	public class Game
	{
		public required Album Left { get; set; }
		public required Album Right { get; set; }
		public required Album Winner { get; set; }

		public bool IsFinished => Winner != null;

		public class GameDB
		{
            public required string id { get; set; }
            public required string Left { get; set; }
            public required string Right { get; set; }
            public required string Winner { get; set; }
        }
	}
}