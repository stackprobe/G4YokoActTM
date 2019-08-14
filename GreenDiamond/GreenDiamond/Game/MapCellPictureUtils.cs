using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;

namespace Charlotte.Game
{
	public static class MapCellPictureUtils
	{
		public static List<MapCellPicture> Pictures = new List<MapCellPicture>();

		public static MapCellPicture GetPicture(string name)
		{
			if (name == "")
				return null;

			return Pictures.First(v => v.Name == name);
		}
	}
}
