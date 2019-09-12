using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games;
using Charlotte.Tools;
using Charlotte.Common;
using System.IO;

namespace Charlotte.Worlds
{
	public class World : IDisposable
	{
		public string MapFile = "t0001.txt"; // 開始マップファイル

		// <---- prm

		public static World I = null;

		public World()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		public void Perform()
		{
			Status status = new Status();

			for (; ; )
			{
				using (Game game = new Game())
				{
					game.Map = MapLoader.Load(Path.Combine("Map", this.MapFile));
					game.Status = status;
					game.Perform();

					switch (game.ExitDir)
					{
						case 4:
							this.Move(-1, 0);
							status.StartPointIndex = 6;
							break;

						case 6:
							this.Move(1, 0);
							status.StartPointIndex = 4;
							break;

						case 8:
							this.Move(0, -1);
							status.StartPointIndex = 2;
							break;

						case 2:
							this.Move(0, 1);
							status.StartPointIndex = 8;
							break;

						case 5:
							goto endLoop;

						default:
							throw null;
					}
				}
			}
		endLoop:
			;
		}

		private void Move(int xa, int ya)
		{
			I2Point pt = WorldMap.GetPoint(this.MapFile, new I2Point());

			if (pt.X == -1)
				throw new DDError("そんなマップありません。" + this.MapFile);

			pt.X += xa;
			pt.Y += ya;

			string file = WorldMap.GetFile(pt.X, pt.Y);

			if (file == null)
				throw new DDError("その方向にマップはありません。");

			this.MapFile = file;
		}
	}
}
