using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;
using SOTS.Biomes;
using System;
using SOTS.Items.Conduit;
using SOTS.Items.AbandonedVillage;
using SOTS.Common.Systems;
using SOTS.Items.Pyramid;
using SOTS.Items.Gems;
using SOTS.Items.ChestItems;
using SOTS.Items.Secrets;
using System.IO;
using SOTS.Items.Whips;
using Terraria.Map;
using Terraria.DataStructures;
using System.Reflection;
using Terraria.GameContent.Drawing;
using Terraria.Graphics;
using static SOTS.NPCs.Town.PortalDrawingHelper;

namespace SOTS.NPCs.Town
{
	public class Archaeologist : ModNPC
	{
		public static Vector2 AnomalyPosition1 = Vector2.Zero;
		public static Vector2 AnomalyPosition2 = Vector2.Zero;
		public static Vector2 AnomalyPosition3 = Vector2.Zero;
		public static float AnomalyAlphaMult = 0f;
		public static float FinalAnomalyAlphaMult = 0f;
		public static int locationTimer = 100000;
		public const int timeToGoToSetPiece = 60000; //This is 1000 seconds
		public bool hasTeleportedYet = false;
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(locationTimer);
			writer.Write(InitialDirection);
			writer.Write(currentLocationType);
			writer.Write(AnomalyAlphaMult);
			writer.Write(AnomalyPosition1.X);
			writer.Write(AnomalyPosition1.Y);
			writer.Write(AnomalyPosition2.X);
			writer.Write(AnomalyPosition2.Y);
			writer.Write(AnomalyPosition3.X);
			writer.Write(AnomalyPosition3.Y);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			locationTimer = reader.ReadInt32();
			InitialDirection = reader.ReadInt32();
			currentLocationType = reader.ReadInt32();
			AnomalyAlphaMult = reader.ReadSingle();
			AnomalyPosition1.X = reader.ReadSingle();
			AnomalyPosition1.Y = reader.ReadSingle();
			AnomalyPosition2.X = reader.ReadSingle();
			AnomalyPosition2.Y = reader.ReadSingle();
			AnomalyPosition3.X = reader.ReadSingle();
			AnomalyPosition3.Y = reader.ReadSingle();
		}
        //private static Profiles.StackedNPCProfile NPCProfile;
        public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 5; // The amount of frames the NPC has
			NPCID.Sets.ExtraFramesCount[Type] = 0; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs.
			NPCID.Sets.AttackFrameCount[Type] = 0;
			NPCID.Sets.DangerDetectRange[Type] = 0; // The amount of pixels away from the center of the npc that it tries to attack enemies.
			NPCID.Sets.PrettySafe[Type] = 0;
			NPCID.Sets.AttackType[Type] = 0; // Shoots a weapon.
			NPCID.Sets.AttackTime[Type] = 0; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 0;
			NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.
			//NPCID.Sets.ShimmerTownTransform[NPC.type] = true; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

			//This sets entry is the most important part of this NPC. Since it is true, it tells the game that we want this NPC to act like a town NPC without ACTUALLY being one.
			//What that means is: the NPC will have the AI of a town NPC, will attack like a town NPC, and have a shop (or any other additional functionality if you wish) like a town NPC.
			//However, the NPC will not have their head displayed on the map, will de-spawn when no players are nearby or the world is closed, will spawn like any other NPC, and have no happiness button when chatting.
			NPCID.Sets.ActsLikeTownNPC[Type] = true;

