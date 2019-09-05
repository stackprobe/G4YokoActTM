using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;

namespace Charlotte.Games
{
	public static class GameUtils
	{
		public static DDScene SceneIncrement(ref int counter, int denom)
		{
			if (1 <= counter)
			{
				if (counter <= denom)
				{
					int numer = counter++;

					return new DDScene()
					{
						Numer = numer,
						Denom = denom,
						Rate = (double)numer / denom,
					};
				}
				else
					counter = 0;
			}
			return null;
		}
	}
}
