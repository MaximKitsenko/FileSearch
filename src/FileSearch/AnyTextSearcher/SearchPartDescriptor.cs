using System.Collections.Generic;

namespace FileSearch
{
	public class SearchPartDescriptor
	{
		public IEnumerable<int> indexes { get; }
		public int BatchId { get; }
		public int Id { get; }
		public string Text { get; }

		public SearchPartDescriptor(IEnumerable<int> indexes, int batchId, int id, string text)
		{
			this.indexes = indexes;
			BatchId = batchId;
			Id = id;
			Text = text;
		}
		public SearchPartDescriptor( int batchId, int id, string text)
		{
			this.indexes = new List<int>();
			BatchId = batchId;
			Id = id;
			Text = text;
		}
	}
}