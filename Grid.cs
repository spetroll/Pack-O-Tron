using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pack_O_Tron
{
	public class Grid
	{
		public int Width { get; private set; }
		public int Height { get; private set; }
		public readonly List<Box> usedBoxes = new List<Box>();
		private readonly string[,] StringMap;

		public Grid(int width, int height)
		{
			Width = width;
			Height = height;
			StringMap = new string[height, width];
		}

		public Grid(Grid oldGrid)
		{
			Width = oldGrid.Width;
			Height = oldGrid.Height;
			StringMap = oldGrid.StringMap.Clone() as string[,];
			usedBoxes.AddRange(oldGrid.usedBoxes.Clone());
		}

		public int MaxLineHeight()
		{
			for (int x = 0; x < Height; x++)
			{
				bool empty = true;
				for (int y = 0; y < Width; y++)
				{
					if (!String.IsNullOrEmpty(StringMap[x, y]))
					{
						empty = false;
						break;
					}
				}
				if (empty)
					return x;
			}
			return Height;
		}

		public bool Valid(Box b, int x, int y)
		{
			if (b == null)
				return false;

			if (x+ b.Height > Height || y + b.Width > Width)
				return false;

			b.X = x;
			b.Y = y;

			for (; x < b.X + b.Height; x++)
			{
				for (; y < b.Y + b.Width; y++)
				{
					if (!String.IsNullOrEmpty(StringMap[x, y]))
						return false;
				}
			}
			return true;
		}

		public void Add(Box b, int x, int y)
		{
			b.X = x;
			b.Y = y;
			Add(b);
		}

		public void Add(Box b)
		{
			if (b == null)
				throw new ArgumentNullException();
			if (b.X + b.Height > Height || b.Y + b.Width > Width)
				throw new ArgumentOutOfRangeException();

			usedBoxes.Add(b);

			for (int x = b.X; x < b.X + b.Height; x++)
			{
				for (int y = b.Y; y < b.Y + b.Width; y++)
				{
					if (!String.IsNullOrEmpty(StringMap[x, y]))
					{
						Console.WriteLine(this.ToString());
						throw new InvalidProgramException(String.Format("Duplicate on {0},{1}: {2} & {3}",
						                                                b.X, b.Y, StringMap[b.X, b.Y], b));
					}
					StringMap[x, y] = b.ID;
				}
			}
		}

		public int Count()
		{
			return usedBoxes.Count();
		}

		public double Efficiency()
		{
			var sum = 0;
			usedBoxes.ForEach(x => sum += x.TotalArea());
			return sum*100/(Width*MaxLineHeight());
		}

		public override string ToString()
		{
			var result = new StringBuilder(String.Format("Grid: {0},{1}: {2} max", Width, Height, MaxLineHeight()));
			result.Append(Environment.NewLine);
			for (int x = 0; x < MaxLineHeight(); x++)
			{
				for (int y = 0; y < StringMap.GetLength(1); y++)
				{
					result.Append(String.Format(" {0} ", String.IsNullOrEmpty(StringMap[x, y]) ? "." : StringMap[x, y]));
				}
				result.Append(Environment.NewLine);
			}

			result.Append(String.Format("Efficiency: {0}", Efficiency()));
			return result.ToString();
		}
	}
}
