namespace TheOneLibrary.Energy.Energy
{
	public interface IEnergyStorage : IEnergyHandler
	{
		long InsertEnergy(long amount);

		long ExtractEnergy(long amount);
	}
}