using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.Items.Furniture.Earthen
{
	public class EarthenPlatingToilet : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(16, 30);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<EarthenPlatingToiletTile>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ModContent.ItemType<EarthenPlating>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class EarthenPlatingToiletTile : Chair<EarthenPlatingToilet>
	{
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
			SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
		}
	}
}