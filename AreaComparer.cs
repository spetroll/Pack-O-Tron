using System.Collections.Generic;

namespace Pack_O_Tron.Comparer
{
	public class AreaComparer : IComparer<Rect>
	{
		public int Compare(Rect x, Rect y)
		{
			int ret = (y.x * y.y).CompareTo(x.x * x.y);
			return ret == 0 ? y.height.CompareTo(x.height) : ret;
		}
	}
}