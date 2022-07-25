using System;
using System.IO;
using Terraria.ModLoader.IO;

namespace EnergyLibrary;

public class EnergyStorage
{
	public ulong Energy { get; private set; }
	public ulong Capacity { get; private set; }
	public ulong MaxExtract { get; private set; }
	public ulong MaxReceive { get; private set; }

	internal EnergyStorage()
	{
	}

	public EnergyStorage(ulong capacity)
	{
		Capacity = capacity;
		MaxReceive = capacity;
		MaxExtract = capacity;
	}

	public EnergyStorage(ulong capacity, ulong maxTransfer)
	{
		Capacity = capacity;
		MaxReceive = maxTransfer;
		MaxExtract = maxTransfer;
	}

	public EnergyStorage(ulong capacity, ulong maxReceive, ulong maxExtract)
	{
		Capacity = capacity;
		MaxReceive = maxReceive;
		MaxExtract = maxExtract;
	}

	public EnergyStorage Clone()
	{
		EnergyStorage storage = (EnergyStorage)MemberwiseClone();
		return storage;
	}

	public void SetCapacity(ulong capacity)
	{
		Capacity = capacity;
		if (Energy > capacity) Energy = Capacity;
	}

	public void SetMaxTransfer(ulong maxTransfer)
	{
		SetMaxReceive(maxTransfer);
		SetMaxExtract(maxTransfer);
	}

	public void SetMaxReceive(ulong maxReceive)
	{
		MaxReceive = maxReceive;
	}

	public void SetMaxExtract(ulong maxExtract)
	{
		MaxExtract = maxExtract;
	}

	public void ModifyCapacity(long capacity)
	{
		if (capacity < 0)
		{
			Capacity -= (ulong)(-capacity);
			Energy = Math.Min(Energy, Capacity);
		}
		else
		{
			Capacity += (ulong)capacity;
		}
	}

	public ulong InsertEnergy(ulong amount)
	{
		ulong CurrentDelta = Utility.Min(Capacity - Energy, MaxReceive, amount);
		Energy += CurrentDelta;

		// DeltaBuffer.Enqueue(CurrentDelta);
		//
		// if (DeltaBuffer.Count > ModContent.GetInstance<EnergyLibraryConfig>().DeltaCacheSize)
		// {
		// 	DeltaBuffer.Dequeue();
		// 	AverageDelta = (long)DeltaBuffer.Average(i => i);
		// }
		// else AverageDelta = CurrentDelta;
		//
		// OnChanged?.Invoke();

		return CurrentDelta;
	}

	public ulong ExtractEnergy(ulong amount)
	{
		ulong CurrentDelta = Utility.Min(Energy, MaxExtract, amount);
		Energy -= CurrentDelta;

		// DeltaBuffer.Enqueue(CurrentDelta);
		//
		// if (DeltaBuffer.Count > ModContent.GetInstance<EnergyLibraryConfig>().DeltaCacheSize)
		// {
		// 	DeltaBuffer.Dequeue();
		// 	AverageDelta = (long)DeltaBuffer.Average(i => i);
		// }
		// else AverageDelta = CurrentDelta;
		//
		// OnChanged?.Invoke();

		return CurrentDelta;
	}

	public TagCompound Save() => new TagCompound
	{
		["Energy"] = Energy,
		["Capacity"] = Capacity,
		["MaxExtract"] = MaxExtract,
		["MaxReceive"] = MaxReceive
	};

	public void Load(TagCompound tag)
	{
		Energy = tag.Get<ulong>("Energy");
		Capacity = tag.Get<ulong>("Capacity");
		MaxExtract = tag.Get<ulong>("MaxExtract");
		MaxReceive = tag.Get<ulong>("MaxReceive");
	}

	public void Write(BinaryWriter writer)
	{
		writer.Write(Energy);
		writer.Write(Capacity);
		writer.Write(MaxExtract);
		writer.Write(MaxReceive);
	}

	public void Read(BinaryReader reader)
	{
		Energy = reader.ReadUInt64();
		Capacity = reader.ReadUInt64();
		MaxExtract = reader.ReadUInt64();
		MaxReceive = reader.ReadUInt64();
	}

	public override string ToString() => $"Energy: {Energy}/{Capacity} MaxExtract: {MaxExtract} MaxReceive: {MaxReceive}";
}