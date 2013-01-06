using System.Collections.Generic;

namespace Pack_O_Tron.Comparer
{
	class GridComparer : IComparer<Grid>
	{
		public int Compare(Grid x, Grid y)
		{
			return y.Efficiency().CompareTo(x.Efficiency());
		}
	}
}
