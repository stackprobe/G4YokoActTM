﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Game
{
	public class Enemy
	{
		public string Name;

		public Enemy()
		{
			// TODO

			EnemyUtils.Enemies.Add(this);
		}
	}
}
