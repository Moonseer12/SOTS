using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Chaos
{
	public class DogmaLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Hyperlight Beam");
		}

		public override void SetDefaults() 
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.timeLeft = 150;
			projectile.penetrate = -1;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}
		float counter = 0;
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
		public const float windUpAngle = 90f;
		public const float windUpLength = 100f;
		bool runOnce = true;
		Vector2 ogVelo = Vector2.Zero;
		float scaleMult = 1f;
		public override void AI() 
		{
			if(runOnce)
            {
				ogVelo = projectile.velocity;
				runOnce = false;
            }
			float angleToTraverse = projectile.ai[0];
			if(counter < windUpLength)
			{
				scaleMult = projectile.ai[1] + 1;
			}
			else
            {
				float timeLeftMult = (projectile.timeLeft - 2) / 51f;
				if (timeLeftMult < 0)
					timeLeftMult = 0;
				projectile.scale = (float)Math.Sqrt(timeLeftMult) * 0.5f + 0.5f * timeLeftMult;
            }
			counter++;
			float lerp = counter / windUpAngle;
			if (lerp > 1f)
				lerp = 1;
			float angle = angleToTraverse * (float)Math.Pow(1 - lerp, 1.5f) * 0.8f + 0.2f * (1 - lerp);
			projectile.velocity = ogVelo.RotatedBy(MathHelper.ToRadians(angle));
			if(counter == windUpLength + 1)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 94, 1.1f, 0.1f);
			}
			if (counter > windUpLength)
			{
				Vector2 position = projectile.Center;
				for (float i = 0; i <= maxDistance; i += 4)
				{
					if (Main.rand.NextBool(1000) || (counter == windUpLength + 1 && Main.rand.NextBool(4)))
					{
						int dust2 = Dust.NewDust(position - new Vector2(12, 12), 16, 16, ModContent.DustType<Dusts.CopyDust4>());
						Dust dust = Main.dust[dust2];
						dust.velocity *= 2f;
						dust.velocity += projectile.velocity * 0.2f;
						dust.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), true);
						dust.noGravity = true;
						dust.alpha = 90;
						dust.fadeIn = 0.1f;
						dust.scale *= 2.5f * projectile.scale;
						position += projectile.velocity.SafeNormalize(Vector2.Zero) * i;
					}
				}
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			float point = 0f;
			Vector2 finalPoint = projectile.Center + projectile.velocity.SafeNormalize(Vector2.Zero) * maxDistance;
			if(counter > windUpLength && counter < 135)
			{
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, finalPoint, 24f * scaleMult * projectile.scale, ref point))
				{
					return true;
				}
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, endPoint, 8f, ref point);
		}
		public const float maxDistance = 3200;
		public override bool PreDraw(SpriteBatch spriteBatch, Color color)
		{
			if (runOnce)
				return false;
			Player player = Main.player[projectile.owner];
			float alphaScale = 0.5f;
			float lerp = counter / windUpLength * 0.5f;
			float scalingFactor = 0.5f;
			if (lerp > 0.5f)
			{
				scalingFactor = 1.2f;
				alphaScale = 0.2f + 0.8f * (float)Math.Sqrt(projectile.scale);
				lerp = 1f;
			}
			else
			{
				lerp = lerp * lerp * 2;
				scalingFactor += 0.8f * lerp;
			}				
			if(counter > windUpLength - 20 && counter <= windUpLength)
            {
				float otherMult = (counter - windUpLength + 20) / 20f;
				alphaScale = 0.1f + 0.4f * (1 - otherMult);
				scalingFactor += 0.2f * otherMult;
			}
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float length = texture.Width * 0.5f * scalingFactor;
			Vector2 unit = projectile.velocity.SafeNormalize(Vector2.Zero);
			Vector2 position = projectile.Center;
			float maxLength = maxDistance / length * lerp;
			for (float i = 0; i <= maxLength; i++)
			{
				float radians = MathHelper.ToRadians((i + VoidPlayer.soulColorCounter) * 2);
				color = VoidPlayer.pastelAttempt(radians);
				color.A = 0;
				float mult = 1 - (i / maxLength);
				float sinusoid = 1;
				if (lerp >= 1)
				{
					mult = 1;
					if(i > maxLength - 40)
                    {
						mult = 1 - (i - maxLength + 40) / 40f;
                    }
					sinusoid = 1.0f + (0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(i * 16 + VoidPlayer.soulColorCounter * 4f))) * projectile.scale;
				}
				float scale = projectile.scale * scalingFactor * scaleMult * sinusoid;
				Vector2 drawPos = position - Main.screenPosition;
				spriteBatch.Draw(texture, drawPos, null, color * alphaScale * mult, projectile.velocity.ToRotation(), origin, new Vector2(scalingFactor, scale), SpriteEffects.None, 0f);
				position += unit * length;
			}
			return false;
		}
	}
}