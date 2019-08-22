using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.Game01.Crash01
{
	public class Crash
	{
		public enum Kind_e
		{
			NONE = 1,
			POINT,
			CIRCLE,
			RECT,
			WHOLE,
		}

		public Kind_e Kind = Kind_e.NONE;
		public D2Point Pt = null;
		public double R = 0.0;
		public D4Rect Rect = null;

		public bool IsCrashed(Crash other)
		{
			return CrashUtils.IsCrashed(this, other);
		}
	}
}
