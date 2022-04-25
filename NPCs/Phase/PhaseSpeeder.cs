using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Chaos;
using SOTS.Items.Fragments;
using SOTS.Items.Otherworld;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Projectiles.Chaos;
using SOTS.Projectiles.Otherworld;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.Phase
{
	public class PhaseSpeeder : ModNPC
	{
		private float tracerPosX
		{
			get => npc.ai[2];
			set => npc.ai[2] = value;
		}
		private float tracerPosY
		{
			get => npc.ai[3];
			set => npc.ai[3] = value;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phase Speeder");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = -1; 
            npc.lifeMax = 650;   
            npc.damage = 64; 
            npc.defense = 40;  
            npc.knockBackResist = 0f; //take no knockback
            npc.width = 62;
            npc.height = 54;
            npc.value = Item.buyPrice(0, 0, 40, 0);
            npc.npcSlots = 1f;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.lavaImmune = true;
			npc.netAlways = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			banner = npc.type;
			bannerItem = ItemType<TwilightScouterBanner>();
			SetupDebuffImmunities();
		}
		public void SetupDebuffImmunities()
		{
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.buffImmune[BuffID.Frostburn] = true;
			npc.buffImmune[BuffID.Ichor] = true;
			npc.buffImmune[BuffID.BetsysCurse] = true;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			Texture2D texture = Main.npcTexture[npc.type];
			Texture2D texture2 = GetTexture("SOTS/NPCs/Phase/PhaseSpeederGlow");
			Texture2D texture3 = GetTexture("SOTS/NPCs/Phase/PhaseSpeederPink");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 2);
			float dir = npc.rotation;
			bool flip = false;
			if (Math.Abs(MathHelper.WrapAngle(dir)) <= MathHelper.ToRadians(90))
			{
				flip = true;
			}
			float bonusDir = !flip ? MathHelper.ToRadians(180) : 0;
			for(int i = 0; i < 4; i++)
            {
				Vector2 circular = new Vector2(2, 0).RotatedBy(npc.rotation + i * MathHelper.PiOver2);
				Main.spriteBatch.Draw(texture3, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY) + circular, npc.frame, new Color(100, 100, 100, 0) * (0.1f + 0.9f * ((255 - npc.alpha) / 255f)), dir - bonusDir, drawOrigin, npc.scale, !flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, Color.White * ((255 - npc.alpha) / 255f), dir - bonusDir, drawOrigin, npc.scale, !flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
        }
		public void MoveCursorToPlayer()
		{
			Player player = Main.player[npc.target];
			Vector2 between = player.Center - new Vector2(tracerPosX, tracerPosY);
			float length = between.Length();
			float speed = 32f;
			if (speed > length)
			{
				speed = length;
			}
			between = between.SafeNormalize(Vector2.Zero) * speed;
			tracerPosX += between.X;
			tracerPosY += between.Y;
		}
		int direction = 0;
		float spinny = 0;
		public bool runOnce = true;
		public override bool PreAI()
		{
			npc.TargetClosest(false);
			Player player = Main.player[npc.target];
			if (runOnce)
			{
				direction = npc.whoAmI % 2 * 2 - 1;
				tracerPosX = npc.Center.X;
				tracerPosY = npc.Center.Y;
				npc.ai[0] = Main.rand.Next(360);
				if (Main.netMode == NetmodeID.Server)
					npc.netUpdate = true;
				runOnce = false;
			}
			return true;
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			MoveCursorToPlayer();
			npc.ai[1]++;
			//Vector2 toPlayer = player.Center - npc.Center;
			if (npc.ai[1] >= 120)
			{
				if (npc.ai[1] >= 165)
				{
					npc.ai[1]++;
				}
				float scale = (float)Math.Sqrt((npc.ai[1] - 120) / 90f); //make the curve better
				float sinusoid = scale * 6 * (float)Math.Sin(MathHelper.ToRadians(npc.ai[1] - 120) * 6f); // 6 * 90 = 540
				Vector2 rotatePos = toTracer.SafeNormalize(Vector2.Zero) * sinusoid;
				npc.Center += rotatePos;
				if (npc.ai[1] >= 210)
				{
					spinny = 720;
					npc.ai[1] = -120;
					rotatePos = rotatePos.SafeNormalize(Vector2.Zero);
					npc.velocity = rotatePos * 24;
					Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 92);
					return;
				}
			}
			else if (npc.ai[1] > -40)
			{
				if (npc.ai[1] < 30)
                {
					if(npc.ai[1] == -29)
					{
						direction *= -1;
						npc.ai[0] = MathHelper.ToDegrees((npc.Center - player.Center).ToRotation());
					}
					npc.ai[0] += direction;
                }
				else
                {
					npc.alpha -= 8;
                }
				float speed = (float)Math.Sin(MathHelper.ToRadians((npc.ai[1] + 30) * 1.2f)); //finished 180 degree
				npc.ai[0] += speed * direction;
				Vector2 rotatePos = new Vector2(320, 0).RotatedBy(MathHelper.ToRadians(npc.ai[0] * (npc.whoAmI % 2 * 2 - 1))); //rotates cw or ccw depending on index
				Vector2 toPos = rotatePos + tracerPos;
				Vector2 goToPos = npc.Center - toPos;
				float length = goToPos.Length();
				if (length > 12)
				{
					length = 12;
				}
				goToPos = goToPos.SafeNormalize(Vector2.Zero);
				npc.velocity = Vector2.Lerp(npc.velocity, goToPos * -length, 0.05f);
			}
			if(npc.ai[1] < 0)
			{
				if (npc.ai[1] < -80)
				{
					npc.alpha = 0;
				}
				else
					npc.alpha += 8;
				npc.velocity *= 0.9775f;
				if(npc.ai[1] > -100)
					npc.velocity = npc.velocity.RotatedBy(MathHelper.ToRadians(direction * -2.4f));
				npc.rotation = toTracer.ToRotation() + MathHelper.ToRadians(spinny);
				spinny = MathHelper.Lerp(0, 720, (npc.ai[1] / 120f) * (npc.ai[1] / 120f));
				if(Math.Abs(npc.ai[1]) % 10 == 0 && npc.ai[1] < -80 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					int damage = npc.damage / 2;
					if (Main.expertMode)
					{
						damage = (int)(damage / Main.expertDamage);
					}
					for (int i = -2; i <= 2; i++)
                    {
						Vector2 spread = new Vector2(2.5f, 0).RotatedBy(MathHelper.ToRadians(i * 30) + npc.rotation);
						Projectile.NewProjectile(npc.Center + spread.SafeNormalize(Vector2.Zero) * 20, spread, ProjectileType<PhaseDart>(), damage, 0, Main.myPlayer, 0);
					}
                }
			}
			npc.alpha = (int)MathHelper.Clamp(npc.alpha, 0, 255);
			if(npc.alpha > 200)
            {
				npc.dontTakeDamage = true;
            }
			else
				npc.dontTakeDamage = false;
		}
        public override void PostAI()
		{
			if (npc.ai[1] >= 0)
				npc.rotation = toTracer.ToRotation();
		}
		public Vector2 tracerPos => new Vector2(tracerPosX, tracerPosY);
		public Vector2 toTracer => tracerPos - npc.Center;
		public override void NPCLoot()
		{
			if (Main.rand.NextBool(5))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<PhaseOre>(), Main.rand.Next(3) + 3); //drops 3 to 5 ore at a time, since you need quite a lot
			if (Main.rand.NextBool(8))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<FragmentOfChaos>(), 1);
			if (Main.rand.NextBool(12))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<TwilightShard>(), 1);
			if (Main.rand.NextBool(20))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<FragmentOfOtherworld>(), 1);
			if (Main.rand.NextBool(40))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<PhaseBar>(), 1);

		}
        public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while (num < damage / npc.lifeMax * 60.0)
				{
					if(Main.rand.NextBool(3))
						Dust.NewDust(npc.position, npc.width, npc.height, DustID.Gold, (float)(2 * hitDirection), -2f, 0, default, 0.9f);
					else
					{
						Dust.NewDust(npc.position, npc.width, npc.height, DustID.Platinum, (float)(2 * hitDirection), -2f, 0, default, 0.9f);
					}
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 50; k++)
				{
					if (k % 4 == 0)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, 242, (float)(2 * hitDirection), -2f, 0, default, 2f);
					}
					if (Main.rand.NextBool(3))
						Dust.NewDust(npc.position, npc.width, npc.height, DustID.Gold, (float)(2 * hitDirection), -2f, 0, default, 0.9f);
					else
					{
						Dust.NewDust(npc.position, npc.width, npc.height, DustID.Platinum, (float)(2 * hitDirection), -2f, 0, default, 0.9f);
					}
				}
				for (int i = 1; i <= 3; i++)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/PhaseSpeeder/PhaseSpeederGore" + i), 1f);
			}
		}
	}
}