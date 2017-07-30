using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileSearch
{
	public class FileSearcher
	{
		private readonly int _chunkSize = 1024 * 1024;
		private readonly Encoding _encoding = Encoding.GetEncoding("Windows-1251");
		private readonly int _maxThreads = 1;
		public ChunksDescriptor _chunksDescriptor = new ChunksDescriptor();
		public IStringSeeker _StringSeeker = new StringSeeker();

		public FileSearcher(int chunkSize, int maxThreads)
		{
			_chunkSize = chunkSize;
			_maxThreads = maxThreads;
		}

		public SearchInfo CalculateCountInFile(string path, string pattern, Action<decimal> progress)
		{
			_chunksDescriptor.InitDescriptor(path, _chunkSize, _maxThreads);
			var semaphore = new SemaphoreSlim(_maxThreads, _maxThreads);
			var chunksReady = 0;
			var results = _chunksDescriptor.ChunksArray.Select(x =>
			{
				return Task.Factory.StartNew(() =>
					{
						try
						{
							semaphore.Wait();
							return CalculateCountInChunks(x, path, pattern, () =>
							{
								lock (this)
									progress(++chunksReady / (decimal) _chunksDescriptor.ChunksCnt);
							});
						}
						catch (Exception e)
						{
							Console.WriteLine(e);
							throw;
						}
						finally
						{
							semaphore.Release();
						}
					}
				);
			}).ToList();

			Task.WhenAll(results).Wait();
			var indexes =
				results.Select(x => x.Result)
					.Where(x => x != null && x.Any())
					.OrderBy(y => y.OrderBy(z => z.Chunk).First().Chunk)
					.ToList();//process first index for each chunk. (find prefix, and search again but length should be less the 2*pattern.length-1)

			var calculateCount = new SearchInfo {Chunk = indexes.Sum(x => x.Sum(y => y.Indexes.Count))};
			return calculateCount;
		}

		private SearchInfo[] CalculateCountInChunks(IEnumerable<int> chunks, string path, string pattern, Action action)
		{
			var chunksList = chunks as IList<int> ?? chunks.ToList();

			if (!chunksList.Any())
				return null;

			var indexes = new SearchInfo[chunksList.Count];
			using (var f = File.OpenRead(path))
			{
				using (var br = new BinaryReader(f))
				{
					var prefix = string.Empty;
					for (var j = 0; j < chunksList.Count; j++)
					{
						f.Position = chunksList[j] * (long) _chunkSize;
						var chunkBytes = br.ReadBytes(_chunkSize);
						var str = prefix + _encoding.GetString(chunkBytes);
						indexes[j] = new SearchInfo()
						{
							Indexes = _StringSeeker.SearchPattern(str, pattern).ToList(),
							Chunk = chunksList[j]
						};
						prefix = DetectPrefix(str, pattern, indexes[j].Indexes);
						action();
					}
				}
			}

			return indexes;
		}

		public static string DetectPrefix(string str, string pattern, List<int> foundIndexes)
		{
			var res = string.Empty;
			if (foundIndexes == null)
				return res;

			if (foundIndexes.Any())
			{
				var lastIdxPos = str.Length - foundIndexes.Last() - pattern.Length;
				if (lastIdxPos == 0)
					res = string.Empty;
				else if (lastIdxPos >= pattern.Length)
					res = str.Substring(str.Length - (pattern.Length - 1));
				else if (lastIdxPos > 0)
					res = str.Substring(foundIndexes.Last() + pattern.Length);
			}
			else
			{
				res = str.Substring(str.Length - (pattern.Length - 1));
			}
			return res;
		}
	}
}