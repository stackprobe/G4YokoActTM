using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Common;

namespace Charlotte.Games
{
	public class Map
	{
		private AutoTable<MapCell> Table;
		private MapCell DefaultCell = new MapCell()
		{
			// 初期化子？
		};

		public Map(int w, int h)
		{
			if (
				w < 1 || IntTools.IMAX < w ||
				h < 1 || IntTools.IMAX < h
				)
				throw new DDError();

			this.Table = new AutoTable<MapCell>(w, h);

			for (int x = 0; x < w; x++)
			{
				for (int y = 0; y < h; y++)
				{
					this.Table[x, y] = new MapCell();
				}
			}
		}

		public int W
		{
			get { return this.Table.W; }
		}

		public int H
		{
			get { return this.Table.H; }
		}

		public MapCell GetCellByPixelPoint(double x, double y)
		{
			return this.GetCellByPixelPoint(new D2Point(x, y));
		}

		public MapCell GetCellByPixelPoint(D2Point pt)
		{
			return this.GetCell(ToTablePoint(pt));
		}

		public MapCell GetCell(I2Point pt)
		{
			return this.GetCell(pt, this.DefaultCell);
		}

		public MapCell GetCell(I2Point pt, MapCell defCell)
		{
			return this.GetCell(pt.X, pt.Y, defCell);
		}

		public MapCell GetCell(int x, int y)
		{
			return this.GetCell(x, y, this.DefaultCell);
		}

		public MapCell GetCell(int x, int y, MapCell defCell)
		{
			if (
				x < 0 || this.Table.W <= x ||
				y < 0 || this.Table.H <= y
				)
				return defCell;

			return Table[x, y];
		}

		// TablePoint ... I2Point Table用の座標
		// PixelPoint ... D2Point 座標(ピクセル単位)

		public static I2Point ToTablePoint(D2Point pixelPt)
		{
			return ToTablePoint(pixelPt.X, pixelPt.Y);
		}

		public static I2Point ToTablePoint(double pixelX, double pixelY)
		{
			int mapTileX = (int)Math.Floor(pixelX / MapTile.WH);
			int mapTileY = (int)Math.Floor(pixelY / MapTile.WH);

			return new I2Point(mapTileX, mapTileY);
		}

		private Dictionary<string, string> Properties = DictionaryTools.Create<string>();

		public void AddProperty(string name, string value)
		{
			this.Properties.Add(name, value);
		}

		public string GetProperty(string name, string defval = null)
		{
			if (this.Properties.ContainsKey(name) == false)
				return defval;

			return this.Properties[name];
		}

		public IEnumerable<KeyValuePair<string, string>> GetProperties()
		{
			return this.Properties;
		}
	}
}
