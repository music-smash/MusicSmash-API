namespace MusicSmash.Models
{
	public class Game
	{
		public required Album Left { get; set; }
		public required Album Right { get; set; }
		public Album Winner { get; set; }
	}
}
