#region Using directives
using System;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public static class Arithmetic
	{
		public static byte DigitalRoot(uint n)
		{
			while (n > 9)
				n = DigitalSum(n);
			
			return (byte)n;
		}
		
		public static SmallIntVector Divisors(uint n)
		{
			return new SmallIntVector(divisorsu(n));
		}
		
		public static Vector Factor(uint n)
		{
			return new Vector(factoru(n));
		}
		
		public static Vector FactorPow(uint n)
		{
			return new Vector(factoru_pow(n));
		}
		
		public static uint FactorialDyadicVal(uint n)
		{
			uint val = 0;
			uint quotient = n;
			
			while (quotient >= 2)
			{
				quotient >>= 1;
				val += quotient;
			}
			
			return val;
		}
		
		public static ulong FactorialDyadicVal(ulong n)
		{
			ulong val = 0;
			ulong quotient = n;
			
			while (quotient >= 2)
			{
				quotient >>= 1;
				val += quotient;
			}
			
			return val;
		}
		
		#region Header
		/// <summary>
		/// Computes the largest power of a prime number that divides a factorial.
		/// </summary>
		/// <param name="n">The upper bound of the factorial.</param>
		/// <param name="p">A prime number.</param>
		/// <returns>
		/// The exponent of <paramref name="p"/> in the factorization of <paramref name="n"/>!, assuming
		/// <paramref name="n"/> is prime.  Undefined if <paramref name="p"/> is composite.
		/// </returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// <paramref name="p"/> is less than 2.
		/// </exception>
		/// <remarks>
		/// This is not a wrapper, but a direct implementation in C# (PARI's implementaion was found to be
		/// suboptimally efficient).  It is O(log(n)).
		/// </remarks>
		#endregion
		public static uint FactorialPrimeVal(uint n, uint p)
		{
			if (p < 2)
				throw new ArgumentOutOfRangeException("p", p, "p must be greater than 1");
			
			uint val = 0;
			uint quotient = n;
			
			while (quotient >= p)
			{
				quotient /= p;
				val += quotient;
			}
			
			return val;
		}
		
		#region Header
		/// <summary>
		/// Computes the largest power of a prime number that divides a factorial.
		/// </summary>
		/// <param name="n">The upper bound of the factorial.</param>
		/// <param name="p">A prime number.</param>
		/// <returns>
		/// The exponent of <paramref name="p"/> in the factorization of <paramref name="n"/>!, assuming
		/// <paramref name="n"/> is prime.  Undefined if <paramref name="p"/> is composite.
		/// </returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// <paramref name="p"/> is less than 2.
		/// </exception>
		/// <remarks>
		/// This is not a wrapper, but a direct implementation in C#.  It is O(log(n)).
		/// </remarks>
		#endregion
		public static ulong FactorialPrimeVal(ulong n, ulong p)
		{
			if (p < 2)
				throw new ArgumentOutOfRangeException("p", p, "p must be greater than 1");
			
			ulong val = 0;
			ulong quotient = n;
			
			while (quotient >= p)
			{
				quotient /= p;
				val += quotient;
			}
			
			return val;
		}
		
		public static uint SumDivisors(uint n)
		{
			uint sum = 0;
			SmallIntVector divs = new SmallIntVector(divisorsu(n));
			
			foreach (uint d in divs)
				sum += d;
			
			GP.Free(divs);
			return sum;
		}
		
		#region External PARI functions
		#region Header
		/// <summary>
		/// Given an integer n, computes d in the unique solution to n = df^2, where d is squarefree.
		/// </summary>
		/// <param name="n">A <c>uint</c>.</param>
		/// <returns>The core of <paramref name="n"/>.</returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "coreu")]
		public static extern uint Core(uint n);
		
		#region Header
		/// <summary>
		/// Sums the digits of an integer in its base 10 representation.
		/// </summary>
		/// <param name="n">A <c>uint</c>.</param>
		/// <returns>The base 10 digital sum of <paramref name="n"/>.</returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "sumdigitsu")]
		public static extern uint DigitalSum(uint n);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr divisorsu(uint n);
		
		#region Header
		/// <summary>
		/// Computes the dyadic valuation of an integer (this is equivalent to the number of trailing zeroes in the
		/// binary representation).
		/// </summary>
		/// <param name="n">A <c>uint</c>.</param>
		/// <returns>
		/// The dyadic valuation of <paramref name="n"/>, or -1 if <paramref name="n"/> is 0.
		/// </returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "vals")]
		public static extern int DyadicVal(uint n);
		
		#region Header
		/// <summary>
		/// Computes the valuation of a factorial at a given prime.
		/// </summary>
		/// <param name="n">The upper bound of the factorial.</param>
		/// <param name="p">A prime number.</param>
		/// <returns>The valuation of <paramref name="n"/>! at <paramref name="p"/>.</returns>
		/// <remarks>
		/// If <paramref name="p"/> is not prime, the result will be invalid, but no exception will be thrown.
		/// <para/>
		/// PARI's implemention of this function was found to be unacceptably inefficient, containing an
		/// unnecessary multiplication operation in a do loop.  Rather than wrap it, a native C# version without the
		/// unnecessary multiplication has been provided.
		/// </remarks>
		#endregion
		[DllImport(GP.DllName)]
		private static extern uint factorial_lval(uint n, uint p);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr factoru(uint n);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr factoru_pow(uint n);
		
		[DllImport(GP.DllName, EntryPoint = "cgcd")]
		public static extern int GCD(int x, int y);
		
		[DllImport(GP.DllName, EntryPoint = "ugcd")]
		public static extern uint GCD(uint x, uint y);
		
		[DllImport(GP.DllName, EntryPoint = "cbezout")]
		public static extern uint GCDExtended(uint x, uint y, out uint u, out uint v);
		
		#region Header
		/// <summary>
		/// Determines wether an integer is not divisible by the square of any prime.
		/// </summary>
		/// <param name="n">A <c>uint</c>.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="n"/> is squarefree; <c>false</c> otherwise.
		/// </returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "uissquarefree")]
		public static extern bool IsSquarefree(uint n);
		
		[DllImport(GP.DllName, EntryPoint = "clcm")]
		public static extern int LCM(int x, int y);
		
		#region Header
		/// <summary>
		/// Computes mu(n), defined as the sum of the primitive nth roots of unity. 
		/// </summary>
		/// <param name="n">A <c>uint</c>.</param>
		/// <returns>mu(<paramref name="n"/>), which will be a value in [-1, 0, 1].</returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "moebiusu")]
		public static extern int Moebius(uint n);
		
		#region Header
		/// <summary>
		/// Computes the totient of an integer, which is defined as the number of positive integers less than it
		/// that are coprime with it.
		/// </summary>
		/// <param name="n">A <c>uint</c>.</param>
		/// <returns>The totient of <paramref name="n"/>.</returns>
		/// <remarks>
		/// The totient of 0 is given as 2, a result that derives from an alternate definition of the totient.
		/// </remarks>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "eulerphiu")]
		public static extern uint Totient(uint n);
		#endregion
	}
}
