using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;

namespace Charlotte.Games
{
	public static class EffectUtils
	{
		public static void 爆発(double x, double y)
		{
			DDGround.EL.Add(new DDIEnumerableTask(爆発Seq(x, y), () => { }));
		}

		private static IEnumerable<bool> 爆発Seq(double x, double y)
		{
			foreach (DDScene scene in DDSceneUtils.Create(5))
			{
				DDDraw.SetBright(1.0, 0.5, 0.5);
				DDDraw.DrawBegin(DDGround.GeneralResource.WhiteCircle, x - DDGround.ICamera.X, y - DDGround.ICamera.Y);
				DDDraw.DrawZoom(0.3 * scene.Rate);
				DDDraw.DrawEnd();
				DDDraw.Reset();

				yield return true;
			}
		}
	}
}
