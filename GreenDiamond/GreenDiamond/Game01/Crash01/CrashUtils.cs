using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Common;

namespace Charlotte.Game01.Crash01
{
	public static class CrashUtils
	{
		public static Crash None()
		{
			return new Crash();
		}

		public static Crash Point(D2Point pt)
		{
			return new Crash()
			{
				Kind = Crash.Kind_e.POINT,
				Pt = pt,
			};
		}

		public static Crash Circle(D2Point pt, double r)
		{
			return new Crash()
			{
				Kind = Crash.Kind_e.CIRCLE,
				Pt = pt,
				R = r,
			};
		}

		public static Crash Rect_CenterSize(D2Point centerPt, D2Size size)
		{
			return Rect(new D4Rect(centerPt.X - size.W / 2.0, centerPt.Y - size.H / 2.0, size.W, size.H));
		}

		public static Crash Rect(D4Rect rect)
		{
			return new Crash()
			{
				Kind = Crash.Kind_e.RECT,
				Rect = rect,
			};
		}

		public static bool IsCrashed(Crash a, Crash b)
		{
			if ((int)b.Kind < (int)a.Kind)
			{
				Crash tmp = a;
				a = b;
				b = tmp;
			}
			if (a.Kind == Crash.Kind_e.NONE)
				return false;

			if (a.Kind == Crash.Kind_e.POINT)
			{
				if (b.Kind == Crash.Kind_e.POINT)
					return false;

				if (b.Kind == Crash.Kind_e.CIRCLE)
					return DDUtils.IsCrashed_Circle_Point(b.Pt, b.R, a.Pt);

				if (b.Kind == Crash.Kind_e.RECT)
					return DDUtils.IsCrashed_Rect_Point(b.Rect, a.Pt);

				return true; // b is WHOLE
			}
			if (a.Kind == Crash.Kind_e.CIRCLE)
			{
				if (b.Kind == Crash.Kind_e.CIRCLE)
					return DDUtils.IsCrashed_Circle_Circle(a.Pt, a.R, b.Pt, b.R);

				if (b.Kind == Crash.Kind_e.RECT)
					return DDUtils.IsCrashed_Circle_Rect(a.Pt, a.R, b.Rect);

				return true; // b is WHOLE
			}
			if (a.Kind == Crash.Kind_e.RECT)
			{
				if (b.Kind == Crash.Kind_e.RECT)
					return DDUtils.IsCrashed_Rect_Rect(a.Rect, b.Rect);

				return true; // b is WHOLE
			}
			return true; // a, b are WHOLE
		}
	}
}
