#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public sealed partial class Matrix
	{
		public struct Enumerator: IEnumerator<Column>
		{
			private Matrix owner;
			private int currentOffset;
			
			#region IEnumerator implementation
			/// <inheritdoc/>
			public Column Current
			{
				get
				{
					unsafe { return new Column(*(IntPtr*)(owner.Address + currentOffset).ToPointer()); }
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
			
			public Enumerator(Matrix owner)
			{
				this.owner = owner;
				currentOffset = 0;
			}
		}
	}
}
