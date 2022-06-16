using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SOTS.Items.Chaos;
using SOTS.Items.Otherworld;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Projectiles.Otherworld;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Common.PlayerDrawing
{
	public class UseItemGlowmask : PlayerDrawLayer
	{
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return true;
		}
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(drawPlayer);
			if (drawInfo.shadow != 0)
				return;
			if (!drawPlayer.HeldItem.IsAir)
			{
				Item item = drawPlayer.HeldItem;
				Texture2D texture = item.GetGlobalItem<ItemUseGlow>().glowTexture;
				Vector2 zero2 = Vector2.Zero;
				SpriteEffects effects = drawInfo.playerEffect; //this is just a guess... might actually require drawInfo.itemEffect
				bool isTwilightPole = item.type == ModContent.ItemType<TwilightFishingPole>() && drawPlayer.ownedProjectileCounts[ModContent.ProjectileType<TwilightBobber>()] > 0;
				if (texture != null && (drawPlayer.itemAnimation > 0 || isTwilightPole))
				{
					Vector2 location = drawInfo.ItemLocation;
					if (item.useStyle == ItemUseStyleID.Shoot)
					{
						if (Item.staff[item.type])
						{
							float rotation = drawPlayer.itemRotation + 0.785f * (float)drawPlayer.direction;
							int width = 0;
							Vector2 origin = new Vector2(0f, (float)Terraria.GameContent.TextureAssets.Item[item.type].Value.Height);

							if (drawPlayer.gravDir == -1f)
							{
								if (drawPlayer.direction == -1)
								{
									rotation += 1.57f;
									origin = new Vector2((float)Terraria.GameContent.TextureAssets.Item[item.type].Value.Width, 0f);
									width -= Terraria.GameContent.TextureAssets.Item[item.type].Value.Width;
								}
								else
								{
									rotation -= 1.57f;
									origin = Vector2.Zero;
								}
							}
							else if (drawPlayer.direction == -1)
							{
								origin = new Vector2((float)Terraria.GameContent.TextureAssets.Item[item.type].Value.Width, (float)Terraria.GameContent.TextureAssets.Item[item.type].Value.Height);
								width -= Terraria.GameContent.TextureAssets.Item[item.type].Value.Width;
							}

							DrawData value = new DrawData(texture, new Vector2((float)((int)(location.X - Main.screenPosition.X + origin.X + (float)width)), (float)((int)(location.Y - Main.screenPosition.Y))), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, Terraria.GameContent.TextureAssets.Item[item.type].Value.Width, Terraria.GameContent.TextureAssets.Item[item.type].Value.Height)), Color.White, rotation, origin, item.scale, effects, 0);
							drawInfo.DrawDataCache.Add(value);
						}
						else
						{
							Vector2 vector10 = new Vector2((float)(Terraria.GameContent.TextureAssets.Item[item.type].Value.Width / 2), (float)(Terraria.GameContent.TextureAssets.Item[item.type].Value.Height / 2));

							//Vector2 vector11 = this.DrawPlayerItemPos(drawPlayer.gravDir, item.type);
							Vector2 vector11 = new Vector2(10, texture.Height / 2);
							if (item.GetGlobalItem<ItemUseGlow>().glowOffsetX != 0)
							{
								vector11.X = item.GetGlobalItem<ItemUseGlow>().glowOffsetX;
							}
							vector11.Y += item.GetGlobalItem<ItemUseGlow>().glowOffsetY * drawPlayer.gravDir;
							int num107 = (int)vector11.X;
							vector10.Y = vector11.Y;
							Vector2 origin5 = new Vector2((float)(-(float)num107), (float)(Terraria.GameContent.TextureAssets.Item[item.type].Value.Height / 2));
							if (drawPlayer.direction == -1)
							{
								origin5 = new Vector2((float)(Terraria.GameContent.TextureAssets.Item[item.type].Value.Width + num107), (float)(Terraria.GameContent.TextureAssets.Item[item.type].Value.Height / 2));
							}

							//value = new DrawData(Terraria.GameContent.TextureAssets.Item[item.type].Value, new Vector2((float)((int)(value2.X - Main.screenPosition.X + vector10.X)), (float)((int)(value2.Y - Main.screenPosition.Y + vector10.Y))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Terraria.GameContent.TextureAssets.Item[item.type].Value.Width, Terraria.GameContent.TextureAssets.Item[item.type].Value.Height)), item.GetAlpha(color37), drawPlayer.itemRotation, origin5, item.scale, effect, 0);
							//drawInfo.DrawDataCache.Add(value);

							Color color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
							int recurse = 1;
							bool rainbow = item.type == ModContent.ItemType<PhaseCannon>() && modPlayer.rainbowGlowmasks;
							if (rainbow)
							{
								recurse = 2;
							}
							Vector2 position = location - Main.screenPosition + vector10;
							if (item.type == ModContent.ItemType<SupernovaStorm>())
							{
								for (int k = 0; k < 6; k++)
								{
									Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount * 6));
									color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60)) * 0.33f;
									color.A = 0;
									DrawData value2 = new DrawData(texture, position + circular, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, Terraria.GameContent.TextureAssets.Item[item.type].Value.Width, Terraria.GameContent.TextureAssets.Item[item.type].Value.Height)), color, drawPlayer.itemRotation, origin5, item.scale, effects, 0);
									drawInfo.DrawDataCache.Add(value2);
								}
								DrawData value = new DrawData(Mod.Assets.Request<Texture2D>("Items/Chaos/SupernovaStorm").Value, position, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, Terraria.GameContent.TextureAssets.Item[item.type].Value.Width, Terraria.GameContent.TextureAssets.Item[item.type].Value.Height)), Lighting.GetColor((int)location.X / 16, (int)location.Y / 16), drawPlayer.itemRotation, origin5, item.scale, effects, 0);
								drawInfo.DrawDataCache.Add(value);
							}
							for (int i = 0; i < recurse; i++)
							{
								DrawData value = new DrawData(texture, position, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, Terraria.GameContent.TextureAssets.Item[item.type].Value.Width, Terraria.GameContent.TextureAssets.Item[item.type].Value.Height)), rainbow ? color : Color.White, drawPlayer.itemRotation, origin5, item.scale, effects, 0);
								drawInfo.DrawDataCache.Add(value);
							}
						}
					}
					else //for swords and stuff
					{
						if (item.type == ModContent.ItemType<RealityShatter>())
						{
							for (int k = 0; k < 6; k++)
							{
								Color color = Color.White;
								Vector2 circular = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount * 6));
								color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60));
								color.A = 0;
								DrawData value = new DrawData(texture, location - Main.screenPosition + circular, new Rectangle(0, 0, texture.Width, texture.Height), color, drawPlayer.itemRotation, new Vector2(texture.Width * 0.5f - texture.Width * 0.5f * (float)drawPlayer.direction, drawPlayer.gravDir == -1 ? 0f : texture.Height), item.scale, effects, 0);
								drawInfo.DrawDataCache.Add(value);
							}
							Texture2D tBlack = Mod.Assets.Request<Texture2D>("Items/Chaos/RealityShatterBlack").Value;
							DrawData value2 = new DrawData(tBlack, location - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.Black, drawPlayer.itemRotation, new Vector2(texture.Width * 0.5f - texture.Width * 0.5f * (float)drawPlayer.direction, drawPlayer.gravDir == -1 ? 0f : texture.Height), item.scale, effects, 0);
							drawInfo.DrawDataCache.Add(value2);
							for (int k = 0; k < 6; k++)
							{
								Color color = Color.Black * 0.7f;
								Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(k * 60));
								DrawData value = new DrawData(tBlack, location - Main.screenPosition + circular, new Rectangle(0, 0, texture.Width, texture.Height), color, drawPlayer.itemRotation, new Vector2(texture.Width * 0.5f - texture.Width * 0.5f * (float)drawPlayer.direction, drawPlayer.gravDir == -1 ? 0f : texture.Height), item.scale, effects, 0);
								drawInfo.DrawDataCache.Add(value);
							}
						}
						else if (item.type == ModContent.ItemType<EtherealScepter>())
						{
							Texture2D tEffect = Mod.Assets.Request<Texture2D>("Items/Chaos/EtherealScepterEffect").Value;
							for (int k = 0; k < 6; k++)
							{
								Color color = Color.White;
								Vector2 circular = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount * 6));
								color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60));
								color.A = 0;
								DrawData value = new DrawData(tEffect, location - Main.screenPosition + circular, new Rectangle(0, 0, texture.Width, texture.Height), color * 0.3f, drawPlayer.itemRotation, new Vector2(texture.Width * 0.5f - texture.Width * 0.5f * (float)drawPlayer.direction, drawPlayer.gravDir == -1 ? 0f : texture.Height), item.scale, effects, 0);
								drawInfo.DrawDataCache.Add(value);
							}
							DrawData value2 = new DrawData(texture, location - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, drawPlayer.itemRotation, new Vector2(texture.Width * 0.5f - texture.Width * 0.5f * (float)drawPlayer.direction, drawPlayer.gravDir == -1 ? 0f : texture.Height), item.scale, effects, 0);
							drawInfo.DrawDataCache.Add(value2);
							value2 = new DrawData(tEffect, location - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, drawPlayer.itemRotation, new Vector2(texture.Width * 0.5f - texture.Width * 0.5f * (float)drawPlayer.direction, drawPlayer.gravDir == -1 ? 0f : texture.Height), item.scale, effects, 0);
							drawInfo.DrawDataCache.Add(value2);
						}
						else
						{
							Color color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
							int recurse = 1;
							if (modPlayer.rainbowGlowmasks)
							{
								recurse = 2;
							}
							for (int i = 0; i < recurse; i++)
							{
								DrawData value = new DrawData(texture,
									new Vector2((float)((int)(location.X - Main.screenPosition.X)),
									(float)((int)(location.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)),
									modPlayer.rainbowGlowmasks ? color : Color.White,
									drawPlayer.itemRotation,
									 new Vector2(texture.Width * 0.5f - texture.Width * 0.5f * (float)drawPlayer.direction, drawPlayer.gravDir == -1 ? 0f : texture.Height),
									item.scale,
									effects,
									0);
								drawInfo.DrawDataCache.Add(value);
							}
						}
					}
				}
			}
		}
	}
}