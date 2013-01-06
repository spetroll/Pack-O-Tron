using System.Collections.Generic;

namespace Pack_O_Tron.Comparer
{
	public class AreaComparer : IComparer<Box>
	{
		public int Compare(Box x, Box y)
		{
			int ret = (y.X*y.Y).CompareTo(x.X*x.X);
			return ret == 0 ? y.Height.CompareTo(x.Height) : ret;
		}
	}
}
