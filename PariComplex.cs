#region Using directives
using System;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public sealed class PariComplex: PariObject
	{
		#region Header
		/// <summary>
		/// Gets the imaginary component of the complex number.
		/// </summary>
		/// <value>
		/// The imaginary component, which may be a <see cref="Real"/>, <see cref="PariInteger"/>, or
		/// <see cref="Fraction"/>.
		/// </value>
		#endregion
		public PariObject Imaginary
		{
			get { return new PariObject(imag_i(Address)); }
		}
		
		#region Header
		/// <summary>
		/// Gets the real component of the complex number.
		/// </summary>
		/// <value>
		/// The real component, which may be a <see cref="Real"/>, <see cref="PariInteger"/>, or
		/// <see cref="Fraction"/>.
		/// </value>
		#endregion
		public PariObject Real
		{
			get { return new PariObject(real_i(Address)); }
		}
		
		/// <inheritdoc/>
		public override PariType Type
		{
			get { return PariType.Complex; }
		}
		
		public PariComplex Conjugate()
		{
			return new PariComplex(conj(Address));
		}
		
		#region External PARI functions
		[DllImport(GP.DllName)]
		private static extern IntPtr conj(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr imag(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr imag_i(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr real(IntPtr x);
		
		[DllImport(GP.DllName)]
		private static extern IntPtr real_i(IntPtr x);
		#endregion
		
		internal PariComplex(IntPtr address): base(address) {}
	}
}
