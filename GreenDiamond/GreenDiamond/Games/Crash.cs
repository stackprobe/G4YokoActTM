using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.Games
{
	public class Crash
	{
		// インスタンスの生成は CrashUtils で、

		public enum Kind_e
		{
			NONE = 1,
			POINT,
			CIRCLE,
			RECT,
		}

		public Kind_e Kind = Kind_e.POINT;
		public D2Point Pt = new D2Point(-1, 0);
		public double R = 0.0;
		public D4Rect Rect = null;

		public bool IsCrashed(Crash other)
		{
			return CrashUtils.IsCrashed(this, other);
		}
	}
}
