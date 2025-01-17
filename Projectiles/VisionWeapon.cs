using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items;
using SOTS.NPCs.Boss.Curse;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.Projectiles
{
	public class VisionWeapon : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vision Weapon");
		}
		public sealed override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 300;
			projectile.netImportant = true;
		}
		public List<CurseFoam> particleList = new List<CurseFoam>();
		public void ResetFoamLists()
		{
			List<CurseFoam> temp = new List<CurseFoam>();
			for (int i = 0; i < particleList.Count; i++)
			{
				if (particleList[i].active && particleList[i] != null)
					temp.Add(particleList[i]);
			}
			particleList = new List<CurseFoam>();
			for (int i = 0; i < temp.Count; i++)
			{
				particleList.Add(temp[i]);
			}
		}
		public void catalogueParticles()
		{
			for (int i = 0; i < particleList.Count; i++)
			{
				CurseFoam particle = particleList[i];
				particle.Update();
				if (!particle.active)
				{
					particle = null;
					particleList.RemoveAt(i);
					i--;
				}
				else
				{
					particle.Update();
					if (!particle.active)
					{
						particle = null;
						particleList.RemoveAt(i);
						i--;
					}
				}
			}
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if(itemType > 0)
			{
				Player player = Main.player[projectile.owner];
				Item item = player.HeldItem;
				Color color = item.GetAlpha(drawColor);
				Texture2D texture = Main.itemTexture[itemType];
				Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / frameCount / 2);
				Rectangle rectangleFrame = new Rectangle(0, texture.Height / frameCount * frame, texture.Width, texture.Height / frameCount);

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				SOTS.VisionShader.Parameters["progress"].SetValue(itemAlpha);
				SOTS.VisionShader.Parameters["lightColor"].SetValue(color.ToVector4());
				SOTS.VisionShader.Parameters["colorMod"].SetValue(GetColor().ToVector4());
				SOTS.VisionShader.Parameters["uImageSize0"].SetValue(new Vector2(texture.Width, texture.Height));
				SOTS.VisionShader.Parameters["uSourceRect"].SetValue(new Vector4(0, texture.Height / frameCount * frame, texture.Width, texture.Height / frameCount));
				SOTS.VisionShader.CurrentTechnique.Passes[0].Apply();
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, rectangleFrame, color, projectile.rotation, drawOrigin, projectile.scale, projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			}
			GetColor();
		}
		public void SpawnDust(Texture2D texture, int rate, float percent, float rotation)
		{
			int width = texture.Width;
			int height = texture.Height / frameCount;
			Color[] data = new Color[texture.Width * texture.Height];
			int startAt = width * height * frame;
			texture.GetData(data);
			Vector2 position;
			int localX = 0;
			int localY = 0;
			for (int i = startAt; i < startAt + width * height; i++)
			{
				localX++;
				if (localX > width)
				{
					localX -= width;
					localY++;
				}
				float percentX = localX / (float)width;
				float percentY = localY / (float)height;
				float length = new Vector2(percentX - 0.5f, percentY - 0.5f).Length();
				if (rate <= 1)
					rate = 1;
				if (data[i].A >= 255 && Main.rand.NextBool(rate) && Math.Abs(length - percent) <= 0.03f)
				{
					Vector2 offset = -new Vector2(width / 2, height / 2) + new Vector2(localX, localY);
					offset.X *= projectile.spriteDirection;
					position = offset.RotatedBy(rotation);
					Vector2 velocity = offset.SafeNormalize(Vector2.Zero).RotatedBy(rotation) * 0.4f;
					particleList.Add(new CurseFoam(position, velocity, 1.7f, true));
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Dusts/CopyDust4");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 6);
			Color color2 = GetColor();
			color2.A = 0;
			for (int i = 0; i < particleList.Count; i++)
			{
				Vector2 drawPos = projectile.Center + particleList[i].position - Main.screenPosition;
				Color color = projectile.GetAlpha(color2) * (0.35f + 0.65f * particleList[i].scale);
				Main.spriteBatch.Draw(texture, drawPos, new Rectangle(0, 0, texture.Width, texture.Height / 3), color, particleList[i].rotation, drawOrigin, particleList[i].scale * 0.9f, SpriteEffects.None, 0f);
			}
			return false;
		}
		public Color GetColor()
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			Color DestinationColor = Color.DarkGray;
			int uniqueGem = modPlayer.UniqueVisionNumber % 8;
			switch (uniqueGem)
			{
				case 0: //geo
					DestinationColor = Color.Orange;
					break;
				case 1: //electro
					DestinationColor = Color.BlueViolet;
					break;
				case 2: //anemo
					DestinationColor = Color.Turquoise;
					break;
				case 3: //cyro
					DestinationColor = Color.LightSkyBlue;
					break;
				case 4: //pyro
					DestinationColor = Color.OrangeRed;
					break;
				case 5: //hydro
					DestinationColor = Color.DodgerBlue;
					break;
				case 6: //dendro
					DestinationColor = Color.Green;
					break;
			}
			return DestinationColor;
        }
		int lastItem = -1;
		float itemAlpha = 0;
		int itemType = -1;
		int ticksPerFrame = 0;
		int frameCounter = 0;
		int frameCount = 1;
		int frame = 0;
		public void FindPosition()
		{
			Player player = Main.player[projectile.owner];
			if (Main.myPlayer != projectile.owner)
				projectile.timeLeft = 20;
			Vector2 idlePosition = player.Center;
			idlePosition.X -= player.direction * 16f;
			projectile.spriteDirection = player.direction;
			projectile.ai[0]++;
			float sin = (float)Math.Sin(MathHelper.ToRadians(projectile.ai[0] * 1.75f)) * 4;
			idlePosition.Y += sin - 4;
			projectile.Center = idlePosition;
			projectile.rotation = (projectile.oldPosition.X - projectile.position.X) * -0.05f;
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			Item item = player.HeldItem;
			FindPosition();
			catalogueParticles();
			if (!item.IsAir && item.active && item.createTile == -1 && item.createWall == -1 && item.useStyle != 0)
			{
				DrawAnimation anim = Main.itemAnimations[item.type];
				if(anim != null)
                {
					frameCount = anim.FrameCount;
					ticksPerFrame = anim.TicksPerFrame;
                }
				else
				{
					frameCount = 1;
				}
				if(item.useStyle == ItemUseStyleID.SwingThrow || Item.staff[item.type] || item.type == ModContent.ItemType<DigitalDaito>())
                {
					projectile.rotation += MathHelper.ToRadians(150) * projectile.spriteDirection;
                }
				else if(item.useStyle == ItemUseStyleID.HoldingOut)
                {
					if(item.height > item.width) //bow type?
					{
						projectile.spriteDirection *= -1;
						projectile.rotation += MathHelper.ToRadians(15) * projectile.spriteDirection;
					}
					else if(item.width >= item.height) //gun type?
					{
						projectile.rotation += MathHelper.ToRadians(105) * projectile.spriteDirection;
					}
                }
				itemType = item.type;
			}
			else
			{
				frameCount = 1;
				itemType = -1;
			}
			if (lastItem != itemType)
			{
				ResetFoamLists();
				lastItem = itemType;
				itemAlpha = 0;
			}
			if (itemType >= 0)
			{
				if (frameCount > 1)
				{
					frameCounter++;
					if (frameCounter > ticksPerFrame)
					{
						frame = (frame + 1) % frameCount;
						frameCounter -= ticksPerFrame;
					}
				}
				else
				{
					ticksPerFrame = 0;
					frame = 0;
					frameCounter = 0;
				}
				if (player.itemAnimation > 0)
				{
					itemAlpha = -0.24f - (!item.autoReuse ? 0.24f : 0);
				}
				else if (itemAlpha < 2)
				{
					if (Main.netMode != NetmodeID.Server)
					{
						SpawnDust(Main.itemTexture[itemType], 7, itemAlpha, projectile.rotation);
					}
					itemAlpha += 0.03f;
				}
				if (itemAlpha > 2)
					itemAlpha = 2;
			}
			Lighting.AddLight(projectile.Center, GetColor().ToVector3() * 0.3f);
		}
	}
}