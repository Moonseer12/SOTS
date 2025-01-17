﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture
{
    public abstract class Bathtub<TDrop> : FurnTile where TDrop : ModItem
    {
        protected override int ItemType => ModContent.ItemType<TDrop>();
        protected override void SetDefaults(TileObjectData t)
        {
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = true;
            t.CopyFrom(TileObjectData.Style4x2);
            t.Width = 4;
            t.Height = 2;
            t.CoordinateHeights = new int[] { 16, 16 };
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 64, 32, ItemType);
        }
    }
}