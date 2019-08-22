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

namespace Charlotte.Game01
{
	public static class EditMode
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

				if (1 <= DDMouse.L.GetInput())
				{
					switch (cursorMenuItemIndex)
					{
						case 0: InputWallFlag = true; break;
						case 1: InputTileFlag = true; break;
						case 2: InputEnemyFlag = true; break;
						case 3: InputEventNameFlag = true; break;

						case 4: DisplayWallFlag = true; break;
						case 5: DisplayTileFlag = true; break;
						case 6: DisplayEnemyFlag = true; break;
						case 7: DisplayEventNameFlag = true; break;

						case 8: Wall = true; break;
					}
				}
				if (1 <= DDMouse.R.GetInput())
				{
					switch (cursorMenuItemIndex)
					{
						case 0: InputWallFlag = false; break;
						case 1: InputTileFlag = false; break;
						case 2: InputEnemyFlag = false; break;
						case 3: InputEventNameFlag = false; break;

						case 4: DisplayWallFlag = false; break;
						case 5: DisplayTileFlag = false; break;
						case 6: DisplayEnemyFlag = false; break;
						case 7: DisplayEventNameFlag = false; break;

						case 8: Wall = false; break;
					}
				}
			}

			if (DDKey.GetInput(DX.KEY_INPUT_S) == 1)
			{
				MapLoader.SaveToLastLoadedFile(Game.I.Map);
			}
		}

		public static void Draw()
		{
			DDDraw.SetAlpha(0.5);
			DDDraw.SetBright(0.0, 0.3, 0.6);
			DDDraw.DrawRect(DDGround.GeneralResource.WhiteBox, MenuRect.L, MenuRect.T, MenuRect.W, MenuRect.H);
			DDDraw.Reset();

			DDPrint.SetPrint(); DDPrint.Print("INPUT WALL: " + InputWallFlag);
			DDPrint.PrintRet(); DDPrint.Print("INPUT TILE: " + InputTileFlag);
			DDPrint.PrintRet(); DDPrint.Print("INPUT ENEMY: " + InputEnemyFlag);
			DDPrint.PrintRet(); DDPrint.Print("INPUT EVENT-NAME: " + InputEventNameFlag);

			DDPrint.PrintRet(); DDPrint.Print("DISPLAY WALL: " + DisplayWallFlag);
			DDPrint.PrintRet(); DDPrint.Print("DISPLAY TILE: " + DisplayTileFlag);
			DDPrint.PrintRet(); DDPrint.Print("DISPLAY ENEMY: " + DisplayEnemyFlag);
			DDPrint.PrintRet(); DDPrint.Print("DISPLAY EVENT-NAME: " + DisplayEventNameFlag);

			DDPrint.PrintRet(); DDPrint.Print("WALL: " + Wall);
			DDPrint.PrintRet(); DDPrint.Print("TILE: " + TileIndex + " / " + MapTileUtils.Tiles.Count);
			DDPrint.PrintRet(); DDPrint.Print("ENEMY: " + EnemyIndex + " / " + EnemyUtils.Enemies.Count);
			DDPrint.PrintRet(); DDPrint.Print("EVENT-NAME=[" + EventName + "]");
		}
	}
}
