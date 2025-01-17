﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using System;

namespace SOTS.Prim.Trails
{
	class WaterTrail : PrimTrail
	{
		public WaterTrail(Projectile projectile, int width = 12)
		{
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;
			Color = new Color(46, 104, 234);
			Width = width;
			Cap = 20;
			Pixellated = false;
		}
		public override void SetDefaults() => AlphaValue = 0.6f;

		public override void PrimStructure(SpriteBatch spriteBatch)
		{
			if (PointCount <= 6) return;
			float widthVar;
			for (int i = 0; i < Points.Count; i++)
			{
				if (i != Points.Count - 1)
				{
					widthVar = MathHelper.Lerp(0, Width, i / (float)Points.Count);
					Color colorvar = Color.Lerp(Color, new Color(140, 193, 252), ((float)i / (float)Points.Count));
					Vector2 normal = CurveNormal(Points, i);
					Vector2 normalAhead = CurveNormal(Points, i + 1);
					Vector2 firstUp = Points[i] - normal * widthVar;
					Vector2 firstDown = Points[i] + normal * widthVar;
					Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

					AddVertex(firstDown, colorvar * AlphaValue, new Vector2((i / ((float)Points.Count)), 1));
					AddVertex(firstUp, colorvar * AlphaValue, new Vector2((i / ((float)Points.Count)), 0));
					AddVertex(secondDown, colorvar * AlphaValue, new Vector2((i + 1) / ((float)Points.Count), 1));

					AddVertex(secondUp, colorvar * AlphaValue, new Vector2((i + 1) / ((float)Points.Count), 0));
					AddVertex(secondDown, colorvar * AlphaValue, new Vector2((i + 1) / ((float)Points.Count), 1));
					AddVertex(firstUp, colorvar * AlphaValue, new Vector2((i / ((float)Points.Count)), 0));
				}
			}
		}

		public override void SetShaders()
		{
			Effect effect = SOTS.WaterTrail;
			effect.Parameters["TrailTexture"].SetValue(ModContent.GetInstance<SOTS>().GetTexture("TrailTextures/Trail_3"));
			effect.Parameters["ColorOne"].SetValue(Color.ToVector4());
			PrepareShader(effect, "MainPS", Counter / 24f);
		}

		public override void OnUpdate()
		{
			if (!(Entity is Projectile proj))
				return;

			Counter++;
			PointCount = Points.Count() * 6;

			if (Cap < PointCount / 6)
				Points.RemoveAt(0);

			if ((!Entity.active && Entity != null) || Destroyed)
				OnDestroy();

			else
				Points.Add(Entity.Center); // - new Vector2(Width / 2, Width / 2));
		}
		public override void OnDestroy()
		{
			Destroyed = true;
			Width *= 0.8f;
			Width += ((float)Math.Sin(Counter * 2) * 0.3f);
			if (Width < 0.05f)
				Dispose();
		}
	}
}