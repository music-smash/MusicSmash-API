namespace MusicSmash.Models
{
	public class Game
	{
		public required Album Left { get; set; }
		public required Album Right { get; set; }
		public Album Winner { get; set; }
	}

	public class FinishedGame : Game
	{
        public required Album Winner { get; set; }

        public static FinishedGame FromWinner(Album winner)
		{
            return new FinishedGame()
			{
				Left = Album.NotDefined,
				Right = Album.NotDefined,
                Winner = winner
            };
        }
    }
}
