using SOTS.Items.ChestItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class CrestofDasuver : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crest of Dasuver");
			Tooltip.SetDefault("Increases crit chance by 6%");
		}
		public override void SetDefaults()
		{
            item.width = 36;     
            item.height = 38;     
            item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
			item.defense = 3;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeCrit += 6;
			player.rangedCrit += 6;
			player.magicCrit += 6;
			player.thrownCrit += 6;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ShieldofDesecar>(), 1);
			recipe.AddIngredient(ModContent.ItemType<ShieldofStekpla>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
