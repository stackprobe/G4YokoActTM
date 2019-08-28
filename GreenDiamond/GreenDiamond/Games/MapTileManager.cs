using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;

namespace Charlotte.Games
{
	public static class MapTileManager
	{
		public static void INIT()
		{
			foreach (string file in DDResource.GetFiles())
			{
				try
				{
					const string filePrefix = "MapTile\\";
					const string fileSuffix = ".png";

					if (
						StringTools.StartsWithIgnoreCase(file, filePrefix) &&
						StringTools.EndsWithIgnoreCase(file, fileSuffix)
						)
					{
						string name = file.Substring(filePrefix.Length, file.Length - filePrefix.Length - fileSuffix.Length).Replace('\\', '/');

						MapTile tile = new MapTile()
						{
							Name = name,
							Picture = DDPictureLoaders.Standard(file),
						};

						Add(tile);
					}
				}
				catch (Exception e)
				{
					throw new AggregateException("file: " + file, e);
				}
			}
		}

		private static List<string> Names = new List<string>();
		private static Dictionary<string, MapTile> Tiles = DictionaryTools.CreateIgnoreCase<MapTile>();

		private static void Add(MapTile tile)
		{
			Names.Add(tile.Name);
			Tiles.Add(tile.Name, tile);
		}

		public static MapTile GetTile(string name)
		{
			if (Tiles.ContainsKey(name) == false)
				return null;

			return Tiles[name];
		}

		public static List<string> GetNames()
		{
			return Names;
		}

		public static int GetCount()
		{
			return Names.Count;
		}
	}
}
