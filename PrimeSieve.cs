#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	#region Header
	/// <summary>
	/// Provides an enumerable interface to PARI's forprime functionality, allowing the use of
	/// <c>foreach</c> loops, as well as wrapping other operations on arbitrarily large primes and psedoprimes.
	/// </summary>
	/// <seealso cref="SmallPrimeSieve"/>
	#endregion
	public sealed partial class PrimeSieve: IEnumerable<PariInteger>
	{
		public readonly PariInteger Start;
		public readonly PariInteger End;
		
		#region IEnumerable implementation
		/// <inheritdoc/>
		public IEnumerator<PariInteger> GetEnumerator()
		{
			return new Enumerator(Start, End);
		}
		
		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		
		public static bool IsPrime(PariInteger num)
		{
			if (object.ReferenceEquals(num, null))
				throw new ArgumentNullException("num");
			
			return isprime(num.Address);
		}
		
		public static bool IsPrime(PariInteger num, PrimalityTest flag)
		{
			if (object.ReferenceEquals(num, null))
				throw new ArgumentNullException("num");
			
			if (!Enum.IsDefined(typeof(PrimalityTest), flag))
				throw new InvalidEnumArgumentException("flag", (int)flag, typeof(PrimalityTest));
			
			//TODO: Adjust return value for the flag == 1 case.
			return gisprime(num.Address, (int)flag) > 0;
		}
		
		public static bool IsPseudoprime(PariInteger num, int mrBases = 0)
		{
			if (object.ReferenceEquals(num, null))
				throw new ArgumentNullException("num");
			
			if (mrBases < 0)
				mrBases = 0;
			
			return gispseudoprime(num.Address, mrBases);
		}
		
		public static bool IsPseudoprimeBPSW(PariInteger num)
		{
			if (object.ReferenceEquals(num, null))
				throw new ArgumentNullException("num");
			
			return BPSW_psp(num.Address);
		}
		
		public static bool IsPseudoprimeMR(PariInteger num, int bases)
		{
			if (object.ReferenceEquals(num, null))
				throw new ArgumentNullException("num");
			
			if (bases <= 0)
				throw new ArgumentOutOfRangeException("bases", bases, "Must use at least one base.");
			
			return millerrabin(num.Address, bases);
		}
		
		#region Header
		/// <summary>
		/// Computes the nth term in the ordered sequence of prime numbers.
		/// </summary>
		/// <param name="n">The one-based index of the prime number to compute.</param>
		/// <returns>The <paramref name="n"/>th prime.</returns>
		/// <remarks>This method uses an inefficient O(n) algorithm.</remarks>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// <paramref name="n"/> is less than 1.
		/// </exception>
		#endregion
		public static PariInteger NthPrime(int n)
		{
			if (n <= 0)
				throw new ArgumentOutOfRangeException("n", n, "n must be natural.");
			
			return new PariInteger(prime(n));
		}
		
		#region Header
		/// <summary>
		/// Finds the first pseudoprime no smaller than a given value.
		/// </summary>
		/// <param name="num">The lower bound on the search for a pseudoprime.</param>
		/// <returns>
		/// The smallest pseudoprime greater than or equal to <paramref name="num"/>.
		/// </returns>
		/// <remarks>
		/// If <paramref name="num"/> itself is pseudoprime, a copy of <paramref name="num"/> is returned.
		/// </remarks>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="num"/> is <c>null</c>.
		/// </exception>
		#endregion
		public static PariInteger NextPseudoprime(PariInteger num)
		{
			if (object.ReferenceEquals(num, null))
				throw new ArgumentNullException("num");
			
			return new PariInteger(nextprime(num.Address));
		}
		
		#region Header
		/// <summary>
		/// Finds the last pseudoprime no greater than a given value.
		/// </summary>
		/// <param name="num">The upper bound on the search for a pseudoprime.</param>
		/// <returns>
		/// The largest pseudoprime less than or equal to <paramref name="num"/>.
		/// </returns>
		/// <remarks>
		/// If <paramref name="num"/> itself is pseudoprime, a copy of <paramref name="num"/> is returned.
		/// </remarks>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="num"/> is <c>null</c>.
		/// </exception>
		#endregion
		public static PariInteger PrecedingPseudoprime(PariInteger num)
		{
			if (object.ReferenceEquals(num, null))
				throw new ArgumentNullException("num");
			
			return new PariInteger(precprime(num.Address));
		}
		
		#region External PARI functions
		[DllImport(GP.DllName)]
		private static extern bool BPSW_isprime(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern bool BPSW_psp(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern uint gisprime(IntPtr x, int flag);
		
		[DllImport(GP.DllName)]
		private static extern bool gispseudoprime(IntPtr x, int flag);
		
		[DllImport(GP.DllName)]
		private static extern bool isprime(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern bool isprimeAPRCL(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern bool millerrabin(IntPtr x, int flag);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr nextprime(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr precprime(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr prime(int n);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr primepi(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr primes0(IntPtr x);
		#endregion
		
		public PrimeSieve(PariInteger end)
		{
			if (object.ReferenceEquals(end, null))
				throw new ArgumentNullException("end");
			
			Start = PariInteger.Two;
			End = end;
		}
		
		public PrimeSieve(PariInteger start, PariInteger end)
		{
			if (object.ReferenceEquals(start, null))
				throw new ArgumentNullException("start");
			
			if (object.ReferenceEquals(end, null))
				throw new ArgumentNullException("end");
			
			Start = start;
			End = end;
		}
	}
}
