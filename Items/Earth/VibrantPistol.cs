using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Earth;

namespace SOTS.Items.Earth
{
	public class VibrantPistol : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Pistol");
			Tooltip.SetDefault("Fires almost as fast as you can pull the trigger");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 15;
            item.ranged = true;
            item.width = 30;
            item.height = 22;
            item.useTime = 5; 
            item.useAnimation = 5;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.noMelee = true;
			item.knockBack = 2f;  
            item.value = Item.sellPrice(0, 0, 80, 0);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item91;
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<VibrantBolt>(); 
            item.shootSpeed = 24f;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Player player = Main.player[Main.myPlayer];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (modPlayer.VibrantArmor)
			{
				Texture2D texture = mod.GetTexture("Items/Earth/VibrantRifle");
				Main.spriteBatch.Draw(texture, position - new Vector2((texture.Width - item.width)/ 2 - 3.5f, 0), null, drawColor, 0f, origin, scale * 0.85f, SpriteEffects.None, 0f); //I had to position and draw this by testing values manually ughh
				return false;
			}
			return true;
		}
		public void triggerItemUpdates(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (modPlayer.VibrantArmor)
			{
				item.autoReuse = true;
				item.noUseGraphic = true;
			}
			else
			{
				item.autoReuse = false;
				item.noUseGraphic = false;
			}
		}
		public override void GetWeaponKnockback(Player player, ref float knockback)
		{
			triggerItemUpdates(player);
			base.GetWeaponKnockback(player, ref knockback);
		}
		public override bool BeforeUseItem(Player player)
		{
			triggerItemUpdates(player);
			return base.BeforeUseItem(player);
		}
		public override int GetVoid(Player player)
		{
			return 1;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			triggerItemUpdates(player);
			if (modPlayer.VibrantArmor)
			{
				float mult = 1.33f;
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(4.5f));
				Projectile.NewProjectile(position, Vector2.Zero, ModContent.ProjectileType<VibrantRifle>(), 0, 0, player.whoAmI, perturbedSpeed.ToRotation() - new Vector2(speedX, speedY).ToRotation());
				speedX = perturbedSpeed.X * mult;
				speedY = perturbedSpeed.Y * mult;
				//Main.PlaySound(SoundID.Item11, position);
			}
			return true; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantBar>(), 4);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}
