using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Items.Tide;
using Terraria.DataStructures;
using SOTS.Projectiles.Lightning;

namespace SOTS.Items.Celestial
{
	public class Apocalypse : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
        public override void SafeSetDefaults()
		{
			Item.damage = 330;
			Item.DamageType = DamageClass.Magic;
			Item.width = 30;
			Item.height = 30;
            Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<GreenLightning>(); 
			Item.shootSpeed = 1;
			Item.knockBack = 5;
			Item.UseSound = SoundID.Item92;
			Item.noUseGraphic = true;
		}
		public override void UpdateInventory(Player player)
		{
			if(Item.favorited)
				Lighting.AddLight(player.Center, 1.15f, 1.3f, 1.15f);
		}
		public override void AddRecipes() => CreateRecipe(1).AddIngredient<SanguiteBar>(15).AddIngredient<GreenJellyfishStaff>(1).AddTile(TileID.MythrilAnvil).Register();
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 2; i++)
			{
				Vector2 speed = velocity.RotatedBy(MathHelper.ToRadians(-1.0f + 2 * i));
				Projectile.NewProjectile(source, position, speed, type, damage, knockback, player.whoAmI, 0, 6f);
			}
			return false;
		}
		public override int GetVoid(Player player)
		{
			return 8;
		}
	}
}