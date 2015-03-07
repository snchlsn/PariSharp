#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
#endregion

namespace PariSharp
{
	public partial class CompositeSieve: IEnumerable<PariInteger>
	{
		public readonly PariInteger Start;
		public readonly PariInteger End;
		
		#region IEnumerable implementation
		/// <inheritdoc/>
		public IEnumerator<PariInteger> GetEnumerator()
		{
			return new Enumerator(Start, End);
		}
		
		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		
		public CompositeSieve(PariInteger end)
		{
			if (object.ReferenceEquals(end, null))
				throw new ArgumentNullException("end");
			
			Start = PariInteger.Two;
			End = end;
		}
		
		public CompositeSieve(PariInteger start, PariInteger end)
		{
			if (object.ReferenceEquals(start, null))
				throw new ArgumentNullException("start");
			
			if (object.ReferenceEquals(end, null))
				throw new ArgumentNullException("end");
			
			Start = start;
			End = end;
		}
	}
}
