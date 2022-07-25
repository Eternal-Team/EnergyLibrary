using System;

namespace EnergyLibrary;

internal static class Utility
{
	public static ulong Min(ulong a, ulong b, ulong c)
	{
		return Math.Min(a, Math.Min(b, c));
	}
}