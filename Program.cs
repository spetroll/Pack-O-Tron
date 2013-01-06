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
			var grid = new Grid(11, 1000);
			var timer = new Stopwatch();
			timer.Start();

			var boxList = new List<Box>(ReadBoxFile(@"test.txt"));

			timer.Stop();
			Console.WriteLine("Reading file: {0}", timer.GetTimeString());

			timer.Restart();


			//var result = OneColumnsSimple(boxList, grid);
			//var result = OneColumnsSimple(boxList, grid, new WidthComparer());
			//var result = OneColumnsHeuristic(boxList, grid);
			//var result = OneColumnsHeuristic(boxList, grid, new HeightComparer());
			//var result = OneColumnsHeuristic(boxList, grid, new WeightComparer());
			//var result = OneColumnsHeuristic(boxList, grid, new AreaComparer());
			var result = FillRecursive(grid, boxList);
			
			timer.Stop();
			Console.WriteLine("Filling grid: {0}", timer.GetTimeString());
			Console.WriteLine("Efficiency {0}%", result.Efficiency());
			Console.WriteLine(result);

			WriteOutputFile(@"c:\result.txt", result);
			Console.Read();

		}

		public static Grid FillRecursive(Grid grid, List<Box> remainingBoxes)
		{
			remainingBoxes.Sort(new HeightComparer());
			var area = new Area(0, 0, grid.Height, grid.Width);
			var results =
				remainingBoxes.Select(box => FillBoundedArea(area, remainingBoxes.Clone(), new Grid(grid), true)).ToList();
			results.Sort(new GridComparer());

			results.ForEach(Console.WriteLine);
			return results.First();
		}

		public static int BoxIndex = 0;

		public static Grid FillBoundedArea(Area area, List<Box> remainingBoxes, Grid grid, bool first = false )
		{
			if (area.X > area.MaxX || area.Y > area.MaxY)
			{
				throw new IndexOutOfRangeException();
			}

			if (remainingBoxes != null)
			{
				Box box = first ? remainingBoxes[BoxIndex++] : remainingBoxes.FirstOrDefault(x => x.FitsArea(area));

				if (box == null || !grid.Valid(box, area.X, area.Y))
					return grid;

				grid.Add(box, area.X, area.Y);
				remainingBoxes.Remove(box);

				var horizontal2 = new Area(area.X, box.Width + area.Y, box.Height+area.X, area.MaxY);
				var horizontal34 = new Area(box.Height + area.X, area.Y, area.MaxX, area.MaxY);

				FillBoundedArea(horizontal2, remainingBoxes, grid);
				FillBoundedArea(horizontal34, remainingBoxes, grid);

				//Doesn't work correctly
				//Area vertical3 = new Area(box.Height + area.X, area.Y, area.MaxX, box.Width + area.Y);
				//Area vertical24 = new Area(area.X, box.Width + area.Y, area.MaxX, area.MaxY);

				//FillBoundedArea(vertical3, remainingBoxes, grid);
				//FillBoundedArea(vertical24, remainingBoxes, grid);
			}

			return grid;
		}

		public static Grid OneColumnsHeuristic(List<Box> boxList, Grid grid, IComparer<Box> comparer = null)
		{
			int x = 0;
			int y = 0;
			if (comparer != null)
			{
				boxList.Sort(comparer);
			}

			var remainingBoxes = boxList.Clone();
			while (remainingBoxes.Count > 0)
			{
				
				var fits = remainingBoxes.FirstOrDefault(box => y + box.Width <= grid.Width);

				if (fits != null)
				{
					grid.Add(fits, x, y);
					remainingBoxes.Remove(fits);
					y += fits.Width;
				}
				else
				{
					x = grid.MaxLineHeight();
					y = 0;
				}
			}
			return grid;
		}

		public static Grid OneColumnsSimple(List<Box> boxList, Grid grid, IComparer<Box> comparer = null)
		{
			int maxX = 0;
			if (comparer != null)
			{
				boxList.Sort(comparer);
			}
			foreach (var box in boxList)
			{
				grid.Add(box, maxX, 0);
				maxX += box.Height;
			}
			return grid;
		}


		#region Working with Files

		private static void WriteOutputFile(string filename, Grid result)
		{
			using (var file = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
			using (var writer = new StreamWriter(file))
			{
				result.usedBoxes.ForEach(x => writer.WriteLine(x.ToString()));
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
					yield return (new Box(split[0], split[1]));
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
