using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Furniture.Earthen
{
    public class EarthenPlatingLamp : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(14, 32);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<EarthenPlatingLampTile>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = new Recipe(mod);
            recipe.AddIngredient(ModContent.ItemType<EarthenPlating>(), 3);
            recipe.AddIngredient(ItemID.Torch, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
    public class EarthenPlatingLampTile : Lamp<EarthenPlatingLamp>
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override Vector3 LightClr => SOTSTile.EarthenPlatingLight * 3f;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int xFrameOffset = Main.tile[i, j].TileFrameX;
            int yFrameOffset = Main.tile[i, j].TileFrameY;
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            Vector2 drawOffset = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawPosition = new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y) + drawOffset;
            Color drawColour = new Color(100, 100, 100, 0);
            for (int k = 0; k < 5; k++)
            {
                spriteBatch.Draw(glowmask, drawPosition + Main.rand.NextVector2Circular(1, 1) * (k * 0.25f), new Rectangle(xFrameOffset, yFrameOffset, 18, 18), drawColour, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
        }
    }
}