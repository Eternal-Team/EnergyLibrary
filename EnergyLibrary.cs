using BaseLibrary;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace EnergyLibrary
{
	public class EnergyLibrary : Mod
	{
		internal static Effect BarShader;

		public override void Load()
		{
			if (!Main.dedServ) BarShader = GetEffect("Effects/BarShader");
		}

		public override void Unload() => this.UnloadNullableTypes();
	}
}