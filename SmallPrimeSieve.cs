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
	/// <c>foreach</c> loops, as well as wrapping other operations on primes.
	/// </summary>
	/// <remarks>
	/// This class is efficient in computation and does not clutter GP's stack, but is limited to primes not exceeding
	/// <c>uint.MaxValue</c>.  For larger primes, use <see cref="PrimeSieve"/> instead.
	/// </remarks>
	/// <seealso cref="PrimeSieve"/>
	#endregion
	public partial class SmallPrimeSieve: IEnumerable<uint>
	{
		#region Header
		/// <summary>
		/// The number of prime numbers that can be represented as a <c>uint</c>.
		/// </summary>
		/// <remarks>
		/// This is equivalent to the return value of <c>PrimePi(uint.MaxValue)</c>.
		/// </remarks>
		#endregion
		public const uint PrimePiUintMax = 203280221;
		
		public readonly uint Start;
		public readonly uint End;
		
		#region Header
		/// <summary>
		/// Gets the largest prime in the table of precomputed primes.
		/// </summary>
		/// <returns>The largest precomputed prime.</returns>
		#endregion
		public static uint MaxPrecomputed
		{
			get { return maxprime(); }
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
		public static uint NthPrime(int n)
		{
			if (n < 1)
				throw new ArgumentOutOfRangeException("n", n, "n must be greater than zero.");
			
			if (n > PrimePiUintMax)
				throw new OverflowException("The nth prime cannot be represented as a uint.");
			
			return uprime(n);
		}
		
		#region Header
		/// <summary>
		/// Finds the first prime no smaller than a given value.
		/// </summary>
		/// <param name="num">The lower bound on the search for a prime.</param>
		/// <returns>
		/// The smallest prime greater than or equal to <paramref name="num"/>.
		/// </returns>
		/// <exception cref="System.OverflowException">
		/// The next prime cannot be represented as a <c>uint</c>.
		/// </exception>
		#endregion
		public static uint NextPrime(uint num)
		{
			uint result = TryNextPrime(num);
			
			if (result == 0)
				throw new OverflowException("The next prime cannot be represented as a uint.");
			
			return result;
		}
		
		#region Header
		/// <summary>
		/// Finds the last prime no greater than a given value.
		/// </summary>
		/// <param name="num">The upper bound on the search for a prime.</param>
		/// <returns>
		/// The largest prime less than or equal to <paramref name="num"/>.
		/// </returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// There is no prime less than or equal to <paramref name="num"/>
		/// (i.e., <paramref name="num"/> is less than <c>2</c>).
		/// </exception>
		#endregion
		public static uint PrecedingPrime(uint num)
		{
			if (num <= 1)
				throw new ArgumentOutOfRangeException("num", num, "num cannot be less than 2");
			
			return TryPrecedingPrime(num);
		}
		
		#region IEnumerable implementation
		/// <inheritdoc/>
		public IEnumerator<uint> GetEnumerator()
		{
			return new Enumerator(Start, End);
		}
		
		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		
		#region External PARI functions
		[DllImport(GP.DllName, EntryPoint = "initprimetable")]
		public static extern void Initialize(uint max = 0);
		
		#region Header
		/// <summary>
		/// Tests a number for primality.
		/// </summary>
		/// <param name="num">The number to test.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="num"/> is prime; <c>false</c> otherwise.
		/// </returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "uisprime")]
		public static extern bool IsPrime(uint num);
		
		[DllImport(GP.DllName)]
		private static extern uint maxprime();
		
		#region Header
		/// <summary>
		/// Finds the first prime no smaller than a given value.
		/// </summary>
		/// <param name="num">The lower bound on the search for a prime.</param>
		/// <returns>
		/// The smallest prime greater than or equal to <paramref name="num"/> -OR-
		/// <c>0</c> in the case of an overflow.
		/// </returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "unextprime")]
		public static extern uint TryNextPrime(uint num);
		
		#region Header
		/// <summary>
		/// Finds the last prime no greater than a given value.
		/// </summary>
		/// <param name="num">The upper bound on the search for a prime.</param>
		/// <returns>
		/// The largest prime less than or equal to <paramref name="num"/> -OR-
		/// <c>0</c> if <paramref name="num"/> is less than <c>2</c>.
		/// </returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "uprecprime")]
		public static extern uint TryPrecedingPrime(uint num);
		
		[DllImport(GP.DllName)]
		private static extern uint uprime(int n);
		
		#region Header
		/// <summary>
		/// Counts the number of prime numbers no greater than a given value.
		/// </summary>
		/// <param name="x">The upper bound on the counting function.</param>
		/// <returns>The number of primes p, p &lt;= <paramref name="x"/>.</returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "uprimepi")]
		public static extern uint PrimePi(uint x);
		#endregion
		
		public SmallPrimeSieve(uint start = 2, uint end = uint.MaxValue)
		{
			if (end < start)
				throw new ArgumentException("end cannot be less than start", "end");
			
			Start = start;
			End = end;
		}
	}
}
