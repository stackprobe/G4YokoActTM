using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Tools;
using System.IO;

namespace Charlotte.Games
{
	public static class WallPictureManager
	{
		private static Dictionary<string, DDPicture> Pictures = DictionaryTools.CreateIgnoreCase<DDPicture>();

		public static DDPicture GetPicutre(string file)
		{
			file = Path.Combine("Wall", file);

			DDPicture ret;

			if (Pictures.ContainsKey(file))
			{
				ret = Pictures[file];
			}
			else
			{
				ret = DDPictureLoaders.Standard(file);
				Pictures.Add(file, ret);
			}
			return ret;
		}
	}
}
