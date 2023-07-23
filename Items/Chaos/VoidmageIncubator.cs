using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using SOTS.Void;
using SOTS.Items.Fragments;
using SOTS.Items.Pyramid;
using Terraria.Localization;
using SOTS.Items.Conduit;

namespace SOTS.Items.Chaos
{
	public class VoidmageIncubator : ModItem
	{
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Player player = Main.LocalPlayer;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Chaos/VoidmageIncubatorSheet");
			frame = new Rectangle(0, 48 * GetGem(unique) + 2, 24, 44);
			spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Player player = Main.LocalPlayer;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Chaos/VoidmageIncubatorSheet");
			Rectangle frame = new Rectangle(0, 48 * GetGem(unique) + 2, 24, 44);
			Vector2 origin = Item.Size / 2;
			spriteBatch.Draw(texture, Item.Center - Main.screenPosition, frame, lightColor, rotation, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 24;     
            Item.height = 44;   
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Player player = Main.LocalPlayer;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.Mod == "Terraria" && line.Name == "Tooltip0") //checks the name of the tootip line
				{
					line.Text = GetTooltip(GetGem(unique));
					return;
				}
			}
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			GetBonuses(player, GetGem(unique));
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.bonusVoidGain += 2f;
			modPlayer.VMincubator = true;
			modPlayer.TimeFreezeImmune = true;
		}
		public int GetGem(int unique)
		{
			return unique % 8;
		}
		public void GetBonuses(Player player, int gem)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			switch (gem)
            {
				case 0: //geo, earth
					player.endurance += 0.05f;
					break;
				case 1: //electro, evil
					player.GetDamage(DamageClass.Generic) += 0.08f;
					break;
				case 2: //anemo, otherworld
					player.moveSpeed += 0.1f;
					player.jumpSpeedBoost += 1.0f;
					break;
				case 3: //cyro, permafrost
					modPlayer.CritBonusMultiplier += 0.2f;
					break;
				case 4: //pyro, inferno
					player.GetCritChance(DamageClass.Generic) += 6;
					break;
				case 5: //hydro, tidal
					player.statLifeMax2 += 40;
					break;
				case 6: //dendro, nature
					player.lifeRegen += 2;
					break;
				case 7: //masterless, chaos
					vPlayer.voidMeterMax2 += 40;
					break;
			}
        }
		public string GetTooltip(int gem)
		{
			string text = Language.GetTextValue($"Mods.SOTS.VoidmageIncubatorTextList.{gem}");
			return text;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<VoidAnomaly>(), 1).AddIngredient(ModContent.ItemType<SkipShard>(), 1).AddIngredient(ModContent.ItemType<DissolvingBrilliance>(), 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}

