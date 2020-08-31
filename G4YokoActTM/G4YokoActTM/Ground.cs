﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;

namespace Charlotte
{
	public class Ground
	{
		public static Ground I;

		public ResourceMusic Music = new ResourceMusic();
		public ResourcePicture Picture = new ResourcePicture();
		public ResourceSE SE = new ResourceSE();

		public DDSubScreen FrontScreen = new DDSubScreen(DDConsts.Screen_W, DDConsts.Screen_H, true);
	}
}
