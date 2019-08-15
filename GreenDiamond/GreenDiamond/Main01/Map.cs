using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Common;

namespace Charlotte.Main01
{
	public static class Map
	{
		private static MapCell DefaultCell = new MapCell();
		private static AutoTable<MapCell> Table;

		public static void INIT()
		{
			Init(100, 30);
		}

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

		public static MapCell GetCell(I2Point pt, MapCell defCell)
		{
			return GetCell(pt.X, pt.Y, defCell);
		}

		public static MapCell GetCell(int x, int y)
		{
			return GetCell(x, y, DefaultCell);
		}

		public static MapCell GetCell(int x, int y, MapCell defCell)
		{
			if (
				x < 0 || Table.W <= x ||
				y < 0 || Table.H <= y
				)
				return defCell;

			return Table[x, y];
		}

		public static void Init(int w, int h)
		{
			if (
				w < 1 || IntTools.IMAX < w ||
				h < 1 || IntTools.IMAX < h
				)
				throw new DDError();

			Table = new AutoTable<MapCell>(w, h);

			for (int x = 0; x < w; x++)
			{
				for (int y = 0; y < h; y++)
				{
					Table[x, y] = new MapCell();
				}
			}
		}
	}
}
