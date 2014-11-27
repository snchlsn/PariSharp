#region Using directives
using System;
using System.Numerics;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	#region Header
	//TODO: Add support for longs and ulongs.
	/// <summary>
	/// Represents an arbitrarily large integer.
	/// </summary>
	/// <remarks>
	/// This class is a wrapper for PARI's t_INT type and related functions.
	/// <para/>
	/// All public methods that return <see cref="PariObject"/>s (including constructors and operators)
	/// clutter GP's stack unless otherwise noted (that is, the value returned is a new object in unmanaged memory).
	/// Some methods that normally clutter the stack may instead return a constant value not on the stack if the
	/// result is between -2 and 2, inclusive. All methods that return primitive types are guaranteed not to clutter
	/// the stack, as are all property getters, regardless of type.
	/// </remarks>
	#endregion
	public class PariInteger: PariObject, IEquatable<PariInteger>, IComparable<PariInteger>, IConvertible
	{
		private const byte signShift = 30;
		
		#region Constants
		private static PariInteger zero, one, negOne, two, negTwo;
		
		public static PariInteger NegOne
		{
			get { return negOne; }
		}
		
		public static PariInteger NegTwo
		{
			get { return negTwo; }
		}
		
		public static PariInteger One
		{
			get { return one; }
		}
		
		public static PariInteger Two
		{
			get { return two; }
		}
		
		public static PariInteger Zero
		{
			get { return zero; }
		}
		#endregion
		
		/// <inheritdoc/>
		public override PariType Type
		{
			get { return PariType.Integer; }
		}
		
		public bool IsOdd
		{
			get { return mpodd(Address); }
		}
		
		public sbyte Sign
		{
			get { return (sbyte)(GetElementSigned(1) >> signShift); }
		}
		
		#region IComparable implementation
		#region Header
		/// <inheritdoc/>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="other"/> is <c>null</c>.
		/// </exception>
		#endregion
		public int CompareTo(PariInteger other)
		{
			if (other == null)
				throw new ArgumentNullException("other");
			
			return cmpii(Address, other.Address);
		}
		#endregion
		
		#region IConvertible implementation
		/// <inheritdoc/>
		public TypeCode GetTypeCode()
		{
			return TypeCode.Object;
		}
		
		#region Header
		/// <inheritdoc/>
		/// <remarks>
		/// This method depends on <see cref="PariObject.Length"/>, and consequently is not guaranteed to be
		/// compatible with the GMP.
		/// </remarks>
		#endregion
		public bool ToBoolean(IFormatProvider format = null)
		{
			return Length != 0;
		}
		
		#region Header
		/// <inheritdoc/>
		/// <exception cref="System.OverflowException">
		/// The <c>PariInteger</c> is too large to be represented as a <c>char</c>.
		/// </exception>
		#endregion
		public char ToChar(IFormatProvider format = null)
		{
			return (char)ToInt32(format);
		}
		
		#region Header
		/// <inheritdoc/>
		/// <exception cref="System.OverflowException">
		/// The <c>PariInteger</c> is too large to be represented as an <c>sbyte</c>.
		/// </exception>
		#endregion
		public sbyte ToSByte(IFormatProvider format = null)
		{
			return (sbyte)ToInt32(format);
		}
		
		#region Header
		/// <inheritdoc/>
		/// <exception cref="System.OverflowException">
		/// The <c>PariInteger</c> is too large to be represented as a <c>short</c>.
		/// </exception>
		#endregion
		public short ToInt16(IFormatProvider format = null)
		{
			return (short)ToInt32(format);
		}
		
		#region Header
		/// <inheritdoc/>
		/// <exception cref="System.OverflowException">
		/// The <c>PariInteger</c> is too large to be represented as an <c>int</c>.
		/// </exception>
		#endregion
		public int ToInt32(IFormatProvider format = null)
		{
			int result = itos_or_0(Address);
			
			if (result == 0 && Length != 0)
				throw new OverflowException();
			
			return result;
		}
		
		public long ToInt64(IFormatProvider format = null)
		{
			throw new NotImplementedException(); //TODO: Implement.
		}
		
		#region Header
		/// <inheritdoc/>
		/// <exception cref="System.OverflowException">
		/// The <c>PariInteger</c> is negative or too large to be represented as a <c>byte</c>.
		/// </exception>
		#endregion
		public byte ToByte(IFormatProvider format = null)
		{
			return (byte)ToUInt32(format);
		}
		
		#region Header
		/// <inheritdoc/>
		/// <exception cref="System.OverflowException">
		/// The <c>PariInteger</c> is negative or too large to be represented as a <c>ushort</c>.
		/// </exception>
		#endregion
		public ushort ToUInt16(IFormatProvider format = null)
		{
			return (ushort)ToUInt32(format);
		}
		
		#region Header
		/// <inheritdoc/>
		/// <exception cref="System.OverflowException">
		/// The <c>PariInteger</c> is negative or too large to be represented as a <c>uint</c>.
		/// </exception>
		#endregion
		public uint ToUInt32(IFormatProvider format = null)
		{
			uint result = itou_or_0(Address);
			
			if (result == 0 && Length != 0)
				throw new OverflowException();
			
			return result;
		}
		
		public ulong ToUInt64(IFormatProvider format = null)
		{
			if (Sign < 0)
				throw new OverflowException();
			
			switch (Length)
			{
				case 0:
					return 0;
				
				case 1:
					return GetElement(2);
				
				case 2:
					return ((ulong)GetElement(2) << 32) | GetElement(3);
				default:
					throw new OverflowException();
			}
		}
		
		public float ToSingle(IFormatProvider format = null)
		{
			return (float)gtodouble(Address); //TODO: Add overflow detection, if possible.
		}
		
		public double ToDouble(IFormatProvider format = null)
		{
			return gtodouble(Address); //TODO: Add overflow detection, if possible.
		}
		
		public decimal ToDecimal(IFormatProvider format = null)
		{
			switch (Length)
			{
				case 0:
					return decimal.Zero;
				
				case 1:
					return new Decimal(GetElementSigned(2), 0, 0, Sign < 0, 0);
				
				case 2:
					return new Decimal(GetElementSigned(3), GetElementSigned(2), 0, Sign < 0, 0);
				
				case 3:
					return new Decimal(GetElementSigned(4), GetElementSigned(3), GetElementSigned(2), Sign < 0, 0);
				
				default:
					throw new OverflowException();
			}
		}
		
		public DateTime ToDateTime(IFormatProvider format = null)
		{
			return new DateTime(ToInt64(format));
		}
		
		public string ToString(IFormatProvider format)
		{
			return ToString();
		}
		
		public object ToType(Type type, IFormatProvider format = null)
		{
			throw new NotImplementedException(); //TODO: Implement.
		}
		#endregion
		
		#region IEquatable implementation
		/// <inheritdoc/>
		public bool Equals(PariInteger other)
		{
			if (other == null)
				return false;
			
			return equalii(Address, other.Address);
		}
		
		/// <inheritdoc/>
		public override bool Equals(object other)
		{
			if (other == null)
				return false;
			
			if (other is PariInteger)
				return equalii(Address, ((PariObject)other).Address);
			
			if (other is PariObject && ((PariObject)other).Type == PariType.Integer)
				return equalii(Address, ((PariObject)other).Address);
			
			return false;
		}
		#endregion
		
		#region Operators
		//Do not attempt to implement increment and decrement operators; results would not be as expected.
		//Also do not wrap comparisons to uints as operators; they compare absolute values, which should be made clear in client code.
		
		public static PariInteger operator +(PariInteger x, PariInteger y)
		{
			return new PariInteger(addii(x.Address, y.Address));
		}
		
		public static PariInteger operator +(PariInteger x, int y)
		{
			return new PariInteger(addsi(y, x.Address));
		}
		
		public static PariInteger operator +(int x, PariInteger y)
		{
			return new PariInteger(addsi(x, y.Address));
		}
		
		public static PariInteger operator +(PariInteger x, uint y)
		{
			return new PariInteger(addui(y, x.Address));
		}
		
		public static PariInteger operator +(uint x, PariInteger y)
		{
			return new PariInteger(addui(x, y.Address));
		}
		
		public static PariInteger operator -(PariInteger x, PariInteger y)
		{
			return new PariInteger(subii(x.Address, y.Address));
		}
		
		public static PariInteger operator -(PariInteger x, int y)
		{
			return new PariInteger(subis(x.Address, y));
		}
		
		public static PariInteger operator -(int x, PariInteger y)
		{
			return new PariInteger(subsi(x, y.Address));
		}
		
		public static PariInteger operator -(PariInteger x, uint y)
		{
			return new PariInteger(subiu(x.Address, y));
		}
		
		public static PariInteger operator -(uint x, PariInteger y)
		{
			return new PariInteger(subui(x, y.Address));
		}
		
		public static PariInteger operator *(PariInteger x, PariInteger y)
		{
			return new PariInteger(mulii(x.Address, y.Address));
		}
		
		public static PariInteger operator *(PariInteger x, int y)
		{
			return new PariInteger(mulsi(y, x.Address));
		}
		
		public static PariInteger operator *(int x, PariInteger y)
		{
			return new PariInteger(mulsi(x, y.Address));
		}
		
		public static PariInteger operator *(PariInteger x, uint y)
		{
			return new PariInteger(mului(y, x.Address));
		}
		
		public static PariInteger operator *(uint x, PariInteger y)
		{
			return new PariInteger(mului(x, y.Address));
		}
		
		public static PariInteger operator /(PariInteger x, PariInteger y)
		{
			if (y.Length == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(divii(x.Address, y.Address));
		}
		
		public static PariInteger operator /(PariInteger x, int y)
		{
			if (y == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(divis(x.Address, y));
		}
		
		public static PariInteger operator /(int x, PariInteger y)
		{
			if (y.Length == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(divsi(x, y.Address));
		}
		
		public static PariInteger operator /(PariInteger x, uint y)
		{
			if (y == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(diviu(x.Address, y));
		}
		
		public static PariInteger operator /(uint x, PariInteger y)
		{
			if (y.Length == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(divui(x, y.Address));
		}
		
		public static PariInteger operator %(PariInteger x, PariInteger y)
		{
			if (y.Length == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(remii(x.Address, y.Address));
		}
		
		public static PariInteger operator %(PariInteger x, int y)
		{
			if (y == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(remis(x.Address, y));
		}
		
		public static PariInteger operator %(int x, PariInteger y)
		{
			if (y.Length == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(remsi(x, y.Address));
		}
		
		public static PariInteger operator %(PariInteger x, uint y)
		{
			if (y == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(remiu(x.Address, y));
		}
		
		public static PariInteger operator %(uint x, PariInteger y)
		{
			if (y.Length == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(remui(x, y.Address));
		}
		
		public static PariInteger operator <<(PariInteger x, int amt)
		{
			return new PariInteger(shifti(x.Address, amt));
		}
		
		public static PariInteger operator >>(PariInteger x, int amt)
		{
			return new PariInteger(shifti(x.Address, -amt));
		}
		
		public static PariInteger operator &(PariInteger x, PariInteger y)
		{
			return new PariInteger(gbitand(x.Address, y.Address));
		}
		
		public static PariInteger operator |(PariInteger x, PariInteger y)
		{
			return new PariInteger(gbitor(x.Address, y.Address));
		}
		
		public static PariInteger operator ^(PariInteger x, PariInteger y)
		{
			return new PariInteger(gbitxor(x.Address, y.Address));
		}
		
		public static bool operator ==(PariInteger x, PariInteger y)
		{
			return equalii(x.Address, y.Address);
		}
		
		public static bool operator ==(PariInteger x, int y)
		{
			return equalsi(y, x.Address);
		}
		
		public static bool operator ==(int x, PariInteger y)
		{
			return equalsi(x, y.Address);
		}
		
		public static bool operator !=(PariInteger x, PariInteger y)
		{
			return equalii(x.Address, y.Address);
		}
		
		public static bool operator !=(PariInteger x, int y)
		{
			return equalsi(y, x.Address);
		}
		
		public static bool operator !=(int x, PariInteger y)
		{
			return equalsi(x, y.Address);
		}
		
		public static bool operator >(PariInteger x, PariInteger y)
		{
			return cmpii(x.Address, y.Address) > 0;
		}
		
		public static bool operator >(PariInteger x, int y)
		{
			return cmpsi(y, x.Address) < 0;
		}
		
		public static bool operator >(int x, PariInteger y)
		{
			return cmpsi(x, y.Address) > 0;
		}
		
		public static bool operator <(PariInteger x, PariInteger y)
		{
			return cmpii(x.Address, y.Address) < 0;
		}
		
		public static bool operator <(PariInteger x, int y)
		{
			return cmpsi(y, x.Address) > 0;
		}
		
		public static bool operator <(int x, PariInteger y)
		{
			return cmpsi(x, y.Address) < 0;
		}
		
		public static bool operator >=(PariInteger x, PariInteger y)
		{
			return cmpii(x.Address, y.Address) >= 0;
		}
		
		public static bool operator >=(PariInteger x, int y)
		{
			return cmpsi(y, x.Address) <= 0;
		}
		
		public static bool operator >=(int x, PariInteger y)
		{
			return cmpsi(x, y.Address) >= 0;
		}
		
		public static bool operator <=(PariInteger x, PariInteger y)
		{
			return cmpii(x.Address, y.Address) <= 0;
		}
		
		public static bool operator <=(PariInteger x, int y)
		{
			return cmpsi(y, x.Address) >= 0;
		}
		
		public static bool operator <=(int x, PariInteger y)
		{
			return cmpsi(x, y.Address) <= 0;
		}
		
		public static PariInteger operator ~(PariInteger x)
		{
			return new PariInteger(gbitneg(x.Address, -1));
		}
		
		public static PariInteger operator -(PariInteger x)
		{
			return new PariInteger(negi(x.Address));
		}
		#endregion
		
		#region Header
		/// <summary>
		/// Computes a factorial.
		/// </summary>
		/// <param name="x">The upper bound of the factorial.</param>
		/// <returns><paramref name="x"/>!</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// <paramref name="x"/> is negative.
		/// </exception>
		#endregion
		public static PariInteger Factorial(int x)
		{
			if (x < 0)
				throw new ArgumentOutOfRangeException("x", x, "x must be positive");
			
			return new PariInteger(mpfact(x));
		}
		
		public static void InitializeConstants()
		{
			zero = new PariInteger(0);
			one = Pow2(0);
			two = new PariInteger(one.Address + 3);
			negOne = new PariInteger(two.Address + 3);
			negTwo = new PariInteger(negOne.Address + 3);
			return;
		}
		
		public static PariInteger Parse(string str)
		{
			if (str == null)
				throw new ArgumentNullException("str");
			
			//TODO: Add validity checks.
			return new PariInteger(strtoi(str));
		}
		
		#region Header
		/// <summary>
		/// Computes a power of 2.
		/// </summary>
		/// <param name="pow">The power of 2 to compute.</param>
		/// <returns>2^<paramref name="pow"/>.</returns>
		#endregion
		public static PariInteger Pow2(uint pow)
		{
			return new PariInteger(int2u(pow));
		}
		
		#region Header
		/// <summary>
		/// Performs Euclidean integer divison and returns the positive remainder.
		/// </summary>
		/// <param name="dividend">The dividend.</param>
		/// <param name="divisor">The divisor</param>
		/// <returns>
		/// The smallest positive representative of <paramref name="dividend"/> % <paramref name="divisor"/>.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="dividend"/> is <c>null</c> -or- <paramref name="divisor"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.DivideByZeroException">
		/// <paramref name="divisor"/> is 0.
		/// </exception>
		#endregion
		public static PariInteger TrueEuclideanRem(PariInteger dividend, PariInteger divisor)
		{
			if (dividend == null)
				throw new ArgumentNullException("dividend");
			
			if (divisor == null)
				throw new ArgumentNullException("divisor");
			
			if (divisor.Length == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(modii(dividend.Address, divisor.Address));
		}
		
		#region Header
		/// <summary>
		/// Performs Euclidean integer divison and returns the positive remainder.
		/// </summary>
		/// <param name="dividend">The dividend.</param>
		/// <param name="divisor">The divisor</param>
		/// <returns>
		/// The smallest positive representative of <paramref name="dividend"/> % <paramref name="divisor"/>.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="dividend"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.DivideByZeroException">
		/// <paramref name="divisor"/> is 0.
		/// </exception>
		#endregion
		public static PariInteger TrueEuclideanRem(PariInteger dividend, int divisor)
		{
			if (dividend == null)
				throw new ArgumentNullException("dividend");
			
			if (divisor == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(modis(dividend.Address, divisor));
		}
		
		#region Header
		/// <summary>
		/// Performs Euclidean integer divison and returns the positive remainder.
		/// </summary>
		/// <param name="dividend">The dividend.</param>
		/// <param name="divisor">The divisor</param>
		/// <returns>
		/// The smallest positive representative of <paramref name="dividend"/> % <paramref name="divisor"/>.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="divisor"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.DivideByZeroException">
		/// <paramref name="divisor"/> is 0.
		/// </exception>
		#endregion
		public static PariInteger TrueEuclideanRem(int dividend, PariInteger divisor)
		{
			if (divisor == null)
				throw new ArgumentNullException("divisor");
			
			if (divisor.Length == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(modsi(dividend, divisor.Address));
		}
		
		#region Header
		/// <summary>
		/// Performs Euclidean integer divison and returns the positive remainder.
		/// </summary>
		/// <param name="dividend">The dividend.</param>
		/// <param name="divisor">The divisor</param>
		/// <returns>
		/// The smallest positive representative of <paramref name="dividend"/> % <paramref name="divisor"/>.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="dividend"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.DivideByZeroException">
		/// <paramref name="divisor"/> is 0.
		/// </exception>
		#endregion
		public static PariInteger TrueEuclideanRem(PariInteger dividend, uint divisor)
		{
			if (dividend == null)
				throw new ArgumentNullException("dividend");
			
			if (divisor == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(modiu(dividend.Address, divisor));
		}
		
		#region Header
		/// <summary>
		/// Performs Euclidean integer divison and returns the positive remainder.
		/// </summary>
		/// <param name="dividend">The dividend.</param>
		/// <param name="divisor">The divisor</param>
		/// <returns>
		/// The smallest positive representative of <paramref name="dividend"/> % <paramref name="divisor"/>.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="divisor"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.DivideByZeroException">
		/// <paramref name="divisor"/> is 0.
		/// </exception>
		#endregion
		public static PariInteger TrueEuclideanRem(uint dividend, PariInteger divisor)
		{
			if (divisor == null)
				throw new ArgumentNullException("divisor");
			
			if (divisor.Length == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(modui(dividend, divisor.Address));
		}
		
		#region Header
		/// <summary>
		/// Creates a deep copy of the <c>PariInteger</c>.
		/// </summary>
		/// <returns>The deep copy.</returns>
		#endregion
		public PariInteger Copy()
		{
			return new PariInteger(icopy(Address));
		}
		
		#region Header
		/// <summary>
		/// Gets the absolute value of the integer.
		/// </summary>
		/// <returns>The absolute value.</returns>
		#endregion
		public PariInteger Abs()
		{
			return new PariInteger(absi(Address));
		}
		
		#region Absolute value comparisons
		public int AbsCompareTo(PariInteger other)
		{
			if (other == null)
				throw new ArgumentNullException("other");
			
			return absi_cmp(Address, other.Address);
		}
		
		public int AbsCompareTo(uint other)
		{
			return cmpiu(Address, other);
		}
		
		public bool AbsEquals(PariInteger other)
		{
			if (other == null)
				return false;;
			
			return absi_equal(Address, other.Address);
		}
		
		public bool AbsEquals(uint other)
		{
			return equalui(other, Address);
		}
		
		public bool AbsGreaterThan(PariInteger other)
		{
			if (other == null)
				throw new ArgumentNullException("other");
			
			return absi_cmp(Address, other.Address) > 0;
		}
		
		public bool AbsGreaterThan(uint other)
		{
			return cmpiu(Address, other) > 0;
		}
		
		public bool AbsGreaterThanOrEqual(PariInteger other)
		{
			if (other == null)
				throw new ArgumentNullException("other");
			
			return absi_cmp(Address, other.Address) >= 0;
		}
		
		public bool AbsGreaterThanOrEqual(uint other)
		{
			return cmpiu(Address, other) >= 0;
		}
		
		public bool AbsLessThan(PariInteger other)
		{
			if (other == null)
				throw new ArgumentNullException("other");
			
			return absi_cmp(Address, other.Address) < 0;
		}
		
		public bool AbsLessThan(uint other)
		{
			return cmpiu(Address, other) < 0;
		}
		
		public bool AbsLessThanOrEqual(PariInteger other)
		{
			if (other == null)
				throw new ArgumentNullException("other");
			
			return absi_cmp(Address, other.Address) <= 0;
		}
		
		public bool AbsLessThanOrEqual(uint other)
		{
			return cmpiu(Address, other) <= 0;
		}
		#endregion
		
		#region Header
		/// <summary>
		/// Gets an upper bound for the number of digits in the decimal expansion.
		/// </summary>
		/// <returns>The approximate number of decimal digits.</returns>
		/// <remarks>
		/// This method is much more efficient than counting characters after converting to a <c>string</c>,
		/// but is inexact.  The result may be off by 1.
		/// <para/>
		/// Analagous methods for bases other than 10 are not available. 
		/// </remarks>
		#endregion
		public int DigitCount()
		{
			return sizedigit(Address);
		}
		
		public Vector Digits(PariInteger b = null)
		{
			return new Vector(digits(Address, b == null ? IntPtr.Zero : b.Address));
		}
		
		#region Header
		/// <summary>
		/// Performs integer division using an optimized algorithm that assumes that the divisor is known to exactly
		/// divide the dividend.
		/// </summary>
		/// <param name="divisor">The divisor.</param>
		/// <returns>The result of dividing this integer by <paramref name="divisor"/>.</returns>
		/// <remarks>
		/// Only use this method for known exact divisors.  If that assumption does not hold, the return value
		/// is undefined and no exception is thrown.
		/// </remarks>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="divisor"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.DivideByZeroException">
		/// <paramref name="divisor"/> is 0.
		/// </exception>
		#endregion
		public PariInteger DivExact(PariInteger divisor)
		{
			if (divisor == null)
				throw new ArgumentNullException("divisor");
			
			if (divisor.Length == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(diviiexact(Address, divisor.Address));
		}
		
		#region Header
		/// <summary>
		/// Performs integer division using an optimized algorithm that assumes that the divisor is known to exactly
		/// divide the dividend.
		/// </summary>
		/// <param name="divisor">The divisor.</param>
		/// <returns>The result of dividing this integer by <paramref name="divisor"/>.</returns>
		/// <remarks>
		/// Only use this method for known exact divisors.  If that assumption does not hold, the return value
		/// is undefined and no exception is thrown.
		/// </remarks>
		/// <exception cref="System.DivideByZeroException">
		/// <paramref name="divisor"/> is 0.
		/// </exception>
		#endregion
		public PariInteger DivExact(uint divisor)
		{
			if (divisor == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(diviuexact(Address, divisor));
		}
		
		#region Header
		/// <summary>
		/// Performs integer division.  Unlike the division operator, this method rounds rational results
		/// rather than truncating.
		/// </summary>
		/// <param name="divisor">The number by which to divide.</param>
		/// <returns>The rounded quotient.</returns>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="divisor"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.DivideByZeroException">
		/// <paramref name="divisor"/> is 0.
		/// </exception>
		#endregion
		public PariInteger DivRound(PariInteger divisor)
		{
			if (divisor == null)
				throw new ArgumentNullException("divisor");
			
			if (divisor.Length == 0)
				throw new DivideByZeroException();
			
			return new PariInteger(diviiround(Address, divisor.Address));
		}
		
		public Vector Divisors()
		{
			return new Vector(divisors(Address));
		}
		
		#region Header
		/// <summary>
		/// Computes the 2-adic valuation of the integer (i.e., the number of trailing zeroes in the binary expansion).
		/// </summary>
		/// <returns>The 2-adic valuation, or -1 if the integer is 0.</returns>
		#endregion
		public int DyadicVal()
		{
			return vali(Address);
		}
		
		public Matrix Factor()
		{
			return new Matrix(factor(Address));
		}
		
		public Matrix FactorBounded(uint lim)
		{
			return new Matrix(boundfact(Address, lim));
		}
		
		#region Header
		/// <summary>
		/// Computes the Hamming weight of the integer (i.e., the number of 1s in the binary expansion).
		/// </summary>
		/// <returns>The Hamming weight.</returns>
		#endregion
		public PariInteger HammingWeight()
		{
			return new PariInteger(hammingweight(Address));
		}
		
		#region Header
		/// <summary>
		/// Gets the integer modulo a power of 2.
		/// </summary>
		/// <param name="pow">The exponent of the modulus.</param>
		/// <returns>The integer mod 2^<paramref name="pow"/>.</returns>
		/// <remarks>
		/// This method calls PARI functions that use hardcoded powers of 2. The range of hardcoded exponents
		/// is 1-6, inclusive, and 32.  For exponents outside this range, the modulo operator, bitwise and operator, or
		/// <see cref="TrueEuclideanRem"/> methods should be used instead.
		/// </remarks>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// <paramref name="pow"/> is outside the allowed range of 1-6 and 32.
		/// </exception>
		#endregion
		public uint ModPow2(byte pow)
		{
			switch (pow)
			{
				case 1:
					return mod2(Address);
				case 2:
					return mod4(Address);
				case 3:
					return mod8(Address);
				case 4:
					return mod16(Address);
				case 5:
					return mod32(Address);
				case 6:
					return mod64(Address);
				case 32:
					return mod2BIL(Address);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		
		#region Header
		/// <summary>
		/// Gets the number of distinct prime divisors of the integer.
		/// </summary>
		/// <returns>The number of prime divisors.</returns>
		#endregion
		public int Omega()
		{
			return omega(Address);
		}
		
		#region Header
		/// <summary>
		/// Computes the square of the integer.
		/// </summary>
		/// <returns>The integer squared.</returns>
		#endregion
		public PariInteger Sqr()
		{
			return new PariInteger(sqri(Address));
		}
		
		#region External PARI functions
		#region Arithmetic functions
		#region Header
		/// <summary>
		/// Gets the absolute value of a t_INT.
		/// </summary>
		/// <param name="x">A t_INT.</param>
		/// <returns>|<paramref name="x"/>|</returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern IntPtr absi(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr addii(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr addumului(uint a, uint b, IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr addsi(int x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr addui(uint x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr divii(IntPtr x, IntPtr y);
		
		#region Header
		/// <summary>
		/// Computes a quotient using an optimized algorithm that assumes that the divsor does exactly divide the
		/// dividend.
		/// </summary>
		/// <param name="x">The dividend, as a t_INT.</param>
		/// <param name="y">The divisor, as a t_INT.  Must be a known divisor of <paramref name="x"/>.</param>
		/// <returns><paramref name="x"/>/<paramref name="y"/>, as a t_INT.</returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern IntPtr diviiexact(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr diviiround(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr divis(IntPtr x, int y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr diviu(IntPtr x, uint y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr diviuexact(IntPtr x, uint y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr divsi(int x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr divui(uint x, IntPtr y);
		
		#region Header
		/// <summary>
		/// Computes the true Euclidean remainder in [0, <paramref name="divisor"/>).
		/// </summary>
		/// <param name="x">The dividend.</param>
		/// <param name="y">The divisor.</param>
		/// <returns>The remainder.</returns>
		/// <remarks>
		/// Despite the nomenclature, this function is not consistent with the behavior of C#'s modulo operator,
		/// as it will always return a posistive value.
		/// </remarks>
		#endregion
		[DllImport(GP.DllName)]
		private static extern IntPtr modii(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr modis(IntPtr x, int y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr modiu(IntPtr x, uint y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr modsi(int x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr modui(uint x, IntPtr y);
		
		#region Header
		/// <summary>
		/// Computes the factorial of an integer.
		/// </summary>
		/// <param name="x">The operand.  Must be positive.</param>
		/// <returns><paramref name="x"/>!, in t_INT form.</returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern IntPtr mpfact(int x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr mulii(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr mulsi(int x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr mului(uint x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr muluui(uint x, uint y, IntPtr z);
		
		#region Header
		/// <summary>
		/// Negates a t_INT.
		/// </summary>
		/// <param name="x">The t_INT to negate.</param>
		/// <returns>-<paramref name="x"/></returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern IntPtr negi(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr sqri(IntPtr x);
		
		#region Header
		/// <summary>
		/// Computes the remainder after integer division.
		/// </summary>
		/// <param name="x">The dividend.</param>
		/// <param name="y">The divisor.</param>
		/// <returns>The remainder.</returns>
		/// <remarks>
		/// The remainder computed by this function keeps the sign of the dividend, which is consistent with the behavior
		/// of C#'s modulo operator.
		/// </remarks>
		#endregion
		[DllImport(GP.DllName)]
		private static extern IntPtr remii(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr remis(IntPtr x, int y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr remiu(IntPtr x, uint y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr remsi(int x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr remui(uint x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr subii(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr subis(IntPtr x, int y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr subiu(IntPtr x, uint y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr subsi(int x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr subui(uint x, IntPtr y);
		
		#region Factorization functions
		[DllImport(GP.DllName)]
		private static extern IntPtr boundfact(IntPtr x, uint lim);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr divisors(IntPtr x);
		
		#region Header
		/// <summary>
		/// Checks whether one t_INT divides another.
		/// </summary>
		/// <param name="x">The dividend, as a t_INT.</param>
		/// <param name="y">The divisor, as a t_INT.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="y"/> divides <paramref name="x"/>; <c>false</c> otherwise.
		/// </returns>
		/// <remarks>
		/// This routine does still treat the case where <paramref name="y"/> is 0 as an error condition,
		/// rather than returning <c>false</c>.
		/// </remarks>
		#endregion
		[DllImport(GP.DllName)]
		private static extern bool dvdii(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern bool dvdis(IntPtr x, int y);
		
		[DllImport(GP.DllName)]
		private static extern bool dvdiu(IntPtr x, uint y);
		
		[DllImport(GP.DllName)]
		private static extern bool dvdsi(int x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern bool dvdui(uint x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr factor(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern bool istotient(IntPtr x, IntPtr n);
		
		#region Header
		/// <summary>
		/// Gets the number of distinct prime divisors of an integer.
		/// </summary>
		/// <param name="x">A t_INT.</param>
		/// <returns>The number of prime divisors of <paramref name="x"/>.</returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern int omega(IntPtr x);
		#endregion
		#endregion
		
		#region Logical functions
		[DllImport(GP.DllName)]
		private static extern IntPtr gbitand(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr gbitneg(IntPtr x, int n);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr gbitor(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr gbitxor(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr hammingweight(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr int2u(uint n);
		
		[DllImport(GP.DllName)]
		private static extern bool mpodd(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern uint mod2(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern uint mod4(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern uint mod8(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern uint mod16(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern uint mod32(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern uint mod64(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern uint mod2BIL(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr shifti(IntPtr x, int n);
		
		[DllImport(GP.DllName)]
		private static extern int vali(IntPtr x);
		#endregion
		
		#region Comparison functions
		[DllImport(GP.DllName)]
		private static extern int absi_cmp(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern bool absi_equal(IntPtr x, IntPtr y);
		
		#region Header
		/// <summary>
		/// Compares the values of two t_INTs.
		/// </summary>
		/// <param name="x">A t_INT.</param>
		/// <param name="y">A t_INT.</param>
		/// <returns>The sign of <paramref name="x"/> - <paramref name="y"/>.</returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern int cmpii(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern int cmpiu(IntPtr x, uint y);
		
		[DllImport(GP.DllName)]
		private static extern int cmpsi(int x, IntPtr y);
		
		#region Header
		/// <summary>
		/// Checks two t_INTs for equality.
		/// </summary>
		/// <param name="x">A t_INT.</param>
		/// <param name="y">A t_INT.</param>
		/// <returns><c>true</c> if the arguments are equal; <c>false</c> otherwise.</returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern bool equalii(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern bool equalsi(int x, IntPtr y);
		
		[DllImport(GP.DllName)]
		private static extern bool equalui(uint x, IntPtr y);
		#endregion
		
		#region Assignment functions
		[DllImport(GP.DllName)]
		private static extern void affii(IntPtr x, IntPtr z);
		
		#region Header
		/// <summary>
		/// Assigns an integer value to a t_INT.
		/// </summary>
		/// <param name="s">The value to assign.</param>
		/// <param name="z">The t_INT to recieve the value.  Size must be at least 3.</param>
		#endregion
		[DllImport(GP.DllName)]
		private static extern void affsi(int s, IntPtr z);
		
		#region Header
		/// <summary>
		/// Assigns an unsigned integer value to a t_INT.
		/// </summary>
		/// <param name="u">The value to assign.</param>
		/// <param name="z">The t_INT to recieve the value.  Size must be at least 3.</param>
		#endregion
		[DllImport(GP.DllName)]
		private static extern void affui(uint u, IntPtr z);
		#endregion
		
		#region Creation functions
		[DllImport(GP.DllName)]
		private static extern IntPtr cgeti(int length);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr cgetipos(int length);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr cgetineg(int length);
		
		#region Header
		/// <summary>
		/// Creates a deep copy of a t_INT on the stack.
		/// </summary>
		/// <param name="x">A t_INT to copy.</param>
		/// <returns>A deep copy of <paramref name="x"/>.</returns>
		/// <remarks>
		/// If the original contains leading zeroes, the deep copy is resized to eliminate them.
		/// </remarks>
		#endregion
		[DllImport(GP.DllName)]
		private static extern IntPtr icopy(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr mkintn(int n, params uint[] a);
		#endregion
		
		#region Conversion functions
		[DllImport(GP.DllName)]
		private static extern IntPtr digits(IntPtr x, IntPtr b);
		
		[DllImport(GP.DllName)]
		private static extern int itos(IntPtr x);
		
		
		#region Header
		/// <summary>
		/// Attempts to convert a t_INT to an <c>int</c>.
		/// </summary>
		/// <param name="x">The t_INT to convert.</param>
		/// <returns>
		/// The value of <paramref name="x"/> as an <c>int</c>, or 0 if the value cannot be represented as an <c>int</c>.
		/// </returns>
		/// <remarks>
		/// The caller must take care to differentiate between an overflow condition and the case where the
		/// value of <paramref name="x"/> is 0, as they both result in the same return value.
		/// </remarks>
		#endregion
		[DllImport(GP.DllName)]
		private static extern int itos_or_0(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern uint itou(IntPtr x);
		
		#region Header
		/// <summary>
		/// Attempts to convert a t_INT to a <c>uint</c>.
		/// </summary>
		/// <param name="x">The t_INT to convert.</param>
		/// <returns>
		/// The value of <paramref name="x"/> as a <c>uint</c>, or 0 if the value cannot be represented as a <c>uint</c>.
		/// </returns>
		/// <remarks>
		/// The caller must take care to differentiate between an overflow condition and the case where the
		/// value of <paramref name="x"/> is 0, as they both result in the same return value.
		/// </remarks>
		#endregion
		[DllImport(GP.DllName)]
		private static extern uint itou_or_0(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern int sizedigit(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr stoi(int s);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr strtoi(string s);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr utoi(uint s);
		
		#region Header
		/// <summary>
		/// Constructs a t_INT from two 32-bit words.
		/// </summary>
		/// <param name="a">The most significant word.</param>
		/// <param name="b">The least significant word.</param>
		/// <returns>
		/// A t_INT containing <paramref name="a"/> &lt;&lt; 32 + <paramref name="b"/>.
		/// </returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern IntPtr uu32toi(uint a, uint b);
		
		#region Header
		/// <summary>
		/// Constructs a t_INT from two 32-bit words.
		/// </summary>
		/// <param name="a">The most significant word.</param>
		/// <param name="b">The least significant word.</param>
		/// <returns>
		/// A t_INT containing <paramref name="a"/> &lt;&lt; 32 + <paramref name="b"/>.
		/// </returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern IntPtr uutoi(uint a, uint b);
		
		#region Header
		/// <summary>
		/// Constructs a t_INT from two 32-bit words.
		/// </summary>
		/// <param name="a">The most significant word.</param>
		/// <param name="b">The least significant word.</param>
		/// <returns>
		/// A t_INT containing -(<paramref name="a"/> &lt;&lt; 32 + <paramref name="b"/>).
		/// </returns>
		#endregion
		[DllImport(GP.DllName)]
		private static extern IntPtr uutoineg(uint a, uint b);
		#endregion
		#endregion
		
		#region Constructors
		#region Header
		/// <summary>
		/// Creates a <c>PariInteger</c> from an <c>int</c>.
		/// </summary>
		/// <param name="num">The value to be represented by the new <c>PariInteger</c>.</param>
		#endregion
		public PariInteger(int num): base(stoi(num)) {}
		
		#region Header
		/// <summary>
		/// Creates a <c>PariInteger</c> from a <c>uint</c>.
		/// </summary>
		/// <param name="num">The value to be represented by the new <c>PariInteger</c>.</param>
		#endregion
		public PariInteger(uint num): base(utoi(num)) {}
		
		#region Header
		/// <summary>
		/// Creates a <c>PariInteger</c> from two <c>uint</c>s.
		/// </summary>
		/// <param name="upper">The most significant of the two words.</param>
		/// <param name="lower">The least significant of the two words.</param>
		/// <param name="neg">
		/// <c>true</c> if the new <c>PariInteger</c> should be negative; <c>false</c> otherwise.  The default
		/// value is <c>false</c>.
		/// </param>
		#endregion
		public PariInteger(uint upper, uint lower, bool neg = false): base(neg ? uutoineg(upper, lower) : uutoi(upper, lower)) {}
		
		public PariInteger(params uint[] a): base(mkintn(a.Length, a)) {}
		
		public PariInteger(ulong num, bool neg = false): base(neg ? uutoineg((uint)(num >> 32), (uint)(num & uint.MaxValue)) : uutoi((uint)(num >> 32), (uint)(num & uint.MaxValue))) {}
		
		#region Header
		/// <summary>
		/// Creates a <c>PariInteger</c> to wrap an existing t_INT.
		/// </summary>
		/// <param name="address">The t_INT to wrap.</param>
		/// <remarks>
		/// No type checking is done; it is assumed that callers will ensure that only t_INTs are passed as arguments.
		/// </remarks>
		#endregion
		internal PariInteger(IntPtr address): base(address) {}
		#endregion
	}
}
