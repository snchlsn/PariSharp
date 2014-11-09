#region Using directives
using System;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public class Real: PariObject
	{
		/// <inheritdoc/>
		public override PariType Type
		{
			get { return PariType.Real; }
		}
		
		#region Header
		/// <summary>
		/// Computes an approximation of the factorial of an integer.
		/// </summary>
		/// <param name="x">The operand.</param>
		/// <param name="prec">
		/// The number of 32-bit words to allocate for the approximation (plus 2 for the codewords).
		/// </param>
		/// <returns>Approximately <paramref name="x"/>!</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// <paramref name="x"/> is negative -or- <paramref name="prec"/> is less than 3.
		/// </exception>
		#endregion
		public static Real Factorial(int x, int prec)
		{
			if (x < 0)
				throw new ArgumentOutOfRangeException("x", x, "x must be positive");
			
			if (prec < 3)
				throw new ArgumentOutOfRangeException("prec", prec, "prec must be at least 3");
			
			return new Real(mpfactr(x, prec));
		}
		
		#region External PARI functions
		#region Header
		/// <summary>
		/// Computes an approximation of the factorial of an integer.
		/// </summary>
		/// <param name="x">The operand.  Must be positive.</param>
		/// <param name="prec">
		/// The number of 32-bit words to allocate for the approximation, including codewords.
		/// </param>
		/// <returns><paramref name="x"/>!, in t_REAL form.</returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern IntPtr mpfactr(int x, int prec);
		#endregion
		
		internal Real(IntPtr address): base(address) {}
	}
}
