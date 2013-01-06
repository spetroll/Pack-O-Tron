using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pack_O_Tron
{
	public sealed class PaddedStringFormatInfo : IFormatProvider, ICustomFormatter
	{
		private int padding;
		private char character;

		public PaddedStringFormatInfo(int i, char c = ' ')
		{
			padding = i.ToString().Length+1;
			character = c;
		}

		public object GetFormat(Type formatType)
		{
			if (typeof(ICustomFormatter).Equals(formatType))
				return this;
			return null;
		}

		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
			if (arg == null)
				throw new ArgumentNullException("Argument cannot be null");

			string[] args;
			if (format != null)
				args = format.Split(':');
			else
				return arg.ToString();

			return (arg as string).PadLeft(padding, args[1][0]);
		}
	}

}
