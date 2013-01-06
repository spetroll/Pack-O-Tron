using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pack_O_Tron
{
	internal static class Extensions
	{

		public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
		{
			return listToClone.Select(item => (T) item.Clone()).ToList();
		}

		public static string GetTimeString(this Stopwatch stopwatch, int numberofDigits = 2)
		{
			double time = stopwatch.ElapsedTicks/(double) Stopwatch.Frequency;
			if (time > 1)
				return Math.Round(time, numberofDigits) + " s";
			if (time > 1e-3)
				return Math.Round(1e3*time, numberofDigits) + " ms";
			if (time > 1e-6)
				return Math.Round(1e6*time, numberofDigits) + " µs";
			if (time > 1e-9)
				return Math.Round(1e9*time, numberofDigits) + " ns";
			throw new NotImplementedException("");
		}

		public static IEnumerable<T> GetValues<T>()
		{
			return Enum.GetValues(typeof (T)).Cast<T>();
		}

	}
}
