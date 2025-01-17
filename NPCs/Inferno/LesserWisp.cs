using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Fragments;
using SOTS.Items.Inferno;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Inferno;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.Inferno
{
	public class LesserWisp : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lesser Wisp");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = 0; 
            npc.lifeMax = 35;   
            npc.damage = 35; 
            npc.defense = 16;  
            npc.knockBackResist = 0.5f;
            npc.width = 26;
            npc.height = 32;
			Main.npcFrameCount[npc.type] = 1;  
            npc.value = 50;
            npc.npcSlots = 0.25f;
            npc.HitSound = SoundID.NPCHit30;
            npc.DeathSound = SoundID.NPCDeath6;
			npc.lavaImmune = true;
			npc.netAlways = true;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.Frostburn] = true;
			npc.buffImmune[BuffID.Ichor] = true;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.ai[0] = 0;
			npc.ai[2] = 1;
			banner = npc.type;
			bannerItem = ItemType<LesserWispBanner>();
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
        public bool sans = false;
		public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			if(sans)
			{
				if (npc.ai[3] >= 24)
				{
					damage = 999999 + defense / 2;
					crit = false;
					return true;
				}
				else
				{
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), new Color(125, 125, 125, 125), "MISS", false, false);
					damage = 0;
					npc.ai[3]++;
					npc.netUpdate = true;
				}
				return false;
			}
			return true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = GetTexture("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color;
			for (int i = 0; i < particleList.Count; i++)
			{
				color = new Color(255, 69, 0, 0);
				if(sans)
					color = new Color(60, 90, 115, 0);
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = npc.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.1f, SpriteEffects.None, 0f);
				}
			}
			texture = sans ? GetTexture("SOTS/NPCs/Inferno/SansWispOutline") : GetTexture("SOTS/NPCs/Inferno/LesserWispOutline");
			drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			color = sans ? new Color(50, 50, 90, 0) : new Color(80, 80, 80, 0);
			for (int k = 0; k < 12; k++)
			{
				Vector2 drawPos = npc.Center - Main.screenPosition;
				Vector2 circular = new Vector2(Main.rand.NextFloat(0, 3), 0).RotatedBy(Math.PI / 6 * k);
				Main.spriteBatch.Draw(texture, drawPos + circular, null, color * 0.9f, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			}
			texture = sans ? GetTexture("SOTS/NPCs/Inferno/SansWisp") : Main.npcTexture[npc.type];
			Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, sans ? Color.White : new Color(180, 180, 180), npc.rotation, drawOrigin, npc.scale * 0.9f, SpriteEffects.None, 0f);
			return false;
		}
        public override bool PreAI()
        {
			if(npc.ai[0] == 0)
            {
				if(Main.netMode != 1)
				{
					npc.ai[1] = -240;
					if (Main.rand.NextBool(100))
                    {
						npc.ai[2] = -1;
						npc.ai[1] = -120;
					}
					else
						for (int i = 0; i < Main.rand.Next(2, 4); i++)
							NPC.NewNPC((int)npc.Center.X, (int)npc.position.Y + npc.height, this.npc.type, 0, -1, -Main.rand.Next(120) - 240);
					npc.netUpdate = true;
				}
				npc.ai[0] = -1;
			}
			if(npc.ai[2] == -1)
            {
				sans = true;
				npc.HitSound = SoundID.DD2_SkeletonHurt;
				npc.DeathSound = SoundID.DD2_SkeletonDeath;
			}
			else
            {
				sans = false;
				npc.HitSound = SoundID.NPCHit30;
				npc.DeathSound = SoundID.NPCDeath6;
			}
			if(Main.netMode != NetmodeID.Server)
			{
				for (int i = 0; i < (SOTS.Config.lowFidelityMode ? 1 : 1 + Main.rand.Next(2)); i++)
				{
					Vector2 rotational = new Vector2(0, -4.4f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-20f, 20f)));
					rotational.X *= 0.5f;
					rotational.Y *= 1f;
					particleList.Add(new FireParticle(npc.Center, rotational, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(1.2f, 1.6f)));
				}
				cataloguePos();
			}
			if (Main.rand.NextBool(20))
			{
				Color color = new Color(155, 69, 0, 0);
				if (sans)
					color = new Color(80, 150, 200, 0);
				int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) - new Vector2(5), npc.width, npc.height, 267);
				Dust dust = Main.dust[dust2];
				dust.color = color;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
			}
			return base.PreAI();
		}
		List<FireParticle> particleList = new List<FireParticle>();
		public void cataloguePos()
		{
			for (int i = 0; i < particleList.Count; i++)
			{
				FireParticle particle = particleList[i];
				particle.Update();
				if (!particle.active)
				{
					particleList.RemoveAt(i);
					i--;
				}
			}
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			int orbital = modPlayer.orbitalCounter;
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				NPC other = Main.npc[i];
				if (npc.type == other.type && other.active && npc.active && npc.target == other.target)
				{
					if (npc == other)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					total++;
				}
			}
			npc.localAI[1]++;
			npc.rotation = npc.velocity.X * 0.03f;

			float dynamicAddition = (float)Math.Sin(MathHelper.ToRadians(npc.localAI[1] * 2)) * 6;
			Vector2 circular = player.Center + new Vector2(0, dynamicAddition) + new Vector2(156 + 4 * total, 0).RotatedBy(MathHelper.ToRadians(orbital * 0.75f + (360f * ofTotal / total)));
			Vector2 toPlayer = circular - npc.Center;
			float speed = 7.8f;
			if (sans)
				speed = 20f;
			float length = toPlayer.Length();
			if(length < speed)
            {
				speed = length;
            }
			npc.velocity *= 0.5f;
			npc.velocity += toPlayer.SafeNormalize(Vector2.Zero) * speed * 0.5f;
			npc.ai[1]++;
			if (npc.ai[1] > 0)
            {
				float num = npc.ai[1];
				float chargeMult = (40f - Math.Abs(num)) / 40f;
				float min = 0;
				if (sans)
					min = 0.2f;
				if (chargeMult < min)
				{
					chargeMult = min;
				}
				npc.velocity *= chargeMult;
				if (npc.ai[1] == 40 || (sans && npc.ai[1] > 40 && npc.ai[1] % 8 == 0))
                {
					if(Main.netMode != 1)
					{
						toPlayer = player.Center - npc.Center;
						toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
						int Damage = npc.damage / 2;
						if (Main.expertMode)
						{
							Damage = (int)(Damage / Main.expertDamage);
						}
						if(sans)
							Damage = (int)(Damage * 1.5f);
						if(!sans)
						{
							Projectile.NewProjectile(npc.Center + toPlayer * 40, toPlayer * 5, ProjectileType<LesserWispLaser>(), Damage, 1f, Main.myPlayer, 0, 0);
						}
						else
						{
							Vector2 spawnPos = npc.Center + toPlayer * 40;
							Vector2 velo = toPlayer * 5;
							int randMod1 = Main.rand.Next(4);
							int randMod2 = Main.rand.Next(11);
							if(randMod1 == 0)
								velo = velo.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-25, 25)));
							if (randMod1 == 1 || randMod2 == 4)
								spawnPos += new Vector2(Main.rand.NextFloat(-350, 350), Main.rand.NextFloat(-350, 350));
							if(randMod2 <= 1)
                            {
								for(int i = -1; i < 2; i++)
								{
									Projectile.NewProjectile(spawnPos, velo.RotatedBy(MathHelper.ToRadians(22.5f * i)), ProjectileType<LesserWispLaser>(), Damage, 1f, Main.myPlayer, 0, -1);
									if (randMod1 == 0)
										velo = velo.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-25, 25)));
								}
                            }
							else if(randMod2 == 10)
							{
								for (int i = 0; i < 8; i++)
								{
									Projectile.NewProjectile(spawnPos, velo.RotatedBy(MathHelper.ToRadians(45 * i)), ProjectileType<LesserWispLaser>(), Damage, 1f, Main.myPlayer, 0, -1);
									if (randMod1 == 0)
										velo = velo.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-25, 25)));
								}
							}
							else
								Projectile.NewProjectile(spawnPos, velo, ProjectileType<LesserWispLaser>(), Damage, 1f, Main.myPlayer, 0, -1);
						}
					}
                }
				if (npc.ai[1] >= 80)
                {
					if (sans)
						npc.ai[1] = -120;
					else
						npc.ai[1] = -(Main.rand.Next(35, 80 + 20 * total) + total * 10);
					if (Main.netMode != 1)
						npc.netUpdate = true;
				}
            }
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void NPCLoot()
		{ 
			if(sans)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<BookOfVirtues>(), 1);
			if (Main.rand.NextBool(15))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, sans ? ItemID.LivingUltrabrightFireBlock : ItemID.LivingFireBlock, Main.rand.Next(10, 21));
			else if (Main.rand.NextBool(25))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, sans ? ItemType<FragmentOfChaos>() : ItemType<FragmentOfInferno>(), 1);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 20.0)
				{
					Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, sans ? 26 : DustID.Fire, (float)(2 * hitDirection), -2f, 0, default, 0.5f);
					dust.scale *= sans? 1.1f : 1.75f;
					num++;
				}
			}
			else
			{
				for (int i = 0; i < particleList.Count; i++)
				{
					Color color = new Color(155, 69, 0, 0);
					if (sans)
						color = new Color(80, 150, 200, 0);
					FireParticle particle = particleList[i];
					Dust dust = Dust.NewDustDirect(new Vector2(particle.position.X - 4, particle.position.Y - 4), 4, 4, DustType<CopyDust4>());
					dust.noGravity = true;
					dust.velocity *= 1.35f;
					dust.scale = particleList[i].scale * 1.25f + 0.25f;
					dust.fadeIn = 0.1f;
					dust.color = color;
				}
				for (int k = 0; k < 30; k++)
				{
					Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, sans ? 26 : DustID.Fire, (float)(2 * hitDirection), -2f, 0, default, 1f);
					dust.scale *= sans ? 1.25f : 2.5f;
				}
			}
		}
	}
}