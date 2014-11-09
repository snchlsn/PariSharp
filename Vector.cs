#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public class Vector: PariListBase
	{
		/// <inheritdoc/>
		public override PariType Type
		{
			get { return PariType.Vector; }
		}
		
		#region External PARI functions
		#endregion
		
		internal Vector(IntPtr address): base(address) {}
	}
}
