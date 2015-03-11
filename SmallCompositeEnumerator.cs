#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public partial class SmallCompositeSieve
	{
		public sealed class Enumerator: IEnumerator<uint>
		{
			private uint current;
			private uint nextPrime;
			private uint end;
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
				if (current == uint.MaxValue)
					return false;
				
				if (++current == nextPrime)
				{
					++current;
					nextPrime = SmallPrimeSieve.Enumerator.u_forprime_next(t);
				}
				return current <= end;
			}
			
			#region Header
			/// <summary>
			/// This method is not implemented.  Do not call.
			/// </summary>
			#endregion
			public void Reset()
			{
				throw new NotImplementedException("Resetting a SmallCompositeSieve.Enumerator is not supported.  Create a new instance instead.");
			}
			#endregion
			
			public Enumerator(uint start = 4, uint end = uint.MaxValue)
			{
				if (end < 4)
					throw new ArgumentOutOfRangeException("end", end, "end cannot be less than 4.");
				if (start < end)
					throw new ArgumentException("end cannot be less than start.", "end");
				
				if (start < 4)
					start = 4;
				
				SmallPrimeSieve.Enumerator.u_forprime_init(t, start, end);
				
				current = start - 1;
				nextPrime = SmallPrimeSieve.Enumerator.u_forprime_next(t);
				this.end = end;
			}
		}
	}
}
