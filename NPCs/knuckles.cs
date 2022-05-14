using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs
{[AutoloadBossHead]
	public class knuckles : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Knuckles");
		}
		public override void SetDefaults()
		{
            NPC.aiStyle =14;  
            NPC.lifeMax = 69696969;
            NPC.damage = 4200;
            NPC.defense = 420;
            NPC.knockBackResist = 0f;
            NPC.width = 156;
            NPC.height = 102;
            npc.value = 420;
            npc.npcSlots = 1f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = true;
			//music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/KnucklesTheme");
			//musicPriority = MusicPriority.BossHigh;
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
			NPC.lifeMax = 420420420;
			NPC.damage = 6969;
            base.ScaleExpertStats(numPlayers, bossLifeScale);
        }
        public override void AI()
		{	
			npc.position.Y += Main.rand.Next(-5, 6);
			npc.position.X += Main.rand.Next(-5, 6);
			npc.velocity += Main.rand.NextVector2Circular(0.1f, 0.1f);

			npc.ai[0]++;
			if (npc.ai[0] == 60)
				Main.NewText("WHY ARE YOU RUNNING????", 0, 255, 0);
			if(npc.ai[0] == 120)
				Main.NewText("DO YOU KNOW DA WAE???", 0, 255, 0);
			if(npc.ai[0] == 180)
				Main.NewText("YOU DO NOT KNOW DA WAE!", 0, 255, 0);
			if(npc.ai[0] >= 240)
			{
				Main.NewText("LET US SHOW YOU DA WAE!!!!!!!!!!!!!!!!!!!!!!", 0, 255, 0);
				NPC.SpawnOnPlayer(0, npc.type);
				npc.ai[0] = 0;
			}
			npc.rotation += Main.rand.NextFloat(-100, 100);
			if (Main.player[npc.target].dead)
			{
			   npc.timeLeft = 0;
			}
			else
			   npc.timeLeft = 10000;
		}
	
	}
}





















