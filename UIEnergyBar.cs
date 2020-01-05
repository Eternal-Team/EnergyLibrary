using BaseLibrary;
using BaseLibrary.UI.New;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace EnergyLibrary
{
	public class UIEnergyBar : BaseElement
	{
		private IEnergyHandler energyHandler;
		public EnergyHandler Handler => energyHandler.EnergyHandler;

		public UIEnergyBar(IEnergyHandler energyHandler)
		{
			this.energyHandler = energyHandler;
			//GetHoverText += () => $"{Handler.Energy.ToSI("N0")}J/{Handler.Capacity.ToSI("N0")}J\n[c/{(Handler.AverageDelta >= 0 ? Color.Lime : Color.Red).ToHex()}:{Handler.AverageDelta.ToSI("N0")}W]";
		}

		protected override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Main.magicPixel, Dimensions, Utility.ColorPanel);
			spriteBatch.DrawOutline(Dimensions.Position(), Dimensions.Position() + Dimensions.Size(), Utility.ColorOutline);

			float progress = Handler.Energy / (float)Handler.Capacity;
			spriteBatch.Draw(Main.magicPixel, new Rectangle(Dimensions.X + 2, (int)(Dimensions.Y + 2 + (Dimensions.Height - 4) * (1f - progress)), Dimensions.Width - 4, (int)((Dimensions.Height - 4) * progress)), null, Color.LimeGreen);
		}
	}
}