#region Using directives
using System;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public class Fraction: PariObject
	{
		public PariInteger Denominator
		{
			get { return null; } //TODO: Implement.
		}
		
		public PariInteger Numerator
		{
			get { return null; } //TODO: Implement.
		}
		
		#region External PARI functions
		#endregion
		
		internal Fraction(IntPtr address): base(address) {}
	}
}
