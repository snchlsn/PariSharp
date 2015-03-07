#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
#endregion

namespace PariSharp
{
	public partial class SmallCompositeSieve: IEnumerable<uint>
	{
		public readonly uint Start;
		public readonly uint End;
		
		#region IEnumerable implementation
		/// <inheritdoc/>
		public IEnumerator<uint> GetEnumerator()
		{
			return new Enumerator(Start, End);
		}
		
		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		
		public SmallCompositeSieve(uint start = 4, uint end = uint.MaxValue)
		{
			if (end < 4)
				throw new ArgumentOutOfRangeException("end", 4, "end cannot be less than 4.");
			if (end < start)
				throw new ArgumentException("end cannot be less than start.", "end");
			
			Start = start;
			End = end;
		}
	}
}
