using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Void;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SOTS.Items.Otherworld.Furniture
{
	internal class SkyPots : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.StyleWrapLimit = 3;
			TileObjectData.newTile.RandomStyleRange = 9;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Pot");
            AddMapEntry(new Color(66, 77, 93), name);
            dustType = DustType<AvaritianDust>();
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            float uniquenessCounter = Main.GlobalTime * -100 + (i + j) * 5;
            Tile tile = Main.tile[i, j];
            Texture2D texture = mod.GetTexture("Items/Otherworld/Furniture/SkyPotsGlow");
            Rectangle frame = new Rectangle(tile.frameX, tile.frameY, 16, 16);
            Color color;
            color = WorldGen.paintColor((int)Main.tile[i, j].color()) * (100f / 255f);
            color.A = 0;
            float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            for (int k = 0; k < 5; k++)
            {
                Vector2 pos = new Vector2((i * 16 - (int)Main.screenPosition.X), (j * 16 - (int)Main.screenPosition.Y)) + zero;
                Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.10f * k;
                Main.spriteBatch.Draw(texture, pos + offset, frame, color * alphaMult * 0.75f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 8;
        }
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			//Projectile.NewProjectile((float)(k * 16) + 15.5f, (float)(num4 * 16 + 16), 0f, 0f, 99, 70, 10f, Main.myPlayer, 0f, 0f);
			if (!WorldGen.gen)
			{
                //Projectile.NewProjectile((i + 1.5f) * 16f, (j + 1.5f) * 16f, 0f, 0f, ProjectileID.Boulder, 70, 10f, Main.myPlayer, 0f, 0f);
                PotDrops(i, j, frameX, frameY);
                //Item.NewItem(i * 16, j * 16, 32, 32, ItemType<Pots>());
            }

		}
        public void PotDrops(int i, int j, int frameX, int frameY)
        {
            Main.PlaySound(SoundID.Shatter, i * 16, j * 16, 1, 1f, 0.0f);
            Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore1"), 1f);
            Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore2"), 1f);
            int num = 0;
            if(frameY < 36)
            {
                num = 1;
                if(frameX >= 36)
                {
                    num = 2;
                    if(frameX >= 72)
                    {
                        num = 3;
                    }
                }
            }
            else if (frameY < 72)
            {
                num = 4;
            }
            else
            {
                num = 5;
            }
            if(num == 1) //star pot
            {
                if(Main.rand.NextBool(2))
                   Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Star, 1, false, 0, false, false);
                Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore6"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore3"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore4"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore5"), 1f);
            }
            if (num == 2) //heart pot
            {
                if (Main.rand.NextBool(2))
                    Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Heart, 1, false, 0, false, false);
                Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore7"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore3"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore4"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore5"), 1f);
            }
            if (num == 3) //void pot?
            {
                Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore8"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore3"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore4"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore5"), 1f);
            }
            if (num == 4)
            {
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore9"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore10"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore11"), 1f);
                if (Main.rand.NextBool(4))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore3"), 1f);
                if (Main.rand.NextBool(4))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore4"), 1f);
                if (Main.rand.NextBool(4))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore5"), 1f);
            }
            if (num == 5)
            {
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore12"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore13"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore14"), 1f);
                if (Main.rand.NextBool(4))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore3"), 1f);
                if (Main.rand.NextBool(4))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore4"), 1f);
                if (Main.rand.NextBool(4))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/Pots/SkyPotGore5"), 1f);
            }



            int maxValue = 350;
            if (Main.rand.Next(maxValue) == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile((float)(i * 16 + 16), (float)(j * 16 + 16), 0.0f, -12f, ProjectileID.CoinPortal, 0, 0.0f, Main.myPlayer, 0.0f, 0.0f);
            }
            else if (WorldGen.genRand.Next(40) == 0 && Main.wallDungeon[(int)Main.tile[i, j].wall])
                Item.NewItem(i * 16, j * 16, 16, 16, 327, 1, false, 0, false, false);
            else if (WorldGen.genRand.Next(35) == 0 || Main.rand.Next(35) == 0 && Main.expertMode)
            {
                int type = Main.rand.Next(4);
                if (type == 0)
                {
                    int num8 = WorldGen.genRand.Next(10);
                    if (num8 == 0)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.IronskinPotion, 1, false, 0, false, false);
                    if (num8 == 1)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.ShinePotion, 1, false, 0, false, false);
                    if (num8 == 2)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.NightOwlPotion, 1, false, 0, false, false);
                    if (num8 == 3)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.SwiftnessPotion, 1, false, 0, false, false);
                    if (num8 == 4)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.MiningPotion, 1, false, 0, false, false);
                    if (num8 == 5)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.CalmingPotion, 1, false, 0, false, false);
                    if (num8 == 6)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.BuilderPotion, 1, false, 0, false, false);
                    if (num8 >= 7)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.RecallPotion, 1, false, 0, false, false);
                }
                else if (type == 1)
                {
                    int num8 = WorldGen.genRand.Next(11);
                    if (num8 == 0)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.RegenerationPotion, 1, false, 0, false, false);
                    if (num8 == 1)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.ShinePotion, 1, false, 0, false, false);
                    if (num8 == 2)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.NightOwlPotion, 1, false, 0, false, false);
                    if (num8 == 3)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.SwiftnessPotion, 1, false, 0, false, false);
                    if (num8 == 4)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.ArcheryPotion, 1, false, 0, false, false);
                    if (num8 == 5)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.GillsPotion, 1, false, 0, false, false);
                    if (num8 == 6)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.HunterPotion, 1, false, 0, false, false);
                    if (num8 == 7)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.MiningPotion, 1, false, 0, false, false);
                    if (num8 == 8)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.TrapsightPotion, 1, false, 0, false, false);
                    if (num8 >= 9)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.RecallPotion, 1, false, 0, false, false);
                }
                else if (type == 2)
                {
                    int num8 = WorldGen.genRand.Next(15);
                    if (num8 == 0)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.SpelunkerPotion, 1, false, 0, false, false);
                    if (num8 == 1)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.FeatherfallPotion, 1, false, 0, false, false);
                    if (num8 == 2)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.NightOwlPotion, 1, false, 0, false, false);
                    if (num8 == 3)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.WaterWalkingPotion, 1, false, 0, false, false);
                    if (num8 == 4)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.ArcheryPotion, 1, false, 0, false, false);
                    if (num8 == 5)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.GravitationPotion, 1, false, 0, false, false);
                    if (num8 == 6)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.ThornsPotion, 1, false, 0, false, false);
                    if (num8 == 7)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.WaterWalkingPotion, 1, false, 0, false, false);
                    if (num8 == 8)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.InvisibilityPotion, 1, false, 0, false, false);
                    if (num8 == 9)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.HunterPotion, 1, false, 0, false, false);
                    if (num8 == 10)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.MiningPotion, 1, false, 0, false, false);
                    if (num8 == 11)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.HeartreachPotion, 1, false, 0, false, false);
                    if (num8 == 12)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.FlipperPotion, 1, false, 0, false, false);
                    if (num8 == 13)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.TrapsightPotion, 1, false, 0, false, false);
                    if (num8 == 14)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.RecallPotion, 1, false, 0, false, false);
                }
                else
                {
                    int num8 = WorldGen.genRand.Next(14);
                    if (num8 == 0)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.SpelunkerPotion, 1, false, 0, false, false);
                    if (num8 == 1)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.FeatherfallPotion, 1, false, 0, false, false);
                    if (num8 == 2)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.ManaRegenerationPotion, 1, false, 0, false, false);
                    if (num8 == 3)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.ObsidianSkinPotion, 1, false, 0, false, false);
                    if (num8 == 4)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.MagicPowerPotion, 1, false, 0, false, false);
                    if (num8 == 5)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.InvisibilityPotion, 1, false, 0, false, false);
                    if (num8 == 6)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.HunterPotion, 1, false, 0, false, false);
                    if (num8 == 7)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.GravitationPotion, 1, false, 0, false, false);
                    if (num8 == 8)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.ThornsPotion, 1, false, 0, false, false);
                    if (num8 == 9)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.WaterWalkingPotion, 1, false, 0, false, false);
                    if (num8 == 10)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.ObsidianSkinPotion, 1, false, 0, false, false);
                    if (num8 == 11)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.BattlePotion, 1, false, 0, false, false);
                    if (num8 == 12)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.HeartreachPotion, 1, false, 0, false, false);
                    if (num8 == 13)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.TitanPotion, 1, false, 0, false, false);
                }
            }
            else if (Main.netMode == NetmodeID.Server && Main.rand.Next(30) == 0)
            {
                Item.NewItem(i * 16, j * 16, 16, 16, 2997, 1, false, 0, false, false);
            }
            else if(Main.rand.Next(50) == 0 || (num == 3 && Main.rand.Next(10) == 0))
            {
                Item.NewItem(i * 16, j * 16, 16, 16, ItemType<DigitalCornSyrup>(), 1, false, 0, false, false);
            }
            else
            {
                int num8 = Main.rand.Next(8);
                if (Main.expertMode)
                    --num8;
                if (num8 == 0 && Main.player[(int)Player.FindClosest(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16)].statLife < Main.player[(int)Player.FindClosest(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16)].statLifeMax2)
                {
                    Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Heart, 1, false, 0, false, false);
                    if (Main.rand.Next(2) == 0)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Heart, 1, false, 0, false, false);
                    if (Main.expertMode)
                    {
                        if (Main.rand.Next(2) == 0)
                            Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Heart, 1, false, 0, false, false);
                        if (Main.rand.Next(2) == 0)
                            Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Heart, 1, false, 0, false, false);
                    }
                }
                else if (num8 == 1 && Main.player[(int)Player.FindClosest(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16)].statMana < Main.player[(int)Player.FindClosest(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16)].statManaMax2)
                    Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Star, 1, false, 0, false, false);
                else if (num8 == 2)
                {
                    int Stack = Main.rand.Next(2, 6);
                    if (Main.expertMode)
                        Stack += Main.rand.Next(1, 7);
                    if ((int)Main.tile[i, j].liquid > 0)
                        Item.NewItem(i * 16, j * 16, 16, 16, 282, Stack, false, 0, false, false);
                    else
                        Item.NewItem(i * 16, j * 16, 16, 16, 8, Stack, false, 0, false, false);
                }
                else if (num8 == 3)
                {
                    int Stack = Main.rand.Next(10, 21);
                    int Type = ItemID.MeteorShot;
                    Item.NewItem(i * 16, j * 16, 16, 16, Type, Stack, false, 0, false, false);
                }
                else if (num8 == 4)
                {
                    int Type = ItemID.HealingPotion;
                    int Stack = 1;
                    if (Main.expertMode && Main.rand.Next(3) != 0)
                        ++Stack;
                    if(NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3)
                    {
                        if (Main.rand.Next(5) < 2)
                        {
                            Type = ItemID.GreaterHealingPotion;
                        }
                        else if(NPC.downedMoonlord)
                        {
                            Type = ItemID.SuperHealingPotion;
                        }
                    }

                    Item.NewItem(i * 16, j * 16, 16, 16, Type, Stack, false, 0, false, false);
                }
                else if (num8 == 5)
                {
                    int Stack = Main.rand.Next(10, 21);
                    int Type = ItemID.JestersArrow;
                    Item.NewItem(i * 16, j * 16, 16, 16, Type, Stack, false, 0, false, false);
                }
                else if (num8 == 6 && Main.rand.Next(5) == 0)
                {
                    int Stack = Main.rand.Next(20, 41);
                    Item.NewItem(i * 16, j * 16, 16, 16, ItemID.SilkRope, Stack, false, 0, false, false);
                }
                else
                {
                    float num11 = (float)(200 + WorldGen.genRand.Next(-100, 101));
                    float num12 = num11 * (float)(1.0 + (double)Main.rand.Next(-20, 21) * 0.01);
                    if (Main.rand.Next(4) == 0)
                        num12 *= (float)(1.0 + (double)Main.rand.Next(5, 11) * 0.01);
                    if (Main.rand.Next(8) == 0)
                        num12 *= (float)(1.0 + (double)Main.rand.Next(10, 21) * 0.01);
                    if (Main.rand.Next(12) == 0)
                        num12 *= (float)(1.0 + (double)Main.rand.Next(20, 41) * 0.01);
                    if (Main.rand.Next(16) == 0)
                        num12 *= (float)(1.0 + (double)Main.rand.Next(40, 81) * 0.01);
                    if (Main.rand.Next(20) == 0)
                        num12 *= (float)(1.0 + (double)Main.rand.Next(50, 101) * 0.01);
                    if (Main.expertMode)
                        num12 *= 2.5f;
                    if (Main.expertMode && Main.rand.Next(2) == 0)
                        num12 *= 1.25f;
                    if (Main.expertMode && Main.rand.Next(3) == 0)
                        num12 *= 1.5f;
                    if (Main.expertMode && Main.rand.Next(4) == 0)
                        num12 *= 1.75f;
                    float num13 = num12;
                    if (NPC.downedBoss1)
                        num13 *= 1.1f;
                    if (NPC.downedBoss2)
                        num13 *= 1.1f;
                    if (NPC.downedBoss3)
                        num13 *= 1.1f;
                    if (NPC.downedMechBoss1)
                        num13 *= 1.1f;
                    if (NPC.downedMechBoss2)
                        num13 *= 1.1f;
                    if (NPC.downedMechBoss3)
                        num13 *= 1.1f;
                    if (NPC.downedPlantBoss)
                        num13 *= 1.1f;
                    if (NPC.downedQueenBee)
                        num13 *= 1.1f;
                    if (NPC.downedGolemBoss)
                        num13 *= 1.1f;
                    if (NPC.downedPirates)
                        num13 *= 1.1f;
                    if (NPC.downedGoblins)
                        num13 *= 1.1f;
                    if (NPC.downedFrost)
                        num13 *= 1.1f;
                    while ((int)num13 > 0)
                    {
                        if ((double)num13 > 1000000.0)
                        {
                            int Stack = (int)((double)num13 / 1000000.0);
                            if (Stack > 50 && Main.rand.Next(2) == 0)
                                Stack /= Main.rand.Next(3) + 1;
                            if (Main.rand.Next(2) == 0)
                                Stack /= Main.rand.Next(3) + 1;
                            num13 -= (float)(1000000 * Stack);
                            Item.NewItem(i * 16, j * 16, 16, 16, ItemID.PlatinumCoin, Stack, false, 0, false, false);
                        }
                        else if ((double)num13 > 10000.0)
                        {
                            int Stack = (int)((double)num13 / 10000.0);
                            if (Stack > 50 && Main.rand.Next(2) == 0)
                                Stack /= Main.rand.Next(3) + 1;
                            if (Main.rand.Next(2) == 0)
                                Stack /= Main.rand.Next(3) + 1;
                            num13 -= (float)(10000 * Stack);
                            Item.NewItem(i * 16, j * 16, 16, 16, 73, Stack, false, 0, false, false);
                        }
                        else if ((double)num13 > 100.0)
                        {
                            int Stack = (int)((double)num13 / 100.0);
                            if (Stack > 50 && Main.rand.Next(2) == 0)
                                Stack /= Main.rand.Next(3) + 1;
                            if (Main.rand.Next(2) == 0)
                                Stack /= Main.rand.Next(3) + 1;
                            num13 -= (float)(100 * Stack);
                            Item.NewItem(i * 16, j * 16, 16, 16, 72, Stack, false, 0, false, false);
                        }
                        else
                        {
                            int Stack = (int)num13;
                            if (Stack > 50 && Main.rand.Next(2) == 0)
                                Stack /= Main.rand.Next(3) + 1;
                            if (Main.rand.Next(2) == 0)
                                Stack /= Main.rand.Next(4) + 1;
                            if (Stack < 1)
                                Stack = 1;
                            num13 -= (float)Stack;
                            Item.NewItem(i * 16, j * 16, 16, 16, 71, Stack, false, 0, false, false);
                        }
                    }
                }
            }
            if (SOTSWorld.downedAdvisor && Main.rand.Next(50) == 0)
                Item.NewItem(i * 16, j * 16, 16, 16, mod.ItemType("TwilightShard"), 1, false, 0, false, false);
        }
    }
    /*internal class Pots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sky Pot");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DartTrap);
			item.width = 24;
			item.height = 26;
			item.createTile = TileType<SkyPots>();
			item.value = 0;
		}
	}*/
}