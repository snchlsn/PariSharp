#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public sealed partial class Matrix: PariObject, IReadOnlyList<Column>, IEnumerable<Column>
	{
		public int ColumnLength
		{
			get { return nbrows(Address); }
		}
		
		public override PariType Type
		{
			get { return PariType.Matrix; }
		}
		
		#region IReadOnlyList implementation
		public int Count
		{
			get { return glength(Address); }
		}
		
		public Column this[int index]
		{
			get
			{
				unsafe { return new Column(*(IntPtr*)(Address + ((index + 1) << 2)).ToPointer()); }
			}
		}
		#endregion
		
		#region IEnumerable implementation
		public IEnumerator<Column> GetEnumerator()
		{
			return new Enumerator(this);
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}
		#endregion
		
		#region External PARI functions
		[DllImport(GP.DllName)]
		private static extern int lgcols(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern int nbrows(IntPtr x);
		#endregion
		
		internal Matrix(IntPtr address): base(address) {}
	}
}
