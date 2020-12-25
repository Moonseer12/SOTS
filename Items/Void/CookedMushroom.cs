using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;

namespace SOTS.Items.Void
{
	public class CookedMushroom : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cooked Mushroom");
			Tooltip.SetDefault("Automatically consumed when void is low\nRefills 13 void\nCauses temporary poison");
		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 22;
            item.value = Item.sellPrice(0, 0, 12, 50);
			item.rare = 1;
			item.maxStack = 999;
			item.useStyle = 2;
			item.useTime = 15;
			item.useAnimation = 15;
			item.UseSound = SoundID.Item2;
			item.consumable = true;
		}
		public override bool UseItem(Player player)
		{
			return true;
		}
		public void RefillEffect(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			player.AddBuff(BuffID.Poisoned, 120, true);
			voidPlayer.voidMeter += 13;
			VoidPlayer.VoidEffect(player, 13);
		}
		public override bool ConsumeItem(Player player)
		{
			return true;
		}
		public override void OnConsumeItem(Player player)
		{
			RefillEffect(player);
			base.OnConsumeItem(player);
		}
		public override void UpdateInventory(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			while(voidPlayer.voidMeter < (voidPlayer.voidMeterMax2 - voidPlayer.lootingSouls) / 10 && voidPlayer.voidMeterMax2 - voidPlayer.lootingSouls > 40)
			{
				RefillEffect(player);
				item.stack--;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.VileMushroom, 2);
			recipe.AddTile(TileID.CookingPots);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ViciousMushroom, 2);
			recipe.AddTile(TileID.CookingPots);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}