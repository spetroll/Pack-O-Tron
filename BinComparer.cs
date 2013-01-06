using System.Collections.Generic;

namespace Pack_O_Tron
{
	class GridComparer : IComparer<MaxRectsBinPack>
	{
		public int Compare(MaxRectsBinPack x, MaxRectsBinPack y)
		{
			return y.Efficiency().CompareTo(x.Efficiency());
		}
	}
}
