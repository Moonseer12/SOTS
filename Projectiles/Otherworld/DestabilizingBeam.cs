using System;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	public class DestabilizingBeam : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Destabilizing Beam");
		}

		public override void SetDefaults() 
		{
			projectile.width = 6;
			projectile.height = 6;
			projectile.timeLeft = 120;
			projectile.penetrate = -1;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void AI() 
		{
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.8f / 255f, (255 - projectile.alpha) * 0.8f / 255f, (255 - projectile.alpha) * 0.8f / 255f);
			//projectile.Center = npc.Center;
			projectile.alpha += 5;
			if (projectile.alpha > 255) {
				projectile.Kill();
			}
			if(projectile.alpha > 40)
            {
				projectile.friendly = false;
            }
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 10;
        }
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if(!projectile.friendly)
            {
				return false;
            }
			Vector2 unit = new Vector2(projectile.velocity.X, projectile.velocity.Y);
			Vector2 currentPos = projectile.Center;
			float radianDir = (float)Math.Atan2(unit.Y, unit.X);
			float size = 0.3f;
			float rotate = projectile.ai[0];
			for (int Distance = 0; Distance < 120; Distance++)
			{
				Vector2 drawPos = currentPos - Main.screenPosition;
				Vector2 position = currentPos;
				int i = (int)(position.X / 16);
				int j = (int)(position.Y / 16);
					if (!WorldGen.InWorld(i, j, 20) || (Main.tile[i, j].active() && Main.tileSolidTop[Main.tile[i, j].type] == false && Main.tileSolid[Main.tile[i, j].type] == true && Main.tile[i, j].nactive() && Distance < 119))
				{
					previousDistance = Distance;
					Distance = 119;
					return false;
				}
				if(targetHitbox.Intersects(new Rectangle((int)(currentPos.X - 7 * (0.2f + size)), (int)(currentPos.Y - 7 * (0.2f + size)), (int)(14 * (0.2f + size)), (int)(14 * (0.2f + size)))))
                {
					return true;
				}
				Vector2 rotateVector = new Vector2(0.4f, 0).RotatedBy(MathHelper.ToRadians(rotate));
				if (size < 12.9f)
					size += 0.03f;
				rotate += 6;
				Vector2 laserVelo = new Vector2((14f) * size, 0f).RotatedBy(radianDir) + rotateVector;
				currentPos.X += laserVelo.X;
				currentPos.Y += laserVelo.Y;
			}
			return false;
		}
		int previousDistance = 120;
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Otherworld/DestabilizingBeam");
			bool dust = false;
			if(projectile.alpha < 10)
			{
				dust = true;
			}
			Vector2 unit = new Vector2(projectile.velocity.X, projectile.velocity.Y);
			Vector2 currentPos = projectile.Center;
			float radianDir = (float)Math.Atan2(unit.Y, unit.X);
			lightColor = new Color(110, 110, 110, 0) * ((255 - projectile.alpha) / 255f);
			float size = 0.3f;
			float rotate = projectile.ai[0];
			for (int Distance = 0; Distance < 120; Distance++)
			{
				Vector2 drawPos = currentPos - Main.screenPosition;
				Vector2 position = currentPos;
				int i = (int)(position.X / 16);
				int j = (int)(position.Y / 16);
				Vector2 rotateVector = new Vector2(0.4f, 0).RotatedBy(MathHelper.ToRadians(rotate));
				if (size < 12.9f)
					size += 0.03f;
				rotate += 6;
				Vector2 laserVelo = new Vector2((14f) * size, 0f).RotatedBy(radianDir) + rotateVector;

				if (!WorldGen.InWorld(i, j, 20) || (Main.tile[i, j].active() && Main.tileSolidTop[Main.tile[i, j].type] == false && Main.tileSolid[Main.tile[i, j].type] == true && Main.tile[i, j].nactive() && Distance < 119))
				{
					position -= laserVelo;
					Vector2 velo = new Vector2(0, Main.rand.Next(-7, 8) * size).RotatedBy(laserVelo.ToRotation());
					for (int k = 0; k < 2; k++)
					{
						int dust1 = Dust.NewDust(position + velo + new Vector2(-4, -4), 0, 0, DustID.Electric, 0, 0, projectile.alpha, default, 1.25f);
						Main.dust[dust1].noGravity = true;
						Main.dust[dust1].velocity *= 1.5f;
						Main.dust[dust1].alpha = projectile.alpha;
						Main.dust[dust1].velocity += projectile.velocity / 4f;
					}
					previousDistance = Distance;
					Distance = 119;
					return false;
				}

				currentPos.X += laserVelo.X;
				currentPos.Y += laserVelo.Y;
				Lighting.AddLight(position, lightColor.R / 255f, lightColor.G / 255f, lightColor.B / 255f);
				for (int s = 0; s < 7; s++)
				{
					Vector2 rotateVector2 = new Vector2(0, Main.rand.Next(-10, 11) * 0.3f).RotatedBy(laserVelo.ToRotation());
					spriteBatch.Draw(texture, drawPos + rotateVector2 + (projectile.velocity.SafeNormalize(Vector2.Zero)), null, lightColor, (float)Math.Atan2(unit.Y, unit.X), new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), size, SpriteEffects.None, 0f);
				}
				if(dust || Main.rand.Next(250) == 0)
				{
					int num1 = Dust.NewDust(new Vector2(position.X - 4, position.Y - 4), projectile.width, projectile.height, mod.DustType("CodeDust2"));
					Main.dust[num1].velocity *= 1.75f;
					Main.dust[num1].scale *= 2.75f;
				}
			}
			return false;
		}
	}
}