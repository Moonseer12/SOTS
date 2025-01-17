using System;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Buffs
{
    public class RubyMonolithAttack : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Ruby Monolith Attack");
			Description.SetDefault("Increases void drain by 6\nIncreases void damage by 10%");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
 
		public override void Update(Player player, ref int buffIndex)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.flatVoidRegen -= 6.0f;
            voidPlayer.voidDamage += 0.1f;
		}
    }
}