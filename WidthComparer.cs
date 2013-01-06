using System.Collections.Generic;

namespace Pack_O_Tron.Comparer
{
	public class WidthComparer : IComparer<Rect>
	{
		public int Compare(Rect x, Rect y)
		{
			int ret = y.width.CompareTo(x.width);
			return ret == 0 ? y.height.CompareTo(x.height) : ret;
		}
	}
}