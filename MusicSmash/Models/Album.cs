﻿namespace MusicSmash.Models
{
	public class Album
	{
		public required string Name;

		public required string Cover;

		public required string ReleaseDate;

		public static Album NotDefined =>
			new Album()
			{
				Name = string.Empty,
				Cover = string.Empty,
				ReleaseDate = string.Empty
			};
	}
}
