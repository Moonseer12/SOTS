using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.Nature
{
	public class NatureHydroponics : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nature Plating Hydroponics");
			Tooltip.SetDefault("Automatically grows an assortment of alchemical herbs\nRight click to harvest all fully grown herbs\nPlants can be harvested at any time or weather");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.Size = new Vector2(34, 30);
			item.rare = ItemRarityID.Orange;
			item.createTile = ModContent.TileType<Hydroponics>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DissolvingNature>(), 1);
			recipe.AddIngredient(ItemID.HerbBag, 1);
			recipe.AddIngredient(ItemID.DirtBlock, 40);
			recipe.AddIngredient(ModContent.ItemType<NaturePlating>(), 40);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DissolvingNature>(), 1);
			recipe.AddRecipeGroup("SOTS:AlchSeeds", 20);
			recipe.AddIngredient(ItemID.DirtBlock, 40);
			recipe.AddIngredient(ModContent.ItemType<NaturePlating>(), 40);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class Hydroponics : ModTile
	{
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileWaterDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
			TileObjectData.newTile.Height = 6;
			TileObjectData.newTile.Width = 6;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawYOffset = 0;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 6, 0);
			TileObjectData.newTile.Origin = new Point16(3, 5);
			TileObjectData.addTile(Type);
			dustType = DustID.Tungsten;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hydroponics");		
			AddMapEntry(SOTSTile.NaturePlatingColor, name);
			disableSmartCursor = false;
		}
		public override bool HasSmartInteract()
		{
			return false;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			GetTopLeft(ref i, ref j);
			noBreak = true;
			resetFrame = false;
			for (int y = 0; y < 7; y++)
			{
				for (int x = 0; x < 6; x++)
				{
					Tile tile = Framing.GetTileSafely(i + x, j + y);
					if(y == 6)
					{
						if (!SOTSWorldgenHelper.TileTopCapable(i + x, j + y))
						{
							noBreak = false;
							break;
						}
					}
					else if(tile != null && (!tile.active() || tile.type != ModContent.TileType<Hydroponics>()))
					{
						noBreak = false;
						break;
					}
				}
			}
			if (!noBreak)
				Kill(i, j);
			return false;
        }
		public static void Kill(int i, int j)
		{
			Tile tileTL = Framing.GetTileSafely(i, j);
			if (tileTL.frameX == 0 && tileTL.frameY == 0 && tileTL.type == ModContent.TileType<Hydroponics>() && tileTL.active())
			{
				for (int y = 0; y < 6; y++)
				{
					for (int x = 0; x < 6; x++)
					{
						if (x >= 1 && x <= 4 && y % 2 == 0)
							DropPlant(i + x, j + y);
						WorldGen.KillTile(i + x, j + y, false, false, true);
						NetMessage.SendData(MessageID.TileChange, Main.myPlayer, Main.myPlayer, null, 0, i + x, j + y, 0f, 0, 0, 0);
					}
				}
			}
		}
        public override bool CreateDust(int i, int j, ref int type)
        {
			type = Main.rand.NextBool(3) ? DustID.Dirt : dustType;
			return true;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
			num = 2;
        }
        public override void RandomUpdate(int i, int j)
		{
			GetTopLeft(ref i, ref j);
			for (int y = 0; y < 6; y++)
			{
				for (int x = 0; x < 6; x++)
				{
					if (x >= 1 && x <= 4 && y % 2 == 0 && WorldGen.genRand.NextBool(240))
					{
						GrowPlant(i + x, j + y);
						NetMessage.SendTileSquare(-1, i + x, j + y, 1, TileChangeType.None);
					}
				}
			}
		}
        public override void PlaceInWorld(int i, int j, Item item)
		{
			GetTopLeft(ref i, ref j);
			for (int y = 0; y < 6; y++)
			{
				for (int x = 0; x < 6; x++)
				{
					Tile growTile = Main.tile[i + x, j + y];
					if (x >= 1 && x <= 4 && y % 2 == 0 && growTile.type == Type)
					{
						SetPlant(i + x, j + y, Main.rand.Next(7));
						NetMessage.SendTileSquare(-1, i + x, j + y, 1, TileChangeType.None);
					}
				}
			}
		}
        public static void SetPlant(int i, int j, int type)
		{
			Tile tile = Main.tile[i, j];
			//daybloom, moonglow, blinkroot, deathweed, waterleaf, fireblossom, shiverthorn
			tile.frameX = (short)(18 * (type + 1));
			tile.frameY = 18;
		}
		public static void GrowPlant(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			if (tile.frameY < 72)
				tile.frameY += 18;
		}
		public static bool DropPlant(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			if(tile.frameY == 72)
			{
				int type = -1;
				int dustType = 3;
				switch (tile.frameX / 18 - 1)
                {
					case 0:
						type = ItemID.Daybloom;
						dustType = 3;
						break;
					case 1:
						type = ItemID.Moonglow;
						dustType = 3;
						break;
					case 2:
						type = ItemID.Blinkroot;
						dustType = 7;
						break;
					case 3:
						type = ItemID.Deathweed;
						dustType = 17;
						break;
					case 4:
						type = ItemID.Waterleaf;
						dustType = 3;
						break;
					case 5:
						type = ItemID.Fireblossom;
						dustType = 6;
						break;
					case 6:
						type = ItemID.Shiverthorn;
						dustType = 224;
						break;
				}
				if (type == -1)
					return false;
				int item = Item.NewItem(i * 16, j * 16, 16, 16, type, 1, false, 0, false, false);
				NetMessage.SendData(MessageID.SyncItem, Main.myPlayer, Main.myPlayer, null, item, 1f, 0.0f, 0.0f, 0, 0, 0);
				SetPlant(i, j, Main.rand.Next(7));
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 1, TileChangeType.None);
				Main.PlaySound(SoundID.Grass, i * 16, j * 16, 1, 1f, 0f);
				for(int a = 0; a < 10; a++)
                {
					Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, dustType);
                }
				return true;
			}
			return false;
		}
		public void GetTopLeft(ref int i, ref int j)
		{
			Tile tile = Main.tile[i, j];
			if (tile.frameX == 0 && tile.frameY == 0 && tile.type == ModContent.TileType<Hydroponics>())
				return;
			if (tile.frameX != 0)
			{
				for (int k = 0; k < 6; k++)
				{
					i--;
					tile = Main.tile[i, j];
					if (tile.frameX == 0)
					{
						break;
					}
				}
			}
			if (tile.frameY != 0)
			{
				for (int k = 0; k < 6; k++)
				{
					j--;
					tile = Main.tile[i, j];
					if (tile.frameY == 0)
					{
						break;
					}
				}
			}
		}
        public override bool NewRightClick(int i, int j)
		{
			GetTopLeft(ref i, ref j);
			for (int y = 0; y < 6; y += 2)
			{
				for (int x = 1; x <= 4; x++)
				{
					Tile growTile = Main.tile[i + x, j + y];
					if (x >= 1 && x <= 4 && y % 2 == 0 && growTile.type == Type)
					{
						if (growTile.frameY > 72)
							SetPlant(i + x, j + y, Main.rand.Next(7));
						DropPlant(i + x, j + y);
					}
				}
			}
			return false;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			//Main.SmartInteractTileCoordsSelected
			Tile tile = Main.tile[i, j];
			bool topLeft = tile.frameY == 0 && tile.frameX == 0;
			if (topLeft) //check for it being the top left tile
			{
				//int highlightCapable = -1;
				Texture2D texture = Main.tileTexture[Type];
				Texture2D textureGlow = ModContent.GetTexture("SOTS/Items/Furniture/Nature/HydroponicsGlow");
				for (int layer = 0; layer < 2; layer ++)
				{
					for (int y = 0; y < 6; y++)
					{
						for (int x = 0; x < 6; x++)
						{
							Vector2 drawOffset = new Vector2(16 * x, 16 * y);
							Vector2 drawPosition = new Vector2(i * 16, j * 16) + zero - Main.screenPosition + drawOffset;
							int frameHeight = 16;
							if (y == 5)
							{
								frameHeight = 18;
							}
							Color lightColor = Lighting.GetColor(i + x, j + y);
							Rectangle frame = new Rectangle(18 * x, 18 * y, 16, frameHeight);
							if (layer == 0)
							{
								if(Main.canDrawColorTile(i + x, j + y))
                                {
									spriteBatch.Draw(Main.tileAltTexture[Type, Main.tile[i + x, j + y].color()], drawPosition, frame, lightColor, 0f, default(Vector2), 1.0f, SpriteEffects.None, 0f);
								}
								else
									spriteBatch.Draw(texture, drawPosition, frame, lightColor, 0f, default(Vector2), 1.0f, SpriteEffects.None, 0f);
								spriteBatch.Draw(textureGlow, drawPosition, frame, Color.White, 0f, default(Vector2), 1.0f, SpriteEffects.None, 0f);
							}
							else if (layer == 1)
							{
								/*if (highlightCapable != -1)
									DrawHighlight(highlightCapable == 2, drawPosition, frame, spriteBatch, lightColor);*/
								if (x >= 1 && x <= 4 && y % 2 == 0)
								{
									Tile growTile = Main.tile[i + x, j + y];
									int growthStage = growTile.frameY / 18 - 1;
									if (growthStage > 0)
									{
										int type = growTile.frameX / 18 - 1;
										if (!Main.tileSetsLoaded[81 + growthStage])
											Main.instance.LoadTiles(81 + growthStage);
										Texture2D texture2 = Main.tileTexture[81 + growthStage];
										int direction = ((i + x) % 2 * 2 - 1);
										drawPosition.Y -= 6;
										if (type == 1)
										{
											drawPosition.X += 4 * direction;
										}
										if(growthStage == 3)
											DoDust(i + x, j + y, type, ref lightColor);
										spriteBatch.Draw(texture2, drawPosition, new Rectangle(type * 18, 0, 16, 20), lightColor, 0f, default(Vector2), 1.0f, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
									}
								}
							} 
						}
					}
				}
				/*if (highlightCapable != -1)
					for(int a = 0; a < Main.SmartInteractTileCoordsSelected.Count; a++)
					{
						spriteBatch.Draw(ModContent.GetTexture("SOTS/Items/Fragments/NaturePlating"), Main.SmartInteractTileCoordsSelected[a].ToVector2() * 16 - Main.screenPosition + zero, null, Color.White * 0.5f, 0f, default(Vector2), 0.5f, SpriteEffects.None, 0f);
					}*/
			}
			return false;
        }
		public void DoDust(int i, int j, int type, ref Color color1) 
		{
			if (type == 0 && Main.rand.Next(100) == 0)
			{
				int dust = Dust.NewDust(new Vector2(i * 16, (float)(j * 16 - 4)), 16, 16, 19, 0.0f, 0.0f, 160, new Color(), 0.1f);
				Main.dust[dust].velocity.X /= 2f;
				Main.dust[dust].velocity.Y /= 2f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].fadeIn = 1f;
			}
			if (type == 1 && Main.rand.Next(100) == 0)
				Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 41, 0.0f, 0.0f, 250, new Color(), 0.8f);
			if (type == 3)
			{
				if (Main.rand.Next(200) == 0)
				{
					int dust = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 14, 0.0f, 0.0f, 100, new Color(), 0.2f);
					Main.dust[dust].fadeIn = 1.2f;
				}

				if (Main.rand.Next(75) == 0)
				{
					int dust = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 27);
					Main.dust[dust].velocity.X /= 2f;
					Main.dust[dust].velocity.Y /= 2f;
				}
			}
			if (type == 4 && Main.rand.Next(150) == 0)
			{
				int dust = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 8, 16);
				Main.dust[dust].velocity.X /= 3f;
				Main.dust[dust].velocity.Y /= 3f;
				Main.dust[dust].velocity.Y -= 0.7f;
				Main.dust[dust].alpha = 50;
				Main.dust[dust].scale *= 0.1f;
				Main.dust[dust].fadeIn = 0.9f;
				Main.dust[dust].noGravity = true;
			}
			if (type == 5)
			{
				if (Main.rand.Next(40) == 0)
				{
					int dust = Dust.NewDust(new Vector2(i * 16, (float)(j * 16 - 6)), 16, 16, DustID.Fire, 0.0f, 0.0f, 0, new Color(), 1.5f);
					Main.dust[dust].velocity.Y -= 2f;
					Main.dust[dust].noGravity = true;
				}
				color1.A = (byte)(Main.mouseTextColor / 2U);
				color1.G = Main.mouseTextColor;
				color1.B = Main.mouseTextColor;
			}
			if (type == 6)
			{
				if (Main.rand.Next(30) == 0)
				{
					var newColor = new Color(50, byte.MaxValue, byte.MaxValue, byte.MaxValue); 
					int dust = Dust.NewDust(new Vector2((i * 16), (j * 16)), 16, 16,43, 0.0f, 0.0f, 254, newColor, 0.5f);
					Main.dust[dust].velocity *= 0.0f;
				}
				var num9 = (byte)((Main.mouseTextColor + color1.G * 2) / 3);
				var num10 = (byte)((Main.mouseTextColor + color1.B * 2) / 3);
				if (num9 > color1.G)
					color1.G = num9;
				if (num10 > color1.B)
					color1.B = num10;
			}
		}
        /*public void DrawHighlight(bool selected, Vector2 drawPosition, Rectangle frame, SpriteBatch spriteBatch, Color lightColor)
		{
			int ave = ((int)lightColor.R + (int)lightColor.G + (int)lightColor.B) / 3;
			if (ave > 10)
			{
				Texture2D texture2 = ModContent.GetTexture(HighlightTexture);
				Color drawColor = !selected ? new Color(ave / 2, ave / 2, ave / 2, ave) : new Color(ave, ave, ave / 3, ave);
				spriteBatch.Draw(texture2, drawPosition, frame, drawColor, 0f, default(Vector2), 1.0f, SpriteEffects.None, 0f);
			}
		}*/
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0;
			g = 0;
			b = 0;
			int top = j;
			int left = i;
			GetTopLeft(ref left, ref top);
			Tile growTile = Main.tile[i, j];
			int x = i - left;
			int y = j - top;
			if (x >= 1 && x <= 4 && y % 2 == 0 && growTile.type == Type)
			{
				if (growTile.frameY == 72)
				{
					switch (growTile.frameX / 18 - 1)
					{
						case 2:
							var num18 = (270 - Main.mouseTextColor) / 800f;
							if (num18 > 1.0)
								num18 = 1f;
							else if (num18 < 0.0)
								num18 = 0.0f;
							r = num18 * 0.7f;
							g = num18;
							b = num18 * 0.1f;
							break;
						case 5:
							var num25 = 0.9f;
							r = num25;
							g = num25 * 0.8f;
							b = num25 * 0.2f;
							break;
						case 6:
							var num26 = 0.08f;
							g = num26 * 0.8f;
							b = num26;
							break;
					}
				}
			}
			base.ModifyLight(i, j, ref r, ref g, ref b);
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			Tile tile = Main.tile[i, j];
			if (tile.frameX == 0 && tile.frameY == 0)
			{
				if(!noItem)
				{
					for (int y = 0; y < 6; y++)
					{
						for (int x = 0; x < 6; x++)
						{
							if (y > 0 && x > 0)
							{
								WorldGen.KillTile(i + x, j + y, false, false, true);
								NetMessage.SendData(MessageID.TileChange, Main.myPlayer, Main.myPlayer, null, 0, i + x, j + y, 0f, 0, 0, 0);
							}
						}
					}
				}
				else
					Item.NewItem(i * 16, j * 16, 96, 96, ModContent.ItemType<NatureHydroponics>());
				noItem = true;
			}
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.showItemIcon2 = ModContent.ItemType<NatureHydroponics>();
			player.noThrow = 2;
			player.showItemIcon = true;
		}
		public override void MouseOverFar(int i, int j)
		{
			MouseOver(i, j);
			Player player = Main.LocalPlayer;
			if (player.showItemIconText == "")
			{
				player.showItemIcon = false;
				player.showItemIcon2 = 0;
			}
		}
	}
}