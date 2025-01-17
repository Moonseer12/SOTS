using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using System.Collections.Generic;
using System;

namespace SOTS.Void
{
    internal enum VoidBarMode
    {
        voidAmount
    }
    class VoidBar : UIElement
    {
        private VoidBarMode stat;
        private float width;
        private float height;
        public VoidBar(VoidBarMode stat, int height, int width)
        {
            this.stat = stat;
            this.width = 200f;
            this.height = 30f;
        }
		private VoidBarSprite barAmount;
		private SoulBar barAmount3;
		private BarDivider barDivider;
		private VoidBarBorder2 barBackground;
		private UIText text;
        public override void OnInitialize()
		{
			Height.Set(height, 0f); //Set Height of element
			Width.Set(width, 0f);   //Set Width of element

			barBackground = new VoidBarBorder2(); 
			barBackground.Left.Set(0f, 0f);
			barBackground.Top.Set(0f, 0f);
			barBackground.Width.Set(width, 0f);
			barBackground.Height.Set(height, 0f);

			barDivider = new BarDivider();
			barDivider.Left.Set(2f, 0f);
			barDivider.Top.Set(4f, 0f);
			barDivider.Width.Set(2, 0f);
			barDivider.Height.Set(20, 0f);

			barAmount = new VoidBarSprite(); 
			barAmount.SetPadding(0);
			barAmount.Left.Set(5.01f, 0f);
			barAmount.Top.Set(6f, 0f);
			barAmount.Width.Set(188, 0f);
			barAmount.Height.Set(18, 0f);

			barAmount3 = new SoulBar();
			barAmount3.SetPadding(0);
			barAmount3.Left.Set(6f, 0f);
			barAmount3.Top.Set(6f, 0f);
			barAmount3.Width.Set(188, 0f);
			barAmount3.Height.Set(18, 0f);

			text = new UIText("0|0"); 
			text.Width.Set(width, 0f);
			text.Height.Set(height, 0f);
			text.Top.Set(height - 42 - text.MinHeight.Pixels / 2, 0f); 

			barBackground.Append(barAmount);
			barBackground.Append(barAmount3);
			barBackground.Append(barDivider);
			barBackground.Append(text);
			base.Append(barBackground);
		}
        public override void Draw(SpriteBatch spriteBatch)
		{
			Player player = Main.player[Main.myPlayer];
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			DrawIce(spriteBatch, true);
			int voidmeter = (int)voidPlayer.voidMeter;
			if(voidmeter < 0)
			{
				voidmeter = 0;
			}
			if (voidPlayer.frozenVoid)
			{
				voidmeter = (int)voidPlayer.frozenVoidCount;
			}
			string voidManaText = voidmeter.ToString();
			int voidMax = voidPlayer.voidMeterMax2 - voidPlayer.VoidMinionConsumption;
			string voidManaMaxText = voidMax.ToString();
			string voidSoulsText = voidPlayer.lootingSouls.ToString();
			barAmount3.backgroundColor = VoidPlayer.soulLootingColor;
			if (text != null)
			{
				if(SOTS.Config.voidBarTextOn)
				{
					if (player.dead)
						voidManaText = "0 ";
					string textThing = "Void: ";
					if (voidMax - voidPlayer.lootingSouls <= 0)
					{
						textThing += "0 ";
					}
					else
					{
						voidManaMaxText = (voidMax - voidPlayer.lootingSouls).ToString();
						textThing += voidManaText + " / " + voidManaMaxText;
					}
					if (!player.dead && (voidPlayer.lootingSouls > 0 || (voidPlayer.VoidMinionConsumption > 0 && !SOTS.Config.simpleVoidText)))
					{
						textThing += " ("; 
						if (voidPlayer.VoidMinionConsumption > 0 && !SOTS.Config.simpleVoidText)
							textThing += voidPlayer.VoidMinionConsumption.ToString();
						if (voidPlayer.lootingSouls > 0 && voidPlayer.VoidMinionConsumption > 0 && !SOTS.Config.simpleVoidText)
							textThing += " + ";
						if (voidPlayer.lootingSouls > 0)
							textThing += voidSoulsText;
						textThing += ")";
					}
					text.SetText(textThing);
				}
				else
				{
					text.SetText("");
				}
			}
			//Calculate quotient
			float quotient = voidPlayer.voidMeter / voidPlayer.voidMeterMax2;
			float quotient2 = (float)voidPlayer.lootingSouls / voidPlayer.voidMeterMax2;
			Texture2D fill = ModContent.GetTexture("SOTS/Void/SoulBar");
			Texture2D divider = ModContent.GetTexture("SOTS/Void/VoidBarDivider");
			if (quotient > 1)
			{
				quotient = 1;
			}
			if (quotient < 0)
				quotient = 0;
			Vector2 padding = new Vector2(6, 6);
			float prevRight = 0;
			float length = 40;
			int height = 18;
			Texture2D fill2 = ModContent.GetTexture("SOTS/Void/VoidBarBorder");
			if(player.dead)
			{
				Recalculate();
				base.Draw(spriteBatch);
				fill2 = ModContent.GetTexture("SOTS/Void/VoidDead");
			}
			spriteBatch.Draw(fill2, new Rectangle((int)VoidPlayer.voidBarOffset.X, (int)VoidPlayer.voidBarOffset.Y + 2, 200, 30), Color.White);
			if (player.dead)
            {
				return;
			}
			List<Rectangle> rectangles = new List<Rectangle>();
			//List<Color> colors = new List<Color>();
			int minionCount = voidPlayer.VoidMinions.Count;
			for (int i = 0; i < minionCount; i++)
            {
				int type = voidPlayer.VoidMinions[i];
				int nextType = -1;
				if (i < voidPlayer.VoidMinions.Count - 1)
					nextType = voidPlayer.VoidMinions[i + 1];
				int cost = VoidPlayer.minionVoidCost(type);
				length = cost / (float)voidPlayer.voidMeterMax2 * 188f;
				Color color = Color.White;
				color = VoidPlayer.minionVoidColor(type);
				if (length + prevRight >= 188)
					length = 188 - prevRight;
				spriteBatch.Draw(fill, new Rectangle((int)(VoidPlayer.voidBarOffset.X + padding.X + (int)prevRight), (int)(VoidPlayer.voidBarOffset.Y + padding.Y), (int)(length + 1), height), color);
				if (SOTS.Config.voidBarBlur)
				{
					color *= 0.15f;
					color.A = 0;
					for (int l = 0; l < 8; l++)
					{
						Vector2 circular = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(45 * l));
						spriteBatch.Draw(fill, new Rectangle((int)(VoidPlayer.voidBarOffset.X + padding.X + (int)prevRight) + (int)circular.X, (int)(VoidPlayer.voidBarOffset.Y + padding.Y) + (int)circular.Y, (int)(length + 1), height), color);
					}
				}
				else
				{
					color.A = 0;
					spriteBatch.Draw(fill, new Rectangle((int)(VoidPlayer.voidBarOffset.X + padding.X + (int)prevRight), (int)(VoidPlayer.voidBarOffset.Y + padding.Y), (int)(length + 1), height), color);
				}
				/*if(i == 0)
				{
					rectangles.Add(new Rectangle((int)(VoidPlayer.voidBarOffset.X + padding.X + (int)prevRight), (int)(VoidPlayer.voidBarOffset.Y + padding.Y - 2), 6, 22));
					colors.Add(color);
				}*/
				prevRight += length;
				if (!SOTS.Config.simpleVoidFill)
					if (nextType == -1 || nextType != type)
					{
						rectangles.Add(new Rectangle((int)(VoidPlayer.voidBarOffset.X + padding.X + (int)prevRight - 1), (int)(VoidPlayer.voidBarOffset.Y + padding.Y - 2), 2, 20));
						//colors.Add(color);
					}
			}
			barAmount.Left.Set(6f + prevRight, 0f);
			length = (int)(quotient * 188);
			if (length + prevRight > 188)
				length = 188 - prevRight;
			barAmount.Width.Set(length, 0f);
			if(voidPlayer.lootingSouls > 0)
			{
				barAmount3.Width.Set(quotient2 * 188, 0f);
				barAmount3.Left.Set(200f - quotient2 * 188 - 6, 0f);
				barDivider.Width.Set(2, 0);
				barDivider.Left.Set(200f - quotient2 * 188 - 8, 0f);
			}
			else
            {
				barAmount3.Width.Set(0, 0);
				barDivider.Width.Set(0, 0);
			}
			Recalculate();
			if (voidPlayer.lerpingVoidMeter > voidPlayer.voidMeter)
			{
				float quotientLerp = voidPlayer.lerpingVoidMeter / voidPlayer.voidMeterMax2;
				fill = ModContent.GetTexture("SOTS/Void/VoidBarSprite");
				float lerpLength = (int)(quotientLerp * 188);
				if (SOTS.Config.voidBarBlur)
				{
					for (int l = 0; l < 8; l++)
					{
						Vector2 circular = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(45 * l));
						spriteBatch.Draw(fill, new Rectangle((int)(VoidPlayer.voidBarOffset.X + padding.X + (int)prevRight + circular.X), (int)(VoidPlayer.voidBarOffset.Y + padding.Y + circular.Y), (int)lerpLength, height), new Color(255, 10, 10, 0) * 0.1f);
					}
				}
				else
					spriteBatch.Draw(fill, new Rectangle((int)(VoidPlayer.voidBarOffset.X + padding.X + (int)prevRight), (int)(VoidPlayer.voidBarOffset.Y + padding.Y), (int)lerpLength, height), new Color(255, 10, 10, 0) * 0.6f);
			}
			base.Draw(spriteBatch);
			Color color2 = new Color(100, 100, 100, 0);
			fill = ModContent.GetTexture("SOTS/Void/VoidBarSprite");
			if(SOTS.Config.voidBarBlur)
			{
				color2 = new Color(15, 15, 15, 0);
				for (int l = 0; l < 8; l++)
				{
					Vector2 circular = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(45 * l));
					spriteBatch.Draw(fill, new Rectangle((int)(VoidPlayer.voidBarOffset.X + padding.X + (int)prevRight) + (int)circular.X, (int)(VoidPlayer.voidBarOffset.Y + padding.Y) + (int)circular.Y, (int)length, height), color2);
				}
			}
			else
			{
				spriteBatch.Draw(fill, new Rectangle((int)(VoidPlayer.voidBarOffset.X + padding.X + (int)prevRight), (int)(VoidPlayer.voidBarOffset.Y + padding.Y), (int)length, height), color2);
			}
			if(!SOTS.Config.simpleVoidFill)
			{
				for (int i = 0; i < rectangles.Count; i++)
				{
					spriteBatch.Draw(divider, rectangles[i], Color.White);
				}
			}
			fill2 = ModContent.GetTexture("SOTS/Void/VoidBarBorder2");
			spriteBatch.Draw(fill2, new Rectangle((int)VoidPlayer.voidBarOffset.X, (int)VoidPlayer.voidBarOffset.Y, 200, 30), Color.White);
			DrawGreenBar(spriteBatch);
			if (voidPlayer.resolveVoidCounter > 0) //resolve visuals
			{
				float resolveAmount = 188f * voidPlayer.resolveVoidAmount / voidPlayer.voidMeterMax2;
				if (voidPlayer.voidRecovery)
					resolveAmount = length;
				else
				{
					if (resolveAmount > length)
					{
						resolveAmount = length;
					}
				}
				prevRight += length - resolveAmount;
				float mult = voidPlayer.resolveVoidCounter / 15f;
				for (int i = -1; i <= 1; i += 2)
				{
					float sinY = i * 4 * (0.5f * (float)Math.Sin(mult * MathHelper.Pi) + mult * 0.5f);
					spriteBatch.Draw(fill, new Rectangle((int)(VoidPlayer.voidBarOffset.X + padding.X + (int)prevRight), (int)(VoidPlayer.voidBarOffset.Y + padding.Y + sinY), (int)(resolveAmount + 1), height), new Color(110, 80, 110, 0) * mult);
				}
				voidPlayer.resolveVoidCounter--;
				if (voidPlayer.resolveVoidCounter <= 0)
					voidPlayer.resolveVoidAmount = 0;
			}
			if (voidPlayer.safetySwitchVisual)
				DrawLock(spriteBatch);
			DrawIce(spriteBatch, false);
			DrawShock(spriteBatch);
		}
		public void DrawGreenBar(SpriteBatch spriteBatch)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(Main.LocalPlayer);
			Texture2D texture = ModContent.GetTexture("SOTS/Void/VoidBarGreen");
			int timerDelay = 90;
			float regenBarPercent = (voidPlayer.voidRegenTimer - timerDelay) / (VoidPlayer.voidRegenTimerMax - timerDelay);
			if(regenBarPercent > 0)
			{
				Rectangle frame1 = new Rectangle((int)(VoidPlayer.voidBarOffset.X + 16), (int)(VoidPlayer.voidBarOffset.Y + 2), (int)(168 * regenBarPercent), 30);
				Rectangle frame = new Rectangle(0, 0, (int)(168 * regenBarPercent), 30);
				spriteBatch.Draw(texture, frame1, frame, Color.White);
			}
			if (voidPlayer.GreenBarCounter > 12 || voidPlayer.voidRecovery)
			{
				float mult = (voidPlayer.GreenBarCounter - 12) / 8f;
				if (voidPlayer.voidRecovery)
					mult = 1f;
				Rectangle frame1 = new Rectangle((int)(VoidPlayer.voidBarOffset.X + 16), (int)(VoidPlayer.voidBarOffset.Y + 2), 168, 30);
				spriteBatch.Draw(texture, frame1, Color.White * mult);
			}
			if (voidPlayer.voidRecovery && voidPlayer.GreenBarCounter < 20 && voidPlayer.GreenBarCounter != 0)
			{
				float mult = voidPlayer.GreenBarCounter / 20f;
				float reverseMult = 1 - mult;
				float sinusoid = (float)Math.Sin(Math.PI * reverseMult);
				Color glow = new Color(60, 60, 60, 0);
				for (int i = 0; i < 6; i++)
				{
					Vector2 circular = new Vector2(sinusoid * 1.5f, 0).RotatedBy(MathHelper.ToRadians(i * 60));
					Rectangle frame1 = new Rectangle(0, 0, 168, 30);
					spriteBatch.Draw(texture, new Vector2((int)(VoidPlayer.voidBarOffset.X + 16) + circular.X, (int)(VoidPlayer.voidBarOffset.Y + 2) + circular.Y), frame1, glow * (0.5f * sinusoid + 0.5f * mult));
				}
			}
			else if (voidPlayer.GreenBarCounter < 20 && voidPlayer.GreenBarCounter != 0)
			{
				float mult = voidPlayer.GreenBarCounter / 20f;
				float reverseMult = 1 - mult;
				float sinusoid = (float)Math.Sin(Math.PI * reverseMult);
				Color glow = new Color(120, 120, 120, 0);
				for (int i = 0; i < 6; i++)
				{
					Vector2 circular = new Vector2(sinusoid * 3, 0).RotatedBy(MathHelper.ToRadians(i * 60));
					Rectangle frame1 = new Rectangle(0, 0, 168, 30);
					spriteBatch.Draw(texture, new Vector2((int)(VoidPlayer.voidBarOffset.X + 16) + circular.X, (int)(VoidPlayer.voidBarOffset.Y + 2) + circular.Y), frame1, glow * (0.5f * sinusoid + 0.5f * mult));
				}
			}
			if (voidPlayer.GreenBarCounter > 0)
				voidPlayer.GreenBarCounter--;
		}
		public void DrawShock(SpriteBatch spriteBatch)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(Main.LocalPlayer);
			if (voidPlayer.voidShockAnimationCounter > 0)
			{
				Texture2D texture = ModContent.GetTexture("SOTS/Void/VoidShockEffect");
				int frameNumber = (int)(voidPlayer.voidShockAnimationCounter / 3.6f);
				if (frameNumber >= 11)
				{
					voidPlayer.voidShockAnimationCounter = 0;
					return;
				}
				Rectangle frame1 = new Rectangle((int)(VoidPlayer.voidBarOffset.X - 10), (int)(VoidPlayer.voidBarOffset.Y - 4), 216, 40);
				Rectangle frame = new Rectangle(0, 40 * frameNumber, 216, 40);
				spriteBatch.Draw(texture, frame1, frame, Color.White);
				voidPlayer.voidShockAnimationCounter++;
			}
		}
		public void DrawLock(SpriteBatch spriteBatch)
		{
			Player player = Main.player[Main.myPlayer];
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			Texture2D Lock = ModContent.GetTexture("SOTS/Void/VoidBarLock");
			spriteBatch.Draw(Lock, new Vector2(VoidPlayer.voidBarOffset.X, VoidPlayer.voidBarOffset.Y), new Rectangle(0, 0, Lock.Width, Lock.Height), new Color(255, 255, 255));
		}
		public void DrawIce(SpriteBatch spriteBatch, bool shadow = false)
		{
			Player player = Main.player[Main.myPlayer];
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			Texture2D frozenBar = ModContent.GetTexture("SOTS/Void/FrozenVoidBar");
			Texture2D fill = ModContent.GetTexture("SOTS/Void/FrozenVoidBarFill");
			Texture2D frozenBarBorder = ModContent.GetTexture("SOTS/Void/FrozenVoidBarBorder");
			Texture2D LockFrozen = ModContent.GetTexture("SOTS/Void/FrozenVoidBarLock");
			Vector2 padding = new Vector2(-2, 0);
			Rectangle frame = new Rectangle((int)(VoidPlayer.voidBarOffset.X + padding.X), (int)(VoidPlayer.voidBarOffset.Y + padding.Y), frozenBar.Width, frozenBar.Height);
			if (voidPlayer.frozenVoid)
			{
				if (!shadow && voidPlayer.frozenDuration >= 30)
				{
					float frozenPercent2 = (float)voidPlayer.frozenDuration / (float)voidPlayer.frozenMaxDuration;
					spriteBatch.Draw(fill, frame, new Color(255, 255, 255) * 0.1f);
					spriteBatch.Draw(frozenBarBorder, new Vector2(VoidPlayer.voidBarOffset.X, VoidPlayer.voidBarOffset.Y) + padding, new Rectangle(0, 0, frozenBarBorder.Width, frozenBarBorder.Height), new Color(255, 255, 255));
					spriteBatch.Draw(frozenBar, new Vector2(VoidPlayer.voidBarOffset.X, VoidPlayer.voidBarOffset.Y) + padding, new Rectangle(0, 0, (int)(frozenBar.Width * frozenPercent2), frozenBar.Height), new Color(255, 255, 255));
					if(voidPlayer.safetySwitchVisual)
						spriteBatch.Draw(LockFrozen, new Vector2(VoidPlayer.voidBarOffset.X, VoidPlayer.voidBarOffset.Y) + padding, new Rectangle(0, 0, LockFrozen.Width, LockFrozen.Height), new Color(255, 255, 255));
				}
				else if (shadow && voidPlayer.frozenDuration < 30)
					for (int i = 0; i < 6; i++)
					{
						float distance = 30 - voidPlayer.frozenDuration;
						float alphaMult = (40 - distance) / 40f;
						Vector2 circular = new Vector2(1.0f * distance, 0).RotatedBy(MathHelper.ToRadians(2 * voidPlayer.frozenDuration + 60 * i));
						spriteBatch.Draw(fill, VoidPlayer.voidBarOffset + circular + padding, new Rectangle(0, 0, fill.Width, fill.Height), new Color(100, 100, 100, 0) * 0.05f * alphaMult);
						spriteBatch.Draw(frozenBarBorder, VoidPlayer.voidBarOffset + circular + padding, new Rectangle(0, 0, frozenBarBorder.Width, frozenBarBorder.Height), new Color(100, 100, 100, 0) * 0.5f * alphaMult);
					}
			}
			else
			{
				if (!shadow)
				{
					float frozenPercent = (float)voidPlayer.frozenCounter / (float)voidPlayer.frozenMinTimer;
					frame = new Rectangle(0, 0, (int)(frozenBarBorder.Width * frozenPercent), frozenBarBorder.Height);
					spriteBatch.Draw(frozenBarBorder, new Vector2(VoidPlayer.voidBarOffset.X, VoidPlayer.voidBarOffset.Y) + padding, frame, new Color(255, 255, 255));
					if (voidPlayer.safetySwitchVisual)
						spriteBatch.Draw(LockFrozen, new Vector2(VoidPlayer.voidBarOffset.X, VoidPlayer.voidBarOffset.Y) + padding, frame, new Color(255, 255, 255));
				}
				else if (voidPlayer.frozenCounter > voidPlayer.frozenMinTimer - 30)
					for (int i = 0; i < 6; i++)
					{
						float distance = voidPlayer.frozenMinTimer - voidPlayer.frozenCounter;
						float alphaMult = (40 - distance) / 40f;
						Vector2 circular = new Vector2(1.0f * distance, 0).RotatedBy(MathHelper.ToRadians(2 * voidPlayer.frozenCounter + 60 * i));
						spriteBatch.Draw(fill, VoidPlayer.voidBarOffset + circular + padding, new Rectangle(0, 0, fill.Width, fill.Height), new Color(100, 100, 100, 0) * 0.05f * alphaMult);
						spriteBatch.Draw(frozenBar, VoidPlayer.voidBarOffset + circular + padding, new Rectangle(0, 0, frozenBar.Width, frozenBar.Height), new Color(100, 100, 100, 0) * 0.5f * alphaMult);
					}
			}
		}
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
        }
    }
}