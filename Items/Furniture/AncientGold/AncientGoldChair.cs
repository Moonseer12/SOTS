using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.AncientGold
{
	public class AncientGoldChair : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(16, 32);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<AncientGoldChairTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<RoyalGoldBrick>(), 4).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class AncientGoldChairTile : Chair<AncientGoldChair>
	{
		protected override void SetStaticDefaults(TileObjectData t)
		{
			Main.tileLighted[Type] = true;
			base.SetStaticDefaults(t);
		}
	}
}