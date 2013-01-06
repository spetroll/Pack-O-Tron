using System.Collections.Generic;

namespace Pack_O_Tron.Comparer
{
	public class WidthComparer : IComparer<Box>
	{
		public int Compare(Box x, Box y)
		{
			int ret = y.Width.CompareTo(x.Width);
			return ret == 0 ? y.Height.CompareTo(x.Height) : ret;
		}
	}
}
