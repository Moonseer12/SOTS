using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GhostTown
{
	public class SootBlockTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			dustType = 38; //mud
			drop = ModContent.ItemType<SootBlock>();
			AddMapEntry(new Color(57, 50, 44));
		}
	}
	public class SootBlock : ModItem
	{
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.createTile = ModContent.TileType<SootBlockTile>();
		}
	}
	public class SootWallTile : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = 38;
			drop = ModContent.ItemType<SootWall>();
			AddMapEntry(new Color(34, 29, 24));
		}
	}
	public class SootWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soot Wall");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneWall);
			item.width = 28;
			item.height = 28;
			item.rare = ItemRarityID.Blue;
			item.createWall = ModContent.WallType<SootWallTile>();
		}
	}
}