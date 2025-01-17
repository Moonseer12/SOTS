using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using SOTS.Buffs;

namespace SOTS.Items.Celestial
{
	public class FoggyClairvoyance : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Foggy Clairvoyance");
			Tooltip.SetDefault("Increases damage by 15% and grants immunity to almost every debuff, but at a cost\n'Cursed'");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 38;     
            item.height = 40;   
            item.value = Item.sellPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Yellow;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SanguiteBar>(), 15);
			recipe.AddIngredient(ModContent.ItemType<Fragments.PrecariousCluster>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.AddBuff(ModContent.BuffType<FluidCurse>(), 3);
			List<int> bList = new List<int>() { BuffID.PotionSickness, ModContent.BuffType<FluidCurse>(), ModContent.BuffType<VoidRecovery>(), ModContent.BuffType<VoidShock>(), ModContent.BuffType<VoidSickness>(), BuffID.ManaSickness, ModContent.BuffType<Satiated>(), ModContent.BuffType<VoidMetamorphosis>(), BuffID.ChaosState };
			Mod catalyst = ModLoader.GetMod("Catalyst");
			if(catalyst != null)
            {
				bList.Add(catalyst.BuffType("InfluxCoreCooldown"));
            }
			for(int i = 0; i < player.buffImmune.Length; i++)
            {
				bool debuff = Main.debuff[i];
				if(debuff && !bList.Contains(i))
                {
					player.buffImmune[i] = true;
                }
            }
			player.allDamage += 0.15f;
		}
	}
}