using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSearch
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine("Incorrect parameters. [filePath] [patten] expected. Press any key to exit");
				Console.ReadLine();
				return;
			}

			var pattern = args[1];
			var filePath = args[0];
			//var fs = new FileSearcher(1024*1024*25,16);//30sec
			//var fs = new FileSearcher(1024*1024*10,32);//28sec
			//var fs = new FileSearcher(1024*1024*5,64);//27.8sec
			//var fs = new FileSearcher(1024 * 1024 * 17, 64); //18sec
			//var fs = new FileSearcher(1024 * 1024 * 1, 32); //18sec
			var fs = new FileSearcher(1024 * 1024 * 1, 8); //17.2sec
			var sw = Stopwatch.StartNew();
			var resss = fs.CalculateCountInFile(filePath, pattern, x =>
			{
				Console.WriteLine("Progress: {0:P1}", x);
			});
			sw.Stop();
			Console.WriteLine("Found: {0} in {1}", resss.Chunk, sw.Elapsed);
			Console.ReadLine();
		}
	}
}