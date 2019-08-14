using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Game;

namespace Charlotte
{
	public class ResourcePicture
	{
		public GamePicture Player = GamePictureLoaders.Standard(@"pata2-magic\free_pochitto.png");

		public GameDerivationTable[] PlayerStands = new GameDerivationTable[5];
		public GameDerivationTable[] PlayerTalks = new GameDerivationTable[5];
		public GamePicture PlayerShagami;
		public GameDerivationTable PlayerWalk;
		public GameDerivationTable PlayerDash;
		public GameDerivationTable PlayerStop;
		public GamePicture[] PlayerJump = new GamePicture[3];
		public GamePicture PlayerAttack;
		public GamePicture PlayerAttackShagami;
		public GameDerivationTable PlayerAttackWalk;
		public GameDerivationTable PlayerAttackDash;
		public GamePicture PlayerAttackJump;
		public GamePicture[] PlayerDamage = new GamePicture[8];

		public ResourcePicture()
		{
			for (int y = 0; y < 2; y++)
				for (int x = 0; x < 5; x++)
					new[] { PlayerStands, PlayerTalks }[y][x] = new GameDerivationTable(() => Player, x * 208, 16 + y * 144, 94, 112, 2, 1, 96);

			PlayerShagami = GameDerivations.GetPicture(() => Player, 0, 304, 94, 112);

			{
				int x = 0;

				PlayerWalk = new GameDerivationTable(() => Player, 112 + x++ * 208, 304, 94, 112, 2, 1, 96);
				PlayerDash = new GameDerivationTable(() => Player, 112 + x++ * 208, 304, 94, 112, 2, 1, 96);
				PlayerStop = new GameDerivationTable(() => Player, 112 + x++ * 208, 304, 94, 112, 2, 1, 96);
			}

			for (int x = 0; x < 3; x++)
				PlayerJump[x] = GameDerivations.GetPicture(() => Player, 736 + x * 112, 304, 94, 112);

			{
				int x = 0;

				PlayerAttack =
					GameDerivations.GetPicture(() => Player, 736 + x++ * 112, 304, 94, 112);
				PlayerAttackShagami =
					GameDerivations.GetPicture(() => Player, 736 + x++ * 112, 304, 94, 112);
			}

			{
				int x = 0;

				PlayerAttackWalk = new GameDerivationTable(() => Player, 224 + x++ * 208, 448, 94, 112, 2, 1, 96);
				PlayerAttackDash = new GameDerivationTable(() => Player, 224 + x++ * 208, 448, 94, 112, 2, 1, 96);
			}

			PlayerAttackJump = GameDerivations.GetPicture(() => Player, 640, 448, 94, 112);

			for (int x = 0; x < 8; x++)
				PlayerDamage[x] = GameDerivations.GetPicture(() => Player, 0 + x * 112, 592, 94, 112);

			new MapCellPicture(@"MapTile\Wall.png");
			new MapCellPicture(@"MapTile\Wall2.png");
		}
	}
}
