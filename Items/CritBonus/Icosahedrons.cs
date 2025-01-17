using SOTS.Items.Fragments;
using SOTS.Items.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.CritBonus
{
	public class BorealisIcosahedron : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Borealis Icosahedron");
			Tooltip.SetDefault("Critical strikes may cause a frostburn explosion, dealing 100% critical damage\n3% increased crit chance");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.Lime;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.CritFrost = true;
			player.meleeCrit += 3;
			player.rangedCrit += 3;
			player.magicCrit += 3;
			player.thrownCrit += 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DissolvingAurora>(), 1);
			recipe.AddIngredient(ItemID.FrostCore, 1);
			recipe.AddIngredient(ModContent.ItemType<AbsoluteBar>(), 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class CursedIcosahedron : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Icosahedron");
			Tooltip.SetDefault("Critical strikes may cause a release of cursed thunder, dealing 50% critical damage\nCritical strikes may also cause frostburn or flaming explosions, dealing 50% critical damage\n3% increased crit chance");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.value = Item.sellPrice(0, 5, 50, 0);
			item.rare = ItemRarityID.Yellow;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.CritCurseFire = true;
			player.meleeCrit += 3;
			player.rangedCrit += 3;
			player.magicCrit += 3;
			player.thrownCrit += 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BorealisIcosahedron>(), 1);
			recipe.AddIngredient(ModContent.ItemType<HellfireIcosahedron>(), 1);
			recipe.AddIngredient(ModContent.ItemType<DissolvingUmbra>(), 1);
			recipe.AddIngredient(ItemID.CursedFlame, 10);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class HellfireIcosahedron : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellfire Icosahedron");
			Tooltip.SetDefault("Critical strikes may cause a flaming explosion, dealing 50% critical damage\n3% increased crit chance");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.CritFire = true;
			player.meleeCrit += 3;
			player.rangedCrit += 3;
			player.magicCrit += 3;
			player.thrownCrit += 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HellstoneBar, 6);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfInferno>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
