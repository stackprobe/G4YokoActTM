﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Common;
using Charlotte.Tools;
using Charlotte.Tests;
using Charlotte.Main01;
using Charlotte.Game01.Map01.Tile01;
using Charlotte.Game01.Enemy01;
using Charlotte.Game01;

namespace Charlotte
{
	public class Program2
	{
		public void Main2()
		{
			try
			{
				Main3();
			}
			catch (Exception e)
			{
				ProcMain.WriteLog(e);
			}
		}

		private void Main3()
		{
			DDAdditionalEvents.Ground_INIT = () =>
			{
				//GameGround.RO_MouseDispMode = true;
			};

			DDAdditionalEvents.PostGameStart = () =>
			{
				// Font >

				//GameFontRegister.Add(@"Font\Genkai-Mincho-font\genkai-mincho.ttf");

				// < Font

				Ground.I = new Ground();

				// *.INIT
				{
					EnemyManager.INIT();
					MapTileManager.INIT();
				}
			};

			DDAdditionalEvents.Save = lines =>
			{
				lines.Add(DateTime.Now.ToString()); // Dummy
				lines.Add(DateTime.Now.ToString()); // Dummy
				lines.Add(DateTime.Now.ToString()); // Dummy

				// 新しい項目をここへ追加...
			};

			DDAdditionalEvents.Load = lines =>
			{
				int c = 0;

				DDUtils.Noop(lines[c++]); // Dummy
				DDUtils.Noop(lines[c++]); // Dummy
				DDUtils.Noop(lines[c++]); // Dummy

				// 新しい項目をここへ追加...
			};

			DDMain2.Perform(Main4);
		}

		private void Main4()
		{
			//new Test0001().Test01();
			new TitleMenu().Perform();
		}
	}
}
