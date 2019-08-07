using BaseLibrary;
using Terraria.ModLoader;

namespace EnergyLibrary
{
	public class EnergyLibrary : Mod
	{
		internal static EnergyLibrary Instance;

		public override void Load() => Instance = this;

		public override void Unload() => Utility.UnloadNullableTypes();
	}
}