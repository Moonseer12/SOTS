using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.ChestItems;
using SOTS.Items.Fragments;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chaos
{
	public class RoseBow : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rose Bow");
			Tooltip.SetDefault("Transforms arrows into spears of light that can travel through walls\nWhen briefly charged, arrows explode for 600% damage\nWhen fully charged, arrows bloom for 1500% damage\nCharging will also conjure thorns, which are launched for 2x100% damage each\nOnly consumes void when charged");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 92;
			item.ranged = true;
			item.width = 44;
			item.height = 92;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 6f;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.UseSound = null;
			item.autoReuse = false;
			item.channel = true;
			item.shoot = ModContent.ProjectileType<Projectiles.Chaos.RoseBow>();
			item.shootSpeed = 12f;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.useAmmo = AmmoID.Arrow;
		}
        public override bool BeforeDrainMana(Player player)
        {
            return false;
        }
        public override int GetVoid(Player player)
        {
            return 12;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, 0, 0, ModContent.ProjectileType<Projectiles.Chaos.RoseBow>(), damage, knockBack, player.whoAmI, (int)(item.useTime / (SOTSPlayer.ModPlayer(player).attackSpeedMod + VoidPlayer.ModPlayer(player).voidSpeed - 1f)), type);
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TerminalCluster>(), 1);
			recipe.AddIngredient(ModContent.ItemType<PrecariousCluster>(), 1);
			recipe.AddIngredient(ModContent.ItemType<GlazeBow>(), 1);
			recipe.AddIngredient(ModContent.ItemType<SpectreSpiritStorm>(), 1);
			recipe.AddIngredient(ModContent.ItemType<StarshotCrossbow>(), 1);
			recipe.AddIngredient(ItemID.ChlorophyteShotbow, 1);
			recipe.AddIngredient(ModContent.ItemType<PhaseBar>(), 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}