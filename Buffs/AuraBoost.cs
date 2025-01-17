using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Buffs
{
    public class AuraBoost : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Aura Boost");
			Description.SetDefault("Void regeneration speed increased by 4%, life regen by 4, defense by 4, and reduces damage taken by 4%");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
 
		public override void Update(Player player, ref int buffIndex)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegenSpeed += 0.04f;
			player.lifeRegen += 4;
			player.statDefense += 4;
			player.endurance += 0.04f;
		}
    }
}