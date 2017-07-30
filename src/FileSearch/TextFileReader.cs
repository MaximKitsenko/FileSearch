using System.IO;
using System.Linq;
using System.Text;

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
	}
}