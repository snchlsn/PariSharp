#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public abstract partial class PariListBase
	{
		public struct Enumerator: IEnumerator<PariObject>
		{
			private readonly PariListBase owner;
			private int currentOffset;
			
			#region IEnumerator implementation
			/// <inheritdoc/>
			public PariObject Current
			{
				get
				{
					unsafe { return new PariObject(*(IntPtr*)(owner.Address + currentOffset).ToPointer()); }
				}
			}
			
			/// <inheritdoc/>
			object IEnumerator.Current
			{
				get { return Current; }
			}
			
			/// <inheritdoc/>
			public void Dispose() {}
			
			/// <inheritdoc/>
			public bool MoveNext()
			{
				currentOffset += 4;
				return (currentOffset >> 2) <= owner.Length;
			}
			
			/// <inheritdoc/>
			public void Reset()
			{
				currentOffset = 0;
			}
			#endregion
			
			public Enumerator(PariListBase owner)
			{
				this.owner = owner;
				currentOffset = 0;
			}
		}
	}
}
