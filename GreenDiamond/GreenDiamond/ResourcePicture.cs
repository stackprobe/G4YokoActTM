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
		public GamePicture Player = GamePictureLoaders.BgTrans(@"pata2-magic\free_pochitto.png");

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
					new[] { PlayerStands, PlayerTalks }[y][x] = new GameDerivationTable(Player, x * 208, 16 + y * 144, 94, 112, 2, 1, 96);

			PlayerShagami = GameDerivations.GetPicture(Player, 0, 304, 94, 112);

			{
				List<GameDerivationTable> buff = new List<GameDerivationTable>();

				for (int x = 0; x < 3; x++)
					buff.Add(new GameDerivationTable(Player, 112 + x * 208, 304, 94, 112, 2, 1, 96));

				{
					int c = 0;

					PlayerWalk = buff[c++];
					PlayerDash = buff[c++];
					PlayerStop = buff[c++];
				}
			}

			for (int x = 0; x < 3; x++)
				PlayerJump[x] = GameDerivations.GetPicture(Player, 736 + x * 112, 304, 94, 112);

			{
				List<GamePicture> buff = new List<GamePicture>();

				for (int x = 0; x < 2; x++)
					buff.Add(GameDerivations.GetPicture(Player, 736 + x * 112, 304, 94, 112));

				{
					int c = 0;

					PlayerAttack = buff[c++];
					PlayerAttackShagami = buff[c++];
				}
			}

			{
				List<GameDerivationTable> buff = new List<GameDerivationTable>();

				for (int x = 0; x < 2; x++)
					buff.Add(new GameDerivationTable(Player, 224 + x * 208, 448, 94, 112, 2, 1, 96));

				{
					int c = 0;

					PlayerAttackWalk = buff[c++];
					PlayerAttackDash = buff[c++];
				}
			}

			PlayerAttackJump = GameDerivations.GetPicture(Player, 640, 448, 94, 112);

			for (int x = 0; x < 8; x++)
				PlayerDamage[x] = GameDerivations.GetPicture(Player, 0 + x * 112, 592, 94, 112);

			new MapCellPicture(@"MapTile\Wall.png");
			new MapCellPicture(@"MapTile\Wall2.png");
		}
	}
}
