using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.ChestItems
{
	public class WorldgenPaste : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worldgen Paste");
			Tooltip.SetDefault("Development tool, NOT MEANT FOR GAMEPLAY\nGenerates the Planetarium Biome on your cursor\nActual biome name pending");
		}
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 34;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 3;
			item.value = 0;
			item.rare = 9;
			item.UseSound = SoundID.Item1;
		}
		public override void HoldItem(Player player)
		{
			player.rulerGrid = true;
		}
		public void PasteStructure()
		{
			Vector2 mousePos = Main.MouseWorld;
			Vector2 tileLocation = mousePos / 16f;
			SOTSWorldgenHelper.GenerateSkyArtifact((int)tileLocation.X, (int)tileLocation.Y, mod);
			for(int i = 0; i < 30; i ++)
			{
				tileLocation = mousePos / 16f;
				if (Main.rand.Next(2) == 0)
				{
					tileLocation.X += Main.rand.Next(300);
				}
				else
				{
					tileLocation.X -= Main.rand.Next(300);
				}

				if (Main.rand.Next(2) == 0)
				{
					tileLocation.Y += Main.rand.Next(50);
				}
				else
				{
					tileLocation.Y -= Main.rand.Next(36) + 50;
				}

				int extend = 0;
				while (!SOTSWorldgenHelper.GenerateArtifactIslands((int)tileLocation.X, (int)tileLocation.Y, i % 10, mod))
				{
					tileLocation = mousePos / 16f;
					if (Main.rand.Next(2) == 0)
					{
						tileLocation.X += Main.rand.Next(300 + extend);
					}
					else
					{
						tileLocation.X -= Main.rand.Next(300 + extend);
					}

					if (Main.rand.Next(2) == 0)
					{
						tileLocation.Y += Main.rand.Next(50);
					}
					else
					{
						tileLocation.Y -= Main.rand.Next(36) + 50;
					}

					extend++;
				}
			}
			SOTSWorldgenHelper.DistributeSkyThings(mod, 24, 7, 7, 4, 5, 45);
		}
		public override bool UseItem(Player player)
		{
			PasteStructure();
            return true;
		}
	}
}