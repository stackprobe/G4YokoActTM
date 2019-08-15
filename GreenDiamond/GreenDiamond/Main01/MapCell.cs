using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;

namespace Charlotte.Main01
{
	public class MapCell
	{
		public bool Wall = false; // ? 壁
		public MapCellPicture MCPicture = null; // null == 画像無し
		public Enemy Enemy = null; // null = 敵無し
		public string EventName = ""; // "" == イベント無し
	}
}
