#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public sealed partial class SmallPrimeSieve
	{
		public sealed class Enumerator: IEnumerator<uint>
		{
			private uint current;
			private IntPtr t = Marshal.AllocCoTaskMem(GP.SizeOfForPrimeT);
			
			#region IEnumerator implementation
			/// <inheritdoc/>
			public uint Current
			{
				get { return current; }
			}
			
			/// <inheritdoc/>
			object IEnumerator.Current
			{
				get { return current; }
			}
			
			/// <inheritdoc/>
			public void Dispose()
			{
				Marshal.FreeCoTaskMem(t);
				return;
			}
			
			/// <inheritdoc/>
			public bool MoveNext()
			{
				return (current = u_forprime_next(t)) != 0;
			}
			
			#region Header
			/// <summary>
			/// This method is not implemented.  Do not call.
			/// </summary>
			#endregion
			public void Reset()
			{
				throw new NotImplementedException("Resetting a SmallPrimeSieve.Enumerator is not supported.  Create a new instance instead.");
			}
			#endregion
			
			#region External PARI functions
			#region Header
			/// <summary>
			///  Initializes the prime iterator on the PARI stack.
			/// </summary>
			/// <param name="t">
			/// PARI expects a pointer to an unitialized forprime_t struct.  The expected size of allocated memory is given
			/// as <c>PariSharp.GP.SizeOfForPrimeT</c>.  The memory used should not be modified in wrapper code after
			/// this function has been called.
			/// </param>
			/// <param name="a">The lower bound for iteration.</param>
			/// <param name="b">The upper bound for iteration.</param>
			/// <returns><c>1</c> on success; <c>0</c> if <paramref name="a"/> > <paramref name="b"/>.</returns>
			#endregion
			[DllImport(GP.DllName)]
			internal static extern int u_forprime_init(IntPtr t, uint a, uint b);
			
			#region Header
			/// <summary>
			/// Advances the iterator and returns the next prime.
			/// </summary>
			/// <param name="t">The same pointer passed to <c>u_forprime_init</c>.</param>
			/// <returns>The next prime in range, or <c>0</c> if there are no more primes.</returns>
			#endregion
			[DllImport(GP.DllName)]
			internal static extern uint u_forprime_next(IntPtr t);
			#endregion
			
			public Enumerator(uint start = 2, uint end = uint.MaxValue)
			{
				if (u_forprime_init(t, start, end) == 0)
					throw new ArgumentException("end cannot be less than start", "end");
			}
		}
	}
}