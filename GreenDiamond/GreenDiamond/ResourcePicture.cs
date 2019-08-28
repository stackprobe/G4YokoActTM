using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Common;

namespace Charlotte
{
	public class ResourcePicture
	{
		public DDPicture Player = DDPictureLoaders.BgTrans(@"pata2-magic\free_pochitto.png");

		public DDPicture[][] PlayerStands = new DDPicture[5][];
		public DDPicture[][] PlayerTalks = new DDPicture[5][];
		public DDPicture PlayerShagami;
		public DDPicture[] PlayerWalk = new DDPicture[2];
		public DDPicture[] PlayerDash = new DDPicture[2];
		public DDPicture[] PlayerStop = new DDPicture[2];
		public DDPicture[] PlayerJump = new DDPicture[3];
		public DDPicture PlayerAttack;
		public DDPicture PlayerAttackShagami;
		public DDPicture[] PlayerAttackWalk = new DDPicture[2];
		public DDPicture[] PlayerAttackDash = new DDPicture[2];
		public DDPicture PlayerAttackJump;
		public DDPicture[] PlayerDamage = new DDPicture[8];

		public ResourcePicture()
		{
			for (int x = 0; x < 5; x++)
			{
				PlayerStands[x] = new DDPicture[2];
				PlayerTalks[x] = new DDPicture[2];

				for (int y = 0; y < 2; y++)
					for (int k = 0; k < 2; k++)
						new[] { PlayerStands, PlayerTalks }[y][x][k] = DDDerivations.GetPicture(Player, x * 208 + k * 96, 16 + y * 144, 94, 112);
			}
			PlayerShagami = DDDerivations.GetPicture(Player, 0, 304, 94, 112);

			for (int x = 0; x < 3; x++)
				for (int k = 0; k < 2; k++)
					new[] { PlayerWalk, PlayerDash, PlayerStop }[x][k] = DDDerivations.GetPicture(Player, 112 + x * 208 + k * 96, 304, 94, 112);

			for (int x = 0; x < 3; x++)
				PlayerJump[x] = DDDerivations.GetPicture(Player, 736 + x * 112, 304, 94, 112);

			{
				List<DDPicture> buff = new List<DDPicture>();

				for (int x = 0; x < 2; x++)
					buff.Add(DDDerivations.GetPicture(Player, x * 112, 448, 94, 112));

				{
					int c = 0;

					PlayerAttack = buff[c++];
					PlayerAttackShagami = buff[c++];
				}
			}

			for (int x = 0; x < 2; x++)
				for (int k = 0; k < 2; k++)
					new[] { PlayerAttackWalk, PlayerAttackDash }[x][k] = DDDerivations.GetPicture(Player, 224 + x * 208 + k * 96, 448, 94, 112);

			PlayerAttackJump = DDDerivations.GetPicture(Player, 640, 448, 94, 112);

			for (int x = 0; x < 8; x++)
				PlayerDamage[x] = DDDerivations.GetPicture(Player, 0 + x * 112, 592, 94, 112);
		}
	}
}
