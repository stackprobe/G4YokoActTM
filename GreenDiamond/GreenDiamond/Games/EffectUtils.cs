using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Common;

namespace Charlotte.Games
{
	public static class EffectUtils
	{
		public static void 小爆発(double x, double y)
		{
			DDGround.EL.Add(EnumerableTools.Supplier(小爆発Seq(x, y)));
		}

		private static IEnumerable<bool> 小爆発Seq(double x, double y)
		{
			foreach (DDScene scene in DDSceneUtils.Create(5))
			{
				DDDraw.SetAlpha(0.7);
				DDDraw.SetBright(1.0, 0.5, 0.5);
				DDDraw.DrawBegin(DDGround.GeneralResource.WhiteCircle, x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
				DDDraw.DrawZoom(0.3 * scene.Rate);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}

		public static void 中爆発(double x, double y)
		{
			DDGround.EL.Add(EnumerableTools.Supplier(中爆発Seq(x, y)));
		}

		private static IEnumerable<bool> 中爆発Seq(double x, double y)
		{
			foreach (DDScene scene in DDSceneUtils.Create(10))
			{
				DDDraw.SetAlpha(0.7);
				DDDraw.SetBright(1.0, 0.6, 0.3);
				DDDraw.DrawBegin(DDGround.GeneralResource.WhiteCircle, x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
				DDDraw.DrawZoom(3.0 * scene.Rate);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}
	}
}
