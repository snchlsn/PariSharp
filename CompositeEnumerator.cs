#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public partial class CompositeSieve
	{
		public sealed class Enumerator: IEnumerator<PariInteger>
		{
			private PariInteger current;
			private IntPtr t = Marshal.AllocCoTaskMem(GP.SizeOfForCompositeT);
			
			#region IEnumerator implementation
			/// <inheritdoc/>
			public PariInteger Current
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
				return (current = new PariInteger(forcomposite_next(t))).Size > 2;
			}
			
			#region Header
			/// <summary>
			/// This method is not implemented.  Do not call.
			/// </summary>
			#endregion
			public void Reset()
			{
				throw new NotImplementedException("Resetting a CompositeSieve.Enumerator is not supported.  Create a new instance instead.");
			}
			#endregion
			
			#region External PARI functions
			[DllImport(GP.DllName)]
			private static extern int forcomposite_init(IntPtr t, IntPtr a, IntPtr b);
			
			[DllImport(GP.DllName)]
			private static extern IntPtr forcomposite_next(IntPtr t);
			#endregion
			
			public Enumerator(PariInteger start, PariInteger end)
			{
				if (forcomposite_init(t, start.Address, end.Address) < 1)
					throw new ArgumentException("end cannot be less than start", "end");
			}
		}
	}
}
