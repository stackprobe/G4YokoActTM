using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;

namespace Charlotte.Game
{
	public class MapCell
	{
		public bool Wall; // ? 壁
		public GamePicture Picture = null; // null == 画像無し
		public Enemy Enemy = null; // null = 敵無し
		public string EventName = ""; // "" == イベント無し
	}
}
