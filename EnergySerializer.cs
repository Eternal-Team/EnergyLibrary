using Terraria.ModLoader.IO;
using TheOneLibrary.Energy.Energy;

namespace TheOneLibrary.Serializers
{
	public class EnergySerializer : TagSerializer<EnergyHandler, TagCompound>
	{
		public override TagCompound Serialize(EnergyHandler value)
		{
			TagCompound tag = new TagCompound();
			if (value.energy < 0) value.energy = 0;

			tag.Set("Energy", value.energy);
			tag.Set("Capacity", value.ccapacity);
			tag.Set("MaxIn", value.maxReceive);
			tag.Set("MaxOut", value.maxExtract);
			return tag;
		}

		public override EnergyHandler Deserialize(TagCompound tag)
		{
			EnergyHandler handler = new EnergyHandler(tag.GetLong("Capacity"), tag.GetLong("MaxIn"), tag.GetLong("MaxOut"));
			handler.energy = tag.GetLong("Energy") > handler.ccapacity ? handler.ccapacity : tag.GetLong("Energy");

			return handler;
		}
	}
}