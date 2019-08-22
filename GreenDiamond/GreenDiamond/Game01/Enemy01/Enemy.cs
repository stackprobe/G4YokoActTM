using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Game01.Map01;
using Charlotte.Tools;

namespace Charlotte.Game01.Enemy01
{
	public class Enemy : IDisposable
	{
		public string Name;

		public MapCell HomeCell;
		public I2Point HomeCellTablePoint;

		public IEnemySpec Spec;

		public void Created()
		{
			this.Spec.Created(this);
		}

		public void Dispose()
		{
			this.Spec.Destroy(this);
		}

		public bool EachFrame() // ret: ? ! 消滅
		{
			return this.Spec.EachFrame(this);
		}

		public void Draw()
		{
			this.Spec.Draw(this);
		}
	}
}
