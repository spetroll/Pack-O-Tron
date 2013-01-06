using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Pack_O_Tron.Comparer;

namespace Pack_O_Tron
{
	class Program
	{
		private static void Main()
		{
			var timer = new Stopwatch();

			timer.Start();
			var boxList = new List<Box>(ReadBoxFile(@"test.txt"));
			timer.Stop();

			Console.WriteLine("Reading file: {0}", timer.GetTimeString());

			timer.Restart();
			List<Grid> results = GetResults(boxList).ToList();
			timer.Stop();

			Console.WriteLine("Heuristics: {0}", timer.GetTimeString());
			results.Sort(new GridComparer());
			results.ForEach(x=> Console.WriteLine(x.ShortString()));
			
			WriteOutputFile(@"c:\result.txt", results.First());
			Console.Read();

		}

		private static IEnumerable<Grid> GetResults(List<Box> boxList)
		{
			foreach (var method in Extensions.GetValues<FreeRectChoiceHeuristic>())
			{
				bool finished = false;
				int width = 5;
				int height = 5;
				Grid bin = null;
				while (!finished)
				{
					width++;
					height++;
					bin = new Grid(width, height, method);
					finished = bin.Insert(boxList.Select(x => new Box(x)).ToList());
				}
				yield return bin;
			}
		}

		#region Working with Files

		private static void WriteOutputFile(string filename, Grid result)
		{
			using (var file = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
			using (var writer = new StreamWriter(file))
			{
				result.UsedBoxes.ForEach(x => writer.WriteLine(x.ToString()));
			}
		}

		private static IEnumerable<Box> ReadBoxFile(string filename)
		{
			using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
			using (var reader = new StreamReader(file))
			{
				while (!reader.EndOfStream)
				{
					var split = reader.ReadLine().Split(' ').Select(n => Convert.ToInt32(n)).ToArray();
					yield return new Box(split[0], split[1]);
				}
			}
		}

		public static void GenerateRandomInputToFile(string filename, int amount, int min, int max)
		{
			var rng = new Random();
			using (var file = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
			using (var writer = new StreamWriter(file))
			{
				foreach (var i in Enumerable.Range(0, amount))
				{
					writer.WriteLine("{0} {1}", rng.Next(min, max), rng.Next(min, max));
				}
			}
		}

		#endregion
	}
}
