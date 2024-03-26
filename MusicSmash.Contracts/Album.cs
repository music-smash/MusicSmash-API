namespace MusicSmash.Models
{
	public class Album
	{
		public required string Id;
		public required string Name;

		public required string Cover;

		public required string ReleaseDate;

		public required int Score;

		public static Album NotDefined =>
			new Album()
			{
				Id = string.Empty,
				Name = string.Empty,
				Cover = string.Empty,
				ReleaseDate = string.Empty,
				Score = 0
			};
	}
}
