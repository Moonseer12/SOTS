using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Permafrost
{  
    public class StormSpell : ModItem
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Storm Spell");
			Tooltip.SetDefault("Create an arctic storm targeted on your cursor");
		}
        public override void SetDefaults()
        {
            item.damage = 14;
            item.magic = true;
            item.width = 40;
            item.height = 38;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 1.35f;
			item.shootSpeed = 9;
            item.value = Item.sellPrice(0, 0, 80, 0);
            item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item92;
			item.mana = 15;
			item.crit = 2;
			item.shoot = ModContent.ProjectileType<IceStorm>();
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Diamond, 1);
			recipe.AddIngredient(ModContent.ItemType<FrigidBar>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 toPos = Main.MouseWorld;
			Projectile.NewProjectile(toPos.X, toPos.Y, 0, 0, type, damage, knockBack, player.whoAmI);
			return false;
		}
    }
}