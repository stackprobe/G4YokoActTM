using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;

namespace Charlotte.Main01
{
	public static class MapLoader
	{
		public static string LastLoadedFile = null; // null == 未読み込み

		public static Map Load(string file)
		{
			LastLoadedFile = file;

			string[] lines = FileTools.TextToLines(Encoding.UTF8.GetString(DDResource.Load(file)));
			int c = 0;

			int w = int.Parse(lines[c++]);
			int h = int.Parse(lines[c++]);

			if (w < 1 || IntTools.IMAX < w) throw new DDError();
			if (h < 1 || IntTools.IMAX < h) throw new DDError();

			Map map = new Map(w, h);

			for (int x = 0; x < w; x++)
			{
				for (int y = 0; y < h; y++)
				{
					if (lines.Length <= c)
						goto endLoad;

					MapCell cell = map.GetCell(x, y);
					var tokens = new BluffList<string>(lines[c++].Split('\t')).FreeRange("");
					int d = 0;

					cell.Wall = int.Parse(tokens[d++]) != 0;
					cell.MCPicture = MapCellPictureUtils.GetPicture(tokens[d++]);
					cell.Enemy = EnemyUtils.GetEnemy(tokens[d++]);
					cell.EventName = tokens[d++];

					// 新しい項目をここへ追加...
				}
			}
		endLoad:
			return map;
		}

		public static void SaveToLastLoadedFile(Map map)
		{
			if (LastLoadedFile == null)
				throw new DDError();

			Save(map, LastLoadedFile);
		}

		public static void Save(Map map, string file)
		{
			List<string> lines = new List<string>();
			int w = map.W;
			int h = map.H;

			lines.Add("" + w);
			lines.Add("" + h);

			for (int x = 0; x < w; x++)
			{
				for (int y = 0; y < h; y++)
				{
					MapCell cell = map.GetCell(x, y);
					List<string> tokens = new List<string>();

					tokens.Add("" + (cell.Wall ? 1 : 0));
					tokens.Add(cell.MCPicture == null ? "" : cell.MCPicture.Name);
					tokens.Add(cell.Enemy == null ? "" : cell.Enemy.Name);
					tokens.Add(cell.EventName);

					// 新しい項目をここへ追加...

					lines.Add(string.Join("\t", tokens).TrimEnd());
				}
			}
			DDResource.Save(file, Encoding.UTF8.GetBytes(FileTools.LinesToText(lines.ToArray())));
		}
	}
}
