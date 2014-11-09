#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public abstract partial class PariListBase: PariObject, IEnumerable<PariObject>, IReadOnlyList<PariObject>
	{
		#region IReadOnlyList implementation
		#region Header
		/// <inheritdoc/>
		/// <remarks>
		/// This property is interchangeable with <see cref="Length"/>, and implemented only
		/// because <see cref="IReadOnlyList"/> requires it.
		/// </remarks>
		#endregion
		public int Count
		{
			get { return glength(Address); }
		}
		
		public PariObject this[int index]
		{
			get
			{
				unsafe { return new PariObject(*(IntPtr*)(Address + ((index + 1) << 2)).ToPointer()); }
			}
		}
		#endregion
		
		#region IEnumerable implementation
		/// <inheritdoc/>
		public IEnumerator<PariObject> GetEnumerator()
		{
			return new Enumerator(this);
		}
		
		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}
		#endregion
		
		internal PariListBase(IntPtr address): base(address) {}
	}
}
