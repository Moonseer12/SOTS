using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs
{
    public class VanillaNPCShop : GlobalNPC
    {
        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
			if(Main.rand.Next(3) == 0)
			{
				shop[nextSlot] = (mod.ItemType("SupremSticker"));  
				nextSlot++;
			}
		}
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			
			
            switch (type)
            {
                case NPCID.Merchant: 
					if (NPC.downedPlantBoss) 
                    {

					}
                    break;
            }
            switch (type)
            {
                case NPCID.ArmsDealer:
 
                    if (Main.hardMode) 
                    {
						
                    }
                    if (NPC.downedBoss3) 
                    {
						
                    }
                    break;
            }
		}
    }
}
