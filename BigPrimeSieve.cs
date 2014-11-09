#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public sealed partial class BigPrimeSieve: IEnumerable<PariInteger>
	{
		public readonly PariInteger Start;
		public readonly PariInteger End;
		
		#region IEnumerable implementation
		public IEnumerator<PariInteger> GetEnumerator()
		{
			return new Enumerator(Start, End);
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		
		public static bool IsPrime(PariInteger num, PrimalityTest flag = PrimalityTest.Combination)
		{
			if (num == null)
				throw new ArgumentNullException("num");
			
			if (!Enum.IsDefined(typeof(PrimalityTest), flag))
				throw new InvalidEnumArgumentException("flag", (int)flag, typeof(PrimalityTest));
			
			//TODO: Adjust return value for the flag == 1 case.
			return (uint)gisprime(num.Address, (int)flag) > 0;
		}
		
		public static PariInteger NthPrime(int n)
		{
			if (n <= 0)
				throw new ArgumentOutOfRangeException("n", n, "n must be natural");
			
			return new PariInteger(prime(n));
		}
		
		public static PariInteger NextPrime(PariInteger num)
		{
			if (num == null)
				throw new ArgumentNullException("num");
			
			return new PariInteger(nextprime(num.Address));
		}
		
		public static PariInteger PrecedingPrime(PariInteger num)
		{
			if (num == null)
				throw new ArgumentNullException("num");
			
			return new PariInteger(precprime(num.Address));
		}
		
		#region External PARI functions
		[DllImport(GP.DllName)]
		private static extern IntPtr gisprime(IntPtr x, int flag);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr gispseudoprime(IntPtr x, int flag);
		
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
		
		public BigPrimeSieve(PariInteger start, PariInteger end)
		{
			if (start == null)
				throw new ArgumentNullException("start");
			
			if (end == null)
				throw new ArgumentNullException("end");
			
			Start = start;
			End = end;
		}
	}
}
