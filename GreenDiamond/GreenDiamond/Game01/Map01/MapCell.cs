using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Game01.Enemy01;
using Charlotte.Game01.Map01.Tile01;

namespace Charlotte.Game01.Map01
{
	public class MapCell
	{
		public bool Wall = false; // ? 壁
		public MapTile Tile = null; // null == 画像無し
		public Enemy Enemy = null; // null = 敵無し
		public string EventName = ""; // "" == イベント無し
	}
}
