using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using TheOneLibrary.Energy.Energy;
using TheOneLibrary.Utils;

namespace TheOneLibrary.UI.Elements
{
	public class UIEnergyBar : BaseElement
	{
		private IEnergyHandler handler;
		public EnergyHandler energy => handler.GetEnergyStorage();

		public long oldEnergy;

		private Color bgColor = Utility.PanelColor;
		private Color barColor = Color.DarkRed;
		public long delta;

		public UIEnergyBar(IEnergyHandler handler)
		{
			this.handler = handler;
			HoverText += () => $"{energy}\n[c/{(delta >= 0 ? Color.Lime : Color.Red).ToHex()}:{(delta >= 0 ? "+" : "-") + delta.AsPower(true)}]";
		}

		protected void DrawBar(SpriteBatch spriteBatch, Color color)
		{
			CalculatedStyle dimensions = GetDimensions();

			spriteBatch.Draw(TheOneLibrary.Textures.corner, dimensions.Position(), color);
			spriteBatch.Draw(TheOneLibrary.Textures.corner, dimensions.Position() + new Vector2(dimensions.Width - 12, 12), null, color, MathHelper.Pi * 0.5f, new Vector2(12, 12), Vector2.One, SpriteEffects.None, 0f);
			spriteBatch.Draw(TheOneLibrary.Textures.corner, dimensions.Position() + new Vector2(12, dimensions.Height - 12), null, color, MathHelper.Pi * 1.5f, new Vector2(12, 12), Vector2.One, SpriteEffects.None, 0f);
			spriteBatch.Draw(TheOneLibrary.Textures.corner, dimensions.Position() + new Vector2(dimensions.Width - 12, dimensions.Height - 12), null, color, MathHelper.Pi * 1f, new Vector2(12, 12), Vector2.One, SpriteEffects.None, 0f);

			spriteBatch.Draw(TheOneLibrary.Textures.side, new Rectangle((int)(dimensions.X + 12), (int)dimensions.Y, (int)(dimensions.Width - 24), 12), color);
			spriteBatch.Draw(TheOneLibrary.Textures.side, new Rectangle((int)dimensions.X, (int)(dimensions.Y + dimensions.Height - 12), (int)(dimensions.Height - 24), 12), null, color, MathHelper.Pi * 1.5f, Vector2.Zero, SpriteEffects.None, 0f);
			spriteBatch.Draw(TheOneLibrary.Textures.side, new Rectangle((int)(dimensions.X + dimensions.Width), (int)(dimensions.Y + 12), (int)(dimensions.Height - 24), 12), null, color, MathHelper.Pi * 0.5f, Vector2.Zero, SpriteEffects.None, 0f);
			spriteBatch.Draw(TheOneLibrary.Textures.side, new Rectangle((int)(dimensions.X + dimensions.Width - 12), (int)(dimensions.Y + dimensions.Height), (int)(dimensions.Width - 24), 12), null, color, MathHelper.Pi, Vector2.Zero, SpriteEffects.None, 0f);

			spriteBatch.Draw(Main.magicPixel, new Rectangle((int)(dimensions.X + 12), (int)(dimensions.Y + 12), (int)(dimensions.Width - 24), (int)(dimensions.Height - 24)), color);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();

			delta = energy.GetEnergy() - oldEnergy;
			oldEnergy = energy.GetEnergy();
			float progress = energy.GetEnergy() / (float)energy.GetCapacity();

			DrawBar(spriteBatch, bgColor);

			spriteBatch.End();

			RasterizerState state = new RasterizerState { ScissorTestEnable = true };

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, state);

			Rectangle prevRect = spriteBatch.GraphicsDevice.ScissorRectangle;
			spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)dimensions.X, (int)(dimensions.Y + dimensions.Height - dimensions.Height * progress), (int)dimensions.Width, (int)(dimensions.Height * progress));

			DrawBar(spriteBatch, barColor);

			spriteBatch.GraphicsDevice.ScissorRectangle = prevRect;
			spriteBatch.End();
			spriteBatch.Begin();
		}
	}
}