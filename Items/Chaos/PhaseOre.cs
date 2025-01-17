using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Otherworld.Furniture;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chaos
{
	public class PhaseOre : ModItem
	{
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 4; k++)
			{
				Vector2 offset = new Vector2(3f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 90));
				Main.spriteBatch.Draw(texture, position + Main.rand.NextVector2Circular(1.0f, 1.0f) + offset, frame, color * 1.1f * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, position, frame, Color.White * 0.65f, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 4; k++)
			{
				Vector2 offset = new Vector2(3f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 90));
				Main.spriteBatch.Draw(texture, item.Center - Main.screenPosition + Main.rand.NextVector2Circular(1.0f, 1.0f) + offset, null, color * 1.1f * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, item.Center - Main.screenPosition, null, Color.White * 0.65f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
        public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.width = 18;
			item.height = 18;
			item.maxStack = 999;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(silver: 20);
			item.createTile = ModContent.TileType<PhaseOreTile>();
		}
	}
	public class PhaseOreTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileShine[Type] = 200;
			Main.tileShine2[Type] = true;
			Main.tileValue[Type] = 1200;
			TileID.Sets.DrawsWalls[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = false;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<PhaseOre>();
			//AddMapEntry(VoidPlayer.ChaosPink);
			mineResist = 3f;
			minPick = 180; //adamantite/chlorophyte level
			soundType = 3;
			soundStyle = 53;
			dustType = ModContent.DustType<CopyDust4>(); //DustID.PinkFlame
		}
		public override bool KillSound(int i, int j)
		{
			Vector2 pos = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
			Main.PlaySound(3, (int)pos.X, (int)pos.Y, 53, 0.25f, 0.6f);
			int type = Main.rand.Next(3) + 1;
			Main.PlaySound(SoundLoader.customSoundType, (int)pos.X, (int)pos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/VibrantOre" + type), 1.85f, -0.2f + Main.rand.NextFloat(0.1f, 0.2f));
			return false;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 7;
		}
        public override bool CreateDust(int i, int j, ref int type)
		{
			Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16), 16, 16, ModContent.DustType<CopyDust4>());
			dust.noGravity = true;
			dust.velocity *= 0.8f;
			dust.scale = 1.4f;
			dust.color = new Color(238, 145, 219, 0);
			dust.fadeIn = 0.1f;
			return false;
		}
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Draw(ModContent.GetTexture("SOTS/Items/Chaos/PhaseOreTileOutline"), ModContent.GetTexture("SOTS/Items/Chaos/PhaseOreTileFill"), i, j);
			return false;
		}
		public static int closestPlayer(int i, int j, ref float minDist)
		{
			//minDist = 32;
			//return Main.myPlayer;
			int p = -1;
			for (int k = 0; k < Main.player.Length; k++)
			{
				Player player = Main.player[k];
				if(player.active && !player.dead)
				{
					Vector2 pos = new Vector2(i * 16 + 8, j * 16 + 8);
					float length = (player.Center - pos).Length();
					if (length < minDist)
					{
						minDist = length;	
						p = player.whoAmI;
						if (Main.netMode == NetmodeID.SinglePlayer)
							break;
					}
				}
			}
			return p;
		}
        public override void NearbyEffects(int i, int j, bool closer)
		{
			float currentDistanceAway = 196;
			if (Main.rand.NextBool(300) && closestPlayer(i, j, ref currentDistanceAway) != -1)
			{
				Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16), 16, 16, ModContent.DustType<CopyDust4>());
				dust.noGravity = true;
				dust.velocity *= 0.6f;
				dust.scale = 1.3f;
				dust.color = new Color(238, 145, 219, 0);
				dust.alpha = 100;
				dust.fadeIn = 0.1f;
			}
			else if(Main.rand.NextBool(1800))
			{
				Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16), 16, 16, ModContent.DustType<CopyDust4>());
				dust.noGravity = true;
				dust.velocity *= 0.1f;
				dust.scale = 1.3f;
				dust.color = Color.Lerp(Color.White, new Color(238, 145, 219), Main.rand.NextFloat(1) * Main.rand.NextFloat(1));
				dust.alpha = 0;
				dust.fadeIn = 0.1f;
			}
		}
        public static void Draw(Texture2D outline, Texture2D fill, int i, int j, float offsetMult = 1f, bool overrideFrame = false)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			float currentDistanceAway = 196;
			int playerN = closestPlayer(i, j, ref currentDistanceAway);
			if (playerN == -1)
			{
				if (tile.frameY <= 72)
				{
					tile.frameY += 90;
				}
				return;
			}
			float alphaScale = (float)Math.Pow(1.0f - currentDistanceAway / 196f, 0.5f);
			if (alphaScale > 0.0)
            {
				if(tile.frameY > 72)
                {
					for(int k = 0; k < 2; k++)
                    {
						Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16), 16, 16, ModContent.DustType<CopyDust4>());
						dust.noGravity = true;
						dust.velocity *= 0.5f;
						dust.scale = 1.2f;
						dust.color = new Color(238, 145, 219, 0);
						dust.alpha = 200;
						dust.fadeIn = 0.1f;
					}
					tile.frameY -= 90;
				}
				Texture2D texture = fill;
				Texture2D texture2 = outline;
				float degOff = (i + j) * 7f;
				for (int k = 0; k < 5 * alphaScale; k++)
				{
					Vector2 offset = new Vector2(2.5f * offsetMult, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 90 + degOff));
					SOTSTile.DrawSlopedGlowMask(i, j, tile.type, texture2, new Color(100, 100, 100, 0) * alphaScale, k == 0 ? Vector2.Zero : offset, overrideFrame);
					offset = new Vector2(1.5f * offsetMult, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 90 + degOff));
					SOTSTile.DrawSlopedGlowMask(i, j, tile.type, texture, new Color(90, 90, 90, 0) * alphaScale, k == 0 ? Vector2.Zero : offset, overrideFrame);
				}
			}	
		}
        public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override bool Slope(int i, int j)
		{
			return true;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			float currentDistanceAway = 196;
			int playerN = closestPlayer(i, j, ref currentDistanceAway);
			if (playerN == -1)
				return;
			float alphaScale = (float)Math.Pow(1.0f - currentDistanceAway / 196f, 0.5f) * 0.3f;
			if (currentDistanceAway >= 195)
				return;
			r = 2.5f * alphaScale;
			g = 1.45f * alphaScale;
			b = 2.2f * alphaScale;
		}
	}
}