using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;

namespace Charlotte.Game01.Map01.Tile01
{
	public static class MapTileUtils
	{
		public static Dictionary<string, MapTile> Tiles = DictionaryTools.Create<MapTile>();

		public static void Add(MapTile tile)
		{
			Tiles.Add(tile.Name, tile);
		}

		public static MapTile GetTile(string name)
		{
			if (string.IsNullOrEmpty(name))
				return null;

			return Tiles[name];
		}
	}
}
