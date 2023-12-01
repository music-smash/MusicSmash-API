namespace MusicSmash.Models
{
	public class Round
	{
		public required int Index { get; set; }

		public required Game[] Games { get; set; }

	}
}
