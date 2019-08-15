using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.Main01
{
	public static class GameUtils
	{
		public static I2Point PointToMapCellPoint(double x, double y)
		{
			int mapTileX = (int)Math.Floor(x / Consts.MAP_TILE_WH);
			int mapTileY = (int)Math.Floor(y / Consts.MAP_TILE_WH);

			return new I2Point(mapTileX, mapTileY);
		}
	}
}
