using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Camera
{
	public class DreamLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1600;
		}
		public override void SetDefaults() 
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.timeLeft = 40;
			Projectile.DamageType = ModContent.GetInstance<Void.VoidMagic>();
			Projectile.penetrate = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.localNPCHitCooldown = 5;
			Projectile.usesLocalNPCImmunity = true;
		}
		bool runOnce = true;
		Color color;
		float scale = 0.8f;
		public const float length = 7f;
		public override bool PreAI() 
		{
			if(runOnce)
			{
				color = DreamingFrame.Green1;
				SetPostitions();
				runOnce = false;
				return true;
            }
			return true;
		}
        public override void AI()
        {
			if (Projectile.alpha > 0 && Projectile.timeLeft > 29)
			{
				Projectile.alpha -= 28;
			}
			else if (Projectile.alpha < 255 && Projectile.timeLeft < 15)
				Projectile.alpha += 23;
			Projectile.alpha = Math.Clamp(Projectile.alpha, 0, 255);
		}
		//bool collided = false;
        public void SetPostitions()
        {
			float speed = length * scale;
			Vector2 direction = new Vector2(speed, 0).RotatedBy(Projectile.velocity.ToRotation());
			int maxDist = (int)(Projectile.ai[1] / speed);
			Vector2 currentPos = Projectile.Center;
			int k = 0;
			while (maxDist > 0)
			{
				k++;
				posList.Add(currentPos);
				currentPos += direction;
				if (Main.rand.NextBool(5))
				{
					Dust dust = Dust.NewDustDirect(posList[posList.Count - 1] - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
					dust.fadeIn = 0.2f;
					dust.noGravity = true;
					dust.alpha = 100;
					dust.color = color;
					dust.scale *= 1.3f;
					dust.velocity *= 1.6f;
					dust.velocity += direction;
				}
				maxDist--;
			}
			SOTSProjectile.DustStar(currentPos, direction.SafeNormalize(Vector2.Zero) * 3.5f, DreamingFrame.Green1 * 0.7f, 0f, 48, 0, 4, 7.25f, 4f, 1f, 0.9f, 0.1f);
		}
        public override bool? CanHitNPC(NPC target)
        {
            return target.whoAmI == Projectile.ai[0] && Projectile.friendly && Projectile.timeLeft == 30;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return true;
		}
		List<Vector2> posList = new List<Vector2>();
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = false; //This projectile will only have an owner in singleplayer, due to the nature of spawning projectiles from NPC death.
        }
        public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width/2, texture.Height/2);
			float alpha = 0;
			Vector2 lastPosition = Projectile.Center;
			for(int i = 0; i < posList.Count; i++)
			{
				Vector2 drawPos = posList[i];
				if(i > posList.Count - 30)
				{
					if (alpha > 0)
						alpha -= 0.033f;
				}
				else
                {
					if (alpha < 1)
						alpha += 0.075f;
                }
				alpha = Math.Clamp(alpha, 0, 1);
				Vector2 direction = drawPos - lastPosition;
				lastPosition = drawPos;
				float rotation = i == 0 ? Projectile.velocity.ToRotation() : direction.ToRotation();
				float alphaMult = ((255 - Projectile.alpha) / 255f);
				for (int j = 0; j < 3; j++)
				{
					Vector2 sinusoid = new Vector2(0, alpha * scale * (4 + 10 * alphaMult) * (float)Math.Sin(MathHelper.ToRadians(i * 2 + SOTSWorld.GlobalCounter * 2 + j * 120))).RotatedBy(rotation);
					Color color = this.color * alphaMult * alpha * 0.2f;
					color.A = 0;
					Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + sinusoid, null, color, rotation, origin, new Vector2(scale * 2, scale * 0.5f * (0.2f + 0.8f * alphaMult)), SpriteEffects.None, 0f);
				}
			}
			return false;
		}
	}
}