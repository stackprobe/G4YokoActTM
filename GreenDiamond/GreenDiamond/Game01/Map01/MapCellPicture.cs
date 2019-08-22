using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Common;

namespace Charlotte.Game01.Map01
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
