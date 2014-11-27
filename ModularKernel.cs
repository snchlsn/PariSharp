#region Using directives
using System;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	#region Header
	/// <summary>
	/// Exposes low-level PARI functions for performing modular arithmetic.
	/// </summary>
	/// <remarks>
	/// In the context of this class, 'modulo' and '%' refer to the true Euclidean remainder, which is not what the
	/// C# modulo operator returns: the modulo operator keeps the sign of the dividend, whereas these methods
	/// always return positive values (unless otherwise specified).
	/// <para/>
	/// In some cases, using C# equivalents may be more efficient.  Functions that clearly would add no value
	/// in C# have not been imported.
	/// <para/>
	/// None of the methods in this class clutter the PARI stack.
	/// </remarks>
	#endregion
	public static class ModularKernel
	{
		#region External PARI functions
		#region Header
		//TODO: Wrap this.
		/// <summary>
		/// Returns the smallest positive representative of x^-1 mod 2^BITS IN LONG, assuming x is odd.
		/// </summary>
		/// <param name="x">An odd integer.</param>
		/// <returns><paramref name="x"/>^1 % 2^BITS IN LONG.</returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern uint invmod2BIL(uint x);
		
		#region Header
		/// <summary>
		/// Returns the smallest positive representative of x+y	modulo m.
		/// </summary>
		/// <param name="x">Left addend.</param>
		/// <param name="y">Right addend.</param>
		/// <param name="m">The modulus.</param>
		/// <returns>(<paramref name="x"/>+<paramref name="y"/>)%<paramref name="m"/></returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "Fl_add")]
		public static extern uint Add(uint x, uint y, uint m);
		
		#region Header
		/// <summary>
		/// Returns the smallest positive representative of -x modulo m.
		/// </summary>
		/// <param name="x">The integer to negate.</param>
		/// <param name="m">The modulus.</param>
		/// <returns>(-<paramref name="x"/>)%<paramref name="m"/></returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "Fl_neg")]
		public static extern uint Negate(uint x, uint m);
		
		#region Header
		/// <summary>
		/// Returns the smallest positive representative of x-y	modulo m.
		/// </summary>
		/// <param name="x">Left addend.</param>
		/// <param name="y">Right addend.</param>
		/// <param name="m">The modulus.</param>
		/// <returns>(<paramref name="x"/>-<paramref name="y"/>)%<paramref name="m"/></returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "Fl_sub")]
		public static extern uint Subtract(uint x, uint y, uint m);
		
		#region Header
		//TODO: Wrap this.
		/// <summary>
		/// Returns the representative in [-m/2;m/2) of x modulo m. Assume 0 &lt;= x &lt; m and mo2 = m &gt;&gt; 1.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="m"></param>
		/// <param name="mo2"></param>
		/// <returns></returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern long Fl_center(uint x, uint m, uint mo2);
		
		#region Header
		/// <summary>
		/// Returns the smallest positive representative of x*y modulo m.
		/// </summary>
		/// <param name="x">Left multiplicand.</param>
		/// <param name="y">Right multiplicand.</param>
		/// <param name="m">The modulus.</param>
		/// <returns>(<paramref name="x"/>*<paramref name="y"/>)%<paramref name="m"/></returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "Fl_mul")]
		public static extern uint Multiply(uint x, uint y, uint m);
		
		#region Header
		/// <summary>
		/// Returns 2x modulo m.
		/// </summary>
		/// <param name="x">The multiplicand.</param>
		/// <param name="m">The modulus.</param>
		/// <returns>(2<paramref name="x"/>)%<paramref name="m"/></returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "Fl_double")]
		public static extern uint Double(uint x, uint m);
		
		#region Header
		/// <summary>
		/// Returns 3x modulo m.
		/// </summary>
		/// <param name="x">The multiplicand.</param>
		/// <param name="m">The modulus.</param>
		/// <returns>(3<paramref name="x"/>)%<paramref name="m"/></returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "Fl_triple")]
		public static extern uint Triple(uint x, uint m);
		
		#region Header
		/// <summary>
		/// Returns the smallest positive representative of x^2 modulo m.
		/// </summary>
		/// <param name="x">The number to be squared.</param>
		/// <param name="m">The modulus.</param>
		/// <returns><paramref name="x"/>^2%<paramref name="m"/></returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "Fl_sqr")]
		public static extern uint Sqr(uint x, uint m);
		
		#region Header
		//TODO: Wrap this.
		/// <summary>
		/// Returns the smallest positive representative of x^-1 modulo m.  If x is not invertible mod m, return 0 (which is ambiguous if m = 1).
		/// </summary>
		/// <param name="x"></param>
		/// <param name="m"></param>
		/// <returns></returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern uint Fl_invsafe(uint x, uint m);
		
		#region Header
		//TODO: Wrap this.
		/// <summary>
		/// Returns the smallest positive representative of x*y^-1 modulo m. If y is not invertible mod m, raise an exception.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="m"></param>
		/// <returns></returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern uint Fl_div(uint x, uint y, uint m);
		
		#region Header
		/// <summary>
		/// Returns the smallest positive representative of x^n modulo m.
		/// </summary>
		/// <param name="x">The base.</param>
		/// <param name="n">The exponent.</param>
		/// <param name="m">The modulus.</param>
		/// <returns><paramref name="x"/>^<paramref name="n"/>%<paramref name="m"/></returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "Fl_powu")]
		public static extern uint Pow(uint x, uint n, uint m);
		
		#region Header
		/// <summary>
		/// Returns the square root of x modulo p (smallest positive representative).  Assumes p to be prime, and x to be a square modulo p.
		/// </summary>
		/// <param name="x">A square modulo <paramref name="p"/>.</param>
		/// <param name="p">The modulus.  Must be prime.</param>
		/// <returns>The square root of <paramref name="x"/> % <paramref name="p"/>.</returns>
		/// <remarks>
		/// No error checking is done.  If preconditios are not met, and invalid result will be returned.
		/// </remarks>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "Fl_sqrt")]
		public static extern uint Sqrt(uint x, uint p); 
		
		#region Header
		//TODO: Wrap this.
		/// <summary>
		/// Returns the order of the t_Fp a. It is assumed that o is a multiple of the order of a, 0 being allowed (no non-trivial information).
		/// </summary>
		/// <param name="a"></param>
		/// <param name="o"></param>
		/// <param name="p"></param>
		/// <returns></returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern uint Fl_order(uint a, uint o, uint p);
		
		#region Header
		/// <summary>
		/// Generates a pseudo-random number modulo a given <c>uint</c>.
		/// </summary>
		/// <param name="m">The modulus.</param>
		/// <returns>A pseudo-random integer uniformly distributed in 0, 1, ... <paramref name="m"/>-1.</returns>
		#endregion
		[DllImport(GP.DllName, EntryPoint = "random_Fl")]
		public static extern uint Random(uint m);
		
		#region Header
		//TODO: Wrap this.
		/// <summary>
		/// Returns the smallest primitive root modulo p, assuming p is prime.
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern uint pgener_Fl(uint p);
		
		#region Header
		//TODO: Wrap this.
		/// <summary>
		/// Returns the smallest primitive root modulo p^k, k > 1, assuming p is an odd prime.
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern uint pgener_Zl(uint p);
		
		#region Header
		//TODO: Wrap this.
		/// <summary>
		/// See gener Fp local, L is an Flv.
		/// </summary>
		/// <param name="p"></param>
		/// <param name="L"></param>
		/// <returns></returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern uint pgener_Fl_local(uint p, IntPtr L);
		#endregion
	}
}
