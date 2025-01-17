using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid
{
	public class ShiftingSands : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shifting Sands");
			Tooltip.SetDefault("Pushes back nearby enemies with a wave of sand");
		}
		public override void SetDefaults()
		{
            item.damage = 16;
            item.magic = true; 
            item.width = 34;    
            item.height = 34; 
            item.useTime = 23; 
            item.useAnimation = 23;
            item.useStyle = 5;    
            item.knockBack = 6.5f;
            item.value = Item.sellPrice(0, 1, 20, 0);
            item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item34;
            item.noMelee = true; 
            item.autoReuse = true;
            item.shootSpeed = 2f; 
			item.shoot = mod.ProjectileType("SandPuff");
			item.mana = 16;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float numberProjectiles = 12; 
            float rotation = MathHelper.ToRadians(30);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(rotation * i);
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
		}
	}
}
