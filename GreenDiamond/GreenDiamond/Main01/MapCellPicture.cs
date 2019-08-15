using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using System.IO;

namespace Charlotte.Main01
{
	public class MapCellPicture
	{
		public string Name;
		public DDPicture Picture;

		public MapCellPicture(string file)
		{
			this.Name = Path.GetFileNameWithoutExtension(file);
			this.Picture = DDPictureLoaders.Standard(file);

			MapCellPictureUtils.Pictures.Add(this);
		}
	}
}
