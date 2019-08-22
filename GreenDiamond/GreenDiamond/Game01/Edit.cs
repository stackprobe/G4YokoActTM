using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Tools;
using Charlotte.Common;
using Charlotte.Game01.Map01;
using Charlotte.Game01.Map01.Tile01;
using Charlotte.Game01.Enemy01;
using Charlotte.Sub01;

namespace Charlotte.Game01
{
	public static class Edit
	{
		public static readonly D4Rect MenuRect = new D4Rect(0, 0, 300, DDConsts.Screen_H);

		public static bool InputWallFlag = true;
		public static bool InputTileFlag = true;
		public static bool InputEnemyFlag = true;
		public static bool InputEventNameFlag = true;

		public static bool DisplayWallFlag = false;
		public static bool DisplayTileFlag = true;
		public static bool DisplayEnemyFlag = true;
		public static bool DisplayEventNameFlag = false;

		public static bool Wall = true;
		public static int TileIndex = 0;
		public static int EnemyIndex = 0;
		public static string EventName = "";

		public static int Rot;
		public static int NoRotFrame;

		public static void EachFrame()
		{
			I2Point pt = Map.ToTablePoint(DDMouse.X + DDGround.ICamera.X, DDMouse.Y + DDGround.ICamera.Y);
			MapCell cell = Game.I.Map.GetCell(pt, null);

			if (cell == null)
				return;

			if (DDUtils.IsOutOfScreen(new D2Point(DDMouse.X, DDMouse.Y)))
				return;

			if (DDUtils.IsOut(new D2Point(DDMouse.X, DDMouse.Y), MenuRect))
			{
				if (1 <= DDMouse.L.GetInput())
				{
					cell.Wall = true;
					cell.Tile = MapTileUtils.GetTile("Wall"); // kari
				}
				if (1 <= DDMouse.R.GetInput())
				{
					cell.Wall = false;
					cell.Tile = null;
				}
			}
			else
			{
				int cursorMenuItemIndex = DDMouse.Y / 16;

				bool l = 1 <= DDMouse.L.GetInput();
				bool r = 1 <= DDMouse.R.GetInput();

				if (l || r)
				{
					bool flag = l;

					switch (cursorMenuItemIndex)
					{
						case 0: InputWallFlag = flag; break;
						case 1: InputTileFlag = flag; break;
						case 2: InputEnemyFlag = flag; break;
						case 3: InputEventNameFlag = flag; break;

						case 4: DisplayWallFlag = flag; break;
						case 5: DisplayTileFlag = flag; break;
						case 6: DisplayEnemyFlag = flag; break;
						case 7: DisplayEventNameFlag = flag; break;

						case 8: Wall = flag; break;
					}
				}

				Rot += DDMouse.Rot;

				if (DDMouse.Rot == 0)
					NoRotFrame++;
				else
					NoRotFrame = 0;

				if (90 < NoRotFrame)
					Rot = 0;

				switch (cursorMenuItemIndex)
				{
					case 9: MenuItemRot(ref TileIndex, MapTileUtils.Tiles.Count); break;
					case 10: MenuItemRot(ref EnemyIndex, EnemyUtils.Enemies.Count); break;
				}

				if (DDMouse.L.GetInput() == 1)
				{
					switch (cursorMenuItemIndex)
					{
						case 11:
							EventName = InputStringSub.InputString("EVENT-NAME", EventName, 30);
							break;
					}
				}
			}

			if (DDKey.GetInput(DX.KEY_INPUT_S) == 1)
			{
				MapLoader.SaveToLastLoadedFile(Game.I.Map);
			}
		}

		private const int MIR_INC_ROT = 3;

		private static void MenuItemRot(ref int itemIndex, int itemCount)
		{
			if (itemCount <= 0)
				throw new DDError(); // 2bs

			while (Rot <= -MIR_INC_ROT)
			{
				itemIndex--;
				Rot += MIR_INC_ROT;
			}
			while (MIR_INC_ROT <= Rot)
			{
				itemIndex++;
				Rot -= MIR_INC_ROT;
			}
			DDUtils.Range(ref itemIndex, 0, itemCount - 1);
		}

		public static void Draw()
		{
			DDDraw.SetAlpha(0.5);
			DDDraw.SetBright(0.0, 0.3, 0.6);
			DDDraw.DrawRect(DDGround.GeneralResource.WhiteBox, MenuRect.L, MenuRect.T, MenuRect.W, MenuRect.H);
			DDDraw.Reset();

			DDPrint.SetPrint();
			DDPrint.PrintLine("INPUT WALL: " + InputWallFlag);
			DDPrint.PrintLine("INPUT TILE: " + InputTileFlag);
			DDPrint.PrintLine("INPUT ENEMY: " + InputEnemyFlag);
			DDPrint.PrintLine("INPUT EVENT-NAME: " + InputEventNameFlag);

			DDPrint.PrintLine("DISPLAY WALL: " + DisplayWallFlag);
			DDPrint.PrintLine("DISPLAY TILE: " + DisplayTileFlag);
			DDPrint.PrintLine("DISPLAY ENEMY: " + DisplayEnemyFlag);
			DDPrint.PrintLine("DISPLAY EVENT-NAME: " + DisplayEventNameFlag);

			DDPrint.PrintLine("WALL: " + Wall);
			DDPrint.PrintLine("TILE: " + TileIndex + " / " + MapTileUtils.Tiles.Count);
			DDPrint.PrintLine("ENEMY: " + EnemyIndex + " / " + EnemyUtils.Enemies.Count);
			DDPrint.PrintLine("EVENT-NAME=[" + EventName + "]");

			I2Point pt = Map.ToTablePoint(DDMouse.X + DDGround.ICamera.X, DDMouse.Y + DDGround.ICamera.Y);
			MapCell cell = Game.I.Map.GetCell(pt);

			DDPrint.PrintLine("CURSOR: " + pt.X + ", " + pt.Y);
			DDPrint.PrintLine("CURSOR WALL: " + cell.Wall);
			DDPrint.PrintLine("CURSOR TILE: " + (cell.Tile == null ? "" : cell.Tile.Name));
			DDPrint.PrintLine("CURSOR ENEMY: " + (cell.Enemy == null ? "" : cell.Enemy.Name));
			DDPrint.PrintLine("CURSOR EVENT-NAME=[" + cell.EventName + "]");
		}
	}
}
