using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

		public static string[] ColourValues = new string[]
			{
				"#a60000", "#f23d3d", "#661a1a", "#4c1313", "#330d0d", "#a65353", "#592d2d", "#402020", "#f2b6b6", "#997373",
				"#ff2200", "#660e00", "#993626", "#d97b6c", "#7f4840", "#4d3c39", "#cc3600", "#8c2500", "#ff7340", "#b2502d",
				"#73341d", "#f29979", "#99614d", "#4c3026", "#33201a", "#f2c6b6", "#b39286", "#806860", "#f26100", "#a64200",
				"#4c1f00", "#331400", "#f2853d", "#bf6930", "#733f1d", "#402310", "#cc8f66", "#7f5940", "#593e2d", "#d9b8a3",
				"#594c43", "#e57a00", "#a65800", "#733d00", "#4c2900", "#d98d36", "#e5b073", "#8c6c46", "#59442d", "#33271a",
				"#807160", "#e59900", "#7f5500", "#a67c29", "#664d1a", "#403010", "#99804d", "#ffeabf", "#b3a486", "#ffcc00",
				"#cca300", "#73621d", "#f2da79", "#b2a159", "#665c33", "#ffee00", "#ccbe00", "#998f00", "#403c00", "#737156",
				"#4d4b39", "#798020", "#f6ff80", "#c5cc66", "#6f7339", "#31331a", "#293300", "#cef23d", "#98b32d", "#dae6ac",
				"#9da67c", "#558000", "#4c661a", "#394d13", "#58a600", "#9df23d", "#7fa653", "#314020", "#aaf279", "#4c5943",
				"#3ad900", "#185900", "#518040", "#c6f2b6", "#688060", "#344030", "#12330d", "#004000", "#238c23", "#73e682",
				"#00bf33", "#86b392", "#00f261", "#00732e", "#2db362", "#2d593e", "#ace6c3", "#005930", "#40ffa6", "#0d3321",
				"#80ffc3", "#60bf93", "#408062", "#4d665a", "#009966", "#394d46", "#00d9ad", "#269982", "#66ccb8", "#bffff2",
				"#698c85", "#00736b", "#004d47", "#40fff2", "#1a3331", "#8fbfbc", "#00eeff", "#269199", "#6cd2d9", "#336366",
				"#00c2f2", "#007a99", "#005266", "#002933", "#80e6ff", "#60acbf", "#b6e6f2", "#739199", "#00aaff", "#0080bf",
				"#005e8c", "#66aacc", "#99bbcc", "#435259", "#303a40", "#007ae6", "#0058a6", "#004480", "#102940", "#79baf2",
				"#2d4459", "#bfe1ff", "#607180", "#002966", "#73a1e6", "#405980", "#000e33", "#4073ff", "#294ba6", "#6079bf",
				"#1a2033", "#b6c6f2", "#7c87a6", "#4d5366", "#000840", "#364cd9", "#7382e6", "#464f8c", "#2d3359", "#0000ff",
				"#2d2db3", "#1d1d73", "#1b00cc", "#0a004d", "#5940ff", "#a099cc", "#646080", "#220080", "#22134d", "#a280ff",
				"#493973", "#494359", "#7d59b3", "#2b2633", "#6d00cc", "#300059", "#742db3", "#3b264d", "#271a33", "#d6b6f2",
				"#aa00ff", "#550080", "#a336d9", "#290033", "#ac60bf", "#f240ff", "#aa2db3", "#792080", "#f780ff", "#6f3973",
				"#8a698c", "#591655", "#40103d", "#ffbffb", "#f23dce", "#a6298d", "#a65395", "#4d2645", "#594355", "#ff00aa",
				"#8c005e", "#660044", "#330022", "#e673bf", "#d9a3c7", "#d9368d", "#4d1332", "#804062", "#402031", "#806071",
				"#403038", "#ff0066", "#99003d", "#4c001f", "#ff408c", "#b22d62", "#7f2046", "#e673a1", "#a65374", "#592d3e",
				"#f20041", "#590018", "#33000e", "#cc335c", "#d9a3b1", "#a67c87", "#8c0013", "#992636", "#e57382", "#7f4048",
				"#664d50"

			};
	}
}
