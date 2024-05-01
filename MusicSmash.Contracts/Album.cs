using MusicSmash.Database.Interfaces;
using static MusicSmash.Models.Album;

namespace MusicSmash.Models
{
	public class Album : Entity<AlbumDB, long>
	{
		public required string Name { get; set; }

		public required string Cover { get; set; }

		public required int Score { get; set; }

		public static Album NotDefined =>
			new Album()
			{
				Id = -1,
				Name = string.Empty,
				Cover = string.Empty,
				Score = 0
			};

        public override string GetId()
        {
            return Id < 0 ? "null" : Id.ToString();
        }

        public class AlbumDB : DBEntity<long>
        {
            public required string Name;

            public required string Cover;

            public required int Score;

			public override string GetId()
			{
                return Id < 0 ? "null" : Id.ToString();
            }
        }
    }
}
