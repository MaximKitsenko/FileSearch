using System.Collections.Generic;
using System.Linq;

namespace FileSearch.Misc
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<IEnumerable<T>> SplitSequential<T>(this IEnumerable<T> src, int parts)
		{
			var srcList = src as IList<T> ?? src.ToList();
			var srcCnt = srcList.Count();
			var chunkSize = srcCnt / parts;
			for (var i = 0; i < parts; i++)
			{
				if (i + 1 == parts)
					yield return srcList.Skip(i * chunkSize);
				else
					yield return srcList.Skip(i * chunkSize).Take(chunkSize);
			}
		}
	}
}