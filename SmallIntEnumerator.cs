#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public sealed partial class SmallIntVector
	{
		public struct Enumerator: IEnumerator<int>
		{
			private readonly SmallIntVector owner;
			private int currentOffset;
			
			#region IEnumerator implementation
			/// <inheritdoc/>
			public int Current
			{
				get
				{
					unsafe { return *(int*)(owner.Address + currentOffset).ToPointer(); }
				}
			}
			
			/// <inheritdoc/>
			object IEnumerator.Current
			{
				get { return Current; }
			}
			
			/// <inheritdoc/>
			public void Dispose() { return; }
			
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
				return;
			}
			#endregion
			
			public Enumerator(SmallIntVector owner)
			{
				this.owner = owner;
				currentOffset = 0;
			}
		}
	}
}
