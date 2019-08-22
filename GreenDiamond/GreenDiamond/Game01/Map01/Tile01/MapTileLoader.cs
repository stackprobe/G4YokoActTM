using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;

namespace Charlotte.Game01.Map01.Tile01
{
	public static class MapTileLoader
	{
		public static void INIT()
		{
			foreach (string file in DDResource.GetFiles())
			{
				try
				{
					if (
						StringTools.StartsWithIgnoreCase(file, @"MapTile\") &&
						StringTools.EndsWithIgnoreCase(file, ".png")
						)
						new MapTile(file);
				}
				catch (Exception e)
				{
					throw new AggregateException("file: " + file, e);
				}
			}
		}
	}
}
