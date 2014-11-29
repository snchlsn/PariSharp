#region Using directives
using System;
using System.IO;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public static class GP
	{
		#region Constants
		#region Header
		/// <summary>
		/// The file name of the PARI library.
		/// </summary>
		/// <remarks>
		/// Never refer to libpari using a literal; always use this constant.  In the event that PariSharp
		/// ever goes cross-platform, this will make the transition easier.
		/// </remarks>
		#endregion
		internal const string DllName = "libpari.dll";
		
		#region Header
		/// <summary>
		/// Size of the struct used by forprime functions to maintain state, in units of 32-bit words.
		/// </summary>
		#endregion
		internal const byte SizeOfForPrimeT = 26 * sizeof(uint);
		#endregion
		
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
		
		#region Header
		/// <summary>
		/// Frees memory on the PARI stack allocated to a given <see cref="PariObject"/> and all <see cref="PariObject"/>s
		/// created after it.
		/// </summary>
		/// <param name="x">The <see cref="PariObject"/> to deallocate.</param>
		/// <remarks>
		/// This is an O(1) operation, as all it does is move the stack pointer.
		/// <para/>
		/// The caller must pay attention to the order of creation of <see cref="PariObjects"/> to ensure that no
		/// memory that is still needed is freed, and also that all memory that sould be freed is.  This is
		/// especially important when dealing with container types, like <see cref="Vector"/> and <see cref="Fraction"/>.
		/// </remarks>
		/// <exception cref="System.ArgumentExcception">
		/// <paramref name="x"/> is not on the PARI stack.
		/// </exception>
		#endregion
		public static void Free(PariObject x)
		{
			if (!x.IsOnStack)
				throw new ArgumentException("Attempt made to free an object that is not on the PARI stack.", "x");
			
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
			int stackTopInt;
			PariInteger temp;
			
			pari_init(size, maxPrime);
			stackTopInt = (temp = new PariInteger(5)).Address.ToInt32();
			stackTopInt -= GetStackSize();
			stackTopInt += temp.Size << 2;
			stackTop = new IntPtr(stackTopInt);
			ClearStack();
			PariInteger.InitializeConstants();
			return;
		}
		
		public static Vector ReadVector(string fileName)
		{
			Vector vec;
			
			using (FileStream fileStream = new FileInfo(fileName).OpenRead())
			{
				vec = new Vector(gp_readvec_stream(fileStream.SafeFileHandle.DangerousGetHandle()));
			}
			return vec;
		}
		
		public static Vector ReadVector(FileInfo file)
		{
			if (file == null)
				throw new ArgumentNullException("file");
			
			Vector vec;
			
			using (FileStream fileStream = file.OpenRead())
			{
				vec = new Vector(gp_readvec_stream(fileStream.SafeFileHandle.DangerousGetHandle()));
			}
			return vec;
		}
		
		#region Header
		/// <summary>
		/// Frees memory on the PARI stack allocated to a given <see cref="PariObject"/> and all <see cref="PariObject"/>s
		/// created after it.
		/// </summary>
		/// <param name="x">The <see cref="PariObject"/> to deallocate.</param>
		/// <remarks>
		/// This is an O(1) operation, as all it does is move the stack pointer.
		/// <para/>
		/// The caller must pay attention to the order of creation of <see cref="PariObjects"/> to ensure that no
		/// memory that is still needed is freed, and also that all memory that should be freed is.  This is
		/// especially important when dealing with container types, like <see cref="Vector"/> and <see cref="Fraction"/>.
		/// <para/>
		/// This method accepts arguments that are not on the PARI stack.  If such an argument is passed, no memory
		/// is freed, no action is taken, and the caller is not notified that the attempt failed.  If notification
		/// is needed, use the <see cref="Free"/> method instead.
		/// </remarks>
		#endregion
		public static void TryFree(PariObject x)
		{
			cgiv(x.Address);
			return;
		}
		
		#region External PARI functions
		//TODO: Import and wrap more flexible garbage collection functions.
		
		[DllImport(DllName)]
		private static extern void cgiv(IntPtr z);
		
		#region Header
		/// <summary>
		/// The amount of memory on the PARI stack that is currently in use, in bytes.
		/// </summary>
		/// <returns>The size of the PARI stack.</returns>
		/// <remarks>
		/// Other PARI functions report sizes in terms of 32-bit words, but this one uses bytes.  Convert
		/// before combining.
		/// </remarks>
		#endregion
		[DllImport(DllName, EntryPoint = "getstack")]
		public static extern int GetStackSize();
		
		[DllImport(DllName)]
		private static extern IntPtr version(); //TODO: Determine whether this clutters the stack, then wrap.
		
		[DllImport(DllName)]
		private static extern IntPtr gp_readvec_stream(IntPtr file);
		
		[DllImport(DllName)]
		private static extern void pari_init(uint size, uint maxPrime);
		#endregion
	}
}
