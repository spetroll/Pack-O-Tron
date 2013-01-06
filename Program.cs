using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
			GenerateRandomInputToFile(@"random.txt", 500, 1, 20);
			var boxList = new List<Rect>(ReadBoxFile(@"random.txt"));
			timer.Stop();

			Console.WriteLine("Reading file: {0}", timer.GetTimeString());

			timer.Restart();
			List<MaxRectsBinPack> results = GetResults(boxList).ToList();
			timer.Stop();

			results.Sort(new GridComparer());
			var best = results.First();

			Console.WriteLine("Heuristics: {0}", timer.GetTimeString());
			Console.WriteLine(best.ShortString());

			GeneratePicture(@"random.png", best);
			WriteOutputFile(@"result.txt", best);
			Console.Read();
		}

		private static void GeneratePicture(string filename, MaxRectsBinPack result)
		{
			int stretch = 3;
			Bitmap bmp = new Bitmap(result.binWidth * stretch, result.binHeight * stretch);
			int count = 0;
			foreach (var rect in result.UsedBoxes)
			{
				Color col = ColorTranslator.FromHtml(Extensions.ColourValues[count]);
				using (Graphics gfx = Graphics.FromImage(bmp))
				using (SolidBrush brush = new SolidBrush(col))
				{
					gfx.FillRectangle(brush, rect.x * stretch, rect.y * stretch, rect.width * stretch, rect.height * stretch);
				}

				count = count == Extensions.ColourValues.Length-1 ? 0 : count +1 ;
			}
			bmp.Save(filename);

		}

		private static IEnumerable<MaxRectsBinPack> GetResults(List<Rect> boxList, int specifiedHeight = 0)
		{
			var comparerList = new List<IComparer<Rect>>() {new HeightComparer(), new AreaComparer(), new WidthComparer()};
			foreach (var comparer in comparerList)
			foreach (var method in Extensions.GetValues<FreeRectChoiceHeuristic>())
			{
				bool finished = false;
				long minArea = boxList.Aggregate<Rect, long>(0, (current, t) => current + t.width * t.height);
				int height = specifiedHeight > 0 ? specifiedHeight : (int)Math.Sqrt( minArea);
				int width = specifiedHeight > 0 ? (int)(minArea / height) : (int)Math.Sqrt(minArea);
				MaxRectsBinPack bin = null;
				while (!finished)
				{
					width++;
					if (specifiedHeight == 0)
						height++;
					bin = new MaxRectsBinPack(width, height, method, comparer);
					finished = bin.Insert(boxList.Select(x => new Rect(x)).ToList());
				}
				yield return bin;
			}
		}

		#region Working with Files

		private static void WriteOutputFile(string filename, MaxRectsBinPack result)
		{
			using (var file = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
			using (var writer = new StreamWriter(file))
			{
				result.UsedBoxes.ForEach(x => writer.WriteLine(x.ToString()));
			}
		}

		private static IEnumerable<Rect> ReadBoxFile(string filename)
		{
			using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
			using (var reader = new StreamReader(file))
			{
				while (!reader.EndOfStream)
				{
					var readLine = reader.ReadLine();
					if (readLine != null)
					{
						var split = readLine.Split(' ').Select(n => Convert.ToInt32(n)).ToArray();
						yield return new Rect(split[0], split[1]);
					}
				}
			}
		}

		public static void GenerateRandomInputToFile(string filename, int amount, int min, int max)
		{
			var rng = new Random();
			using (var file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
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
