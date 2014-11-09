#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public partial class BigPrimeSieve
	{
		public sealed class Enumerator: IEnumerator<PariInteger>
		{
			private PariInteger current;
			private uint[] t = new uint[GP.SizeOfForPrimeT];
			
			#region IEnumerator implementation
			/// <inheritdoc />
			public PariInteger Current
			{
				get { return current; }
			}
			
			object IEnumerator.Current
			{
				get { return current; }
			}
			
			public void Dispose()
			{
				t = null;
				return;
			}
			
			/// <inheritdoc />
			public bool MoveNext()
			{
				return (current = new PariInteger(forprime_next(t))).Size > 2;
			}
			
			#region Header
			/// <summary>
			/// This method is not implemented.  Do not call.
			/// </summary>
			#endregion
			public void Reset()
			{
				throw new NotImplementedException("Resetting a BigPrimeEnumerator is not supported.  Create a new instance instead.");
			}
			#endregion
			
			#region External PARI functions
			[DllImport(GP.DllName)]
			private static extern int forprime_init(uint[] t, IntPtr a, IntPtr b);
			
			[DllImport(GP.DllName)]
			private static extern IntPtr forprime_next(uint[] t);
			#endregion
			
			public Enumerator(PariInteger start, PariInteger end)
			{
				if (forprime_init(t, start.Address, end.Address) < 1)
					throw new ArgumentException("end cannot be less than start", "end");
			}
		}
	}
}
