using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Tools;
using Charlotte.Common;

namespace Charlotte.Games
{
	public static class GameEdit
	{
		private static readonly D4Rect MenuRect = new D4Rect(0, 0, 300, DDConsts.Screen_H);

		private static bool InputWallFlag = true;
		private static bool InputTileFlag = true;
		private static bool InputEnemyFlag = false;

		private static bool DisplayWallFlag = false;
		public static bool DisplayTileFlag = true;
		private static bool DisplayEnemyFlag = true;

		private static bool Wall = true;
		private static int TileIndex = 0;
		private static int EnemyIndex = 0;

		private static int Rot;
		private static int NoRotFrame;

		private static bool HideMenu;

		public static void EachFrame()
		{
			I2Point pt = Map.ToTablePoint(DDMouse.X + DDGround.ICamera.X, DDMouse.Y + DDGround.ICamera.Y);
			MapCell cell = Game.I.Map.GetCell(pt, null);

			if (cell == null)
				return;

			if (DDUtils.IsOutOfScreen(new D2Point(DDMouse.X, DDMouse.Y)))
				return;

			HideMenu = 1 <= DDKey.GetInput(DX.KEY_INPUT_LCONTROL) || 1 <= DDKey.GetInput(DX.KEY_INPUT_RCONTROL);

			if (HideMenu || DDUtils.IsOut(new D2Point(DDMouse.X, DDMouse.Y), MenuRect))
			{
				if (1 <= DDMouse.L.GetInput())
				{
					if (InputWallFlag) cell.Wall = Wall;
					if (InputTileFlag) cell.Tile = MapTileManager.GetTile(MapTileManager.GetNames()[TileIndex]);
					if (InputEnemyFlag) cell.EnemyLoader = EnemyManager.GetEnemyLoader(EnemyManager.GetNames()[EnemyIndex]);
				}
				if (1 <= DDMouse.R.GetInput())
				{
					cell.Wall = false;
					cell.Tile = null;
					cell.EnemyLoader = null;
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

						case 3: DisplayWallFlag = flag; break;
						case 4: DisplayTileFlag = flag; break;
						case 5: DisplayEnemyFlag = flag; break;

						case 6: Wall = flag; break;

						case 7:
						case 8:
							IntoInputTileMode();
							break;

						case 9:
						case 10:
							IntoInputEnemyMode();
							break;
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
					case 7:
					case 8:
						if (MenuItemRot(ref TileIndex, MapTileManager.GetCount()))
						{
							IntoInputTileMode();
						}
						break;

					case 9:
					case 10:
						if (MenuItemRot(ref EnemyIndex, EnemyManager.GetCount()))
						{
							IntoInputEnemyMode();
						}
						break;
				}
			}

			if (DDKey.GetInput(DX.KEY_INPUT_C) == 1)
			{
				if (InputWallFlag) Wall = cell.Wall;
				if (InputTileFlag) TileIndex = cell.Tile == null ? 0 : MapTileManager.GetNames().IndexOf(cell.Tile.Name);
				if (InputEnemyFlag) EnemyIndex = cell.EnemyLoader == null ? 0 : EnemyManager.GetNames().IndexOf(cell.EnemyLoader.Name);

				if (TileIndex == -1) // 2bs
					TileIndex = 0;

				if (EnemyIndex == -1) // 2bs
					EnemyIndex = 0;
			}
			if (DDKey.GetInput(DX.KEY_INPUT_S) == 1)
			{
				MapLoader.SaveToLastLoadedFile(Game.I.Map);
			}
		}

		private static void IntoInputTileMode()
		{
			InputWallFlag = true;
			InputTileFlag = true;
			InputEnemyFlag = false;
		}

		private static void IntoInputEnemyMode()
		{
			InputWallFlag = false;
			InputTileFlag = false;
			InputEnemyFlag = true;
		}

		private const int MIR_INC_ROT = 1;

		private static bool MenuItemRot(ref int itemIndex, int itemCount)
		{
			bool changed = false;

			if (itemCount <= 0)
				throw new DDError(); // 2bs

			while (Rot <= -MIR_INC_ROT)
			{
				changed = true;
				itemIndex--;
				Rot += MIR_INC_ROT;
			}
			while (MIR_INC_ROT <= Rot)
			{
				changed = true;
				itemIndex++;
				Rot -= MIR_INC_ROT;
			}
			DDUtils.Range(ref itemIndex, 0, itemCount - 1);
			return changed;
		}

