﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.Game
{
	public static class AGameUtils
	{
		public static I2Point PointToMapCellPoint(double x, double y)
		{
			int mapTileX = (int)(x / Consts.MAP_TILE_WH);
			int mapTileY = (int)(y / Consts.MAP_TILE_WH);

			return new I2Point(mapTileX, mapTileY);
		}
	}
}
