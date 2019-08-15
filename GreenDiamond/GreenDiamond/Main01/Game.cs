using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Main01
{
	public class Game
	{
		public static Game I = null;

		// Param {
		public double Prm_StartX = Map.Get_W() * Consts.MAP_TILE_WH / 2.0;
		public double Prm_StartY = Map.Get_H() * Consts.MAP_TILE_WH / 2.0;
		// }

		// Return {
		// }

		public class PlayerInfo
		{
			public double X;
			public double Y;
			public double YSpeed;
			public bool FacingLeft;
			public int MoveFrame;
			public bool MoveSlow; // ? 低速移動
			public int JumpFrame;
			public bool TouchGround;
			public int AirborneFrame;
		}

		public PlayerInfo Player = new PlayerInfo();

		public bool CamSlideMode; // ? モード中
		public int CamSlideCount;
		public int CamSlideX; // -1 ～ 1
		public int CamSlideY; // -1 ～ 1
	}
}
