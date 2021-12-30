using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Evil;
using SOTS.Projectiles.Inferno;
using SOTS.Projectiles.Tide;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Constructs
{
	public class InfernoSpirit : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[npc.type] = 1;
			DisplayName.SetDefault("Inferno Spirit");
			NPCID.Sets.TrailCacheLength[npc.type] = 5;  
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(phase);
			writer.Write(counter);
			writer.Write(nextDestination.X);
			writer.Write(nextDestination.Y);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			phase = reader.ReadInt32();
			counter = reader.ReadInt32();
			nextDestination.X = reader.ReadSingle();
			nextDestination.Y = reader.ReadSingle();
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 10;
            npc.lifeMax = 4000; 
            npc.damage = 100; 
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 70;
            npc.height = 70;
            npc.value = Item.buyPrice(0, 18, 0, 0);
            npc.npcSlots = 10f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = false;
			npc.rarity = 2;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.damage = 135;
			npc.lifeMax = 6000;
		}
		Vector2 nextDestination = Vector2.Zero;
		private int InitiateHealth = 15000;
		private float ExpertHealthMult = 1.5f; //22500
		public const int ProbeCount = 7;
		public const int timeToFire = 2;
		public const int timeToDash = 45;
		int counter2 = 0;
		int phase = 1;
		int counter = 0;
		bool runOnce = true;
		List<MiniSpirit> miniSpirits = new List<MiniSpirit>();
		public override void AI()
		{
			if(runOnce)
            {
				runOnce = false;
				for (int i = 0; i < ProbeCount; i++)
				{
					miniSpirits.Add(new MiniSpirit(0, i * 360f / ProbeCount));
				}
			}
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.15f / 255f, (255 - npc.alpha) * 0.25f / 255f, (255 - npc.alpha) * 0.65f / 255f);
			Player player = Main.player[npc.target];
			if (phase == 3)
			{
				npc.aiStyle = -1;
				npc.dontTakeDamage = false;
				int damage = npc.damage / 2;
				if (Main.expertMode)
				{
					damage = (int)(damage / Main.expertDamage);
				}
				if((int)npc.ai[0] == 0)
                {
					npc.ai[0] = 1;
                }
				else
                {
					npc.ai[1]++;
					if(npc.ai[1] < (timeToDash * 4))
					{
						for (int i = 0; i < ProbeCount; i++)
						{
							MiniSpirit spirit = miniSpirits[i];
							spirit.offset = MathHelper.Lerp(spirit.offset, 96, 0.06f);
							spirit.rotation -= Direction();
						}
						if(npc.ai[1] >= 0)
						{
							float multiplier = npc.ai[1] / (timeToDash * 4);
							if (nextDestination != Vector2.Zero)
							{
								int rnCounter = (int)npc.ai[1] % timeToDash;
								if (rnCounter == 0)
								{
									Vector2 toPlayer = player.Center - npc.Center;
									nextDestination = npc.Center + toPlayer * (0.25f + 0.8f * multiplier) + Main.rand.NextVector2CircularEdge(160, 160) * (1.0f - 0.8f * multiplier);
									if (Main.netMode != NetmodeID.MultiplayerClient)
										npc.netUpdate = true;
								}
								else
								{
									npc.Center = Vector2.Lerp(npc.Center, nextDestination, 0.06f);
								}
							}
							else
							{
								nextDestination = npc.Center;
								npc.velocity *= 0;
							}
						}
					}
					else if(npc.ai[1] <= (timeToDash * 4) + 30)
					{
						float bonus = (npc.ai[1] - (timeToDash * 4)) / 30f;
						for (int i = 0; i < ProbeCount; i++)
						{
							MiniSpirit spirit = miniSpirits[i];
							spirit.offset = MathHelper.Lerp(96, 60, bonus);
							spirit.rotation -= Direction() * (1.0f + bonus * 2);
						}
					}
					else if(npc.ai[1] > (timeToDash * 4) + 30)
					{
						for (int i = 0; i < ProbeCount; i++)
						{
							MiniSpirit spirit = miniSpirits[i];
							spirit.offset = MathHelper.Lerp(spirit.offset, 60, 0.06f);
							spirit.rotation -= Direction() * 3.0f;
							if((int)npc.ai[3] % ProbeCount == i)
                            {
								float mult = npc.ai[1] % timeToFire / (timeToFire - 1);
								if (mult == 0)
									mult = 1;
								spirit.offset = 60 + 32 * mult;
                            }
						}
						if (npc.ai[1] % timeToFire == 0)
						{
							int i = (int)npc.ai[3] % ProbeCount;
							MiniSpirit spirit = miniSpirits[i];
							Vector2 normal = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(spirit.rotation));
							Projectile.NewProjectile(spirit.getCenter(npc.Center), normal, ModContent.ProjectileType<InfernoBolt>(), damage, 3.5f, Main.myPlayer, Direction(), 0); //lava beam should do a ludicrous amount of damage
							npc.ai[3]++;
						}
					}
					if (npc.ai[1] > (timeToDash * 4) + 240)
					{
						npc.ai[1] = -60;
						npc.ai[2]++;
					}
				}
				counter2++;
				Vector2 idle = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(counter2 * 2));
				npc.velocity *= 0.95f;
				npc.velocity += idle * 0.1f;
			}
			if (phase == 2)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
					npc.netUpdate = true;
				npc.dontTakeDamage = false;
				npc.aiStyle = -1;
				npc.ai[0] = 0;
				npc.ai[1] = -120;
				npc.ai[2] = 0;
				npc.ai[3] = 0;
				phase = 3;
			}
			else if(phase == 1)
			{
				counter++;
			}
			if(phase != 3)
			{
				for (int i = 0; i < ProbeCount; i++)
				{
					MiniSpirit spirit = miniSpirits[i];
					spirit.offset = MathHelper.Lerp(spirit.offset, 60, 0.03f);
					spirit.rotation -= 1.0f;
				}
			}
			if (Main.player[npc.target].dead)
			{
				counter++;
			}
			else if(phase != 1 && counter > 0)
			{
				counter--;
			}
			if(counter >= 1800)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.netUpdate = true;
				}
				phase = 1;
				npc.aiStyle = -1;
				npc.velocity.Y -= 0.014f;
				npc.dontTakeDamage = true;
			}
			int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.RainbowMk2);
			Dust dust = Main.dust[dust2];
			dust.color = new Color(255, 130, 15);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
		}
		public int Direction()
		{
			return (int)npc.ai[2] % 2 * 2 - 1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < npc.oldPos.Length; k++) {
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
				Color color = new Color(150, 80, 70, 0) * ((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, npc.rotation, drawOrigin, npc.scale * 1.1f, SpriteEffects.None, 0f);
			}
			if(!runOnce)
			{
				for (int k = 0; k < ProbeCount; k++)
				{
					MiniSpirit spirit = miniSpirits[k];
					spirit.Draw(spirit.getCenter(npc.Center));
				}
			}
			return false;
		}	
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for(int i = 0; i < 50; i ++)
				{
					Dust dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.RainbowMk2);
					dust.color = new Color(239, 139, 18);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 2f;
					dust.velocity *= 5f;
				}
				if(phase == 1)
				{
					phase = 2;
					npc.lifeMax = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
					npc.life = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
				}
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				Main.spriteBatch.Draw(texture, npc.Center + Main.rand.NextVector2Circular(4f, 4f) - Main.screenPosition, null, color, 0f, drawOrigin, npc.scale * 1.1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<DissolvingNether>(), 1);	
		}	
	}
	public class MiniSpirit
    {
		public float offset;
		public float rotation; 
		public MiniSpirit(float offset, float rotation)
        {
			this.offset = offset;
			this.rotation = rotation;
        }
		public Vector2 getCenter(Vector2 npcCenter)
        {
			return npcCenter + new Vector2(offset, 0).RotatedBy(MathHelper.ToRadians(rotation));
		}
		public void Draw(Vector2 drawLocation)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Constructs/InfernoSpiritMini");
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				Main.spriteBatch.Draw(texture, drawLocation + Main.rand.NextVector2Circular(4f, 4f) - Main.screenPosition, null, color, 0f, drawOrigin, 1.1f, SpriteEffects.None, 0f);
			}
		}
    }
}