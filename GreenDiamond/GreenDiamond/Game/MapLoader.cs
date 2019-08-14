using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;

namespace Charlotte.Game
{
	public static class MapLoader
	{
		public static void Load(string file)
		{
			string[] lines = FileTools.TextToLines(Encoding.UTF8.GetString(GameResource.Load(file)));
			int c = 0;

			int w = int.Parse(lines[c++]);
			int h = int.Parse(lines[c++]);

			if (w < 1 || IntTools.IMAX < w) throw new GameError();
			if (h < 1 || IntTools.IMAX < h) throw new GameError();

			Map.Init(w, h);

			for (int x = 0; x < w; x++)
			{
				for (int y = 0; y < h; y++)
				{
					if (lines.Length <= c)
						goto endLoad;

					MapCell cell = Map.GetCell(x, y);
					string[] tokens = lines[c++].Split('\t');
					int d = 0;

					cell.Wall = int.Parse(tokens[d++]) != 0;
					cell.MCPicture = MapCellPictureUtils.GetPicture(tokens[d++]);
					cell.Enemy = EnemyUtils.GetEnemy(tokens[d++]);
					cell.EventName = tokens[d++];
				}
			}
		endLoad:
			;
		}

		public static void Save(string file)
		{
			List<string> lines = new List<string>();
			int w = Map.Get_W();
			int h = Map.Get_H();

			lines.Add("" + w);
			lines.Add("" + h);

			for (int x = 0; x < w; x++)
			{
				for (int y = 0; y < h; y++)
				{
					MapCell cell = Map.GetCell(x, y);
					List<string> tokens = new List<string>();

					tokens.Add("" + (cell.Wall ? 1 : 0));
					tokens.Add(cell.MCPicture == null ? "" : cell.MCPicture.Name);
					tokens.Add(cell.Enemy == null ? "" : cell.Enemy.Name);
					tokens.Add(cell.EventName);

					lines.Add(string.Join("\t", tokens));
				}
			}
			GameResource.Save(file, Encoding.UTF8.GetBytes(FileTools.LinesToText(lines.ToArray())));
		}
	}
}
