using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Potions
{
	public class StarlightTonic : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starlight Tonic");
			Tooltip.SetDefault("Receive the following:\nAssassination for 15 minutes\nMagic Power for 13 minutes\nHeartreach for 11 minutes\nMana Regeneration for 9 minutes");
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Potions/StarlightTonicEffect");
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 1; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2(position.X + x, position.Y + y),
				null, color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Potions/StarlightTonicEffect");
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 1; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y + 2),
				null, color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 32;
            item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 3;
			item.maxStack = 30;
            item.UseSound = SoundID.Item3;            
            item.useStyle = 2;        
            item.useTurn = true;
            item.useAnimation = 16;
            item.useTime = 16;
            item.consumable = true;     
			item.buffType = BuffID.VortexDebuff;
            item.buffTime = 60;
		}
		public override bool ConsumeItem(Player player) 
		{
			return true;
		}
		public override bool UseItem(Player player)
		{
			int minute = 3600;
			int buff1 = mod.BuffType("Assassination");
			int buff2 = BuffID.MagicPower;
			int buff3 = BuffID.Heartreach;
			int buff4 = BuffID.ManaRegeneration;

			player.AddBuff(buff1, minute * 15, true);
			player.AddBuff(buff2, minute * 13, true);
			player.AddBuff(buff3, minute * 11, true);
			player.AddBuff(buff4, minute * 9, true);
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DissolvingAether", 1);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}