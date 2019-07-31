namespace TheOneLibrary.Energy.Energy
{
	public interface IEnergyProvider : IEnergyHandler
	{
		long ExtractEnergy(long maxExtract);
	}
}