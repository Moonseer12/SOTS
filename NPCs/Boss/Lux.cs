using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.NPCs.Constructs;
using SOTS.Projectiles.Chaos;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{
	[AutoloadBossHead]
	public class Lux : ModNPC
	{
		List<RingManager> rings = new List<RingManager>();
		private float wingCounter
		{
			get => npc.ai[0];
			set => npc.ai[0] = value;
		}

		private float attackPhase
		{
			get => npc.ai[1];
			set => npc.ai[1] = value;
		}

		private float attackTimer1
		{
			get => npc.ai[2];
			set => npc.ai[2] = value;
		}

		private float attackTimer2
		{
			get => npc.ai[3];
			set => npc.ai[3] = value;
		}
		float wingSpeedMult = 1f;
		float wingHeight; //wings offset in degrees
		float drawNewWingsCounter = 0;
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < npc.oldPos.Length; k++)
			{
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
				Color color = VoidPlayer.pastelRainbow * ((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length);
				color.A = 0;
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, npc.rotation, drawOrigin, npc.scale * 1.1f, SpriteEffects.None, 0f);
			}
			float bonusScale = drawNewWingsCounter * 0.1f;
			float bonusWidth = drawNewWingsCounter * 16f;
			float bonusDegree = drawNewWingsCounter * -13f;
			DrawRings(spriteBatch, false);
			DrawWings(1f + bonusScale, bonusWidth, bonusDegree, 1f);
			if(drawNewWingsCounter > 0)
            {
				DrawWings((1f + bonusScale) * 1.25f, bonusWidth + 12, bonusDegree + 36, drawNewWingsCounter);
				DrawWings((1f + bonusScale) * 0.75f, bonusWidth - 18, bonusDegree - 36, drawNewWingsCounter);
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				Color color = new Color(100, 100, 100, 0);
				Vector2 circular = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount));
				if (k != 0)
				{
					color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60));
					color.A = 0;
				}
				else
					circular *= 0f;
				Main.spriteBatch.Draw(texture, npc.Center + circular - Main.screenPosition, null, color, 0f, drawOrigin, npc.scale * 1.1f, SpriteEffects.None, 0f);
			}
			DrawRings(spriteBatch, true);
		}
		public void WingStuff()
		{
			wingCounter += 7.5f * wingSpeedMult;
			float dipAndRise = (float)Math.Sin(MathHelper.ToRadians(wingCounter));
			//dipAndRise *= (float)Math.sqrt(dipAndRise);
			wingHeight = 19 + dipAndRise * 27;
		}
		public void DrawWings(float sizeMult = 1f, float widthOffset = 0, float degreeOffset = 0, float genPercent = 1f)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Constructs/ChaosParticle");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float dipAndRise;
			float supposedWingHeight = wingHeight - 19;
			dipAndRise = supposedWingHeight / 27f;
			dipAndRise = MathHelper.Clamp(dipAndRise, -1, 1);
			int width = (int)(130 * sizeMult);
			int height = (int)(90 * sizeMult);
			int amtOfParticles = 120;
			for (int j = -1; j <= 1; j += 2)
			{
				float positionalRotation = npc.rotation + MathHelper.ToRadians((wingHeight + degreeOffset) * -j);
				Vector2 position = npc.Center + new Vector2(-28 * j * (1 - drawNewWingsCounter), 16).RotatedBy(npc.rotation) + new Vector2(((width + npc.width) / 2 + 54 + widthOffset) * j, 0).RotatedBy(positionalRotation);
				float degreesCount = 0f;
				float flapMult = 0.0f;
				float baseGrowth = 0.35f;
				float scaleGrowth = 0.15f;
				float totalSin = 450f;
				if (dipAndRise < 0)
				{
					baseGrowth = MathHelper.Lerp(baseGrowth, 0.2f, -dipAndRise);
					scaleGrowth = MathHelper.Lerp(scaleGrowth, 0.3f, -dipAndRise);
					totalSin = MathHelper.Lerp(totalSin, 405, (float)Math.Pow((-dipAndRise), 1.2f));
				}
				else
				{
					baseGrowth = MathHelper.Lerp(baseGrowth, 0.1f, dipAndRise);
					scaleGrowth = MathHelper.Lerp(scaleGrowth, 0.05f, dipAndRise);
					flapMult = MathHelper.Lerp(scaleGrowth, 0.3f, (float)Math.Pow(dipAndRise, 1.2f));
				}
				for (float i = 0; i < amtOfParticles;)
				{
					float half = amtOfParticles / 2f;
					if((i <= half * genPercent && i < half) || (i > half && i > amtOfParticles - half * genPercent))
					{
						float sinusoid = (float)Math.Sin(MathHelper.ToRadians(degreesCount));
						if (degreesCount < 0)
							sinusoid = 0;
						float radians = MathHelper.ToRadians(i * 360f / amtOfParticles);
						Color c = VoidPlayer.pastelAttempt(radians + MathHelper.ToRadians(Main.GameUpdateCount));
						Vector2 circular = new Vector2(-1, 0).RotatedBy(radians);
						float increaseAmount = 1f;
						if (i < amtOfParticles / 2)
						{
							degreesCount = MathHelper.Lerp(0, totalSin, (float)Math.Pow(i / amtOfParticles * 2f, 1.2f));
							circular.Y *= flapMult;
							circular.Y -= sinusoid * (baseGrowth + scaleGrowth * i / amtOfParticles);
						}
						else
						{
							float mult = (1 - (i - 60f) / amtOfParticles * 7f);
							if (mult < 0)
								mult = 0;
							circular.Y -= (float)Math.Sin(MathHelper.ToRadians(totalSin)) * (scaleGrowth + baseGrowth) * mult;
							if (circular.Y > 0)
							{
								sinusoid = (float)Math.Sin(MathHelper.ToRadians(Main.GameUpdateCount * 20f + i * 36));
								if (sinusoid < 0.0f)
								{
									if (sinusoid > -0.2f)
										increaseAmount = 0.35f;
									else if (sinusoid > -0.4f)
										increaseAmount = 0.5f;
									else if (sinusoid > -0.6f)
										increaseAmount = 0.75f;
									sinusoid = 0.0f;
								}
								else
								{
									increaseAmount = 0.25f;
								}
								circular.Y *= 1f + sinusoid * 0.4f;
							}
						}
						i += increaseAmount;
						circular.X *= width / 2 * j;
						circular.Y *= height / 2;
						circular = circular.RotatedBy(positionalRotation);
						Main.spriteBatch.Draw(texture, position - Main.screenPosition + circular, null, new Color(c.R, c.G, c.B, 0), radians * j, origin, npc.scale * 0.8f * (0.5f + 0.5f * (float)Math.Sqrt(increaseAmount)), SpriteEffects.None, 0f);
					}
					else
                    {
						i++;
                    }
				}
			}
		}
		public void DrawRings(SpriteBatch spriteBatch, bool front = false)
        {
			if (runOnce)
				return;
			for(int i = 0; i < rings.Count; i++)
            {
				rings[i].Draw(spriteBatch, npc.Center, 1f, drawNewWingsCounter, 1f, npc.rotation, front);
            }
        }
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[npc.type] = 1;
			DisplayName.SetDefault("Lux");
			NPCID.Sets.TrailCacheLength[npc.type] = 10;  
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 0;
            npc.lifeMax = 60000; 
            npc.damage = 100; 
            npc.defense = 30;   
            npc.knockBackResist = 0f;
            npc.width = 70;
            npc.height = 70;
            npc.value = Item.buyPrice(0, 20, 0, 0);
            npc.npcSlots = 10f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = false;
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return attackPhase != -1;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * bossLifeScale * 0.75f);
			npc.damage = (int)(npc.damage * 0.75f); //150 damage
		}
		bool runOnce = true;
        public override bool PreAI()
		{
			Player player = Main.player[npc.target];
			npc.TargetClosest(false);
			for (int i = 0; i < 4; i++)
            {
				if (runOnce)
					rings.Add(new RingManager(MathHelper.PiOver2 * i, 0.6f, 4 - (i / 2), 60 + (i / 2) * 24));
				else
					rings[i].CalculationStuff();
			}
			if (runOnce)
            {
				npc.dontTakeDamage = true;
				attackTimer1 = -20;
				attackPhase = -1;
				runOnce = false;
			}
			if (attackPhase == -1)
			{
				npc.velocity *= 0.95f;
				attackTimer1++;
				if (attackTimer1 > 60)
				{
					npc.dontTakeDamage = true;
					float mult = (attackTimer1 - 60f) / 60f;
					mult = MathHelper.Clamp(mult, 0, 1f);
					wingSpeedMult = MathHelper.Lerp(2f, 1.0f, mult);
					npc.Center += new Vector2(0, (float)Math.Sin(MathHelper.ToRadians(attackTimer1 * 12)) * 12f * (1 - mult));
					if(attackTimer1 > 120)
                    {
						//swap phases
                    }
				}
				else if(attackTimer1 > 0)
				{
					float mult = attackTimer1 / 60f;
					mult = MathHelper.Clamp(mult, 0, 1f);
					wingSpeedMult = MathHelper.Lerp(0f, 2f, mult);
					drawNewWingsCounter = mult;
					if (attackTimer1 > 0)
						npc.Center += new Vector2(0, (float)Math.Sin(MathHelper.ToRadians(attackTimer1 * 12)) * 12f * mult);
				}
				else
                {
					wingSpeedMult = MathHelper.Lerp(wingSpeedMult, 0f, 0.06f);
                }
				Vector2 toPlayer = player.Center - npc.Center;
				npc.velocity += 0.2f * toPlayer.SafeNormalize(Vector2.Zero);
			}
			WingStuff();
			npc.rotation = npc.velocity.X * 0.06f;
			return true;
		}
		public override void AI()
		{
			int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.RainbowMk2);
			Dust dust = Main.dust[dust2];
			dust.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), true);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
		}
        public override void PostAI()
        {

        }
        public void DoPassiveMovement(Vector2 center)
		{
			float sinusoid = (float)Math.Sin(MathHelper.ToRadians(npc.ai[3]));
			Vector2 offset = new Vector2(0, -300).RotatedBy(MathHelper.ToRadians(sinusoid * 30));
			offset.X *= 1.5f;
			Vector2 position = center + offset;
			float verticalOffset = 40 + 40 * (float)Math.Sin(MathHelper.ToRadians(npc.ai[1] * 2f));
			position.Y -= verticalOffset;
			Vector2 goTo = position - npc.Center;
			float speed = 13f;
			if (goTo.Length() < speed)
			{
				speed = goTo.Length();
			}
			npc.velocity *= 0.3f;
			npc.velocity += 0.6f * speed * goTo.SafeNormalize(Vector2.Zero);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				if (Main.netMode != NetmodeID.Server)
				{
					for (int i = 0; i < 50; i++)
					{
						Dust dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.RainbowMk2);
						dust.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), true);
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 2.2f;
						dust.velocity *= 4f;
					}
				}
			}
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<DissolvingBrilliance>(), 3);	
		}
	}
	public class RingManager
	{
		public float nextRotation = 0f;
		public float nextCompression = 0f;
		public float prevRotation = 0f;
		public float prevCompression = 0f;
		public float rotation = 0f;
		public float compression = 0f;
		public float radius = 32f;
		public float counter = -1;
		public float countingSpeed = 3f;
		public RingManager(float rotation, float compression, float countingSpeed = 3f, float radius = 32f)
		{
			this.rotation = rotation;
			this.compression = compression;
			this.radius = radius;
			this.countingSpeed = countingSpeed;
		}
		public void CalculationStuff()
		{
			if (counter <= 0)
			{
				counter = 0;
				nextRotation = Main.rand.NextFloat(-1 * (float)Math.PI, (float)Math.PI);
				nextCompression = Main.rand.NextFloat(0, 1);
				prevRotation = rotation;
				prevCompression = compression;
			}
			if (counter < 180)
				counter += countingSpeed;
			float scale = 0.5f - 0.5f * (float)Math.Cos(MathHelper.ToRadians(counter));
			if (counter >= 180)
			{
				counter -= 180;
			}
			rotation = MathHelper.Lerp(prevRotation, nextRotation, scale);
			compression = MathHelper.Lerp(prevCompression, nextCompression, scale);
		}
		public void Draw(SpriteBatch spriteBatch, Vector2 Center, float alphaMult = 1f, float radiusMult = 1f, float sizeMult = 1f, float baseRotation = 0f, bool front = false)
		{
			Texture2D texture = Main.projectileTexture[ModContent.ProjectileType<ChaosSphere>()];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			int start = 0;
			int end = 180;
			if (front)
			{
				start += 180;
				end += 180;
			}
			for (int i = start; i < end; i += 4)
			{
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i));
				Vector2 center = Center;
				Vector2 rotationV = new Vector2(radius * radiusMult, 0).RotatedBy(MathHelper.ToRadians(i + Main.GameUpdateCount));
				rotationV.X *= compression;
				rotationV = rotationV.RotatedBy(rotation);
				spriteBatch.Draw(texture, center - Main.screenPosition + rotationV, null, new Color(color.R, color.G, color.B, 0) * alphaMult, baseRotation, drawOrigin, 0.8f * sizeMult, SpriteEffects.None, 0f);
			}
		}
	}
}