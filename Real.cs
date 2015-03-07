#region Using directives
using System;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	#region Header
	/// <summary>
	/// Represents an arbitrarily large floating-point number.
	/// </summary>
	/// <remarks>
	/// This class is a wrapper for PARI's t_REAL type and related functions.
	/// <para/>
	/// All public methods that return <see cref="PariObject"/>s (including constructors and operators)
	/// clutter GP's stack unless otherwise noted (that is, the value returned is a new object in unmanaged memory).
	/// All methods that return primitive types are guaranteed not to clutter the stack, as are all property getters,
	/// regardless of type.
	/// <para/>
	/// Due to rounding error and the arbitrarily precise implementation, this class should be considered to
	/// represent a range of real numbers, rather than a single real number.
	/// <para/>
	/// Constructors and methods that return <c>Real</c>s will often take a parameter called <c>prec</c>.  This
	/// parameter specifies the precision of the returned <c>Real</c>.  It is given as the length in 32-bit words,
	/// not of the mantissa, but of the whole <c>Real</c>, including two codewords.  The minimum value is consequently
	/// always 3.
	/// </remarks>
	#endregion
	public class Real: PariObject, IComparable<Real>, IEquatable<Real>
	{
		private const string precOutOfRangeMsg = "prec must be at least 3.";
		
		#region Properties
		/// <inheritdoc/>
		public override PariType Type
		{
			get { return PariType.Real; }
		}
		#endregion
		
		#region IComparable implementation
		#region Header
		/// <inheritdoc/>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="other"/> is <c>null</c>.
		/// </exception>
		#endregion
		public int CompareTo(Real other)
		{
			if (object.ReferenceEquals(other, null))
				throw new ArgumentNullException("other");
			
			return cmprr(Address, other.Address);
		}
		#endregion
		
		#region IEquatable implementation
		/// <inheritdoc/>
		public bool Equals(Real other)
		{
			if (object.ReferenceEquals(other, null))
				return false;
			
			return equalrr(Address, other.Address);
		}
		
		/// <inheritdoc/>
		public override bool Equals(object other)
		{
			if (object.ReferenceEquals(other, null))
				return false;
			
			if (other is Real)
				return equalrr(Address, ((PariObject)other).Address);
			
			if (other is PariObject && ((PariObject)other).Type == PariType.Real)
				return equalrr(Address, ((PariObject)other).Address);
			
			return false;
		}
		#endregion
		
		#region Operators
		public static Real operator +(Real x, Real y)
		{
			return new Real(addrr(x.Address, y.Address));
		}
		
		public static Real operator +(Real x, PariInteger y)
		{
			return new Real(addir(y.Address, x.Address));
		}
		
		public static Real operator +(PariInteger x, Real y)
		{
			return new Real(addir(x.Address, y.Address));
		}
		
		public static Real operator +(Real x, int y)
		{
			return new Real(addsr(y, x.Address));
		}
		
		public static Real operator +(int x, Real y)
		{
			return new Real(addsr(x, y.Address));
		}
		
		public static Real operator +(Real x, uint y)
		{
			return new Real(addur(y, x.Address));
		}
		
		public static Real operator +(uint x, Real y)
		{
			return new Real(addur(x, y.Address));
		}
		
		public static Real operator -(Real x, Real y)
		{
			return new Real(subrr(x.Address, y.Address));
		}
		
		public static Real operator -(Real x, PariInteger y)
		{
			return new Real(subri(x.Address, y.Address));
		}
		
		public static Real operator -(PariInteger x, Real y)
		{
			return new Real(subir(x.Address, y.Address));
		}
		
		public static Real operator -(Real x, int y)
		{
			return new Real(subrs(x.Address, y));
		}
		
		public static Real operator -(int x, Real y)
		{
			return new Real(subsr(x, y.Address));
		}
		
		public static Real operator -(Real x, uint y)
		{
			return new Real(subru(x.Address, y));
		}
		
		public static Real operator -(uint x, Real y)
		{
			return new Real(subur(x, y.Address));
		}
		
		public static Real operator *(Real x, Real y)
		{
			return new Real(mulrr(x.Address, y.Address));
		}
		
		public static Real operator *(Real x, PariInteger y)
		{
			return new Real(mulir(y.Address, x.Address));
		}
		
		public static Real operator *(PariInteger x, Real y)
		{
			return new Real(mulir(x.Address, y.Address));
		}
		
		public static Real operator *(Real x, int y)
		{
			return new Real(mulsr(y, x.Address));
		}
		
		public static Real operator *(int x, Real y)
		{
			return new Real(mulsr(x, y.Address));
		}
		
		public static Real operator *(Real x, uint y)
		{
			return new Real(mulur(y, x.Address));
		}
		
		public static Real operator *(uint x, Real y)
		{
			return new Real(mulur(x, y.Address));
		}
		
		public static Real operator /(Real x, Real y)
		{
			return new Real(divrr(x.Address, y.Address));
		}
		
		public static Real operator /(Real x, PariInteger y)
		{
			return new Real(divri(x.Address, y.Address));
		}
		
		public static Real operator /(PariInteger x, Real y)
		{
			return new Real(divir(x.Address, y.Address));
		}
		
		public static Real operator /(Real x, int y)
		{
			return new Real(divrs(x.Address, y));
		}
		
		public static Real operator /(int x, Real y)
		{
			return new Real(divsr(x, y.Address));
		}
		
		public static Real operator /(Real x, uint y)
		{
			return new Real(divru(x.Address, y));
		}
		
		public static Real operator /(uint x, Real y)
		{
			return new Real(divur(x, y.Address));
		}
		
		public static Real operator <<(Real x, int amt)
		{
			return new Real(shiftr(x.Address, amt));
		}
		
		public static Real operator >>(Real x, int amt)
		{
			return new Real(shiftr(x.Address, -amt));
		}
		
		public static bool operator ==(Real x, Real y)
		{
			if (object.ReferenceEquals(x, y))
				return true;
			if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
				return false;
			
			return equalrr(x.Address, y.Address);
		}
		
		public static bool operator ==(Real x, int y)
		{
			if (object.ReferenceEquals(x, null))
				return false;
			
			return equalsr(y, x.Address);
		}
		
		public static bool operator ==(int x, Real y)
		{
			if (object.ReferenceEquals(y, null))
				return false;
			
			return equalsr(x, y.Address);
		}
		
		public static bool operator !=(Real x, Real y)
		{
			if (object.ReferenceEquals(x, y))
				return false;
			if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
				return true;
			
			return !equalrr(x.Address, y.Address);
		}
		
		public static bool operator !=(Real x, int y)
		{
			if (object.ReferenceEquals(x, null))
				return true;
			
			return !equalsr(y, x.Address);
		}
		
		public static bool operator !=(int x, Real y)
		{
			if (object.ReferenceEquals(y, null))
				return true;
			
			return !equalsr(x, y.Address);
		}
		
		public static bool operator <(Real x, Real y)
		{
			if (x.Address == y.Address)
				return false;
			
			return cmprr(x.Address, y.Address) < 0;
		}
		
		public static bool operator <(Real x, int y)
		{
			return cmpsr(y, x.Address) > 0;
		}
		
		public static bool operator <(int x, Real y)
		{
			return cmpsr(x, y.Address) < 0;
		}
		
		public static bool operator <=(Real x, Real y)
		{
			if (x.Address == y.Address)
				return true;
			
			return cmprr(x.Address, y.Address) <= 0;
		}
		
		public static bool operator <=(Real x, int y)
		{
			return cmpsr(y, x.Address) >= 0;
		}
		
		public static bool operator <=(int x, Real y)
		{
			return cmpsr(x, y.Address) <= 0;
		}
		
		public static bool operator >(Real x, Real y)
		{
			if (x.Address == y.Address)
				return false;
			
			return cmprr(x.Address, y.Address) > 0;
		}
		
		public static bool operator >(Real x, int y)
		{
			return cmpsr(y, x.Address) < 0;
		}
		
		public static bool operator >(int x, Real y)
		{
			return cmpsr(x, y.Address) > 0;
		}
		
		public static bool operator >=(Real x, Real y)
		{
			if (x.Address == y.Address)
				return true;
			
			return cmprr(x.Address, y.Address) >= 0;
		}
		
		public static bool operator >=(Real x, int y)
		{
			return cmpsr(y, x.Address) <= 0;
		}
		
		public static bool operator >=(int x, Real y)
		{
			return cmpsr(x, y.Address) >= 0;
		}
		
		public static Real operator -(Real x)
		{
			return new Real(negr(x.Address));
		}
		#endregion
		
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
				throw new ArgumentOutOfRangeException("prec", prec, precOutOfRangeMsg);
			
			return new Real(mpfactr(x, prec));
		}
		
		public static Real MakeOne(int prec = 3, bool neg = false)
		{
			if (prec < 3)
				throw new ArgumentOutOfRangeException("prec", prec, precOutOfRangeMsg);
			
			return new Real(neg ? real_m1(prec) : real_1(prec));
		}
		
		public static Real MakeZero(int prec = 3)
		{
			if (prec < 3)
				throw new ArgumentOutOfRangeException("prec", prec, precOutOfRangeMsg);
			
			return new Real(real_0(prec));
		}
		
		public static Real Pow2(int n, int prec, bool neg = false)
		{
			if (n < 0)
				throw new ArgumentOutOfRangeException("n", n, "n must be positive");
			
			if (prec < 3)
				throw new ArgumentOutOfRangeException("prec", prec, precOutOfRangeMsg);
			
			return new Real(neg ? real_m2n(n, prec) : real2n(n, prec));
		}
		
		#region Header
		/// <summary>
		/// Creates a deep copy of the <c>Real</c>.
		/// </summary>
		/// <returns>The deep copy.</returns>
		#endregion
		public Real Copy()
		{
			return new Real(rcopy(Address));
		}
		
		#region Header
		/// <summary>
		/// Gets the absolute value of the real.
		/// </summary>
		/// <returns>The absolute value.</returns>
		#endregion
		public Real Abs()
		{
			return new Real(absr(Address));
		}
		
		public PariInteger Ceil()
		{
			return new PariInteger(ceil_safe(Address));
		}
		
		public PariInteger Floor()
		{
			return new PariInteger(floor_safe(Address));
		}
		
		public PariInteger Round()
		{
			return new PariInteger(roundr_safe(Address));
		}
		
		public PariInteger Trunc()
		{
			return new PariInteger(trunc_safe(Address));
		}
		
		#region External PARI functions
		#region Arithmetic functions
		[DllImport(GP.DllName)]
		private static extern IntPtr absr(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr addir(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr addrr(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr addsr(int x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr addur(uint x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr divir(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr divrr(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr divri(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr divrs(IntPtr x, int y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr divru(IntPtr x, uint y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr divsr(int x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr divur(uint x, IntPtr y);
		
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
		
		[DllImport(GP.DllName)]
		private static extern IntPtr mulir(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr mulrr(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr mulsr(int x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr mulur(uint x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr negr(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr real2n(int n, int prec);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr real_m2n(int n, int prec);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr sqrr(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr subir(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr subrr(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr subri(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr subrs(IntPtr x, int y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr subru(IntPtr x, uint y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr subsr(int x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr subur(uint x, IntPtr y);
		#endregion
		
		#region Logical functions
		[DllImport(GP.DllName)]
		private static extern IntPtr mantissa2nr(IntPtr x, int n);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr shiftr(IntPtr x, int n);
		
		[DllImport(GP.DllName)]
		private static extern void shiftr_inplace(IntPtr x, int n);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr trunc2nr(IntPtr x, int n);
		#endregion
		
		#region Comparison functions
		[DllImport(GP.DllName)]
		private static extern bool absrnz_equal2n(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern bool absrnz_equal1(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern int absr_cmp(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern int cmprr(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern int cmpru(IntPtr x, uint y);
		
		[DllImport(GP.DllName)]
		private static extern int cmpsr(int x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern bool equalrr(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern bool equalsr(int x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern bool equalur(uint x, IntPtr y);
		#endregion
		
		#region Creation functions
		[DllImport(GP.DllName)]
		private static extern IntPtr rcopy(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr real_1(int prec);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr real_m1(int prec);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr real_0(int prec);
		#endregion
		
		#region Conversion functions
		[DllImport(GP.DllName)]
		private static extern IntPtr itor(IntPtr x, int prec);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr stor(int s, int prec);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr strtor(string s, int prec);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr utor(uint s, int prec);
		
		#region Rounding functions
		[DllImport(GP.DllName)]
		private static extern IntPtr ceil_safe(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr floor_safe(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr roundr_safe(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr trunc_safe(IntPtr x);
		#endregion
		#endregion
		#endregion
		
		#region Constructors
		#region Header
		/// <summary>
		/// Creates a <c>Real</c> from an <c>int</c>.
		/// </summary>
		/// <param name="num">The value to be represented by the new <c>Real</c>.</param>
		/// <param name="prec">
		/// The precision of the new <c>Real</c>.  The minimum value is 3, but no exception will be thrown if a lower
		/// value is passed.  Any lower value will be silently treated as if it were 3.
		/// </param>
		#endregion
		public Real(int num, int prec): base(stor(num, prec >= 3 ? prec : 3)) {}
		
		#region Header
		/// <summary>
		/// Creates a <c>Real</c> from a <c>uint</c>.
		/// </summary>
		/// <param name="num">The value to be represented by the new <c>Real</c>.</param>
		/// <param name="prec">
		/// The precision of the new <c>Real</c>.  The minimum value is 3, but no exception will be thrown if a lower
		/// value is passed.  Any lower value will be silently treated as if it were 3.
		/// </param>
		#endregion
		public Real(uint num, int prec): base(utor(num, prec >= 3 ? prec : 3)) {}
		
		#region Header
		/// <summary>
		/// Creates a <c>Real</c> to wrap an existing t_REAL.
		/// </summary>
		/// <param name="address">The t_REAL to wrap.</param>
		/// <remarks>
		/// No type checking is done; it is assumed that callers will ensure that only t_REALs are passed as arguments.
		/// </remarks>
		#endregion
		internal Real(IntPtr address): base(address) {}
		#endregion
	}
}
