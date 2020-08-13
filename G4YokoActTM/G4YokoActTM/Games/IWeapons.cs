using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.Games
{
	public static class IWeapons
	{
		public static IWeapon Load(IWeapon weapon, double x, double y, bool facingLeft)
		{
			return Load(weapon, new D2Point(x, y), facingLeft);
		}

		public static IWeapon Load(IWeapon weapon, D2Point pt, bool facingLeft)
		{
			weapon.Loaded(pt, facingLeft);
			return weapon;
		}
	}
}
