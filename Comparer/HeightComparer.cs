using System.Collections.Generic;

namespace Pack_O_Tron.Comparer
{
	public class HeightComparer : IComparer<Box>
	{
		public int Compare(Box x, Box y)
		{
			int ret = y.Height.CompareTo(x.Height);
			return ret == 0 ? y.Width.CompareTo(x.Width) : ret;
		}
	}
}
