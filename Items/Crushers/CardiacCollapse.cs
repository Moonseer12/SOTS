using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Projectiles.Crushers;
using SOTS.Items.Fragments;

namespace SOTS.Items.Crushers
{
	public class CardiacCollapse : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cardiac Collapse");
			Tooltip.SetDefault("Charge to increase damage up to 800%\nTakes 3.5 seconds to reach max charge\nKilled enemies regenerate health");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 23;
            item.melee = true;  
            item.width = 48;
            item.height = 48;  
            item.useTime = 30; 
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.knockBack = 8.5f;
            item.value = Item.sellPrice(0, 0, 33, 0);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<CardiacCollapseCrusher>(); 
            item.shootSpeed = 18f;
			item.channel = true;
            item.noUseGraphic = true; 
            item.noMelee = true;
			item.crit = 6;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.ownedProjectileCounts[type] <= 0;
		}
		public override int GetVoid(Player player)
		{
			return 6;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrimtaneBar, 12);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}