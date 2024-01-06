namespace MusicSmash.Extensions
{
	public static class CollectionExtension
	{
		public static T[] Shuffle<T>(this T[] target)
		{
			var length = target.Length;
			var random = new Random();
			for (int i = length - 1; i >= 0; i--)
			{
				var index = random.Next(length);

				var tmp = target[index];
				target[index] = target[i];
				target[i] = tmp;
			}

			return target;
		}

        public static List<T> Shuffle<T>(this List<T> target)
		{
            var length = target.Count;
            var random = new Random();
            for (int i = length - 1; i >= 0; i--)
			{
                var index = random.Next(length);

                var tmp = target[index];
				target[index] = target[i];
				target[i] = tmp;
            }

            return target;
        }


    }
}
