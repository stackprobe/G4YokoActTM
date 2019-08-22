using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Tools;
using Charlotte.Common;
using Charlotte.Game01.Map01;
using Charlotte.Game01.Map01.Tile01;

namespace Charlotte.Game01
{
	public static class EditMenu
	{
		public static void EachFrame()
		{
			I2Point pt = Map.ToTablePoint(DDMouse.X + DDGround.ICamera.X, DDMouse.Y + DDGround.ICamera.Y);
			MapCell cell = Game.I.Map.GetCell(pt, null);

			if (cell == null)
				return;

			if (1 <= DDMouse.Get_L())
			{
				cell.Wall = true;
				cell.Tile = MapTileUtils.GetTile("Wall"); // kari
			}
			if (1 <= DDMouse.Get_R())
			{
				cell.Wall = false;
				cell.Tile = null;
			}

			if (DDKey.GetInput(DX.KEY_INPUT_S) == 1)
			{
				MapLoader.SaveToLastLoadedFile(Game.I.Map);
			}
		}

		public static void Draw()
		{
			// todo ???
		}
	}
}
