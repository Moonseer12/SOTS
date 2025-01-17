using SOTS.Items.Fragments;
using SOTS.Items.Potions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Inferno
{	
	public class WispInAJar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wisp in a Jar");
			Tooltip.SetDefault("Summons a Inferno Wisp that assists in combat\nKilled enemies explode into flames for 30% of the damage dealt to them on the killing blow");
		}
		public override void SetDefaults()
		{
			item.damage = 60;
			item.summon = true;
            item.width = 26;     
            item.height = 34;   
            item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.petFreeWisp += (int)(item.damage * (1f + (player.minionDamage - 1f) + (player.allDamage - 1f)));
			modPlayer.BlueFireOrange = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Glass, 20);
			recipe.AddIngredient(ModContent.ItemType<DissolvingNether>(), 1);
			recipe.AddIngredient(ModContent.ItemType<BluefirePotion>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}