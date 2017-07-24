using System.Collections.Generic;

namespace FileSearch
{
	public interface IStringSeeker
	{
		IEnumerable<int> SearchPattern(string s, string pattern);
	}
}