using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;
using DxLibDLL;

namespace Charlotte.Game
{
	public static class EditMenu
	{
		public static MapCellPicture MCPicture;
		public static Enemy Enemy;
		public static string EventName;

		public static void EachFrame()
		{
			I2Point pt = AGameUtils.PointToMapCellPoint(GameMouse.X + GameGround.ICamera.X, GameMouse.Y + GameGround.ICamera.Y);
			MapCell cell = Map.GetCell(pt, null);

			if (cell == null)
				return;

			if (1 <= GameMouse.Get_L())
			{
				cell.Wall = true;
				cell.MCPicture = MapCellPictureUtils.GetPicture("Wall");
			}
			if (1 <= GameMouse.Get_R())
			{
				cell.Wall = false;
				cell.MCPicture = null;
			}

			if (GameKey.GetInput(DX.KEY_INPUT_S) == 1 && MapLoader.LastLoadedFile != null)
			{
				MapLoader.Save(MapLoader.LastLoadedFile);
			}
		}

		public static void Draw()
		{
			// todo ???
		}
	}
}
