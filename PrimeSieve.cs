#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	#region Header
	/// <summary>
	/// Provides an enumerable interface to PARI's forprime functionality, allowing the use of
	/// <c>foreach</c> loops.
	/// </summary>
	/// <remarks>
	/// This class is efficient in computation and does not clutter GP's stack, but is limited to primes not exceeding
	/// <c>uint.MaxValue</c>.  For larger primes, use <c>BigPrimeSieve</c> instead.
	/// </remarks>
	#endregion
	public partial class PrimeSieve: IEnumerable<uint>
	{
		public readonly uint Start;
		public readonly uint End;
		
		public static uint MaxPrime
		{
			get { return maxprime(); }
		}
		
		public static uint NextPrime(uint num)
		{
			uint result = TryNextPrime(num);
			
			if (result == 0)
				throw new OverflowException();
			
			return result;
		}
		
		public static uint PrecedingPrime(uint num)
		{
			if (num <= 1)
				throw new ArgumentOutOfRangeException("num", num, "num cannot be less than 2");
			
			return TryPrecedingPrime(num);
		}
		
		#region IEnumerable implementation
		public IEnumerator<uint> GetEnumerator()
		{
			return new Enumerator(Start, End);
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		
		#region External PARI functions
		[DllImport(GP.DllName, EntryPoint = "initprimetable")]
		public static extern void Initialize(uint max = 0);
		
		[DllImport(GP.DllName, EntryPoint = "uisprime")]
		public static extern bool IsPrime(uint num);
		
		[DllImport(GP.DllName)]
		private static extern uint maxprime();
		
		[DllImport(GP.DllName, EntryPoint = "unextprime")]
		public static extern uint TryNextPrime(uint num);
		
		[DllImport(GP.DllName, EntryPoint = "uprecprime")]
		public static extern uint TryPrecedingPrime(uint num);
		#endregion
		
		public PrimeSieve(uint start = 2, uint end = uint.MaxValue)
		{
			if (end < start)
				throw new ArgumentException("end cannot be less than start", "end");
			
			Start = start;
			End = end;
		}
	}
}
