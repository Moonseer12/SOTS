using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Items.Fragments;

namespace SOTS.Items.Void
{
	public class ScarletStar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarlet Crescent");
			Tooltip.SetDefault("Increases max void by 50 and void regeneration speed 5%\nCan only be used once");
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.useAnimation = 12;
			Item.useTime = 12;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = 0;
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 999;
			Item.autoReuse = false;
			Item.consumable = true;
			ItemID.Sets.ItemNoGravity[Item.type] = false; 
		}
		public override bool UseItem(Player player)
		{
			SoundEngine.PlaySound(SoundID.NPCKilled, (int)player.Center.X, (int)player.Center.Y, 39);
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			if(voidPlayer.voidStar < 1)
			{
				voidPlayer.voidMeterMax += 50;
				VoidPlayer.VoidEffect(player, 50);
				voidPlayer.voidStar += 1;
				return true;
			}
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DissolvingEarth>(), 1);
			recipe.AddIngredient(ItemID.ManaCrystal, 1);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 5);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfOtherworld>(), 5);
			recipe.AddIngredient(ItemID.TissueSample, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}