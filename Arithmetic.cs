#region Using directives
using System;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public static class Arithmetic
	{
		#region External PARI functions
		[DllImport(GP.DllName, EntryPoint = "factorial_lval")]
		public static extern int FactorialValuation(uint n, uint p);
		#endregion
	}
}
