using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;
using Charlotte.Common;

namespace Charlotte.Worlds
{
	public static class WorldMap
	{
		private static string[][] Rows;

		public static int W;
		public static int H;

		public static void INIT()
		{
			using (MemoryStream mem = new MemoryStream(DDResource.Load("World.csv")))
			using (StreamReader memReader = new StreamReader(mem, StringTools.ENCODING_SJIS))
			using (CsvFileReader reader = new CsvFileReader(memReader))
			{
				Rows = reader.ReadToEnd();
			}
			W = ArrayTools.Largest(Rows.Select(row => row.Length), IntTools.Comp);
			H = Rows.Length;
		}

		public static I2Point GetPoint(string file, I2Point defval)
		{
			for (int y = 0; y < Rows.Length; y++)
			{
				string[] row = Rows[y];

				for (int x = 0; x < row.Length; x++)
					if (StringTools.EqualsIgnoreCase(row[x], file))
						return new I2Point(x, y);
			}
			return defval;
		}

		public static string GetFile(int x, int y, string defval = null)
		{
			if (x < 0 || y < 0 || H <= y)
				return defval;

			string[] row = Rows[y];

			if (row.Length <= x)
				return defval;

			string cell = row[x];

			if (cell == "")
				return defval;

			return cell;
		}
	}
}
