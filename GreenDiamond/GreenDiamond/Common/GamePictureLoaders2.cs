﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public static class GamePictureLoaders2
	{
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static GamePicture Wrapper(int handle, int w, int h)
		{
			GamePicture.PictureInfo info = new GamePicture.PictureInfo()
			{
				Handle = handle,
				W = w,
				H = h,
			};

			return new GamePicture(() => info, v => { }, v => { });
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static GamePicture Wrapper(int handle, I2Size size)
		{
			return Wrapper(handle, size.W, size.H);
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public static GamePicture Wrapper(GameSubScreen subScreen)
		{
			return Wrapper(subScreen.GetHandle(), subScreen.GetSize());
		}
	}
}
