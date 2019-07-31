using BaseLibrary;
using BaseLibrary.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EnergyLibrary
{
	public class UIEnergyBar : BaseElement
	{
		private IEnergyHandler energyHandler;
		public EnergyHandler Handler => energyHandler.EnergyHandler;

		public UIEnergyBar(IEnergyHandler energyHandler)
		{
			this.energyHandler = energyHandler;
			GetHoverText += () => $"{Handler.Energy.ToSI("N0")}J/{Handler.Capacity.ToSI("N0")}J\n[c/{(Handler.AverageDelta >= 0 ? Color.Lime : Color.Red).ToHex()}:{Handler.AverageDelta.ToSI("N0")}J/s]";
		}

		// todo: actually draw the energy -> needs a shader
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			float progress = Handler.Energy / (float)Handler.Capacity;

			spriteBatch.DrawSlot(Dimensions);
		}
	}
}