#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
#endregion

namespace PariSharp
{
	public class Column: PariListBase
	{
		public override PariType Type
		{
			get { return PariType.Column; }
		}
		
		internal Column(IntPtr address): base(address) {}
	}
}
