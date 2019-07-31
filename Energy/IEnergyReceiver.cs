namespace TheOneLibrary.Energy.Energy
{
	public interface IEnergyReceiver : IEnergyHandler
	{
		long ReceiveEnergy(long maxReceive);
	}
}