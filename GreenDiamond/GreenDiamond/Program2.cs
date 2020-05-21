using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Common;
using Charlotte.Tools;
using Charlotte.Tests;
using Charlotte.Games;
using Charlotte.Worlds;
using Charlotte.Tests.Games;
using Charlotte.Tests.Worlds;
using Charlotte.TitleMenus;

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
					WorldMap.INIT();
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
			if (ProcMain.ArgsReader.ArgIs("/D"))
			{
				Main4_Debug();
			}
			else
			{
				Main4_Release();
			}
		}

		private void Main4_Debug()
		{
			//new Test0001().Test01();
			//new TitleMenu().Perform();
			//new GameTest().Test01();
			new WorldTest().Test01();
		}

		private void Main4_Release()
		{
			new TitleMenu().Perform();
		}
	}
}
