using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.Game3Common
{
	//
	//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	//
	public class Crash
	{
		// インスタンスの生成は CrashUtils で、

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public enum Kind_e
		{
			NONE = 1,
			POINT,
			CIRCLE,
			RECT,
		}

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public Kind_e Kind = Kind_e.POINT;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public D2Point Pt = new D2Point(-1, 0);
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public double R = 0.0;
		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public D4Rect Rect = null;

		//
		//	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
		//
		public bool IsCrashed(Crash other)
		{
			return CrashUtils.IsCrashed(this, other);
		}
	}
}
