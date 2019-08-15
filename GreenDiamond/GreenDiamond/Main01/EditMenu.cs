using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;
using DxLibDLL;

namespace Charlotte.Main01
{
	public static class EditMenu
	{
		public static MapCellPicture MCPicture;
		public static Enemy Enemy;
		public static string EventName;

		public static void EachFrame()
		{
			I2Point pt = GameUtils.PointToMapCellPoint(DDMouse.X + DDGround.ICamera.X, DDMouse.Y + DDGround.ICamera.Y);
			MapCell cell = Map.GetCell(pt, null);

			if (cell == null)
				return;

			if (1 <= DDMouse.Get_L())
			{
				cell.Wall = true;
				cell.MCPicture = MapCellPictureUtils.GetPicture("Wall"); // kari
			}
			if (1 <= DDMouse.Get_R())
			{
				cell.Wall = false;
				cell.MCPicture = null;
			}

			if (DDKey.GetInput(DX.KEY_INPUT_S) == 1)
			{
				MapLoader.SaveToLastLoadedFile();
			}
		}

		public static void Draw()
		{
			// todo ???
		}
	}
}
