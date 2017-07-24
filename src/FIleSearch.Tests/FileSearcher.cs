using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace FileSearch
{
	[TestFixture]
	public class FileSearchertest
	{
		public class MyTestCase
		{
			public MyTestCase(string str, string pattern, List<int> foundIndexes, string res)
			{
				Str = str;
				Pattern = pattern;
				FoundIndexes = foundIndexes;
				Res = res;
			}

			public string Str { get; }
			public string Pattern { get; }
			public List<int> FoundIndexes { get; }
			public string Res { get; }
		}

		public class MyFactoryClass
		{
			public static IEnumerable TestCases
			{
				get
				{
					yield return new TestCaseData(new MyTestCase("012345", "345", new List<int> {3}, "")).SetName("pattern in the end")
						;
					yield return
						new TestCaseData(new MyTestCase("0123456", "345", new List<int> {3}, "6")).SetName(
							"there are few symbols after pattern");
					yield return
						new TestCaseData(new MyTestCase("012345678", "345", new List<int> {3}, "78")).SetName(
							"there are (pattern.Len) symbols after pattern");
					yield return
						new TestCaseData(new MyTestCase("0123456789", "345", new List<int> {3}, "89")).SetName(
							"there are (pattern.Len + N)  symbols after pattern");
					yield return
						new TestCaseData(new MyTestCase("0123456789", "3_5", new List<int>(), "89")).SetName("pattern was not found");
				}
			}
		}

		[Test]
		[TestCaseSource(typeof(MyFactoryClass), nameof(MyFactoryClass.TestCases))]
		public void DetectPrefix(MyTestCase testCase)
		{
			//a
			//a
			var r = FileSearcher.DetectPrefix(testCase.Str, testCase.Pattern, testCase.FoundIndexes);
			//a
			Assert.AreEqual(r, testCase.Res);
		}
	}
}