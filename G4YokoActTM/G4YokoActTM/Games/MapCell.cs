using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;

namespace Charlotte.Games
{
	public class MapCell
	{
		public bool Wall = false; // ? 壁
		public MapTile Tile = null; // null == 画像無し
		public EnemyLoader EnemyLoader = null; // null == 敵無し
	}
}
