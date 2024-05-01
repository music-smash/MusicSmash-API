﻿using MusicSmash.Database.Interfaces;
using static MusicSmash.Models.Round;

namespace MusicSmash.Models
{
	public class Round : Entity<RoundDB, long>
	{
		public required int Index { get; set; }

		public required Game[] Games { get; set; }

		public required User Owner { get; set; }

		public RoundType RoundType => 
			(RoundType)(int)
			Math.Floor
			( 
				Math.Clamp(Games.Length, 0, (int)RoundType.Undefined * 2) 
				/ 
				2f 
			);

		public class RoundDB : DBEntity<long>
		{
            public required int Index { get; set; }
			public required string userId { get; set; }
        }
	}

	public enum RoundType
	{
		Finished = 0, // number of competitors / 2 => 0/2 = 0
		Final = 1, // 2 => 2/2 = 0
		SemiFinal = 2, // 4 => 4/2 = 2
        QuarterFinal= 3,
        EighthFinal= 4,
		Undefined = 5
	}

}
