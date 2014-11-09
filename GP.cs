#region Using directives
using System;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public static class GP
	{
		internal const string DllName = "libpari.dll";
		
		#region Header
		/// <summary>
		/// Size of the struct used by forprime functions to maintain state, in units of 32-bit words.
		/// </summary>
		#endregion
		internal const byte SizeOfForPrimeT = 26 * sizeof(uint);
		
		private static IntPtr stackTop;
		
		#region Header
		/// <summary>
		/// Clears GP's entire stack, freeing memory.
		/// </summary>
		/// <remarks>
		/// This is an O(1) operation, as all it does is return the stack pointer to the top.
		/// <para/>
		/// Although they will not be immediately overwritten, preexisting <c>PariObject</c>s should be considered
		/// disposed after this method has been called, and not accessed again.
		/// </remarks>
		#endregion
		public static void ClearStack()
		{
			cgiv(stackTop);
			return;
		}
		
		public static void Free(PariObject x)
		{
			cgiv(x.Address);
			return;
		}
		
		#region Header
		/// <summary>
		/// Performs setup of the GP environment, including stack memory allocation, precomputation of low primes,
		/// and getting handles to the named integer constants.
		/// </summary>
		/// <param name="size">
		/// The amount of memory to allocate to the stack, in 32-bit words.  The default is 500000, and it is recommended
		/// that you not go below this amount, although there is no strictly enforced minimum.
		/// </param>
		/// <param name="maxPrime">
		/// The upper bound for precomputed primes.  Note that this bound is not strictly enforced; PARI has a minimum
		/// requirement for precomputed primes.  Indeed, the default value is 0, which ensures a minumum of
		/// precomputation.
		/// </param>
		/// <remarks>
		/// Important: In general, you must call this method before calling any other method in the PariSharp
		/// namespace.  Methods that take and return only primitive types may still work if you don't, but they
		/// may also internally use the stack or the prime table.  Behavior of other methods is undefined when not
		/// called after this method.
		/// </remarks>
		#endregion
		public static void Initialize(uint size = 500000, uint maxPrime = 0)
		{
			pari_init(size, maxPrime);
			stackTop = new PariInteger(5).Address;
			ClearStack();
			PariInteger.InitializeConstants();
			return;
		}
		
		#region External PARI functions
		[DllImport(DllName)]
		private static extern void cgiv(IntPtr z);
		
		[DllImport(DllName, EntryPoint = "getstack")]
		public static extern int GetStackSize();
		
		[DllImport(DllName)]
		private static extern void pari_init(uint size, uint maxPrime);
		#endregion
	}
}
