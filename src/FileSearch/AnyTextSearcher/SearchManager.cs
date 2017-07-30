using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileSearch
{
	public class SearchManager
	{
		private readonly int _partSize = 1000;
		private readonly int _processorsCount;
		private readonly List<Task> _textReaders;
		private int _currentIdx;
		private readonly List<Queue<SearchPartDescriptor>> _partsQueue;
		private string _path;

		public SearchManager(int processorsCount, string path)
		{
			_processorsCount = processorsCount;
			_path = path;
			_partsQueue = Enumerable.Range(0, _processorsCount).Select((x, i) => new Queue<SearchPartDescriptor>()).ToList();
			_textReaders = Enumerable.Range(0, _processorsCount)
				.Select((x, i) => new TextFileReader(path, Encoding.UTF8).ReadSymbolsUntilEof(
					() => Interlocked.Increment(ref _currentIdx),
					(s, id) => { _partsQueue[i].Enqueue(new SearchPartDescriptor(i, id, s)); },
					_partSize)).ToList();
		}

		public void SearchInFile(string pattern, string filePath)
		{
			var partNum = 0;
			_textReaders
		}
	}
}