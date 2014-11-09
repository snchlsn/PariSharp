#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public sealed partial class SmallIntVector: PariObject, IReadOnlyList<int>, IEnumerable<int>
	{
		public override PariType Type
		{
			get { return PariType.SmallIntVector; }
		}
		
		#region IReadOnlyList implementation
		public int Count
		{
			get { return glength(Address); }
		}
		
		public int this[int index]
		{
			get
			{
				unsafe { return *(int*)(Address + ((index + 1) << 2)).ToPointer(); }
			}
		}
		
		public IEnumerator<int> GetEnumerator()
		{
			return new Enumerator(this);
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}
		#endregion
		
		#region External PARI functions
		#endregion
		
		internal SmallIntVector(IntPtr address): base(address) {}
	}
}
