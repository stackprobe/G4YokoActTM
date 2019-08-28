using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Common;

namespace Charlotte.Games
{
	public class MapTile
	{
		public const int WH = 32; // タイル(マップセル)の幅と高さ [ピクセル]

		public string Name;
		public DDPicture Picture;
	}
}