		public static void Draw()
		{
			DrawMap();

			if (HideMenu == false)
				DrawMenu();
		}

		private static void DrawMenu()
		{
			DDDraw.SetAlpha(0.5);
			DDDraw.SetBright(0.0, 0.3, 0.6);
			DDDraw.DrawRect(DDGround.GeneralResource.WhiteBox, MenuRect.L, MenuRect.T, MenuRect.W, MenuRect.H);
			DDDraw.Reset();

			DDPrint.SetPrint();
			DDPrint.PrintLine("INPUT WALL: " + InputWallFlag);
			DDPrint.PrintLine("INPUT TILE: " + InputTileFlag);
			DDPrint.PrintLine("INPUT ENEMY: " + InputEnemyFlag);

			DDPrint.PrintLine("DISPLAY WALL: " + DisplayWallFlag);
			DDPrint.PrintLine("DISPLAY TILE: " + DisplayTileFlag);
			DDPrint.PrintLine("DISPLAY ENEMY: " + DisplayEnemyFlag);

			DDPrint.PrintLine("WALL: " + Wall);
			DDPrint.PrintLine("TILE: " + TileIndex + " / " + MapTileManager.GetCount());
			DDPrint.PrintLine("TILE=[" + MapTileManager.GetNames()[TileIndex] + "]");
			DDPrint.PrintLine("ENEMY: " + EnemyIndex + " / " + EnemyManager.GetCount());
			DDPrint.PrintLine("ENEMY=[" + EnemyManager.GetNames()[EnemyIndex] + "]");

			I2Point pt = Map.ToTablePoint(DDMouse.X + DDGround.ICamera.X, DDMouse.Y + DDGround.ICamera.Y);
			MapCell cell = Game.I.Map.GetCell(pt);

			DDPrint.PrintLine("CURSOR: " + pt.X + ", " + pt.Y);
			DDPrint.PrintLine("CURSOR WALL: " + cell.Wall);
			DDPrint.PrintLine("CURSOR TILE=[" + (cell.Tile == null ? "" : cell.Tile.Name) + "]");
			DDPrint.PrintLine("CURSOR ENEMY=[" + (cell.EnemyLoader == null ? "" : cell.EnemyLoader.Name) + "]");

			DDPrint.PrintLine("");
			DDPrint.PrintLine("キー操作");
			DDPrint.PrintLine("C = COPY");
			DDPrint.PrintLine("S = SAVE");
		}

		private static void DrawMap()
		{
			int w = Game.I.Map.W;
			int h = Game.I.Map.H;

			int camL = DDGround.ICamera.X;
			int camT = DDGround.ICamera.Y;
			int camR = camL + DDConsts.Screen_W;
			int camB = camT + DDConsts.Screen_H;

			for (int x = 0; x < w; x++)
			{
				for (int y = 0; y < h; y++)
				{
					int mapTileX = x * MapTile.WH + MapTile.WH / 2;
					int mapTileY = y * MapTile.WH + MapTile.WH / 2;

					if (DDUtils.IsOut(new D2Point(mapTileX, mapTileY), new D4Rect(camL, camT, camR, camB), 100.0) == false) // マージン要調整
					{
						MapCell cell = Game.I.Map.GetCell(x, y);

						if (DisplayWallFlag && cell.Wall)
						{
							DDDraw.SetAlpha(0.3);
							DDDraw.SetBright(1.0, 0.5, 0.0);
							DDDraw.DrawBegin(DDGround.GeneralResource.WhiteBox, mapTileX - camL, mapTileY - camT);
							DDDraw.DrawSetSize(MapTile.WH, MapTile.WH);
							DDDraw.DrawEnd();
							DDDraw.Reset();
						}
						if (DisplayEnemyFlag && cell.EnemyLoader != null)
						{
							DDDraw.SetAlpha(0.3);
							DDDraw.SetBright(0.0, 0.5, 1.0);
							DDDraw.DrawBegin(DDGround.GeneralResource.WhiteBox, mapTileX - camL, mapTileY - camT);
							DDDraw.DrawSetSize(MapTile.WH, MapTile.WH);
							DDDraw.DrawEnd();
							DDDraw.Reset();

							DDPrint.SetBorder(new I3Color(0, 0, 255));
							DDPrint.SetPrint(mapTileX - camL - MapTile.WH / 2, mapTileY - camT - 8);
							DDPrint.Print(cell.EnemyLoader.Name);
							DDPrint.Reset();
						}
					}
				}
			}
		}
	}
}
