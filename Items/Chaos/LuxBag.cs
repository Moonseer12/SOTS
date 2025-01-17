using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Crushers;
using SOTS.NPCs.Boss;
using SOTS.NPCs.Boss.Lux;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chaos
{
	public class LuxBag : ModItem
	{
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Chaos/LuxBag");
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Color color;
			for (int k = 0; k < 12; k++)
			{
				Vector2 circular = new Vector2(3 * scale, 0).RotatedBy(MathHelper.ToRadians(k * 30 + Main.GameUpdateCount * 1.2f));
				color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 30));
				color.A = 0;
				Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2) + circular, null, color * 0.4f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, Color.White * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Color color;
			for (int k = 0; k < 12; k++)
			{
				Vector2 circular = new Vector2(2 * scale, 0).RotatedBy(MathHelper.ToRadians(k * 30 + Main.GameUpdateCount * 1.2f));
				color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 30));
				color.A = 0;
				Main.spriteBatch.Draw(texture, position + circular, frame, color * 0.4f, 0f, origin, scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, position, frame, Color.White * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 34;
			item.value = 0;
			item.rare = ItemRarityID.LightPurple;
			item.maxStack = 999;
			item.consumable = true;
			item.expert = true;
		}
		public override int BossBagNPC => ModContent.NPCType<Lux>();
		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.TryGettingDevArmor();
			player.QuickSpawnItem(ModContent.ItemType<VoidAnomaly>());
			player.QuickSpawnItem(ModContent.ItemType<PhaseOre>(), Main.rand.Next(120, 181)); //12 to 18 bars
			player.QuickSpawnItem(ItemID.SoulofLight, Main.rand.Next(10, 20));
		}
	}
}