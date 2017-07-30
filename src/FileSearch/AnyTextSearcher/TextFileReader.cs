using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileSearch
{
	public class TextFileReader
	{
		private FileStream _file;
		private readonly string _path;
		private readonly Encoding _encoding;

		private FileStream CurrentFile => _file ?? (_file = File.OpenRead(this._path));

		public TextFileReader(string path, Encoding encoding)
		{
			this._path = path;
			this._encoding = encoding;
		}

		public string ReadSymbols(int idx, int cnt)
		{
			var buffer = new char[cnt];
			var sr = new StreamReader(CurrentFile, _encoding);
			var readed = sr.Read(buffer, idx, cnt);
			var readedString = new string(buffer.Take(readed).ToArray());
			return readedString;
		}

		public Task ReadSymbolsUntilEof(Func<int> idxFactory, Action<string,int> onReaded, int cnt)
		{
			return new Task(() =>
			{
				var idx = idxFactory();

				do
				{
					var readSymbols = ReadSymbols(idx, cnt);
					if (!string.IsNullOrWhiteSpace(readSymbols))
					{
						onReaded(readSymbols, idx);
					}
					else
					{
						return;
					}
				} while (true);
			});
		}
	}
}