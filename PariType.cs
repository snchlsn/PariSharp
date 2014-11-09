using System;

namespace PariSharp
{
	#region Header
	/// <summary>
	/// Enumerates the custom data types implemented by PARI.
	/// </summary>
	/// <remarks>
	/// See section 4.5 in the libpari manual for implementation details.
	/// </remarks>
	#endregion
	public enum PariType: int
	{
		#region Header
		/// <summary>
		/// t_INT: Arbitrarily large integer.
		/// </summary>
		#endregion
		Integer = 1,
		
		#region Header
		/// <summary>
		/// t_REAL: Arbitrarily precise floating-point number.
		/// </summary>
		#endregion
		Real = 2,
		
		#region Header
		/// <summary>
		/// t_INTMOD: A class of integers that are congruent modulo a given integer. 
		/// </summary>
		#endregion
		IntegerMod = 3,
		
		#region Header
		/// <summary>
		/// t_FRAC: A rational number with arbitrarily large numerator and denominator.
		/// </summary>
		#endregion
		Fraction = 4,
		FiniteFieldElement = 5,
		
		#region Header
		/// <summary>
		/// t_COMPLEX: A complex number, where each component may be represented by a t_INT, t_REAL, or t_FRAC.
		/// </summary>
		#endregion
		Complex =  6,
		PAdic = 7,
		Quadratic = 8,
		PolynomialMod = 9,
		Polynomial = 10,
		PowerSeries = 11,
		RationalFunction = 13,
		IndefiniteBinaryQuadratic = 15,
		DefiniteBinaryQuadratic = 16,
		Vector = 17,
		Column = 18,
		Matrix = 19,
		List = 20,
		String = 21,
		
		#region Header
		/// <summary>
		/// t_VECSMALL: An array of <c>Int32</c>s.
		/// </summary>
		#endregion
		SmallIntVector = 22,
		Closure = 23,
		Error = 24
	}
}
