using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Lightning
{
	public class BlueLightning : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blue Lightning");
		}
		public override void SetDefaults()
		{
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.timeLeft = 1200;
			projectile.width = 12;
			projectile.height = 14;
			projectile.extraUpdates = 3;
		}
		bool runOnce = true;
		Vector2[] trailPos = new Vector2[4];
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				Color color = new Color(130, 130, 140, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = projectile.GetAlpha(color) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
				float max = betweenPositions.Length() / (texture.Height * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 5; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.2f * scale;
						float y = Main.rand.Next(-10, 11) * 0.2f * scale;
						if (j < 2)
						{
							x = 0;
							y = 0;
						}
						if (trailPos[k] != projectile.Center)
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
			return false;
		}
		public void cataloguePos()
		{
			Vector2 current = projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
		public void checkPos()
		{
			bool flag = false;
			float iterator = 0f;
			Vector2 current = projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				if (current == previousPosition)
				{
					iterator++;
					flag = true;
				}
			}
			if (flag || endHow == 1)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 56);
				Main.dust[dust].scale *= 1.2f * (trailPos.Length - iterator) / (float)trailPos.Length;
				Main.dust[dust].velocity *= 1.2f;
				Main.dust[dust].noGravity = true;
			}
			if (iterator >= trailPos.Length)
				projectile.Kill();
		}
		int endHow = 0;
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			TriggerStop();
			return false;
		}
		public void TriggerStop()
		{
			endHow = 1;
			projectile.tileCollide = false;
			projectile.velocity *= 0f;
			projectile.friendly = false;
		}
		int counter = 0;
		public override bool PreAI()
		{
			if (projectile.timeLeft < 220)
			{
				TriggerStop();
			}
			if (runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			projectile.ai[0] -= 0.02f / 4f;
			if (Main.myPlayer == projectile.owner && endHow == 0)
			{
				projectile.velocity.Y += 0.03f * Main.rand.Next(-3, 4);
				projectile.velocity.X += 0.03f * Main.rand.Next(-3, 4);
			}
			return base.PreAI();
		}
		public override void AI()
		{
			checkPos();
			counter++;
			if (counter >= 0)
			{
				cataloguePos();
				counter = -12;
				if (projectile.owner == Main.myPlayer && endHow == 0)
				{
					if (projectile.velocity.Length() != 0f)
					{
						projectile.velocity = new Vector2(projectile.velocity.Length(), 0).RotatedBy(projectile.velocity.ToRotation() + MathHelper.ToRadians(projectile.ai[1]));
						projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
					}
					projectile.ai[1] = Main.rand.Next(-4, 5);
					projectile.netUpdate = true;
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 0;
			TriggerStop();
		}
	}
}
		