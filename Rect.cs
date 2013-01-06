using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pack_O_Tron
{
	public class Rect
	{
		public int x;
		public int y;
		public int width;
		public int height;
		public int ID;

		public static int count = 0;

		public Rect(Rect b) : this(b.x, b.y, b.width, b.height)
		{
		}

		public Rect(int width, int height) : this(0,0,width, height)
		{
		}

		public Rect()
		{
		}

		public Rect(int x, int y, int width, int height)
		{
			this.ID = count++;
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		public int CompareRectShortSide(Rect a, Rect b)
		{

			int smallerSideA = Math.Min(a.width, a.height);
			int smallerSideB = Math.Min(b.width, b.height);

			if (smallerSideA != smallerSideB)
				return smallerSideA.CompareTo(smallerSideB);

			// Tie-break on the larger side.
			int largerSideA = Math.Max(a.width, a.height);
			int largerSideB = Math.Max(b.width, b.height);

			return largerSideA.CompareTo(largerSideB);
		}
		
		public static bool IsContainedIn(Rect a, Rect b)
		{
			return a.x >= b.x && a.y >= b.y
			       && a.x + a.width <= b.x + b.width
			       && a.y + a.height <= b.y + b.height;
		}

		public override string ToString()
		{
			return String.Format("{0} {1} {2} {3}", x, y, width, height, ID);
		}
	}
}
