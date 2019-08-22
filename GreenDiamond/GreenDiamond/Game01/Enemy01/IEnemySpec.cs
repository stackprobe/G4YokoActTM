using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Game01.Map01.Tile01;
using Charlotte.Tools;

namespace Charlotte.Game01.Enemy01
{
	public interface IEnemySpec
	{
		void Created(Enemy enemy);
		void Destroy(Enemy enemy);
		bool EachFrame(Enemy enemy); // ret: ? ! 消滅
		void Draw(Enemy enemy);
	}
}
