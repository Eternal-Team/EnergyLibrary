using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace EnergyLibrary
{
	public class EnergyLibraryConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[DefaultValue(60), Range(5, 120), Label("$Mods.EnergyLibrary.Config.DeltaCacheSize"), Tooltip("$Mods.EnergyLibrary.Config.DeltaCacheSizeTooltip")]
		public int DeltaCacheSize;
	}
}