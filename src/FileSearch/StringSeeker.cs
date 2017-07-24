using System;
using System.Collections.Generic;

namespace FileSearch
{
	public class StringSeeker : IStringSeeker
	{
		public IEnumerable<int> SearchPattern(string s, string pattern)
		{
			var res = new List<int>();
			var idx = 0;
			do
			{
				idx = s.IndexOf(pattern, idx, StringComparison.OrdinalIgnoreCase);
				if (idx > -1)
				{
					res.Add(idx);
					idx += pattern.Length;
				}
			} while (idx > -1);
			return res;
		}
	}
}