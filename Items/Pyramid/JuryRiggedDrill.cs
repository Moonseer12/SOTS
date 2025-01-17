using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class JuryRiggedDrill : ModItem
	{
		int counter = 0;
		int index = -1;
		bool inInventory = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jury Rigged Drill");
			Tooltip.SetDefault("Can break the walls of the pyramid\n'Might only withstand a few hits'");
		}
		public override void SetDefaults()
		{
			item.damage = 24;
			item.melee = true;
			item.width = 42;
			item.height = 22;
			item.useTime = 5;
			item.useAnimation = 25;
			item.channel = true;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.pick = 110;
			item.tileBoost++;
			item.useStyle = 5;
			item.knockBack = 0;
			item.value = Item.sellPrice(0, 0, 1, 50);
			item.rare = 5;
			item.UseSound = SoundID.Item23;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("JuryRiggedDrill");
			item.shootSpeed = 20f;
			item.consumable = true;
			item.maxStack = 999;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            index = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			return false;
		}
		public override void UpdateInventory(Player player)
		{
			inInventory = true;
		}
		public override bool CanUseItem(Player player)
		{
			if(inInventory)
			return true;
		
			return false;
		}
		public override bool ConsumeItem(Player player) 
		{
			inInventory = false;
			if(counter > 8 && index != -1 && Main.projectile[index].active && Main.player[Main.projectile[index].owner] == player && Main.projectile[index].type == item.shoot)
			{
				counter = 0;
				Main.projectile[index].Kill();
				Main.PlaySound(SoundID.Item14, (int)(Main.projectile[index].Center.X), (int)(Main.projectile[index].Center.Y));
				for(int i = 0; i < 15; i ++)
				{
					int num1 = Dust.NewDust(new Vector2(Main.projectile[index].position.X, Main.projectile[index].position.Y), 20, 34, 32);

					
					Main.dust[num1].noGravity = false;
					Main.dust[num1].velocity *= 1.5f;
					Main.dust[num1].scale *= 1.3f;
				}
				for(int i = 0; i < 2; i++)
				{
					int goreIndex = Gore.NewGore(new Vector2(Main.projectile[index].position.X, Main.projectile[index].position.Y), default(Vector2), Main.rand.Next(61,64), 1f);	
					Main.gore[goreIndex].scale = 0.65f;
				}
				index = -1;
				return true;
			}
			counter++;
			return false;
		}
	}
}