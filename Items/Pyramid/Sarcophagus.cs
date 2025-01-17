using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Pyramid
{
	public class Sarcophagus : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Sarcophagus");
			Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 28;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 4;
			item.consumable = true;
			item.createTile = mod.TileType("SarcophagusTile");
		}
	}
}