			//NPCID.Sets.SpawnsWithCustomName[Type] = true;
			//NPCID.Sets.AllowDoorInteraction[Type] = true;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Direction = 1
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			/*NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, -1),
				new Profiles.DefaultNPCProfile(Texture + "_Shimmer", -1)
			);*/
			NPC.behindTiles = true;
		}
		public override void SetDefaults()
		{
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 42;
			NPC.height = 60;
			NPC.aiStyle = 7;
			NPC.damage = 100;
			NPC.defense = 150;
			NPC.lifeMax = 25000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			NPC.behindTiles = true;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
		}
		public bool playerNearby = false;
		public int AnimCycles = 0;
		public int FrameY = 0;
		public int FrameSpeed = 5;
		public int TotalIdleFrameCycles = 6;
		public int TotalLookUpFrameCycles = 3;
		public const int MinimumIdleCycles = 6;
		public const int MaximumIdleCycles = 22;
		public const int MinimunLookCycles = 5;
		public const int MaximumLookCycles = 14;
		public int DrawTimer = 0;
		public int aiTimer = 0;
		public int InitialDirection = 0;
        public override bool CheckActive()
        {
            return false;
        }
        public override void ModifyTypeName(ref string typeName)
        {
			typeName = Language.GetTextValue("Mods.SOTS.NPCName.Archaeologist");
			if(hasPlayerChattedBefore)
			{
				typeName = Language.GetTextValue("Mods.SOTS.NPCName.ArchaeologistNearby");
			}
		}
		public void DrawHoverPlatforms(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = ModContent.Request<Texture2D>("SOTS/NPCs/Town/ArchaeologistPlatform").Value;
			int height = texture.Height;
			int width = texture.Width;
			Vector2 origin = new Vector2(width / 2, 0);
			float grainy = (float)(DrawTimer % 150);
			drawColor = NPC.GetAlpha(Color.Lerp(drawColor, Color.White, 0.5f));
			for (int i = 0; i < height - 1; i++)
			{
				float sinusoid = 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(-DrawTimer * 2.4f + i * 18));
				float bonusAlphaMult = 0;
				float xOffset = 0;
				float xScale = 1;
				float progress = (float)i / height;
				if (sinusoid > 0.9f)
				{
					sinusoid -= 0.9f;
					sinusoid *= 1 / 0.1f;
					sinusoid = sinusoid * sinusoid;
					bonusAlphaMult += sinusoid;
					xScale += sinusoid * 0.075f;
				}
				else if (grainy > 138)
				{
					int grainDirection = NPC.direction * (((i / 2) % 2) * 2 - 1);
					float grainProgress = (float)Math.Sin(MathHelper.ToRadians(360 * (grainy - 138) / 12f));
					float grainMult = 1f * (1 - 0.5f * bonusAlphaFromBeingNear);
					xOffset += grainDirection * grainProgress * grainMult * (0.6f + 0.4f * (float)Math.Sin(progress * MathHelper.Pi));
				}
				Vector2 drawFromPosition = new Vector2(NPC.Center.X, NPC.position.Y + NPC.height + height - 1) + new Vector2(xOffset, -1 * i);
				Rectangle frame = new Rectangle(0, height - (i + 1), width, 1);
				float baseAlpha = 0.10f + 0.61f * bonusAlphaFromBeingNear;
				float gradientAlpha = 0.4f * (1 - 0.9f * bonusAlphaFromBeingNear);
				spriteBatch.Draw(texture, drawFromPosition - screenPos, frame, drawColor * (bonusAlphaMult * 0.35f + (baseAlpha + gradientAlpha * (float)Math.Sqrt(progress))), NPC.rotation, origin, new Vector2(xScale, 1), SpriteEffects.None, 0f);
			}
		}
        public void Draw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture = TextureAssets.Npc[NPC.type].Value;
			int height = texture.Height / 5;
			int width = texture.Width;
			Vector2 origin = new Vector2(width / 2, 0);
			int startingFrame = NPC.frame.Y;
			float grainy = (float)(DrawTimer % 150);
			drawColor = NPC.GetAlpha(Color.Lerp(drawColor, Color.White, 0.5f));
			if (screenPos != Main.screenPosition) //this should check for bestiary?
			{
				NPC.spriteDirection = -1;
				drawColor = Color.White;
				bonusAlphaFromBeingNear = 0.7f;
			}
			for (int i = 0; i < height - 1; i++)
			{
				float sinusoid = 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(-DrawTimer * 2.4f + i * 18));
				float bonusAlphaMult = 0;
				float xOffset = 0;
				float xScale = 1;
				float progress = (float)i / height;
				if (sinusoid > 0.9f)
                {
					sinusoid -= 0.9f;
					sinusoid *= 1 / 0.1f;
					sinusoid = sinusoid * sinusoid;
					bonusAlphaMult += sinusoid;
					xScale += sinusoid * 0.075f;
                }
				else if(grainy > 138)
                {
					int grainDirection = NPC.direction * (((i / 2) % 2) * 2 - 1);
					float grainProgress = (float)Math.Sin(MathHelper.ToRadians(360 * (grainy - 138) / 12f));
					float grainMult = 1f * (1 - 0.5f * bonusAlphaFromBeingNear);
					xOffset += grainDirection * grainProgress * grainMult * (0.6f + 0.4f * (float)Math.Sin(progress * MathHelper.Pi));
                }
				Vector2 drawFromPosition = new Vector2(NPC.Center.X, NPC.position.Y + NPC.height) + new Vector2(xOffset, -1 * i);
				Rectangle frame = new Rectangle(0, startingFrame + height - (i + 1), width, 1);
				float baseAlpha = 0.10f + 0.61f * bonusAlphaFromBeingNear;
				float gradientAlpha = 0.4f * (1 - 0.9f * bonusAlphaFromBeingNear);
				spriteBatch.Draw(texture, drawFromPosition - screenPos, frame, drawColor * (bonusAlphaMult * 0.35f + (baseAlpha + gradientAlpha * (float)Math.Sqrt(progress))), NPC.rotation, origin, new Vector2(xScale, 1), NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (screenPos != Main.screenPosition) //this should check for bestiary?
			{
				Draw(spriteBatch, screenPos, drawColor);
			}
			else
				DrawHoverPlatforms(spriteBatch, screenPos, drawColor);
			return false;
        }
        float bonusAlphaFromBeingNear = 0f;
        public override void FindFrame(int frameHeight)
		{
			if(playerNearby)
            {
				AnimCycles = TotalIdleFrameCycles;
            }
			NPC.frameCounter++;
			if (AnimCycles < TotalIdleFrameCycles)
			{
				if (NPC.frameCounter >= FrameSpeed)
				{
					FrameSpeed = Main.rand.Next(3, 13);
					NPC.frameCounter = 0;
					FrameY++;
				}
				if (FrameY > 1)
				{
					FrameY = 0;
					AnimCycles++;
				}
			}
			else if (AnimCycles >= TotalIdleFrameCycles)
			{
				if (NPC.frameCounter >= FrameSpeed)
				{
					FrameSpeed = 8;
					NPC.frameCounter = 0;
					if (AnimCycles >= TotalLookUpFrameCycles + TotalIdleFrameCycles)
					{
						FrameY--;
					}
					else
						FrameY++;
				}
				if (FrameY > 4)
				{
					FrameY = 4;
					AnimCycles++;
				}
				if (FrameY < 0)
				{
					FrameY = 0;
					AnimCycles = 0;
					TotalIdleFrameCycles = Main.rand.Next(MinimumIdleCycles, MaximumIdleCycles);
					TotalLookUpFrameCycles = Main.rand.Next(MinimunLookCycles, MaximumLookCycles);
				}
			}
			NPC.frame.Y = frameHeight * FrameY;
			DrawTimer++;
		}
        public override bool PreAI()
        {
			if(NPC.CountNPCS(Type) > 1)
            {
				NPC.active = false;
				return false;
            }
			else if(hasTeleportedYet)
			{
				AnomalyAlphaMult += 1 / 60f;
				AnomalyPosition1 = NPC.Center;
			}
			if (InitialDirection == 0)
            {
				InitialDirection = 1;
            }
			int directionToGoTo = InitialDirection;
			playerNearby = false;
			bool playerWithinSecondRange = false;
			for(int i = 0; i < Main.player.Length; i++)
            {
				Player player = Main.player[i];
				if (player.active)
                {
					if(player.Distance(NPC.Center) < 1600)
                    {
						playerWithinSecondRange = true;

						if (player.Distance(NPC.Center) < 120)
						{
							playerNearby = true;
							if (NPC.Center.X > player.Center.X)
							{
								directionToGoTo = -1;
							}
							else
							{
								directionToGoTo = 1;
							}
							break;
						}
					}
				}
            }
			if(!playerWithinSecondRange)
            {
				if(locationTimer > timeToGoToSetPiece || !hasTeleportedYet)
                {
					aiTimer = 0;
					locationTimer = 0;
					if(Main.netMode != NetmodeID.MultiplayerClient)
					{
						FindALocationToGoTo();
						InitialDirection = NPC.direction;
					}
					if (!hasTeleportedYet)
					{
						locationTimer = timeToGoToSetPiece - 120;
					}
				}
				else if(locationTimer > timeToGoToSetPiece - 60)
                {
					AnomalyAlphaMult = 1 - (locationTimer - timeToGoToSetPiece + 60) / 60f;
                }
				locationTimer++;
            }
			NPC.direction = NPC.spriteDirection = directionToGoTo;
			if (playerNearby)
			{
				bonusAlphaFromBeingNear += 0.02f;
			}
			else
            {
				bonusAlphaFromBeingNear -= 0.02f;
            }
			bonusAlphaFromBeingNear = MathHelper.Clamp(bonusAlphaFromBeingNear, 0, 1);
			NPC.dontTakeDamage = true;
			NPC.dontTakeDamageFromHostiles = true;
			NPC.velocity.X *= 0;
			aiTimer++;
            if (aiTimer > 120)
            {
				NPC.velocity.Y *= 0f;
				NPC.alpha = 0;
            }
			else
            {
				NPC.alpha = (int)(255 * (1f - aiTimer / 120f));
				NPC.velocity.Y += 0.1f;
			}
			AnomalyAlphaMult = MathHelper.Clamp(AnomalyAlphaMult, 0, 1);
			FinalAnomalyAlphaMult = MathHelper.Clamp(AnomalyAlphaMult * 1.1f - 0.1f, 0, 1);
			return base.PreAI();
        }
        public override void PostAI()
        {
            base.PostAI();
			NPC.velocity.X *= 0f;
			NPC.velocity = Collision.TileCollision(new Vector2(NPC.Center.X - 8, NPC.position.Y), NPC.velocity, 16, NPC.height, false);
		}
        //Make sure to allow your NPC to chat, since being "like a town NPC" doesn't automatically allow for chatting.
        public override bool CanChat()
		{
			return true;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			ModBiomeBestiaryInfoElement Planetarium = ModContent.GetInstance<PlanetariumBiome>().ModBiomeBestiaryInfoElement;
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				Planetarium,
				new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.ArchaeologistLore")
			});
		}
		public bool hasPlayerChattedBefore = false;
		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();
			if (hasPlayerChattedBefore)
			{
				chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogue3"), 0.5);
				chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogue4"), 0.5);
				chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogue5"), 0.5);
				chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogue6"), 0.5);
				if (currentLocationType == ImportantTileID.AcediaPortal || currentLocationType == ImportantTileID.AvaritiaPortal)
                {
					if(currentLocationType != ImportantTileID.AvaritiaPortal)
						chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialoguePortalNotAvaritia"));
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialoguePortal1"));
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialoguePortal2"));
				}
				if (currentLocationType == ImportantTileID.AcediaPortal)
				{
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueAcedia1"));
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueAcedia2"));
				}
				if (currentLocationType == ImportantTileID.AvaritiaPortal)
				{
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueAvaritia1"));
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueAvaritia2"));
				}
				if (currentLocationType == ImportantTileID.bigCrystal)
				{
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueBigCrystal"));
				}
				if (currentLocationType == ImportantTileID.coconutIslandMonument)
				{
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueCoconutMonument"));
				}
				if (currentLocationType == ImportantTileID.coconutIslandMonumentBroken)
				{
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueCoconutBroken"));
				}
				if (currentLocationType == ImportantTileID.damoclesChain)
				{
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueDamocles"));
				}
				if (currentLocationType == ImportantTileID.iceMonument)
				{
					chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueIceMonument"));
				}
				if (currentLocationType >= ImportantTileID.gemlockAmethyst && currentLocationType <= ImportantTileID.gemlockAmber)
				{
					if(currentLocationType == ImportantTileID.gemlockDiamond)
					{
						chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueGemlockDiamond"));
					}
					else chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogueGemlockNotDiamond"));
				}
			}
			else if(!hasPlayerChattedBefore)
			{
				chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogue1"));
				chat.Add(Language.GetTextValue("Mods.SOTS.Dialogue.ArchaeologistDialogue2"), 0.5);
				hasPlayerChattedBefore = true;
			}
			return chat; // chat is implicitly cast to a string.
		}
		public override void SetChatButtons(ref string button, ref string button2)
		{ 
			button = Language.GetTextValue("LegacyInterface.28"); //This is the key to the word "Shop"
		}
		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				shop = true;
			}
		}
		private static void AddItemToShop(Chest shop, ref int nextSlot, int itemID)
		{
			shop.item[nextSlot].SetDefaults(itemID);
			nextSlot++;
		}
		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			AddItemToShop(shop, ref nextSlot, ModContent.ItemType<AnomalyLocator>());
			AddItemToShop(shop, ref nextSlot, ModContent.ItemType<ArchaeologistToolbelt>());
			AddItemToShop(shop, ref nextSlot, ModContent.ItemType<GoldenTrowel>());
			AddItemToShop(shop, ref nextSlot, ModContent.ItemType<OldKey>());
			AddItemToShop(shop, ref nextSlot, ModContent.ItemType<ConduitChassis>());
			if (currentLocationType == ImportantTileID.AcediaPortal)
			{
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<NatureConduit>());
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<CursedApple>());
				if(SOTSWorld.DreamLampSolved)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<DreamLamp>());
				}
			}
			if (currentLocationType == ImportantTileID.AvaritiaPortal)
            {
				if(SOTSWorld.downedAdvisor)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<Items.Otherworld.MeteoriteKey>());
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<Items.Otherworld.SkywareKey>());
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<Items.Otherworld.StrangeKey>());
				}
			}
			if (currentLocationType == ImportantTileID.gemlockAmethyst)
			{
				if(SOTSWorld.AmethystKeySlotted)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<AmethystRing>());
				}
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<RockCandy>());
			}
			if (currentLocationType == ImportantTileID.gemlockTopaz)
			{
				if (SOTSWorld.TopazKeySlotted)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<TopazRing>());
				}
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<BetrayersKnife>());
			}
			if (currentLocationType == ImportantTileID.gemlockSapphire)
			{
				if (SOTSWorld.SapphireKeySlotted)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<SapphireRing>());
				}
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<BagOfAmmoGathering>());
			}
			if (currentLocationType == ImportantTileID.gemlockEmerald)
			{
				if (SOTSWorld.EmeraldKeySlotted)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<EmeraldRing>());
				}
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<Items.Invidia.VorpalKnife>());
			}
			if (currentLocationType == ImportantTileID.gemlockRuby)
			{
				if (SOTSWorld.RubyKeySlotted)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<RubyRing>());
				}
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<SyntheticLiver>());
			}
			if (currentLocationType == ImportantTileID.gemlockDiamond)
			{
				if (SOTSWorld.DiamondKeySlotted)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<DiamondRing>());
				}
			}
			if (currentLocationType == ImportantTileID.gemlockAmber)
			{
				if (SOTSWorld.AmberKeySlotted)
				{
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<AmberRing>());
				}
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<KelpWhip>());
			}
			if (currentLocationType == ImportantTileID.iceMonument)
			{
				if (NPC.downedBoss1)
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<GlazeBow>());
			}
			if (currentLocationType == ImportantTileID.coconutIslandMonument)
			{
				if (NPC.downedBoss1)
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<CoconutGun>());
			}
			if (currentLocationType == ImportantTileID.coconutIslandMonumentBroken)
			{
				if (NPC.downedBoss1)
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<CoconutGun>());
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<PhotonGeyser>());
			}
			if (currentLocationType == ImportantTileID.damoclesChain)
			{
				if (NPC.downedBoss1)
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<Items.Crushers.BoneClapper>());
				AddItemToShop(shop, ref nextSlot, ItemID.Terragrim);
			}
			if (currentLocationType == ImportantTileID.bigCrystal)
			{
				if (NPC.downedBoss1)
					AddItemToShop(shop, ref nextSlot, ModContent.ItemType<PerfectStar>());
				AddItemToShop(shop, ref nextSlot, ModContent.ItemType<VisionAmulet>());
			}
		}
		public static int currentLocationType = -1;
		public void FindALocationToGoTo()
		{
			NPC.netUpdate = true;
			int olderLocationType = currentLocationType;
			currentLocationType = 0;
			int newDirection = 0;
			Vector2? destination = ImportantTilesWorld.RandomImportantLocation(ref currentLocationType, ref newDirection);
			if(destination.HasValue)
			{
				hasTeleportedYet = true;
				NPC.Center = destination.Value;
				NPC.direction = newDirection;
				VoidAnomaly.KillOtherAnomalies();
				VoidAnomaly.PlaceDownAnomalies();
				AnomalyAlphaMult = 0;
				if (Main.netMode == NetmodeID.Server)
					Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("The location is at: " + currentLocationType), Color.Gray);
				else
					Main.NewText("The location is at: " + currentLocationType);
			}
			else
            {
				currentLocationType = olderLocationType;
            }
		}
	}
	public class VoidAnomaly : ModProjectile
	{
		public const int Radius = 6;
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			overWiresUI.Add(index);
        }
        public static TileDrawInfo RunGet_currentTileDrawInfo(TileDrawing tDrawer)
		{
			Type type = tDrawer.GetType();
			FieldInfo field = type.GetField("_currentTileDrawInfoNonThreaded", BindingFlags.NonPublic | BindingFlags.Instance);
			return (TileDrawInfo)field.GetValue(tDrawer);
		}
		public static void Run_DrawSingleTile(TileDrawing tDrawer, TileDrawInfo drawData, bool solidLayer, int waterStyleOverride, Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY)
		{
			Type type = tDrawer.GetType();
			MethodInfo method = type.GetMethod("DrawSingleTile", BindingFlags.NonPublic | BindingFlags.Instance);
			if (method == null)
				return;
			method.Invoke(tDrawer, new object[] { drawData, solidLayer, waterStyleOverride, screenPosition, screenOffset, tileX, tileY });
		}
		public static void DrawWall(ref SaveTileData Data, int i, int j, int h, int k, Vector2 offset, int pass)
		{
			Tile otherTile = Main.tile[i, j];
			Tile myTile = Main.tile[h, k];
			int oType = otherTile.WallType;
			if(pass == 2)
			{
				Data.OntoTileW(myTile);
			}
			else if(pass == 1)
			{
				if (otherTile.WallType != 0)
				{
					SpriteBatch spriteBatch = Main.spriteBatch;
					if (WallLoader.PreDraw(h, k, oType, spriteBatch))
					{
						DrawWallM(spriteBatch, -offset, otherTile, i, j, oType);
					}
					WallLoader.PostDraw(h, k, oType, spriteBatch);
				}
			}
			else
			{
				if (otherTile.WallType != 0)
				{
					if (!TextureAssets.Wall[oType].IsLoaded)
					{
						Main.instance.LoadWall(oType);
					}
				}
				Data.IntoSaveW(myTile);
				Data.CopyTileToTileW(otherTile, myTile);
			}
		}
		public static void DrawTile(ref SaveTileData Data, TileDrawInfo info, int i, int j, int h, int k, Vector2 offset, int pass)
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Tile otherTile = Main.tile[i, j];
			Tile myTile = Main.tile[h, k];
			int oType = otherTile.TileType;
			if(pass == 2)
			{
				Data.OntoTile(myTile);
			}
			else if(pass == 1)
			{
				if (otherTile.HasTile && !TileID.Sets.IsATreeTrunk[otherTile.TileType] && !TileID.Sets.CountsAsGemTree[otherTile.TileType] && oType != TileID.PalmTree)
				{
					if (TileLoader.PreDraw(h, k, oType, Main.spriteBatch))
					{
						Run_DrawSingleTile(Main.instance.TilesRenderer, info, true, -1, unscaledPosition + offset, Vector2.Zero, i, j);
					}
					TileLoader.PostDraw(h, k, oType, Main.spriteBatch);
				}
				else if(otherTile.LiquidAmount > 0)
				{
					int liquidType = 0;
					bool water = false;
					switch (otherTile.LiquidType)
					{
						case 0:
							water = true;
							break;
						case 1:
							liquidType = 1;
							break;
						case 2:
							liquidType = 11;
							break;
					}
					if (liquidType == 0)
						liquidType = Main.waterStyle;
					bool flag7 = false;
					if (water)
					{
						for (int a = 0; a < 13; a++)
						{
							if (Main.IsLiquidStyleWater(a) && Main.liquidAlpha[a] > 0f && a != liquidType)
							{
								Main.spriteBatch.Draw(TextureAssets.Liquid[a].Value, new Vector2(h, k + 1) * 16 - Main.screenPosition, new Rectangle(0, 8, 16, 8), Color.White, 0f, new Vector2(0, 8), new Vector2(1, 2 * otherTile.LiquidAmount / 255f), 0, 0f);
								flag7 = true;
								break;
							}
						}
					}
					Main.spriteBatch.Draw(TextureAssets.Liquid[liquidType].Value, new Vector2(h, k + 1) * 16 - Main.screenPosition, new Rectangle(0, 8, 16, 8), Color.White * (flag7 ? Main.liquidAlpha[liquidType] : 1f), 0f, new Vector2(0, 8), new Vector2(1, 2 * otherTile.LiquidAmount / 255f), 0, 0f);
				}
			}
			else
			{
				if (!TextureAssets.Tile[oType].IsLoaded)
				{
					Main.instance.LoadTiles(oType);
				}
				Data.IntoSave(myTile);
				Data.CopyTileToTile(otherTile, myTile);
			}
		}
		private bool canIMove = false;
		private SaveTileData[] Data = null;
		public void DrawTilesFromOtherPortal()
        {
			TileDrawInfo value = RunGet_currentTileDrawInfo(Main.instance.TilesRenderer);
			int x = (int)(positionOfOtherPortal.X / 16);
			int y = (int)(positionOfOtherPortal.Y / 16);
			int x2 = (int)(Projectile.Center.X / 16);
			int y2 = (int)(Projectile.Center.Y / 16);
			Vector2 offset = positionOfOtherPortal - Projectile.Center;
			int maxSize = (int)Math.Pow((Radius * 2 + 1), 2);
			float currentRadius = Radius * alphaMult;
			if (Data == null)
				Data = new SaveTileData[maxSize];
			if (WorldGen.InWorld(x, y, 40))
			{
				if (WorldGen.InWorld(x2, y2, 40))
				{
					Main.drawToScreen = true;
					Main.gameMenu = true;
					canIMove = false;
					for (int k = 0; k < 4; k++)
					{
						int currentIndex = 0;
						for (int j = Radius; j >= -Radius; j--)
						{
							for (int i = -Radius; i <= Radius; i++)
							{
								float dist = i * i + j * j;
								if (dist < currentRadius * currentRadius)
								{
									if (k == 0)
									{
										DrawTile(ref Data[currentIndex], value, x + i, y + j, x2 + i, y2 + j, offset, 0);
										DrawWall(ref Data[currentIndex], x + i, y + j, x2 + i, y2 + j, offset, 0);
									}
									if (k == 1)
									{
										DrawWall(ref Data[currentIndex], x + i, y + j, x2 + i, y2 + j, offset, 1);
									}
									if (k == 2)
									{
										DrawTile(ref Data[currentIndex], value, x + i, y + j, x2 + i, y2 + j, offset, 1);
									}
									if (k == 3)
									{
										DrawTile(ref Data[currentIndex], value, x + i, y + j, x2 + i, y2 + j, offset, 2);
										DrawWall(ref Data[currentIndex], x + i, y + j, x2 + i, y2 + j, offset, 2);
									}
								}
								currentIndex++;
							}
						}
					}
					canIMove = true;
					Main.gameMenu = false;
					Main.drawToScreen = false;
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 drawPos = Projectile.Center;
			Color color = Color.Lerp(new Color(120, 100, 140, 0), Color.Black, 0.76f);
			for (int j = 1; j <= 4; j++)
				for (int i = -1; i <= 1; i += 2)
				{
					float rotation = j * MathHelper.PiOver4 / 2f;
					Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color, i * Projectile.rotation + rotation, texture.Size() / 2, 3f, i == -1 ? SpriteEffects.FlipHorizontally : 0, 0f);
				}
			return false;
        }
        public override void PostDraw(Color lightColor)
		{
			if (Projectile.ai[0] == -1 || Projectile.ai[0] == -2)
			{
				DrawTilesFromOtherPortal();
			}
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 drawPos = Projectile.Center;
			Color color = Color.Lerp(new Color(160, 120, 180, 0), Color.Black, 0.15f);
			for(int j = 1; j <= 2; j++)
				for (int i = -1; i <= 1; i += 2)
				{
					float rotation = j * MathHelper.PiOver4;
					Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color * 0.125f * j, i * Projectile.rotation * -1 + rotation, texture.Size() / 2, 1f + j, i == -1 ? SpriteEffects.FlipHorizontally : 0, 0f);
				}
		}
		public static void PlaceDownAnomalies()
        {
			if (Main.netMode == NetmodeID.MultiplayerClient)
				return;
			int padding = 50;
			for(int i = 1;  i <= 2; i++)
            {
				float randX = Main.rand.Next(padding, Main.maxTilesX - padding);
				float randY = Main.rand.Next(padding, Main.maxTilesY - padding);
				randX = 400 + i * 16;
				randY = 400;
				Vector2 randomPosition = new Vector2(randX * 16 + 8, randY * 16 + 8);
				Projectile.NewProjectile(new EntitySource_Misc("SOTS:ArchaeologistPortals"), randomPosition, Vector2.Zero, ModContent.ProjectileType<VoidAnomaly>(), 0, 0, Main.myPlayer, -i, -60);
            }
        }
		public static void KillOtherAnomalies()
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
				return;
			for (int i = 0; i < Main.maxProjectiles; i++)
            {
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.type == ModContent.ProjectileType<VoidAnomaly>())
                {
					proj.ai[0] = -3;
					proj.netUpdate = true;
                }
            }
        }
		public override void SetDefaults()
		{
			Projectile.height = 82;
			Projectile.width = 82;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.timeLeft = 100000;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.hide = true;
		}
		public float alphaMult
        {
			get
            {
				float mult = (Projectile.ai[1] / 60f) * (Projectile.timeLeft) / 120f;
				if (mult < 0)
					return 0;
				else
					return mult;
			}
        }
		public Vector2 positionOfOtherPortal
        {
			get
            {
				if(Projectile.ai[0] == -1)
					return Archaeologist.AnomalyPosition3;
				if (Projectile.ai[0] == -2)
					return Archaeologist.AnomalyPosition2;
				return Vector2.Zero;
			}
        }
		public override void AI()
		{
			if (Projectile.ai[1] < 60)
				Projectile.ai[1]++;
			if (Projectile.ai[0] == -1)
			{
				if (Main.mouseMiddle && canIMove)
				{
					Projectile.Center = new Vector2((int)(Main.MouseWorld.X / 16) * 16 + 8, (int)(Main.MouseWorld.Y / 16) * 16 + 8);
				}
                if (Projectile.ai[1] >= 0 && Projectile.timeLeft >= 119)
					Archaeologist.AnomalyPosition2 = Projectile.Center;
			}
			if (Projectile.ai[0] == -2)
			{
				if (Projectile.ai[1] >= 0 && Projectile.timeLeft >= 119)
					Archaeologist.AnomalyPosition3 = Projectile.Center;
			}
			if (Projectile.ai[0] == -3) // start dying
			{
				if (Projectile.timeLeft > 120)
					Projectile.timeLeft = 120;
			}
			else
				Projectile.timeLeft = 120;
			Projectile.alpha = (int)(255 * (1 - alphaMult));
			Projectile.rotation += MathHelper.ToRadians(3.5f);
		}
    }
	public static class PortalDrawingHelper
	{
		public struct SaveTileData
		{
			bool saveActive;
			ushort saveTileType;
			short saveTileFrameX;
			short saveTileFrameY;
			byte saveTileColor;
			ushort saveWallType;
			int saveWallFrameX;
			int saveWallFrameY;
			byte saveWallColor;
			byte saveLiquid;
			int saveLiquidT;
			public void IntoSaveW(Tile data)
			{
				saveWallType = data.WallType;
				saveWallFrameX = data.WallFrameX;
				saveWallFrameY = data.WallFrameY;
				saveWallColor = data.WallColor;
			}
			public void OntoTileW(Tile data)
			{
				data.WallType = saveWallType;
				data.WallFrameX = saveWallFrameX;
				data.WallFrameY = saveWallFrameY;
				data.WallColor = saveWallColor;
			}
			public void CopyTileToTileW(Tile CopyFrom, Tile CopyOnto)
			{
				CopyOnto.WallType = CopyFrom.WallType;
				CopyOnto.WallFrameX = CopyFrom.WallFrameX;
				CopyOnto.WallFrameY = CopyFrom.WallFrameY;
				CopyOnto.WallColor = CopyFrom.WallColor;
			}
			public void IntoSave(Tile data)
			{
				saveActive = data.HasTile;
				saveTileType = data.TileType;
				saveTileFrameX = data.TileFrameX;
				saveTileFrameY = data.TileFrameY;
				saveTileColor = data.TileColor;
				saveLiquid = data.LiquidAmount;
				saveLiquidT = data.LiquidType;
			}
			public void OntoTile(Tile data)
            {
				data.HasTile = saveActive;
				data.TileType = saveTileType;
				data.TileFrameX = saveTileFrameX;
				data.TileFrameY = saveTileFrameY;
				data.TileColor = saveTileColor;
				data.LiquidAmount = saveLiquid;
				data.LiquidType = saveLiquidT;
			}
			public void CopyTileToTile(Tile CopyFrom, Tile CopyOnto)
			{
				CopyOnto.HasTile = CopyFrom.HasTile;
				CopyOnto.TileType = CopyFrom.TileType;
				CopyOnto.TileFrameX = CopyFrom.TileFrameX;
				CopyOnto.TileFrameY = CopyFrom.TileFrameY;
				CopyOnto.TileColor = CopyFrom.TileColor;
				CopyOnto.LiquidAmount = CopyFrom.LiquidAmount;
				CopyOnto.LiquidType = CopyFrom.LiquidType;
			}
        }
		public static Texture2D Run_GetTileDrawTexture(WallDrawing wDrawer, Tile tile, int tileX, int tileY)
		{
			Type type = wDrawer.GetType();
			MethodInfo method = type.GetMethod("GetTileDrawTexture", BindingFlags.NonPublic | BindingFlags.Instance);
			if (method == null)
				return null;
			return (Texture2D)method.Invoke(wDrawer, new object[] { tile, tileX, tileY });
		}
		public static void DrawWallM(SpriteBatch spriteBatch, Vector2 offset, Tile tile, int i, int j, int wall)
		{
			Vector2 value = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				value = Vector2.Zero;
			}
			value += offset;
			float gfxQuality = Main.gfxQuality;
			int num21 = (int)(120f * (1f - gfxQuality) + 40f * gfxQuality);
			int num13 = (int)(num21 * 0.4f);
			int num14 = (int)(num21 * 0.35f);
			int num15 = (int)(num21 * 0.3f);
			Color color = Lighting.GetColor(i, j);
			if (tile.WallColor == 31)
			{
				color = Color.White;
			}
			if (color.R == 0 && color.G == 0 && color.B == 0 && j < Main.UnderworldLayer)
			{
				return;
			}
			Rectangle value2 = new Rectangle(0, 0, 32, 32);
			value2.X = tile.WallFrameX;
			value2.Y = tile.WallFrameY + Main.wallFrame[wall] * 180;
			if ((uint)(tile.WallType - 242) <= 1u)
			{
				int num11 = 20;
				int num12 = (Main.wallFrameCounter[wall] + i * 11 + j * 27) % (num11 * 8);
				value2.Y = tile.WallFrameY + 180 * (num12 / num11);
			}
			if (Lighting.NotRetro && !Main.wallLight[wall] && tile.WallType != 241 && (tile.WallType < 88 || tile.WallType > 93) && !WorldGen.SolidTile(tile))
			{
				Texture2D tileDrawTexture = Run_GetTileDrawTexture(Main.instance.WallsRenderer, tile, i, j);
				if (tile.WallType == 44)
				{
					color = new Color((int)(byte)Main.DiscoR, (int)(byte)Main.DiscoG, (int)(byte)Main.DiscoB);
				}
				Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X - 8), (float)(j * 16 - (int)Main.screenPosition.Y - 8));
				spriteBatch.Draw(tileDrawTexture, pos + value, value2, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				Color color2 = color;
				if (wall == 44)
				{
					color2 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
				}
				Texture2D tileDrawTexture2 = Run_GetTileDrawTexture(Main.instance.WallsRenderer, tile, i, j);
				Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X - 8), (float)(j * 16 - (int)Main.screenPosition.Y - 8));
				spriteBatch.Draw(tileDrawTexture2, pos + value, value2, color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			if (color.R > num13 || color.G > num14 || color.B > num15)
			{
				bool num22 = Main.tile[i - 1, j].WallType > 0 && Main.wallBlend[Main.tile[i - 1, j].WallType] != Main.wallBlend[tile.WallType];
				bool flag = Main.tile[i + 1, j].WallType > 0 && Main.wallBlend[Main.tile[i + 1, j].WallType] != Main.wallBlend[tile.WallType];
				bool flag2 = Main.tile[i, j - 1].WallType > 0 && Main.wallBlend[Main.tile[i, j - 1].WallType] != Main.wallBlend[tile.WallType];
				bool flag3 = Main.tile[i, j + 1].WallType > 0 && Main.wallBlend[Main.tile[i, j + 1].WallType] != Main.wallBlend[tile.WallType];
				if (num22)
				{
					spriteBatch.Draw(TextureAssets.WallOutline.Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + value, (Rectangle?)new Rectangle(0, 0, 2, 16), color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				}
				if (flag)
				{
					spriteBatch.Draw(TextureAssets.WallOutline.Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X + 14), (float)(j * 16 - (int)Main.screenPosition.Y)) + value, (Rectangle?)new Rectangle(14, 0, 2, 16), color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				}
				if (flag2)
				{
					spriteBatch.Draw(TextureAssets.WallOutline.Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + value, (Rectangle?)new Rectangle(0, 0, 16, 2), color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				}
				if (flag3)
				{
					spriteBatch.Draw(TextureAssets.WallOutline.Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y + 14)) + value, (Rectangle?)new Rectangle(0, 14, 16, 2), color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				}
			}
		}
    }
}