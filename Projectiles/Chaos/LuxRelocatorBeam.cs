using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Dusts;
using SOTS.NPCs;
using SOTS.NPCs.ArtificialDebuffs;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{    
    public class LuxRelocatorBeam : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Relocator Beam");
		}
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24; 
            projectile.timeLeft = 50;
            projectile.penetrate = -1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.localNPCHitCooldown = 200;
            projectile.usesLocalNPCImmunity = true;
        }
        List<Vector2> drawPositionList = new List<Vector2>();
        List<Vector2> desinationList = new List<Vector2>();
        bool runOnce = true;
        public const float Speed = 3f;
        public int GrowthRange = 20;
        public int DegradeRange = 10;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (runOnce)
                return false;
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Color color = new Color(140, 140, 140, 0);
            float endPercent = projectile.timeLeft / 50f;
            float rotation = projectile.velocity.ToRotation();
            int max = drawPositionList.Count;
            int startPos = (int)((1 - endPercent) * max);
            int progress = 0;
            for (int i = startPos; i < max; i++)
            {
                if (!SOTS.Config.lowFidelityMode || i % 2 == 0)
                {
                    float scale = 1.25f;
                    if (progress < GrowthRange)
                    {
                        scale *= progress / (float)GrowthRange;
                    }
                    else if (i > max - DegradeRange)
                    {
                        int index = i - (max - DegradeRange);
                        scale *= 1 - index / (float)DegradeRange;
                    }
                    Vector2 drawPos = drawPositionList[i];
                    Color otherC = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 3), false);
                    otherC.A = 0;
                    Vector2 sinusoid = new Vector2(0, 14 * scale * (float)Math.Sin(MathHelper.ToRadians(Main.GameUpdateCount * 8 + i * 3))).RotatedBy(rotation);
                    spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, color * ((255 - projectile.alpha) / 255f), rotation, origin, new Vector2(1, scale * 1f) * projectile.scale, SpriteEffects.None, 0f);
                    spriteBatch.Draw(texture, drawPos + sinusoid - Main.screenPosition, null, otherC * ((255 - projectile.alpha) / 255f), rotation, origin, new Vector2(1, scale * 0.75f) * projectile.scale, SpriteEffects.None, 0f);
                    if (i != drawPositionList.Count - 1)
                        rotation = (drawPositionList[i + 1] - drawPos).ToRotation();
                }
                progress++;
            }
            return false;
        }
        public void SetupLaser()
        {
            Vector2 finalDestination = new Vector2(projectile.ai[0], projectile.ai[1]);
            Vector2 playerCenter = projectile.velocity;
            for(int i = 0; i < 5; i++)
            {
                Vector2 destinationToPlayer = playerCenter - finalDestination;
                Vector2 startToPlayer = playerCenter - projectile.Center;
                float r = destinationToPlayer.ToRotation();
                float x = startToPlayer.ToRotation() - r;
                x = MathHelper.WrapAngle(x);
                float lerpedAngle = MathHelper.Lerp(x, 0, (i + 1) / 6f);
                lerpedAngle += r;
                float maxLength = MathHelper.Lerp(startToPlayer.Length(), destinationToPlayer.Length(), (i + 1) / 6f);
                Vector2 circular = new Vector2(-maxLength * Main.rand.NextFloat(0.8f, 1.2f), 0).RotatedBy(lerpedAngle);
                Vector2 fromPlayer = playerCenter + circular;
                desinationList.Add(fromPlayer);
            }
            projectile.velocity = Vector2.Zero;
            float radians = (float)projectile.velocity.ToRotation();
            Vector2 position = projectile.Center;
            Vector2 velocity = projectile.velocity.SafeNormalize(new Vector2 (0, 1)) * Speed;
            bool end = false;
            int counter = 0;
            while(!end)
            {
                position += velocity;
                drawPositionList.Add(position);
                if (Main.rand.NextBool(4))
                {
                    Dust dust2 = Dust.NewDustPerfect(position, ModContent.DustType<CopyDust4>(), Main.rand.NextVector2Circular(3, 3), 120);
                    dust2.velocity += projectile.velocity * 0.1f;
                    dust2.noGravity = true;
                    dust2.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(0, 6.28f), true);
                    dust2.noGravity = true;
                    dust2.fadeIn = 0.2f;
                    dust2.scale *= 2.2f;
                }
                bool continueToFinal = true;
                if (desinationList.Count > 0)
                {
                    Vector2 goTo = desinationList[0];
                    Rectangle hitbox = new Rectangle((int)position.X - projectile.width / 2, (int)position.Y - projectile.height / 2, projectile.width, projectile.height);
                    if (hitbox.Contains(goTo.ToPoint()))
                    {
                        desinationList.RemoveAt(0);
                    }
                    else
                    {
                        redirectGrowth = 0;
                        radians = Redirect(radians, position, goTo);
                        continueToFinal = false;
                    }
                }
                if (continueToFinal)
                {
                    Rectangle hitbox = new Rectangle((int)position.X - projectile.width / 2, (int)position.Y - projectile.height / 2, projectile.width, projectile.height);
                    if (hitbox.Contains(finalDestination.ToPoint()))
                    {
                        end = true;
                    }
                    radians = Redirect(radians, position, finalDestination);
                }
                velocity = new Vector2(1, 0).RotatedBy(radians) * Speed;
                counter++;
            }
            if (drawPositionList.Count / 3 < DegradeRange)
                DegradeRange = drawPositionList.Count / 3;
            if (drawPositionList.Count / 6 < GrowthRange)
                GrowthRange = drawPositionList.Count / 6;
            //projectile.velocity = velocity;
            projectile.Center = position;
        }
        float redirectGrowth = 0.0f;
        public float Redirect(float radians, Vector2 pos, Vector2 npc)
        {
            Vector2 toNPC = npc - pos;
            float speed = Speed / 3.5f + redirectGrowth;
            Vector2 rnVelo = new Vector2(Speed, 0).RotatedBy(radians);
            rnVelo += toNPC.SafeNormalize(Vector2.Zero) * speed;
            float npcRad = rnVelo.ToRotation();
            redirectGrowth += 0.2f;
            return npcRad;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
        {
            if (runOnce)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
                    dust2.velocity += projectile.velocity * 0.1f;
                    dust2.noGravity = true;
                    dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 18), true);
                    dust2.noGravity = true;
                    dust2.fadeIn = 0.2f;
                    dust2.scale *= 2.2f;
                }
                SetupLaser();
                Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 72, 1.0f, -0.3f);
                for (int i = 0; i < 10; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
                    dust2.velocity += projectile.velocity * 0.1f;
                    dust2.noGravity = true;
                    dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 18), true);
                    dust2.noGravity = true;
                    dust2.fadeIn = 0.2f;
                    dust2.scale *= 2.2f;
                }
                runOnce = false;
            }
            float endPercent = projectile.timeLeft / 50f;
            projectile.alpha = (int)(255 - 235 * endPercent * endPercent);
        }
	}
}
		
			