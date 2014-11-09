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
		private static extern uint Negate(uint x, uint m);
		
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
		private static extern uint Subtract(uint x, uint y, uint m);
		
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
		//TODO: Wrap this.
		/// <summary>
		/// Returns the smallest positive representative of x*y modulo m.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="m"></param>
		/// <returns></returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern uint Fl_mul(uint x, uint y, uint m);
		
		#region Header
		//TODO: Wrap this.
		/// <summary>
		/// Returns 2x modulo m.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="m"></param>
		/// <returns></returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern uint Fl_double(uint x, uint m);
		
		#region Header
		//TODO: Wrap this.
		/// <summary>
		/// Returns 3x modulo m.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="m"></param>
		/// <returns></returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern uint Fl_triple(uint x, uint m);
		
		#region Header
		//TODO: Wrap this.
		/// <summary>
		/// Returns the smallest positive representative of x^2 modulo m.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="m"></param>
		/// <returns></returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern uint Fl_sqr(uint x, uint m);
		
		#region Header
		//TODO: Wrap this.
		/// <summary>
		/// Returns the smallest positive representative of x^-1 modulo m.  If x is not invertible mod m, raise an exception.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="m"></param>
		/// <returns></returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern uint Fl_inv(uint x, uint m);
		
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
		//TODO: Wrap this.
		/// <summary>
		/// Returns the smallest positive representative of x^n modulo m.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="n"></param>
		/// <param name="m"></param>
		/// <returns></returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern uint Fl_powu(uint x, uint n, uint m);
		
		#region Header
		//TODO: Wrap this.
		/// <summary>
		/// Returns the square root of x modulo p (smallest positive representative).  Assumes p to be prime, and x to be a square modulo p.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="p"></param>
		/// <returns></returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern uint Fl_sqrt(uint x, uint p); 
		
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
		//TODO: Wrap this.
		/// <summary>
		/// Returns a pseudo-random integer uniformly distributed in 0, 1, ... p-1.
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern uint random_Fl(uint p);
		
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
