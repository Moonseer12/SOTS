using SOTS.Items.Chaos;
using Microsoft.Xna.Framework;
using System;
using System.Threading;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.ID;
using SOTS.Items.Invidia;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Gems;
using SOTS.Items.Permafrost;
using SOTS.Items.Otherworld.Blocks;
using SOTS.Items.Otherworld.Furniture;

namespace SOTS.WorldgenHelpers
{
    public static class GemStructureWorldgenHelper
    {
		private static void PlaceAndGenerateEmerald()
		{
			int underworldHeight = Main.UnderworldLayer + 39;
			int rightSideOfWorld = Main.maxTilesX * 11 / 12;
			int chosenX = -1000;
			int chosenY = -1;
			int mostLava = -1;
			for (int xOffset = -100; xOffset <= 100; xOffset++)
			{
				int tempY = -1;
				int tempLava = 0;
				for (int yOffset = 0; yOffset < 500; yOffset++)
				{
					Tile tile = Framing.GetTileSafely(rightSideOfWorld + xOffset, underworldHeight + yOffset);
					if (!tile.HasTile && tile.LiquidType == LiquidID.Lava && tile.LiquidAmount > 50)
					{
						if (tempY == -1)
							tempY = yOffset;
						if (yOffset + underworldHeight < Main.maxTilesY - 100)
							tempLava++;
						else
						{
							break;
						}
					}
					else if (tempLava > 3)
					{
						break;
					}
				}
				if (mostLava < tempLava)
				{
					chosenX = xOffset;
					chosenY = tempY;
					mostLava = tempLava;
				}
			}
			if (chosenX == -1000)
				chosenX = 0;
			if (chosenY == -1)
				chosenY = 0;
			int length = mostLava;
			GenerateEmeraldVoidRuins(rightSideOfWorld + chosenX, underworldHeight + chosenY - 20, length + 20);
		}
		private static void PlaceAndGenerateDiamond()
		{
			int i = 130 + WorldGen.genRand.Next(-10, 11); //140 tiles from left
			int j = 90 + WorldGen.genRand.Next(-4, 5); //90 tiles from top
			GenerateDiamondSkyStructure(i, j);
		}
		private static void PlaceAndGenerateTopaz()
        {
			int chosenX = Main.maxTilesX / 2;
			int totalSpan = 0;
			int heightToBeat = Main.maxTilesY;
			for(int i = 100; i < Main.maxTilesX - 100; i++)
            {
				for (int j = 100; j < Main.maxTilesY - 100; j++)
                {
					Tile tile = Framing.GetTileSafely(i, j);
					if(tile.HasTile && tile.TileType == TileID.LihzahrdBrick)
                    {
						if (j < heightToBeat)
						{
							totalSpan = 0;
							chosenX = i;
							heightToBeat = j;
						}
						else if(j == heightToBeat)
                        {
							totalSpan++; //this should count how many tiles total on the same layer
						}
                        else
                        {
							break;
                        }
                    }
                }
            }
			chosenX += totalSpan / 2;
			GenerateTopazLihzahrdCamp(chosenX, heightToBeat);
			//for loop to find top of lihzahrd temple.
			//Place at the very top of the lihzahrd temple, assuming there is no door it cuts off
        }
		public static void GenerateGemStructures()
		{
			PlaceAndGenerateDiamond();
			PlaceAndGenerateEmerald();
			PlaceAndGenerateTopaz();
		}
		public static ushort EvostoneWall => (ushort)ModContent.WallType<EvostoneBrickWallTile>(); 
		public static ushort EvostoneBrick => (ushort)ModContent.TileType<EvostoneBrickTile>();
		public static ushort Evostone => (ushort)ModContent.TileType<EvostoneTile>();
		public static ushort RuinedChest => (ushort)ModContent.TileType<RuinedChestTile>();
		public static ushort GemChest => (ushort)ModContent.TileType<GemChestTile>();
		public static ushort GemLock => (ushort)ModContent.TileType<SOTSGemLockTiles>();
		public static void GenerateAmethystDesertCamp(int spawnX, int spawnY)
		{
			int PosX = spawnX - 32; //spawnX and spawnY is where you want the anchor to be when this generates
			int PosY = spawnY - 16;
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,2,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,2,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,2,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,2,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,1,2,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,1,2,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,1,0,0,0,1,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,2,2,2,2,1,0,0,0,1,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,2,0,0,1,0,0,1,2,2,1,0,0,0,0,0,0,0,0,0,3,2,2,2,1,0,0,0,1,3,3,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,2,0,0,1,0,0,1,0,2,1,0,0,0,0,0,0,0,0,0,3,2,2,2,1,0,0,0,1,3,3,2,3,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0},
				{0,0,0,0,0,1,1,0,0,1,0,0,1,0,0,1,0,0,0,0,0,1,1,1,3,3,3,3,3,3,1,1,1,3,3,3,2,2,1,1,1,1,1,1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0},
				{0,0,0,0,0,1,1,0,1,2,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,1,1,1,1,1,0,0},
				{0,0,0,0,0,1,1,0,2,2,2,2,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,1,1,1,1,1,1,0,0,0,0,1,0,0},
				{0,0,0,0,0,1,1,2,2,2,2,2,2,2,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0},
				{0,0,0,0,0,1,1,1,0,0,0,0,0,2,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0},
				{0,0,0,4,4,1,4,1,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,4,4,1,4,4,4,4,1,4,4,4,4,1,4,4,0,0,0,0,0},
				{0,0,0,0,4,4,4,1,4,4,4,4,4,1,4,4,0,0,0,0,0,0,0,4,5,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,4,4,4,4,4,1,4,4,4,4,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,5,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,5,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,5,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,5,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,6,6,6,6,4,4,4,6,6,6,6,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,6,4,4,4,6,4,4,4,6,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,7,6,7,4,4,6,7,7,7,6,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,6,7,7,7,6,6,7,7,6,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,6,7,7,7,6,7,7,7,6,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,6,7,7,7,6,7,7,7,6,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,7,7,7,6,7,7,7,6,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,7,7,7,6,7,7,7,6,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,8,7,7,7,6,7,7,7,6,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,8,7,7,7,6,7,7,7,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,8,8,8,8,8,8,8,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
			};
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 152;
								break;
							case 2:
								tile.WallType = 151;
								break;
							case 3:
								tile.WallType = (ushort)ModContent.WallType<Items.Pyramid.PyramidWalls.PyramidBrickWallWall>();
								break;
							case 4:
								tile.WallType = 304;
								break;
							case 5:
								tile.WallType = 226;
								break;
							case 6:
								tile.WallType = 231;
								break;
							case 7:
								tile.WallType = EvostoneWall;
								break;
							case 8:
								tile.WallType = 154;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,4,4,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,4,4,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,3,3,3,6,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,4,4,4,4,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,4,4,8,0,0,0,0,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,4,4,9,10,11,0,0,0,0,12,4,0,0,0,0,0,0,0,0,0,0,13,10,14,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,4,15,15,15,15,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,16,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,18,19,5,5,4,4,4,4,4,4,20,19,19,19,5,5,5,5,7,7,19,21,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,3,4,0,0,0,0,0,0,0,0,0,21,19,19,5,5,5,5,5,5,5,5,5,20,20,20,19,6,6,6,6,6,7,7,19,19,19,19,19,21,0,0,0,0,19,19,19,21,21,0,0,21,19,19,19,19,19,20,22},
				{0,0,0,0,0,3,3,4,4,4,4,0,17,3,4,4,3,19,19,19,19,5,19,19,19,23,19,19,23,5,19,19,20,23,23,23,23,23,23,23,7,5,19,19,19,19,19,19,19,0,0,0,24,19,19,19,19,19,19,19,19,19,20,20,20,20},
				{0,0,0,0,25,4,4,4,4,4,4,4,4,4,4,4,4,19,19,19,19,5,19,19,5,5,5,6,6,5,20,20,20,5,19,19,5,5,5,5,7,5,19,19,20,20,26,27,0,0,0,0,0,0,0,0,0,0,0,0,28,26,20,20,20,20},
				{29,20,26,26,26,27,0,0,0,0,0,0,0,0,0,28,26,19,19,19,19,5,19,23,5,5,19,5,5,5,20,19,5,5,19,19,19,19,19,23,19,5,19,20,20,20,26,26,26,28,28,30,31,31,31,31,32,31,31,33,26,26,20,20,20,20},
				{20,20,20,26,26,26,26,26,28,34,35,35,30,33,26,26,26,19,19,19,19,5,19,5,5,19,36,0,0,0,20,0,0,0,0,0,24,19,19,19,19,5,19,20,20,20,20,26,26,26,26,26,26,26,26,26,26,26,26,26,26,20,20,20,20,20},
				{20,20,20,20,20,26,26,26,26,26,26,26,26,26,26,20,20,19,19,19,19,5,19,5,5,19,0,0,0,29,20,0,0,0,0,0,0,19,19,23,19,5,19,20,20,20,20,20,20,26,26,26,26,26,26,26,26,26,26,26,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,19,19,19,5,5,5,5,19,0,0,0,20,20,0,0,0,0,0,0,19,19,23,19,5,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,19,19,19,7,5,5,19,0,0,0,20,20,22,0,0,0,0,0,24,19,23,23,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,19,19,19,7,19,5,6,0,0,29,20,20,20,37,0,0,0,0,0,19,23,19,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,19,19,7,19,19,6,6,29,20,20,20,20,20,0,0,0,0,0,19,19,23,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,19,19,7,5,19,19,6,20,20,20,20,20,20,37,0,0,0,0,7,19,23,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,19,7,19,19,5,5,5,5,5,5,5,5,5,5,0,0,0,7,19,7,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,19,7,7,23,5,5,5,5,5,5,5,5,0,0,0,0,0,23,23,7,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,19,23,7,23,5,5,0,0,0,0,0,0,0,0,0,0,0,23,7,7,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,19,7,23,23,5,0,0,0,0,0,0,0,0,0,0,0,0,23,23,23,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,26,38,38,38,0,0,0,0,0,0,0,0,0,0,0,0,0,38,38,38,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,26,26,38,38,38,0,0,0,0,0,0,0,0,0,0,0,0,0,38,38,38,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,26,26,38,38,38,0,0,0,0,0,0,0,0,0,0,0,0,0,38,38,38,39,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,26,39,40,23,23,22,0,0,0,0,0,0,0,0,0,0,0,0,23,5,23,39,39,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,39,39,40,23,23,20,0,0,0,0,0,0,0,0,0,0,0,0,5,5,23,39,39,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,39,26,40,40,23,20,0,41,0,0,0,0,0,0,0,0,0,0,5,5,23,26,39,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,39,26,40,40,23,23,6,6,6,23,23,0,0,42,0,0,0,0,5,5,23,26,39,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,39,26,40,40,40,23,23,23,23,40,40,0,0,0,0,0,0,0,23,5,23,26,39,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,39,26,40,40,40,23,0,0,0,0,0,0,0,0,0,0,0,0,23,5,23,26,39,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,39,26,44,40,40,40,0,43,0,0,28,26,26,0,0,0,0,40,23,23,40,26,39,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,39,39,44,40,40,40,0,0,0,28,26,26,26,27,45,0,40,40,23,40,40,26,39,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,39,39,44,44,44,40,40,40,23,23,23,6,6,23,46,46,40,40,23,40,40,39,39,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,39,39,39,44,44,44,44,44,44,40,23,23,23,23,44,44,44,40,40,40,39,39,39,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,39,39,39,44,44,44,44,44,44,44,44,44,44,44,44,44,44,44,26,26,39,39,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,39,39,39,26,26,44,44,44,44,44,44,44,44,44,44,44,26,26,26,39,39,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,39,39,39,26,26,39,26,26,26,26,26,26,26,26,26,39,39,39,39,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,39,39,39,39,39,39,39,39,26,26,26,26,26,39,39,39,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,39,39,39,39,39,39,39,39,39,39,39,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20}
			};
			for (int confirmPlatforms = 0; confirmPlatforms < 3; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, RuinedChest, true, true, -1, 1);
									}
									break;
								case 2:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 178, true, true, -1, 0);
									}
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 322;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = 322;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<Items.Pyramid.OvergrownPyramidTileSafe>();
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<Items.Pyramid.AltPyramidBlocks.RuinedPyramidBrickTile>();
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 7:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<Items.Pyramid.AltPyramidBlocks.PyramidRubbleTile>();
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 8:
									tile.HasTile = true;
									tile.TileType = 322;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 9:
								case 10:
								case 11:
								case 14:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, TileID.Books, true, true, -1, WorldGen.genRand.Next(6));
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 12:
									tile.HasTile = true;
									tile.TileType = 322;
									tile.Slope = (SlopeType)4;
									tile.IsHalfBlock = false;
									break;
								case 13:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 13, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 15:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 17);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 16:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 14, true, true, -1, 26);
									}
									break;
								case 17:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 320, true, true, -1, 0);
									}
									break;
								case 18:
									tile.HasTile = true;
									tile.TileType = 151;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 19:
									tile.HasTile = true;
									tile.TileType = 151;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 20:
									tile.HasTile = true;
									tile.TileType = 53;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									if(i >= _structure.GetLength(0) - 2)
                                    {
										tile.TileType = TileID.HardenedSand;
                                    }
									break;
								case 21:
									tile.HasTile = true;
									tile.TileType = 151;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 22:
									tile.HasTile = true;
									tile.TileType = 53;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 23:
									tile.HasTile = true;
									tile.TileType = 817;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 24:
									tile.HasTile = true;
									tile.TileType = 151;
									tile.Slope = (SlopeType)4;
									tile.IsHalfBlock = false;
									break;
								case 25:
									tile.HasTile = true;
									tile.TileType = 322;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 26:
									tile.HasTile = true;
									tile.TileType = 397;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 27:
									tile.HasTile = true;
									tile.TileType = 397;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 28:
									tile.HasTile = true;
									tile.TileType = 397;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 29:
									tile.HasTile = true;
									tile.TileType = 53;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 30:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 81, true, true, -1, 1);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 31:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
										tile.LiquidAmount = 202;
										tile.LiquidType = 0;
									}
									break;
								case 32:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 81, true, true, -1, 4);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 33:
									tile.HasTile = true;
									tile.TileType = 397;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 34:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 81, true, true, -1, 2);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 35:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
										tile.LiquidAmount = 210;
										tile.LiquidType = 0;
									}
									break;
								case 36:
									tile.HasTile = true;
									tile.TileType = 151;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 37:
									tile.HasTile = true;
									tile.TileType = 53;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 38:
									tile.HasTile = true;
									tile.TileType = 472;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 39:
									tile.HasTile = true;
									tile.TileType = 396;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 40:
									tile.HasTile = true;
									tile.TileType = EvostoneBrick;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 41:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 79, true, true, -1, 22);
									}
									break;
								case 42:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 240, true, true, -1, 16);
									}
									break;
								case 43:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, GemLock, true, true, -1, 4);
									}
									break;
								case 44:
									tile.HasTile = true;
									tile.TileType = Evostone;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 45:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, GemChest, true, true, -1, 9);
									}
									break;
								case 46:
									tile.HasTile = true;
									tile.TileType = 262;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
		}
        public static void GenerateTopazLihzahrdCamp(int spawnX, int spawnY)
		{
			int PosX = spawnX - 13;
			int PosY = spawnY - 14;
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
				{0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
				{0,1,1,1,1,1,1,1,1,1,1,1,2,2,2,1,1,1,1,1,1,2,2,2,1,1,2},
				{1,1,1,2,2,2,1,1,1,1,1,1,2,2,2,1,1,1,1,1,1,2,2,2,1,1,2},
				{1,1,1,2,2,2,3,3,3,3,3,3,1,2,1,1,1,1,1,1,1,1,2,3,3,3,2},
				{1,1,1,1,2,3,3,3,3,3,3,3,3,2,1,1,4,4,4,1,1,1,2,3,3,3,2},
				{0,1,1,3,2,3,3,3,3,3,3,3,3,2,3,4,4,4,4,4,3,3,2,3,3,3,2},
				{0,3,3,3,2,3,3,3,3,3,3,3,3,3,3,4,4,5,4,4,3,3,2,3,3,3,2},
				{0,3,3,3,2,3,3,3,3,3,3,3,3,3,6,4,4,5,4,4,6,3,2,3,3,3,2},
				{0,3,3,3,2,3,3,3,3,3,3,3,6,6,6,6,4,5,4,6,6,6,2,6,3,3,2},
				{0,3,3,6,2,6,6,6,6,6,6,6,6,6,6,6,6,5,6,6,6,6,2,6,6,3,2},
				{0,3,6,6,2,6,6,6,6,6,6,6,6,6,6,6,6,5,6,6,6,6,2,6,6,3,2},
				{0,6,6,6,2,6,6,5,5,5,5,5,5,5,5,5,5,5,6,6,6,6,6,6,6,6,2},
				{0,6,6,6,2,6,6,6,6,6,6,6,6,6,6,6,6,5,6,6,6,6,6,6,6,6,2},
				{0,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,5,6,6,6,6,6,6,6,6,2},
				{0,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,5,6,6,6,6,6,6,6,6,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
			};
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								break;
							case 1:
								tile.WallType = WallID.MudstoneBrick;
								break;
							case 2:
								tile.WallType = WallID.RichMaogany;
								break;
							case 3:
								tile.WallType = WallID.Jungle;
								break;
							case 4:
								tile.WallType = EvostoneWall;
								break;
							case 5:
								tile.WallType = WallID.TopazGemspark;
								break;
							case 6:
								tile.WallType = WallID.LihzahrdBrick; //safe version intentionally
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24,24},
				{24,24,24,24,24,24,1,1,1,2,2,2,2,2,2,2,2,2,2,1,1,1,24,24,24,24,24},
				{24,24,24,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,24},
				{24,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
				{2,2,2,2,2,2,2,2,3,0,0,4,2,5,2,2,2,2,2,2,2,2,5,2,3,0,5},
				{2,2,2,2,5,3,0,0,0,0,0,0,0,5,4,2,2,2,2,2,2,2,5,0,0,0,5},
				{2,2,2,3,5,0,0,0,0,0,0,0,0,5,0,0,0,0,0,2,2,0,5,0,0,0,5},
				{2,3,0,0,5,8,9,9,9,6,0,9,13,5,0,0,0,0,0,0,0,0,5,0,0,0,5},
				{0,0,0,0,15,16,16,16,16,16,16,16,16,15,0,0,0,14,0,0,0,0,5,0,7,0,5},
				{0,0,0,0,5,0,0,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,15,16,16,16,15},
				{0,0,0,0,5,0,0,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,5,0,0,0,5},
				{0,0,0,0,5,0,0,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,5,0,0,0,5},
				{0,0,0,0,5,0,0,17,0,0,0,0,0,5,0,0,0,0,0,0,0,18,5,19,18,0,5},
				{0,0,0,19,5,20,20,21,21,20,20,19,19,5,0,0,0,0,0,0,18,20,20,20,20,0,5},
				{0,0,20,20,20,20,20,20,20,20,20,20,20,20,19,18,0,0,0,0,20,20,20,20,20,22,5},
				{0,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,19,0,0,23,20,20,20,20,20,20,20},
				{23,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,21,20,20,20,20,20,20,20,20,20},
				{20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20,20}
			};
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)   
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = TileID.Mudstone;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = TileID.Mudstone;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = TileID.Mudstone;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = TileID.Mudstone;
									tile.Slope = (SlopeType)4;
									tile.IsHalfBlock = false;
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = TileID.RichMahoganyBeam;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, RuinedChest, true, true, -1, 1);
									}
									break;
								case 7:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 219, true, true, -1, 0);
									}
									break;
								case 8:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, TileID.Bottles, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 9:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, TileID.Books, true, true, -1, Main.rand.Next(6));
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 13:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 239, true, true, -1, 5);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 14:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, GemLock, true, true, -1, 3);
									}
									break;
								case 15:
									tile.HasTile = true;
									tile.TileType = 158;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 16:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 2);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 17:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, GemChest, true, true, -1, 7);
									}
									break;
								case 18:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 178, true, true, -1, 1);
									}
									break;
								case 19:
									tile.HasTile = true;
									tile.TileType = 226;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 20:
									tile.HasTile = true;
									tile.TileType = 226;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 21:
									tile.HasTile = true;
									tile.TileType = 263;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 22:
									tile.HasTile = true;
									tile.TileType = 226;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 23:
									tile.HasTile = true;
									tile.TileType = 226;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
		}
        public static void GenerateSapphireIceCamp(int spawnX, int spawnY)
		{
			int PosX = spawnX - 12;
			int PosY = spawnY - 20;
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,1,1,1,1,1,1,1,1,1,1,1,1,2,2,0,0,0,0,0,0,0,0,0,0,0},
				{0,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,0,0,0,0,0,0,0,0,0},
				{0,1,1,1,1,1,1,1,1,1,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,0},
				{0,1,1,1,1,1,1,1,1,1,1,1,3,3,3,3,3,4,1,1,5,5,5,5,4,0},
				{0,1,1,3,1,1,1,1,1,1,2,2,2,3,3,3,3,4,3,3,5,5,5,5,4,0},
				{0,4,4,3,3,3,3,1,1,2,2,2,2,2,3,4,4,4,5,5,5,5,5,5,4,0},
				{0,4,4,4,4,3,3,1,3,2,2,6,2,2,3,4,4,4,5,5,5,5,5,5,4,0},
				{0,4,4,4,4,4,4,1,3,2,2,6,2,2,4,4,4,4,5,5,5,5,5,5,4,0},
				{0,4,4,4,4,4,4,1,4,4,2,6,2,4,4,4,4,4,5,5,5,5,5,5,4,0},
				{0,4,4,4,4,4,4,4,3,3,3,6,3,3,3,3,4,4,5,5,5,5,5,5,4,0},
				{0,4,4,4,4,4,4,3,3,3,3,6,3,3,3,3,3,4,3,5,5,5,5,5,4,0},
				{0,4,4,4,4,1,1,1,1,3,3,6,3,3,3,3,3,4,3,3,1,5,5,5,4,0},
				{0,4,4,4,1,1,1,1,1,1,3,6,3,3,3,3,3,4,3,1,1,5,5,5,4,0},
				{0,0,4,1,1,3,1,1,1,1,1,6,3,3,1,1,1,4,3,1,1,1,5,5,4,0},
				{0,0,4,1,1,1,1,1,3,1,1,6,3,3,1,1,1,4,3,3,3,3,4,4,0,0},
				{0,0,0,4,4,4,4,4,4,6,6,6,6,6,6,6,6,1,3,3,4,4,4,0,0,0},
				{0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,3,3,3,4,4,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
			};
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 266;
								break;
							case 2:
								tile.WallType = EvostoneWall;
								break;
							case 3:
								tile.WallType = 249;
								break;
							case 4:
								tile.WallType = 1;
								break;
							case 5:
								tile.WallType = 149;
								break;
							case 6:
								tile.WallType = 165;
								break;
						}
					}
				}
			}
			_structure = new int[,]  {
				{37,37,37,37,37,37,37,37,37,37,0,0,0,0,37,37,37,37,37,37,37,37,37,37,37,37},
				{37,37,37,37,37,37,0,0,0,0,0,0,0,0,0,0,0,0,0,0,37,37,37,37,37,37},
				{37,37,0,0,0,0,0,0,1,1,2,2,2,3,4,0,0,0,0,0,0,0,0,37,37,37},
				{0,0,0,0,0,0,5,6,2,2,2,2,3,3,4,7,0,0,0,0,0,0,0,0,0,0},
				{0,2,8,0,0,0,3,2,2,2,2,9,9,4,4,4,0,0,0,5,5,10,4,4,10,0},
				{3,2,2,11,11,11,3,2,2,0,0,0,12,0,4,13,0,10,4,4,4,4,9,9,3,3},
				{3,3,9,0,0,0,3,9,9,0,0,0,12,0,4,0,0,4,9,9,9,9,3,3,2,2},
				{3,9,3,0,0,0,9,3,9,0,0,0,0,0,0,0,0,14,14,14,14,14,14,14,14,2},
				{0,12,3,0,0,0,9,3,9,0,0,0,0,0,0,0,0,15,0,0,0,0,0,0,15,0},
				{0,12,3,0,0,0,12,3,3,0,0,0,0,0,0,0,0,15,0,0,0,0,0,0,15,0},
				{0,0,0,0,0,0,0,3,0,0,0,17,0,0,0,0,0,15,0,0,0,0,0,0,15,0},
				{0,18,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,15,19,19,19,0,16,0,15,0},
				{0,18,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,15,22,22,22,22,22,22,15,0},
				{23,18,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15,0,0,0,0,0,0,15,0},
				{25,25,26,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15,19,19,24,0,19,19,15,28},
				{25,25,18,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15,22,22,22,22,22,22,15,18},
				{25,25,18,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15,0,0,0,0,0,0,14,25},
				{18,25,25,0,0,30,31,31,30,0,0,0,0,0,0,0,0,15,0,0,29,0,0,33,14,25},
				{25,18,18,34,31,31,31,31,31,31,31,31,30,0,0,32,0,15,35,14,14,14,14,14,18,18},
				{25,18,18,25,31,31,31,31,31,31,31,31,31,31,31,36,36,14,14,14,14,25,25,18,18,25},
				{25,25,18,18,18,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,34,18,18,18,18,25},
				{25,25,25,18,18,25,9,9,9,9,9,9,9,9,9,9,9,25,25,18,18,18,18,18,25,25},
				{25,25,25,25,18,18,18,25,25,25,25,9,9,9,9,9,25,25,25,25,25,18,25,25,25,25}
			};
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<FrigidIceTile>();
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<FrigidIceTile>();
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 206;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = EvostoneBrick;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 5:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 178, true, true, -1, 2);
									}
									break;
								case 6:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<FrigidIceTile>();
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 7:
									tile.HasTile = true;
									tile.TileType = EvostoneBrick;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 8:
									tile.HasTile = true;
									tile.TileType = 206;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 9:
									tile.HasTile = true;
									tile.TileType = 161;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 10:
									tile.HasTile = true;
									tile.TileType = EvostoneBrick;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 11:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 43);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 12:
									tile.HasTile = true;
									tile.TileType = 165;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 13:
									tile.HasTile = true;
									tile.TileType = EvostoneBrick;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 14:
									tile.HasTile = true;
									tile.TileType = 321;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 15:
									tile.HasTile = true;
									tile.TileType = 574;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 16:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 101, true, true, -1, 25);
									}
									break;
								case 17:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, GemLock, true, true, -1, 1);
									}
									break;
								case 18:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 19:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 50, true, true, -1, WorldGen.genRand.Next(6));
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 22:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 19);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 23:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 24:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, RuinedChest, true, true, -1, 1);
									}
									break;
								case 25:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 26:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 28:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 29:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 376, true, true, -1, 18);
									}
									break;
								case 30:
									tile.HasTile = true;
									tile.TileType = 147;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 31:
									tile.HasTile = true;
									tile.TileType = 147;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 32:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, GemChest, true, true, -1, 2);
									}
									break;
								case 33:
									tile.HasTile = true;
									tile.TileType = 321;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 34:
									tile.HasTile = true;
									tile.TileType = 63;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 35:
									tile.HasTile = true;
									tile.TileType = 321;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 36:
									tile.HasTile = true;
									tile.TileType = 264;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
		}
        public static void GenerateRubyAbandonedLab(int spawnX, int spawnY, bool crimson = false)
		{
			int PosX = spawnX - 10;
			int PosY = spawnY - 5;
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,0,0,0,2,2,2,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
				{0,0,0,0,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,0,0},
				{0,0,0,0,0,0,2,2,2,2,2,0,0,1,1,1,1,1,1,1,1,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,0,0},
				{0,0,0,0,3,3,3,3,3,3,3,3,0,1,1,1,1,1,1,1,1,0,0},
				{0,0,0,0,0,3,3,3,3,3,3,0,1,1,1,1,1,1,1,1,1,0,0},
				{0,0,1,1,0,0,3,3,3,3,0,1,1,1,1,1,1,1,1,1,1,1,0},
				{0,0,1,1,1,0,4,3,3,4,1,1,1,1,1,1,1,1,1,1,1,1,0},
				{0,0,1,1,1,1,4,3,3,4,1,1,1,1,1,0,0,0,0,0,0,0,0},
				{0,0,1,1,1,1,4,3,3,4,1,1,5,5,5,5,5,5,5,5,5,5,0},
				{0,0,6,6,1,1,4,3,3,4,1,1,5,1,1,0,3,3,3,3,3,3,0},
				{0,6,6,6,6,1,4,3,3,4,1,1,5,1,1,0,3,3,3,3,3,3,0},
				{0,6,6,0,6,6,4,3,3,4,1,1,5,1,1,1,0,3,3,3,3,3,0},
				{0,0,0,0,0,0,0,5,5,5,5,5,5,1,1,1,1,0,0,3,3,3,0},
				{0,7,7,7,7,7,4,3,3,4,1,1,5,1,1,1,1,1,0,0,0,0,0},
				{0,7,3,3,3,0,4,3,3,4,1,1,5,1,1,1,1,1,4,0,0,0,4},
				{0,7,3,3,0,1,4,3,3,4,1,1,5,1,1,1,1,1,4,8,8,8,4},
				{0,7,3,0,1,1,4,3,3,4,1,1,5,1,1,1,1,1,4,8,8,8,4},
				{0,0,4,1,1,1,4,3,3,4,1,7,5,7,1,1,1,1,4,8,8,8,4},
				{0,0,4,0,0,6,6,3,3,4,7,7,5,7,7,1,1,1,4,8,8,8,4},
				{0,0,4,4,4,6,6,3,3,6,7,7,5,7,7,4,1,1,4,8,8,8,4},
				{0,0,0,0,0,0,6,3,3,6,7,7,7,7,7,4,1,4,0,0,0,0,0},
				{0,0,0,0,0,0,0,4,4,4,7,7,7,7,7,4,4,4,0,0,0,0,0},
				{0,0,0,0,0,0,7,7,7,7,7,7,7,7,7,7,7,7,7,7,0,0,0},
				{0,0,0,0,0,0,7,7,7,7,7,7,7,7,7,7,7,7,7,7,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
			};
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								if(tile.WallType == 0)
									tile.WallType = crimson ? WallID.CrimstoneUnsafe : WallID.EbonstoneUnsafe;
								break;
							case 1:
								tile.WallType = crimson ? WallID.CrimstoneUnsafe : WallID.EbonstoneUnsafe;
								break;
							case 2:
								tile.WallType = (ushort)ModContent.WallType<CharredWoodWallTile>();
								break;
							case 3:
								tile.WallType = crimson ? WallID.LeadBrick : WallID.IronBrick;
								break;
							case 4:
								tile.WallType = crimson ? WallID.ShadewoodFence : WallID.EbonwoodFence;
								break;
							case 5:
								tile.WallType = WallID.RubyGemspark;
								break;
							case 6:
								tile.WallType = (ushort)ModContent.WallType<SootWallTile>();
								break;
							case 7:
								tile.WallType = EvostoneWall;
								break;
							case 8:
								tile.WallType = crimson ? WallID.Shadewood : WallID.Ebonwood;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
				{0,0,0,1,3,4,4,4,3,1,2,1,1,5,1,1,1,1,1,1,1,0,0},
				{0,0,0,4,4,4,4,4,4,8,8,8,8,5,1,1,6,1,1,6,1,0,0},
				{0,0,0,9,9,9,9,9,9,9,9,9,9,5,1,1,1,1,1,1,1,0,0},
				{0,0,0,10,11,1,12,13,10,11,1,12,13,5,1,1,1,1,1,1,1,0,0},
				{0,0,0,1,10,9,13,1,1,10,9,13,1,5,1,1,1,1,1,1,1,0,0},
				{0,0,1,1,1,10,11,1,1,12,13,1,1,5,1,1,1,1,1,1,1,1,0},
				{0,0,1,1,1,1,10,11,12,13,1,1,1,5,1,1,1,1,1,1,1,1,0},
				{0,0,1,1,1,1,1,9,9,1,1,1,1,5,1,14,14,1,14,14,1,14,9},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,5,1,9,15,15,15,15,15,15,9},
				{0,1,1,1,1,1,1,1,1,1,1,1,1,5,1,9,9,9,9,9,9,9,9},
				{0,1,16,16,17,1,1,1,1,1,1,1,1,5,1,10,11,1,1,1,12,13,9},
				{0,19,19,19,19,18,1,1,1,1,1,1,1,5,1,1,10,11,1,12,13,1,9},
				{20,20,20,21,21,22,22,21,5,1,1,1,1,5,1,1,1,10,9,13,1,1,9},
				{20,9,9,9,9,9,9,21,5,1,1,1,1,5,1,1,1,1,9,9,9,9,9},
				{21,9,1,1,12,13,1,1,5,1,1,1,1,5,1,1,1,1,8,8,8,8,8},
				{21,9,1,12,13,1,1,1,5,1,1,1,1,5,1,1,1,1,1,1,1,1,1},
				{21,9,12,13,1,1,1,1,5,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
				{21,9,13,1,1,1,1,1,5,1,1,1,1,1,1,1,1,1,1,23,24,23,1},
				{21,20,1,17,16,16,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
				{21,20,1,19,19,19,28,25,1,1,1,1,26,1,1,1,1,1,1,1,27,1,1},
				{21,21,8,8,8,4,4,19,19,28,29,1,1,1,29,29,29,29,8,8,8,8,8},
				{21,21,30,30,30,30,4,19,19,19,19,16,16,31,31,31,31,31,30,30,30,30,30},
				{21,21,20,20,20,21,30,4,4,4,4,8,8,8,8,8,8,8,30,20,20,21,21},
				{21,21,21,21,20,21,30,30,30,30,30,30,30,30,30,30,30,30,30,21,20,20,21},
				{21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21,21}
			};
			for (int confirmPlatforms = 0; confirmPlatforms < 3; confirmPlatforms++)
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 1:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 2:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.DemonAltar, true, true, -1, crimson ? 1 : 0);
									}
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<CharredWoodTile>();
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = TileID.Chain;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										tile.TileType = TileID.ShadowOrbs;
										tile.HasTile = true;
										Main.tile[k, l + 1].TileType = TileID.ShadowOrbs;
										Main.tile[k + 1, l + 1].TileType = TileID.ShadowOrbs;
										Main.tile[k + 1, l].TileType = TileID.ShadowOrbs;
										Framing.GetTileSafely(k, l + 1).HasTile = true;
										Framing.GetTileSafely(k + 1, l + 1).HasTile = true;
										Framing.GetTileSafely(k + 1, l).HasTile = true;
										Framing.GetTileSafely(k, l + 1).TileFrameY += 18;
										Framing.GetTileSafely(k + 1, l + 1).TileFrameX += 18;
										Framing.GetTileSafely(k + 1, l + 1).TileFrameY += 18;
										Framing.GetTileSafely(k + 1, l).TileFrameX += 18;
										if (crimson)
                                        {
											tile.TileFrameX += 36;
											Framing.GetTileSafely(k, l + 1).TileFrameX += 36;
											Framing.GetTileSafely(k + 1, l + 1).TileFrameX += 36;
											Framing.GetTileSafely(k + 1, l).TileFrameX += 36;
										}
									}
									break;
								case 8:
									tile.HasTile = true;
									tile.TileType = crimson ? TileID.Shadewood : TileID.Ebonwood;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 9:
									tile.HasTile = true;
									tile.TileType = crimson ? TileID.LeadBrick : TileID.IronBrick;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 10:
									tile.HasTile = true;
									tile.TileType = crimson ? TileID.LeadBrick : TileID.IronBrick;
									tile.Slope = (SlopeType)4;
									tile.IsHalfBlock = false;
									break;
								case 11:
									tile.HasTile = true;
									tile.TileType = crimson ? TileID.LeadBrick : TileID.IronBrick;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 12:
									tile.HasTile = true;
									tile.TileType = crimson ? TileID.LeadBrick : TileID.IronBrick;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 13:
									tile.HasTile = true;
									tile.TileType = crimson ? TileID.LeadBrick : TileID.IronBrick;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 14:
									tile.HasTile = true;
									tile.TileType = crimson ? TileID.LeadBrick : TileID.IronBrick;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 15:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<Items.Fragments.DissolvingUmbraTile>();
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 16:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 17:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 178, true, true, -1, 4);
									}
									break;
								case 18:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, GemChest, true, true, -1, 1);
									}
									break;
								case 19:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 20:
									tile.HasTile = true;
									tile.TileType = Evostone;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 21:
									tile.HasTile = true;
									tile.TileType = EvostoneBrick;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 22:
									tile.HasTile = true;
									tile.TileType = 266;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 23:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 50, true, true, -1, WorldGen.genRand.Next(6));
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 24:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 49, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 25:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, RuinedChest, true, true, -1, 1);
									}
									break;
								case 26:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, GemLock, true, true, -1, 0);
									}
									break;
								case 27:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 14, true, true, -1, 1);
									}
									break;
								case 28:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<SootBlockTile>();
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 29:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
										tile.LiquidAmount = 255;
										tile.LiquidType = 0;
									}
									break;
								case 30:
									tile.HasTile = true;
									tile.TileType = 152;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 31:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
										tile.LiquidAmount = 255;
										tile.LiquidType = 0;
									}
									break;
							}
						}
					}
				}
			}
		}
        public static void GenerateDiamondSkyStructure(int spawnX, int spawnY)
		{
			int PosX = spawnX - 31;
			int PosY = spawnY - 28;
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,2,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,2,3,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,2,3,3,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,3,3,3,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,2,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,3,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
			};
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								break;
							case 1:
								tile.WallType = EvostoneWall;
								break;
							case 2:
								tile.WallType = 155;
								break;
							case 3:
								tile.WallType = 73;
								break;
							case 4:
								tile.WallType = (ushort)ModContent.WallType<DullPlatingWallWall>();
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,8,8,8,7,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,9,8,8,7,7,10,0,2,0,0,0,0,0,0,0,0,0,3,0,4,0,5,10,0,0,0,10,0,0,0,0,0,0,6,0,0,0,0,0,0,0,0,0,10,0,7,8,8,8,8,8,8,7,0,0,0,0,0},
				{0,0,0,0,0,0,9,8,8,8,8,8,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,0,0,0,11,11,11,11,11,11,11,12,12,11,11,11,11,11,11,11,11,11,8,8,8,8,8,8,8,8,8,8,0,0,0,0},
				{0,0,0,0,0,0,8,8,8,8,8,8,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,13,0,0,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,8,8,8,8,8,8,8,8,8,8,0,0,0,0},
				{0,0,0,0,0,0,8,8,8,8,8,8,8,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,0,0,0,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,8,8,8,8,8,8,8,8,8,8,0,0,0,0,0},
				{0,0,0,0,0,0,0,8,8,8,8,8,8,8,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,0,0,13,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,8,8,8,8,8,8,8,8,8,8,8,0,0,0,0,0},
				{0,0,0,0,0,9,8,8,8,8,8,8,8,8,8,8,11,11,11,11,11,11,11,11,11,11,11,11,11,11,0,0,0,11,11,11,11,11,11,11,11,11,11,11,11,11,11,8,8,8,8,8,8,8,8,8,8,8,8,14,0,0,0,0,0},
				{0,0,0,9,8,8,8,8,8,8,8,8,8,8,8,8,8,8,11,11,11,11,11,11,11,11,11,11,11,11,13,0,0,11,11,11,11,11,11,11,11,11,11,11,11,8,8,8,8,8,8,8,8,8,8,8,8,0,0,0,0,0,0,0,0},
				{7,7,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,11,11,11,11,11,11,11,11,11,11,0,0,0,11,11,11,11,11,11,11,11,11,11,8,8,8,8,8,8,8,8,8,8,8,8,8,14,0,0,0,0,0,0,0,0},
				{8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,11,11,11,8,8,11,11,0,0,13,11,11,8,8,11,11,11,8,8,8,8,8,8,8,8,8,8,8,8,8,8,14,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,15,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,11,11,11,8,8,11,11,0,0,0,11,11,8,8,11,11,11,8,8,8,8,8,8,8,8,8,8,8,8,8,8,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,15,8,8,8,8,8,8,8,8,8,8,11,11,11,11,11,11,11,11,11,11,11,11,13,0,0,11,11,11,11,11,11,11,11,11,11,11,8,8,8,8,8,8,8,8,8,8,8,8,8,8,7,7,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,8,8,8,11,11,11,11,11,11,11,11,11,11,11,11,0,0,0,11,11,11,11,11,11,11,11,11,11,11,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,16,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,7,8,8,8,8,11,11,11,11,11,11,11,11,11,11,11,11,0,0,13,11,11,11,11,11,11,11,11,11,11,11,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8},
				{0,0,0,0,0,0,0,0,0,0,0,9,8,8,8,8,8,8,8,8,8,8,8,11,11,11,8,8,11,11,0,0,0,11,11,8,8,11,11,11,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,14,0,0,0},
				{0,0,0,0,0,0,0,0,0,8,8,8,8,8,8,8,8,8,8,8,8,8,8,11,11,11,8,8,11,11,13,0,0,11,11,8,8,11,11,11,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,14,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,9,8,8,8,8,8,8,8,8,8,8,8,8,8,8,11,11,11,8,8,11,11,0,0,0,11,11,8,8,11,11,11,8,8,8,8,8,8,8,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,11,11,11,0,0,0,0,0,0,0,11,11,8,8,11,11,11,8,8,8,8,8,8,8,8,8,7,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,15,8,8,8,8,8,8,8,8,8,8,8,8,11,11,11,0,0,0,0,0,0,0,11,11,8,8,11,11,11,8,8,8,8,8,8,8,8,8,8,8,8,8,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,15,8,8,8,8,8,8,8,8,11,11,11,0,0,0,0,0,0,0,11,11,8,8,11,11,11,8,8,8,8,8,8,8,8,8,8,8,8,14,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,11,11,8,8,11,11,11,8,8,8,8,8,8,8,8,8,14,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,10,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,17,17,17,17,17,17,17,11,17,17,17,17,17,17,17,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,10,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,17,17,17,17,17,17,17,11,17,17,17,17,17,17,17,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
			};
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									break;
								case 1:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, GemLock, true, true, -1, 5);
									}
									break;
								case 2:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<HardlightFabricatorTile>(), true, true, -1, 0);
									}
									break;
								case 3:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<HardlightChairTile>(), true, true, -1, 1);
									}
									break;
								case 4:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<HardlightTableTile>(), true, true, -1, 0);
									}
									break;
								case 5:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, ModContent.TileType<HardlightChairTile>(), true, true, -1, 0);
									}
									break;
								case 6:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, GemChest, true, true, -1, 11);
									}
									break;
								case 7:
									tile.HasTile = true;
									tile.TileType = 189;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									tile.WallType = WallID.Cloud;
									break;
								case 8:
									tile.HasTile = true;
									tile.TileType = 189;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									tile.WallType = WallID.Cloud;
									break;
								case 9:
									tile.HasTile = true;
									tile.TileType = 189;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									tile.WallType = WallID.Cloud;
									break;
								case 10:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, ModContent.TileType<SkyChainTile>(), true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 11:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<DullPlatingTile>();
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 12:
									tile.HasTile = true;
									tile.TileType = 267;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 13:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 178, true, true, -1, 5);
									}
									break;
								case 14:
									tile.HasTile = true;
									tile.TileType = 189;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									tile.WallType = WallID.Cloud;
									break;
								case 15:
									tile.HasTile = true;
									tile.TileType = 189;
									tile.Slope = (SlopeType)4;
									tile.IsHalfBlock = false;
									tile.WallType = WallID.Cloud;
									break;
								case 16:
									tile.HasTile = true;
									tile.TileType = 189;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									tile.WallType = WallID.Cloud;
									break;
								case 17:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<HardlightBlockTile>();
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}

		}
		public static void GenerateAmberWaterVault(int spawnX, int spawnY)
		{
			int PosX = spawnX - 26;
			int PosY = spawnY - 5;
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,3,3,3,3,3,1,1,1,1,1,2,2,2,1,1,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,3,3,3,3,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,1,1,1,1,1,1,1,4,1,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,3,3,1,1,1,1,1,1,3,3,2,2,2,3,3,3,3,3,3,3,1,1,1,4,4,4,4,1,2,2},
				{2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,3,3,3,3,2,2,2,2,2,3,3,3,3,3,3,3,3,1,1,1,1,1,1,2,2},
				{2,2,2,2,2,2,2,2,2,2,1,1,1,5,5,5,5,5,5,5,5,5,5,2,2,6,6,6,6,6,3,3,3,1,1,1,1,1,1,2,2},
				{2,2,2,2,7,7,7,7,7,3,1,1,3,5,3,3,3,3,3,3,2,2,2,2,2,6,6,6,6,6,6,6,3,2,1,1,1,1,1,2,2},
				{2,2,2,2,7,7,7,7,7,3,3,3,3,5,3,3,6,6,6,6,6,2,2,2,6,6,6,6,6,6,6,6,6,2,4,4,4,1,1,2,2},
				{2,2,2,2,7,7,7,7,7,3,3,3,3,5,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,2,4,4,4,4,2,2,2},
				{2,2,2,2,7,7,7,7,7,3,3,3,6,5,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,2,2,2,4,4,2,2,2,2},
				{2,2,2,2,7,7,7,7,7,3,3,6,6,5,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,2,2,2,4,4,2,2,2,2},
				{2,2,2,2,7,7,7,7,7,3,3,6,6,5,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,2,2,2,4,4,2,2,2,2},
				{2,2,2,8,8,8,8,8,8,8,6,6,6,5,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,2,2,2,4,4,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,6,6,6,5,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,2,2,2,4,4,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,6,6,5,8,8,8,8,8,8,8,8,8,6,6,6,6,6,6,6,6,6,2,2,2,4,4,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,5,5,5,5,5,5,6,6,6,6,6,6,6,6,6,6,6,6,6,2,2,2,4,4,2,2,2,2},
				{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,6,6,6,6,2,9,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4,2,2,2,2},
				{2,2,2,4,4,4,4,4,4,4,4,4,4,4,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,4,2,2,2,2},
				{2,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,2},
				{2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,2},
				{2,2,4,4,4,4,4,4,11,11,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,2},
				{2,2,4,4,4,4,4,4,4,11,11,11,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,11,11,2,2,2},
				{2,2,4,4,4,4,4,4,4,4,11,11,11,11,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,11,11,11,2,2,2,2,2,2},
				{2,2,4,4,4,4,4,4,4,4,11,11,11,11,11,11,11,4,4,4,4,4,4,4,4,4,4,4,11,11,11,11,11,2,2,2,2,2,2,2,2},
				{2,2,4,4,4,4,4,4,4,4,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,2,2,2,2,2,2,2,2},
				{2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0}
			};
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = 275;
								break;
							case 2:
								tile.WallType = EvostoneWall;
								break;
							case 3:
								tile.WallType = 226;
								break;
							case 4:
								tile.WallType = WallID.LeadBrick;
								break;
							case 5:
								tile.WallType = 153;
								break;
							case 6:
								tile.WallType = 304;
								break;
							case 7:
								tile.WallType = 151;
								break;
							case 8:
								tile.WallType = 235;
								break;
							case 9:
								tile.WallType = 346;
								break;
							case 11:
								tile.WallType = 323;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,2,0,0,0,0,0,0,0,2,3,2,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,2,0,0,0,0,0,0,0,2,3,2,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,2,0,0,0,0,0,0,0,2,3,2,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,3,2,0,0,0,0,0,0,0,2,3,2,2,2,2,2,2,2,2,2},
				{0,0,0,0,0,0,0,0,2,4,4,4,4,4,4,4,4,4,4,4,4,3,3,3,3,3,3,3,3,3,3,3,4,4,4,4,4,4,4,4,2},
				{0,0,0,0,0,0,0,0,2,4,2,2,2,2,2,2,5,2,2,2,2,2,6,7,7,7,7,7,7,7,8,5,5,2,2,2,2,2,2,4,2},
				{2,2,2,2,2,2,2,2,2,4,2,9,9,9,9,5,5,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,5,5,9,9,9,9,2,4,2},
				{2,4,4,4,4,4,4,4,4,4,2,9,9,5,5,5,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,5,5,9,9,9,5,4,2},
				{2,4,2,2,2,2,2,2,2,5,5,5,5,5,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,5,5,5,5,5,4,2},
				{2,4,2,11,11,11,11,11,11,11,7,7,7,7,7,7,7,7,7,7,7,7,10,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,5,4,2},
				{2,4,2,11,7,7,7,7,7,12,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,13,13,13,14,15,7,7,7,7,7,7,7,7,2,4,2},
				{2,4,2,11,16,16,18,19,17,12,7,7,7,7,7,7,20,13,13,13,13,9,9,9,9,9,9,9,9,9,13,7,7,7,7,7,7,7,2,4,2},
				{2,4,2,11,21,21,21,21,21,12,21,21,21,21,21,5,22,22,9,9,9,9,9,9,9,9,9,9,9,9,9,23,7,7,7,7,7,7,2,4,2},
				{2,4,2,11,7,7,7,7,7,12,7,7,7,7,7,12,25,22,22,22,22,22,9,9,9,9,9,9,9,9,9,9,4,4,4,7,7,4,4,4,2},
				{2,4,2,11,16,16,7,7,7,12,7,7,7,7,7,12,7,7,27,7,27,25,5,9,22,22,9,9,9,9,9,9,4,2,4,7,7,4,2,4,2},
				{2,4,2,11,11,11,7,7,7,12,7,7,7,7,7,12,7,7,7,7,7,7,12,25,22,22,22,9,9,9,9,9,4,2,4,7,7,4,2,4,2},
				{2,4,2,11,7,7,7,26,7,12,7,15,7,7,7,12,7,7,7,7,7,7,12,7,27,7,22,22,9,9,9,9,4,2,4,7,7,4,2,4,2},
				{2,4,2,11,11,11,11,11,11,11,9,9,13,13,28,12,28,7,7,28,28,28,12,30,13,9,9,9,9,9,9,9,4,2,4,7,7,4,2,4,2},
				{2,4,2,11,11,11,11,11,11,11,9,9,9,9,9,12,31,29,7,31,14,13,12,9,9,9,9,9,9,9,9,9,4,2,4,7,7,4,2,4,2},
				{2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,5,9,3,3,9,9,9,5,4,4,4,4,4,4,4,4,4,4,2,4,7,7,4,2,4,2},
				{2,4,32,32,32,32,32,32,32,32,32,32,32,32,4,9,9,9,9,9,9,9,9,4,2,2,2,2,2,2,2,2,2,2,4,7,7,4,2,4,2},
				{2,4,2,7,7,7,7,7,7,7,7,7,7,7,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,7,7,4,4,4,2},
				{2,4,2,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,2,4,2},
				{2,4,2,33,7,34,7,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,2,4,2},
				{2,4,4,4,4,4,4,4,4,5,31,20,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,27,30,5,4,2},
				{2,2,2,2,2,2,2,2,2,5,5,9,13,30,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,27,27,36,37,9,9,5,4,2},
				{0,0,0,0,0,0,0,0,0,2,5,9,9,9,23,31,31,31,31,31,31,31,31,31,31,31,31,31,31,38,13,13,9,9,9,9,9,9,5,4,2},
				{0,0,0,0,0,0,0,0,0,2,5,5,5,5,9,9,9,13,13,31,15,31,36,31,31,31,31,31,9,9,9,9,9,9,9,9,5,5,5,4,2},
				{0,0,0,0,0,0,0,0,0,2,4,4,4,5,5,5,5,5,5,5,4,4,4,4,4,4,4,5,5,5,5,5,5,5,5,5,5,4,4,4,2},
				{0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2}
			};
			for (int confirmPlatforms = 0; confirmPlatforms < 3; confirmPlatforms++)
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 1:
									tile.HasTile = true;
									tile.TileType = 473;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = 473;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = 268;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = EvostoneBrick;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = 479;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									tile.HasTile = true;
									tile.TileType = 473;
									tile.Slope = (SlopeType)3;
									tile.IsHalfBlock = false;
									break;
								case 7:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 8:
									tile.HasTile = true;
									tile.TileType = 473;
									tile.Slope = (SlopeType)4;
									tile.IsHalfBlock = false;
									break;
								case 9:
									tile.HasTile = true;
									tile.TileType = 53;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 10:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, GemLock, true, true, -1, 6);
									}
									break;
								case 11:
									tile.HasTile = true;
									tile.TileType = 322;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 12:
									tile.HasTile = true;
									tile.TileType = 577;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 13:
									tile.HasTile = true;
									tile.TileType = 53;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 14:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 81, true, true, -1, 1);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 15:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 81, true, true, -1, 5);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 16:
								case 17:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 50, true, true, -1, WorldGen.genRand.Next(6));
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 18:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 13, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 19:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 13, true, true, -1, 8);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 20:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 81, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 21:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 17);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 22:
									tile.HasTile = true;
									tile.TileType = 397;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 23:
									tile.HasTile = true;
									tile.TileType = 53;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 24:
									if (confirmPlatforms == 2)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.CookingPots, true, true, -1, 0);
									}
									break;
								case 25:
									tile.HasTile = true;
									tile.TileType = 397;
									tile.Slope = (SlopeType)4;
									tile.IsHalfBlock = false;
									break;
								case 26:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 355, true, true, -1, 0);
									}
									break;
								case 27:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 178, true, true, -1, 6);
									}
									break;
								case 28:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
										tile.LiquidAmount = 150;
										tile.LiquidType = 0;
									}
									break;
								case 29:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, GemChest, true, true, -1, 13);
									}
									break;
								case 30:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 81, true, true, -1, 4);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 31:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
										tile.LiquidAmount = 255;
										tile.LiquidType = 0;
									}
									break;
								case 32:
									tile.HasTile = true;
									tile.TileType = 151;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 33:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 376, true, true, -1, 24);
									}
									break;
								case 34:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, RuinedChest, true, true, -1, 1);
									}
									break;
								case 35:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
										tile.LiquidAmount = 237;
										tile.LiquidType = 0;
									}
									break;
								case 36:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 81, true, true, -1, 2);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 37:
									tile.HasTile = true;
									tile.TileType = 53;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 38:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 81, true, true, -1, 3);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
						}
					}
				}
			}
		}
		public static void GenerateEmeraldVoidRuins(int spawnX, int spawnY, int height = 1)
		{
			int offsetX = 15;
			int offsetY = 14;
			GenerateEmeraldVoidRuinsTop(spawnX - offsetX, spawnY - offsetY);
			for(int j = height; j > 0; j--)
			{
				for(int i = 10; i >= -10; i--)
				{
					Tile tile = Framing.GetTileSafely(spawnX + i, spawnY + 34 + j);
					if(Math.Abs(i) <= 2)
                    {
						tile.HasTile = false;
                    }
					else
					{
						tile.HasTile = true;
						tile.TileType = EvostoneBrick;
						tile.Slope = 0;
						tile.IsHalfBlock = false;
					}
					tile.LiquidAmount = 0;
					if (Math.Abs(i) <= 9)
					{
						tile.WallType = EvostoneWall;
					}
				}
			}
			GenerateEmeraldVoidRuinsBottom(spawnX - offsetX, spawnY - offsetY + 49 + height);
		}
		public static void GenerateEmeraldVoidRuinsTop(int spawnX, int spawnY)
		{
			int PosX = spawnX;
			int PosY = spawnY;
			int[,] _structure = {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0}
			};
			//i = vertical, j = horizontal
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 1:
								tile.WallType = EvostoneWall;
								break;
						}
					}
				}
			}
			PosX -= 2;
			_structure = new int[,] {
				{0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,0,0,2,3,0,0,0,0,0,0,0,0,0,3,1,0,0,2,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,2,3,0,0,3,3,0,4,3,3,3,3,3,4,0,3,3,0,0,3,1,0,0,0,0,0,0,0},
				{0,0,0,1,0,0,0,3,3,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0,3,3,0,0,0,2,0,0,0},
				{0,0,2,3,0,0,0,3,3,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,3,3,0,0,0,3,1,0,0},
				{0,0,3,3,1,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0,2,3,3,0,0},
				{0,0,3,3,3,4,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,3,3,3,0,0},
				{0,0,3,3,3,3,3,3,3,3,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,3,3,3,3,3,3,3,3,0,0},
				{0,0,3,3,3,3,3,3,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,3,3,3,3,3,3,0,0},
				{0,0,0,3,3,3,3,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,3,3,3,3,0,0,0},
				{0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,5,5,5,5,5,0,0,0,0,7,0,7,0,0,0,0,5,5,5,5,5,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,5,5,5,5,5,7,0,0,0,0,0,0,0,0,0,7,5,5,5,5,5,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,5,5,5,5,5,0,0,0,0,0,8,0,0,0,0,0,5,5,5,5,5,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,5,5,5,5,5,7,0,0,0,0,0,0,0,0,0,7,5,5,5,5,5,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,10,10,10,10,10,10,10,5,5,5,5,5,6,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,9,0,0,0,0,0,0,0,0,5,5,5,5,5,6,6,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,9,0,0,0,0,0,0,0,0,0,5,5,5,5,5,6,6,6,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,9,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,6,6,6,6,0,0,6},
				{0,0,0,0,0,0,6,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,6,6,6,6,6,6,6},
				{6,0,0,0,0,6,6,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,6,6,6,6,6,6,6},
				{6,6,0,0,6,6,6,5,5,5,5,5,11,0,0,0,0,0,0,0,0,0,11,5,5,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,5,5,11,11,11,0,0,12,0,0,0,11,11,11,11,5,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,5,5,5,5,11,11,11,11,11,11,11,5,5,5,5,5,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,5,5,5,5,11,11,11,11,11,11,11,11,11,11,5,5,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,11,5,5,11,11,5,5,5,11,11,11,11,11,11,11,5,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,11,11,5,11,11,11,5,5,11,11,11,11,5,5,11,11,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,5,11,11,11,11,11,5,5,11,11,11,5,5,5,5,11,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,5,11,11,5,5,11,11,5,11,11,11,5,5,5,5,11,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,5,11,11,5,5,11,11,11,11,11,11,11,5,5,5,11,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,5,11,5,5,5,11,11,11,11,11,11,11,5,5,5,11,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,5,5,5,5,5,11,11,11,11,11,11,11,11,5,11,5,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,5,5,5,5,5,11,11,11,11,11,11,11,11,11,11,5,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,5,5,5,5,5,11,11,11,11,11,11,11,5,5,5,5,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,5,5,5,5,5,11,11,11,11,11,11,5,5,5,5,5,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,5,5,5,5,5,5,11,11,11,0,11,5,5,5,5,5,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,5,5,5,5,5,5,11,11,11,0,11,5,5,5,5,5,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,5,5,5,5,5,5,11,11,0,0,11,5,5,5,5,5,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,5,5,5,5,5,5,11,0,0,0,0,5,5,5,5,5,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,5,5,5,5,5,5,11,0,0,0,0,5,5,5,5,5,5,5,5,6,6,6,6,6,6,6},
				{6,6,6,6,6,6,6,5,5,5,5,5,5,5,5,0,0,0,0,0,5,5,5,5,5,5,5,5,6,6,6,6,6,6,6}
			};
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 1:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<DarkShinglesTile>();
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 2:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<DarkShinglesTile>();
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 3:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<DarkShinglesTile>();
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									tile.HasTile = true;
									tile.TileType = (ushort)ModContent.TileType<DarkShinglesTile>();
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = EvostoneBrick;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 7:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 178, true, true, -1, 3);
									}
									break;
								case 8:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 4, true, true, -1, 3);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 9:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 28);
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 10:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 28);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 11:
									tile.HasTile = true;
									tile.TileType = Evostone;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 12:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, RuinedChest, true, true, -1, 1);
									}
									break;
							}
                            if (_structure[i, j] != 6)
                            {
								tile.LiquidAmount = 0;
							}
						}
					}
				}
			}
		}
		public static void GenerateEmeraldVoidRuinsBottom(int spawnX, int spawnY)
		{
			int PosX = spawnX + 5;
			int PosY = spawnY;
			int[,] _structure = {
				{0,0,0,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,0,0,0},
				{0,0,0,1,1,1,3,1,1,1,2,1,1,1,1,1,1,1,0,0,0},
				{0,0,0,1,1,3,3,1,1,1,2,3,1,1,1,3,1,1,0,0,0},
				{0,0,0,1,1,3,2,1,1,2,2,3,2,1,1,3,3,1,0,0,0},
				{0,0,0,1,1,3,2,3,1,2,2,3,2,1,3,3,3,1,0,0,0},
				{0,0,0,1,1,3,3,3,3,2,4,2,2,2,3,3,3,1,0,0,0},
				{0,0,0,1,2,3,2,3,2,2,4,4,2,2,3,3,2,1,0,0,0},
				{0,0,0,1,3,3,2,2,2,4,4,4,2,2,3,2,2,2,0,0,0},
				{0,0,0,1,2,2,4,2,2,4,4,4,2,1,1,1,2,1,0,0,0},
				{0,0,0,1,2,4,4,4,2,4,4,4,1,1,1,1,1,1,0,0,0},
				{0,0,0,1,2,4,4,4,2,4,4,4,1,1,5,1,1,1,0,0,0},
				{0,0,0,1,2,4,4,4,2,2,4,4,1,1,5,1,1,1,0,0,0},
				{0,0,0,1,2,2,4,4,2,3,4,2,2,1,5,1,2,1,0,0,0},
				{0,0,0,1,2,2,4,2,3,3,2,3,3,3,5,2,2,1,0,0,0},
				{0,0,0,1,2,3,2,3,3,1,2,2,3,3,5,3,2,1,0,0,0},
				{0,0,0,1,2,3,3,3,1,1,2,2,1,3,5,3,2,1,0,0,0},
				{0,0,0,1,2,1,1,1,2,1,2,1,1,1,5,3,3,1,0,0,0},
				{0,0,0,1,1,1,2,2,2,2,1,1,1,1,5,1,3,1,0,0,0},
				{0,0,0,1,2,5,5,5,5,5,5,5,5,5,5,2,2,1,0,0,0},
				{0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
			};
			for (int i = 0; i < _structure.GetLength(0); i++)
			{
				for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
				{
					int k = PosX + j;
					int l = PosY + i;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (_structure[i, j])
						{
							case 0:
								tile.WallType = 0;
								break;
							case 1:
								tile.WallType = EvostoneWall;
								break;
							case 2:
								tile.WallType = 1;
								break;
							case 3:
								tile.WallType = 5;
								break;
							case 4:
								tile.WallType = 91;
								break;
							case 5:
								tile.WallType = 156;
								break;
						}
					}
				}
			}
			_structure = new int[,] {
				{0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,1,1,1,1,2,1,1,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,0,0,3,1,1,1,1,1,1,1,1,1,1,1,3,5,0,0,0},
				{0,0,0,5,1,1,1,1,1,1,1,1,1,1,1,1,1,6,5,0,0},
				{0,0,0,6,1,1,1,1,1,1,7,1,4,1,7,7,5,5,6,0,0},
				{0,0,6,5,1,8,1,1,1,1,9,9,9,9,9,9,6,5,0,0,0},
				{0,0,5,6,10,1,1,1,1,1,1,1,1,1,1,13,14,5,0,0,0},
				{0,0,0,6,5,11,1,1,1,1,8,1,12,1,15,16,14,14,0,0,0},
				{0,0,0,5,5,17,17,6,6,14,15,15,16,16,16,16,14,14,0,0,0},
				{0,0,0,0,5,5,6,6,14,14,14,16,16,16,16,14,14,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
			};
			//i = vertical, j = horizontal
			for (int confirmPlatforms = 0; confirmPlatforms < 2; confirmPlatforms++)    //Increase the iterations on this outermost for loop if tabletop-objects are not properly spawning
			{
				for (int i = 0; i < _structure.GetLength(0); i++)
				{
					for (int j = _structure.GetLength(1) - 1; j >= 0; j--)
					{
						int k = PosX + j;
						int l = PosY + i;
						if (WorldGen.InWorld(k, l, 30))
						{
							Tile tile = Framing.GetTileSafely(k, l);
							switch (_structure[i, j])
							{
								case 0:
									tile.HasTile = true;
									tile.TileType = EvostoneBrick;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 1:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 2:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, GemLock, true, true, -1, 2);
									}
									break;
								case 3:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 4, true, true, -1, 3);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 4:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, 355, true, true, -1, 0);
									}
									break;
								case 5:
									tile.HasTile = true;
									tile.TileType = 1;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 6:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 7:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 78, true, true, -1, 0);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 8:
									if (confirmPlatforms == 0)
									{
										tile.HasTile = false;
										tile.IsHalfBlock = false;
										tile.Slope = 0;
									}
									break;
								case 9:
									if (confirmPlatforms == 0)
										tile.HasTile = false;
									WorldGen.PlaceTile(k, l, 19, true, true, -1, 43);
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 10:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = (SlopeType)1;
									tile.IsHalfBlock = false;
									break;
								case 11:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, GemChest, true, true, -1, 5);
									}
									break;
								case 12:
									if (confirmPlatforms == 1)
									{
										tile.HasTile = false;
										tile.Slope = 0;
										tile.IsHalfBlock = false;
										WorldGen.PlaceTile(k, l, TileID.FishingCrate, true, true, -1, 22);
									}
									break;
								case 13:
									tile.HasTile = true;
									tile.TileType = 38;
									tile.Slope = (SlopeType)2;
									tile.IsHalfBlock = false;
									break;
								case 14:
									tile.HasTile = true;
									tile.TileType = Evostone;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 15:
									tile.HasTile = true;
									tile.TileType = 56;
									tile.Slope = 0;
									tile.IsHalfBlock = true;
									break;
								case 16:
									tile.HasTile = true;
									tile.TileType = 56;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
								case 17:
									tile.HasTile = true;
									tile.TileType = 265;
									tile.Slope = 0;
									tile.IsHalfBlock = false;
									break;
							}
							tile.LiquidAmount = 0;
						}
					}
				}
			}
		}
		private static bool isChest(Tile tile)
        {
			if (tile.TileType != TileID.Containers && tile.TileType != TileID.Containers2 && TileLoader.GetTile(tile.TileType) is Items.Furniture.ContainerType)
				return false;
			else
				return true;
		}
	}
}