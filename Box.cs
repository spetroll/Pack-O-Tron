using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pack_O_Tron
{
	public class Box :ICloneable
	{
		private static int index = 0;
		public string ID { get; private set; }
		public int X { get; set;}
		public int Y { get; set; }
		public int Width { get; private set; }
		public int Height { get; private set; }

		public Box(int width, int height, int x = 0, int y = 0)
		{
			ID = index.ToString();
			index++;
			Width = width;
			Height = height;
			X = x;
			Y = y;
		}

		public void Set(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int TotalArea()
		{
			return Width*Height;
		}

		public bool FitsArea(Area area)
		{
			return Width <= (area.MaxY - area.Y) && Height <= (area.MaxX - area.X);
		}

		public object Clone()
		{
			return MemberwiseClone();
		}

		public override string ToString()
		{
			return String.Format("{0} {1} {2} {3}", X, Y, Height, Width, ID);
		}
	}
}
