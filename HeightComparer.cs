using System.Collections.Generic;

namespace Pack_O_Tron.Comparer
{
	public class HeightComparer : IComparer<Rect>
	{
		public int Compare(Rect x, Rect y)
		{
			int ret = y.height.CompareTo(x.height);
			return ret == 0 ? y.width.CompareTo(x.width) : ret;
		}
	}
}