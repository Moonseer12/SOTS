using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Fragments
{
	public class DissolvingNetherBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nether Block");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.rare = ItemRarityID.Orange;
			item.createTile = ModContent.TileType<DissolvingNetherTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DissolvingNether>(), 1);
			recipe.SetResult(this, 20);
			recipe.AddRecipe();
		}
	}
	public class DissolvingNetherTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<DissolvingNetherBlock>();
			AddMapEntry(new Color(255, 120, 0));
			mineResist = 0.2f;
			TileID.Sets.GemsparkFramingTypes[Type] = Type;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 5;
		}
		public override bool CreateDust(int i, int j, ref int type)
		{
			Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) - new Vector2(5), 16, 16, DustID.RainbowMk2);
			dust.color = new Color(255, 120, 0);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.8f;
			dust.velocity *= 2.4f;
			return false;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 2.4f;
			g = 1.4f;
			b = 0.2f;
			r *= 0.25f;
			g *= 0.25f;
			b *= 0.25f;
		}
		public static void DrawEffects(int i, int j, SpriteBatch spriteBatch, Mod mod, bool wall = false)
        {
			Texture2D textureBlock = mod.GetTexture("Assets/SpiritBlocks/NetherBlockOutline");
			float timer = Main.GlobalTime * 100 + (i + j) * 20;
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].color()) * (100f / 255f);
			color.A = 0;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int tileSeed = (int)(((i * 3 + j * 2) + j * 3 % 4 + i % 2 * 9 - i % 3 / 2 - j % 9 / 3) / 0.179f) % 4; //just throwing together a random equation
			int tileSeed11 = (int)(((i * 3 + j * 2) + j * 3 % 4 + i % 2 * 9 - i % 8 * 2 - j % 3) / 0.4325f) % 3; //just throwing together a random equation
			int tileSeed12 = (int)(((i * 3 + j * 2) + j * 4 % 7 + i % 11 * 5 - j % 2) / 0.2354f) % 2; //just throwing together a random equation
			int tileSeed2 = 1 + ((i * 2 + j * 3) + j * 4 % 3 + i % 3 * 6 - i % 12 / 3 - j % 12 / 2 + j % 8 / 4 - i % 6 * 3) % 3; //just throwing together a random equation
			int number = tileSeed;
			for (int a = 0; a < tileSeed2; a++)
			{
				if ((a + i) % 3 == 0)
					number = tileSeed;
				if ((a + i) % 3 == 1)
					number = tileSeed11;
				if ((a + i) % 3 == 2)
					number = tileSeed12;
				int maxLength = 2 + number;
				for (int j2 = 1; j2 < maxLength; j2++)
				{
					Tile tile2 = Framing.GetTileSafely(i, j - j2);
					if ((tile2.active() && Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type]) || !WorldGen.InWorld(i, j - j2, 27))
					{
						maxLength = j2;
						break;
					}
				}
				if (maxLength > 1)
				{
					float offset = 8;
					if(tileSeed2 == 2)
                    {
						if (a == 0)
							offset = 5;
						if (a == 1)
							offset = 11;
                    }
					if(tileSeed2 == 3)
					{
						if (a == 0)
							offset = 3;
						if (a == 1)
							offset = 8;
						if (a == 2)
							offset = 13;
					}
					float vOffset = 0;
					if (Main.tile[i, j].halfBrick())
						vOffset = 8;
					else
					{
						if (Main.tile[i, j].slope() == 1)
							vOffset += offset * 0.75f;
						if (Main.tile[i, j].slope() == 2)
							vOffset += 12 - offset * 0.75f;
					} 
					Vector2 previous = new Vector2(i * 16 + offset, j * 16 + 16 + vOffset);
					DrawChains(maxLength, timer, 270f / tileSeed2 * a, previous, zero, color);
				}
			}
			if (Main.tileSolid[Main.tile[i, j].type] && !Main.tileSolidTop[Main.tile[i, j].type])
			{
				color = WorldGen.paintColor((int)Main.tile[i, j].color());
				if (wall)
					color = WorldGen.paintColor((int)Main.tile[i, j].wallColor());
				color = new Color(color.R, color.G, color.B, 0);
				for (int l = 0; l < 7 - (Main.tile[i, j].inActive() ? 1 : 0); l++)
				{
					float x = Main.rand.Next(-16, 17) * 0.1f;
					float y = Main.rand.Next(-16, 17) * 0.1f;
					if (Main.tile[i, j].inActive() && l < 4)
					{
						x = 0;
						y = 0;
					}
					bool canUp = true;
					bool canDown = true;
					bool canLeft = true;
					bool canRight = true;
					if (Main.tile[i, j - 1].active() && Main.tileSolid[Main.tile[i, j - 1].type])
						canUp = false;

					if (Main.tile[i, j + 1].active() && Main.tileSolid[Main.tile[i, j + 1].type])
						canDown = false;

					if (Main.tile[i + 1, j].active() && Main.tileSolid[Main.tile[i + 1, j].type])
						canRight = false;

					if (Main.tile[i - 1, j].active() && Main.tileSolid[Main.tile[i - 1, j].type])
						canLeft = false;

					if (!canUp && !canDown)
					{
						y = 0;
					}
					else if (!canUp || !canDown)
					{
						if (!canUp)
							y = Math.Abs(y);

						if (!canDown)
							y = -Math.Abs(y);
					}
					if (!canRight && !canLeft)
					{
						x = 0;
					}
					else if (!canRight || !canLeft)
					{
						if (!canRight)
							x = -Math.Abs(x);

						if (!canLeft)
							x = Math.Abs(x);
					}
					Main.spriteBatch.Draw(textureBlock, new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + x, (float)(j * 16 - (int)Main.screenPosition.Y) + y - 2) + zero,
					new Rectangle(0, 20 * (Main.tile[i, j].halfBrick() ? 1 : Main.tile[i, j].slope() > 0 ? Main.tile[i, j].slope() + 1 : 0), 16, 20), color, 0f, default, 1f, SpriteEffects.None, 0f);
				}
			}
		}
		public static void DrawChains(int maxLength, float timer, float bonusDegrees, Vector2 startingPosition, Vector2 zero, Color color)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Assets/SpiritBlocks/NetherParticle");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float height = texture.Width * 0.5f;
			maxLength = (int)(maxLength * (16f / height) + 0.5f);
			Vector2 og = startingPosition;
			for (int z = 0; z < maxLength; z++)
			{
				float dynamicMult = 0.52f + 0.48f * (float)Math.Cos(MathHelper.ToRadians(180f * z / maxLength));
				Vector2 dynamicAddition = new Vector2(20f / maxLength * z * 0.3f + 0.4f, 0).RotatedBy(MathHelper.ToRadians(z * 24 + timer + bonusDegrees)) * dynamicMult;
				Vector2 pos = og - new Vector2(0, 16);
				pos += new Vector2(0, z * -height);
				pos += dynamicAddition;
				Vector2 rotateTo = pos - startingPosition;
				float lengthTo = rotateTo.Length();
				float stretch = (lengthTo / height) * 1.00275f;
				if (z == 0)
					stretch = 1f;
				float compress = 0.9f - (z / (float)maxLength) * 0.6f;
				Vector2 scaleVector2 = new Vector2(stretch, compress);
				if (z != 0)
				{
					float alphaScale = 1f - (z / (float)maxLength);
					for (int k = 0; k < 2; k++)
					{
						Main.spriteBatch.Draw(texture, startingPosition + zero - Main.screenPosition + Main.rand.NextVector2Circular(0.5f, 0.5f), null, color * alphaScale * 1.5f, rotateTo.ToRotation(), origin, scaleVector2 * (1f + alphaScale), SpriteEffects.None, 0f);
					}
				}
				startingPosition = pos;
			}
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			DrawEffects(i, j, spriteBatch, mod);
			return true;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Framing.SelfFrame8Way(i, j, Main.tile[i, j], resetFrame);
            return false;
        }
	}
}