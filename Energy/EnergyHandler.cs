using BaseLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.ModLoader.IO;

namespace EnergyLibrary
{
	public class EnergyHandler
	{
		// note: these can be ulongs, problem is delta can be both negative and positive
		public long Energy { get; private set; }
		public long Capacity { get; private set; }
		public long MaxExtract { get; private set; }
		public long MaxReceive { get; private set; }

		public long CurrentDelta { get; private set; }
		public long AverageDelta { get; private set; }

		private Queue<long> DeltaBuffer = new Queue<long>();

		public Action OnChanged = () => { };

		public EnergyHandler()
		{
		}

		public EnergyHandler(long capacity)
		{
			Capacity = capacity;
			MaxReceive = capacity;
			MaxExtract = capacity;
		}

		public EnergyHandler(long capacity, long maxTransfer)
		{
			Capacity = capacity;
			MaxReceive = maxTransfer;
			MaxExtract = maxTransfer;
		}

		public EnergyHandler(long capacity, long maxReceive, long maxExtract)
		{
			Capacity = capacity;
			MaxReceive = maxReceive;
			MaxExtract = maxExtract;
		}

		public EnergyHandler Clone() => new EnergyHandler
		{
			Energy = Energy,
			Capacity = Capacity,
			MaxExtract = MaxExtract,
			MaxReceive = MaxReceive,
			OnChanged = (Action)OnChanged.Clone()
		};

		public void SetCapacity(long capacity)
		{
			Capacity = capacity;
			if (Energy > capacity) Energy = Capacity;

			OnChanged?.Invoke();
		}

		public void AddCapacity(long capacity)
		{
			Capacity += capacity;
			if (Energy > Capacity) Energy = Capacity;

			OnChanged?.Invoke();
		}

		public void SetMaxTransfer(long maxTransfer)
		{
			SetMaxReceive(maxTransfer);
			SetMaxExtract(maxTransfer);

			OnChanged?.Invoke();
		}

		public void SetMaxReceive(long maxReceive)
		{
			MaxReceive = maxReceive;

			OnChanged?.Invoke();
		}

		public void SetMaxExtract(long maxExtract)
		{
			MaxExtract = maxExtract;

			OnChanged?.Invoke();
		}

		public long InsertEnergy(long amount)
		{
			CurrentDelta = Utility.Min(Capacity - Energy, MaxReceive, amount);
			Energy += CurrentDelta;

			DeltaBuffer.Enqueue(CurrentDelta);

			if (DeltaBuffer.Count > EnergyLibrary.Instance.GetConfig<EnergyLibraryConfig>().DeltaCacheSize)
			{
				DeltaBuffer.Dequeue();
				AverageDelta = (long)DeltaBuffer.Average(i => i);
			}
			else AverageDelta = CurrentDelta;

			OnChanged?.Invoke();

			return CurrentDelta;
		}

		public long ExtractEnergy(long amount)
		{
			CurrentDelta = -Utility.Min(Energy, MaxExtract, amount);
			Energy += CurrentDelta;

			DeltaBuffer.Enqueue(CurrentDelta);

			if (DeltaBuffer.Count > EnergyLibrary.Instance.GetConfig<EnergyLibraryConfig>().DeltaCacheSize)
			{
				DeltaBuffer.Dequeue();
				AverageDelta = (long)DeltaBuffer.Average(i => i);
			}
			else AverageDelta = CurrentDelta;

			OnChanged?.Invoke();

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
			Energy = tag.GetLong("Energy");
			Capacity = tag.GetLong("Capacity");
			MaxExtract = tag.GetLong("MaxExtract");
			MaxReceive = tag.GetLong("MaxReceive");
		}

		// bug: delta doesn't get sent over (but it also is a lot of data so I might just need to send extract/insert events and have it calculated on client/server)
		public void Write(BinaryWriter writer)
		{
			writer.Write(Energy);
			writer.Write(Capacity);
			writer.Write(MaxExtract);
			writer.Write(MaxReceive);
		}

		public void Read(BinaryReader reader)
		{
			Energy = reader.ReadInt64();
			Capacity = reader.ReadInt64();
			MaxExtract = reader.ReadInt64();
			MaxReceive = reader.ReadInt64();
		}

		public override string ToString() => $"Energy: {Energy.ToSI("N1")}/{Capacity.ToSI("N1")}J MaxExtract: {MaxExtract} MaxReceive: {MaxReceive}";
	}
}