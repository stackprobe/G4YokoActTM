using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;
using Charlotte.Main01;

namespace Charlotte
{
	public class ResourcePicture
	{
		public DDPicture Player = DDPictureLoaders.BgTrans(@"pata2-magic\free_pochitto.png");

		public DDDerivationTable[] PlayerStands = new DDDerivationTable[5];
		public DDDerivationTable[] PlayerTalks = new DDDerivationTable[5];
		public DDPicture PlayerShagami;
		public DDDerivationTable PlayerWalk;
		public DDDerivationTable PlayerDash;
		public DDDerivationTable PlayerStop;
		public DDPicture[] PlayerJump = new DDPicture[3];
		public DDPicture PlayerAttack;
		public DDPicture PlayerAttackShagami;
		public DDDerivationTable PlayerAttackWalk;
		public DDDerivationTable PlayerAttackDash;
		public DDPicture PlayerAttackJump;
		public DDPicture[] PlayerDamage = new DDPicture[8];

		public ResourcePicture()
		{
			for (int y = 0; y < 2; y++)
				for (int x = 0; x < 5; x++)
					new[] { PlayerStands, PlayerTalks }[y][x] = new DDDerivationTable(Player, x * 208, 16 + y * 144, 94, 112, 2, 1, 96);

			PlayerShagami = DDDerivations.GetPicture(Player, 0, 304, 94, 112);

			{
				List<DDDerivationTable> buff = new List<DDDerivationTable>();

				for (int x = 0; x < 3; x++)
					buff.Add(new DDDerivationTable(Player, 112 + x * 208, 304, 94, 112, 2, 1, 96));

				{
					int c = 0;

					PlayerWalk = buff[c++];
					PlayerDash = buff[c++];
					PlayerStop = buff[c++];
				}
			}

			for (int x = 0; x < 3; x++)
				PlayerJump[x] = DDDerivations.GetPicture(Player, 736 + x * 112, 304, 94, 112);

			{
				List<DDPicture> buff = new List<DDPicture>();

				for (int x = 0; x < 2; x++)
					buff.Add(DDDerivations.GetPicture(Player, 736 + x * 112, 304, 94, 112));

				{
					int c = 0;

					PlayerAttack = buff[c++];
					PlayerAttackShagami = buff[c++];
				}
			}

			{
				List<DDDerivationTable> buff = new List<DDDerivationTable>();

				for (int x = 0; x < 2; x++)
					buff.Add(new DDDerivationTable(Player, 224 + x * 208, 448, 94, 112, 2, 1, 96));

				{
					int c = 0;

					PlayerAttackWalk = buff[c++];
					PlayerAttackDash = buff[c++];
				}
			}

			PlayerAttackJump = DDDerivations.GetPicture(Player, 640, 448, 94, 112);

			for (int x = 0; x < 8; x++)
				PlayerDamage[x] = DDDerivations.GetPicture(Player, 0 + x * 112, 592, 94, 112);

			new MapCellPicture(@"MapTile\Wall.png");
			new MapCellPicture(@"MapTile\Wall2.png");
		}
	}
}
