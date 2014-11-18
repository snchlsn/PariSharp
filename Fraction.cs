#region Using directives
using System;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	#region Header
	/// <summary>
	/// Represents a rational number with arbitrarily large numerator and denominator.
	/// </summary>
	/// <remarks>
	/// This class is a wrapper for PARI's t_FRAC type and related functions.
	/// <para/>
	/// All public methods that return <see cref="PariObject"/>s (including constructors and operators)
	/// clutter GP's stack unless otherwise noted (that is, the value returned is a new object in unmanaged memory).
	/// All methods that return primitive types are guaranteed not to clutter the stack, as are all property getters,
	/// regardless of type.
	/// </remarks>
	#endregion
	public class Fraction: PariObject
	{
		private const byte denominatorOffset = 8;
		private const byte numeratorOffset = 4;
		
		public PariInteger Denominator
		{
			get
			{
				unsafe { return new PariInteger(*((IntPtr*)(Address + denominatorOffset).ToPointer())); }
			}
		}
		
		public PariInteger Numerator
		{
			get
			{
				unsafe { return new PariInteger(*((IntPtr*)(Address + numeratorOffset).ToPointer())); }
			}
		}
		
		#region External PARI functions
		#endregion
		
		#region Header
		/// <summary>
		/// Creates a <c>Fraction</c> to wrap an existing t_FRAC.
		/// </summary>
		/// <param name="address">The t_FRAC to wrap.</param>
		/// <remarks>
		/// No type checking is done; it is assumed that callers will ensure that only t_FRACs are passed as arguments.
		/// </remarks>
		#endregion
		internal Fraction(IntPtr address): base(address) {}
	}
}
