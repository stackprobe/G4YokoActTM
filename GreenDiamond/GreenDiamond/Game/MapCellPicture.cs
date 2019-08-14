using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using System.IO;

namespace Charlotte.Game
{
	public class MapCellPicture
	{
		public string Name;
		public GamePicture Picture;

		public MapCellPicture(string file)
		{
			this.Name = Path.GetFileNameWithoutExtension(file);
			this.Picture = GamePictureLoaders.Standard(file);

			MapCellPictureUtils.Pictures.Add(this);
		}
	}
}
