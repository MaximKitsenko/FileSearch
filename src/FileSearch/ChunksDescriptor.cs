using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MaxKits.Tools.Extensions;

namespace FileSearch
{
	public class ChunksDescriptor
	{
		public ChunksDescriptor()
		{
		}

		private ChunksDescriptor(int chunksCnt, IEnumerable<IEnumerable<int>> chunks)
		{
			ChunksCnt = chunksCnt;
			ChunksArray = chunks;
		}

		public int ChunksCnt { get; private set; }
		public IEnumerable<IEnumerable<int>> ChunksArray { get; private set; }

		public ChunksDescriptor InitDescriptor(string path, int chunkSize, int maxThreads)
		{
			using (var f = File.OpenRead(path))
			{
				var chunks0 = f.Length / chunkSize;
				var chunksCnt = chunks0 + (f.Length % chunkSize > 0 ? 1 : 0);

				var chunks = Enumerable.Range(0, (int) chunksCnt)
					.SplitSequential(Environment.ProcessorCount > maxThreads ? maxThreads : Environment.ProcessorCount);

				ChunksCnt = (int) chunksCnt;
				ChunksArray = chunks;
				return this;
			}
		}
	}
}