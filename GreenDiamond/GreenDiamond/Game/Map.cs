using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Common;

namespace Charlotte.Game
{
	public static class Map
	{
		private static AutoTable<MapCell> Table = new AutoTable<MapCell>(100, 30);
		private static MapCell DefaultCell = new MapCell();

		public static int Get_W()
		{
			return Table.W;
		}

		public static int Get_H()
		{
			return Table.H;
		}

		public static MapCell GetCell(I2Point pt)
		{
			return GetCell(pt.X, pt.Y);
		}

		public static MapCell GetCell(int x, int y)
		{
			return GetCell(x, y, DefaultCell);
		}

		public static MapCell GetCell(int x, int y, MapCell defCell)
		{
			MapCell cell = Table[x, y];

			if (cell == null)
				cell = defCell;

			return cell;
		}

		public static void Init(int w, int h)
		{
			Table = new AutoTable<MapCell>(w, h);
		}

		public static void SetCell(int x, int y, MapCell cell)
		{
			Table[x, y] = cell;
		}
	}
}
