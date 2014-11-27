#region Using directives
using System;
using System.IO;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public class PariObject
	{
		protected const uint TypeMask = (uint)((1 << 7) - 1);
		protected const uint LengthMask = (uint)(~((uint)(1 << 25) - 1));
		
		internal readonly IntPtr Address;
		
		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return 0x7FFFFFFF & hash_GEN(Address);
		}
		
		#region Header
		/// <summary>
		/// Gets a value indicating whether the current <c>PariObject</c> is on the PARI stack.
		/// </summary>
		/// <value>
		/// <c>true</c> if the current <c>PariObject</c> is on the PARI stack; <c>false</c> otherwise.
		/// </value>
		#endregion
		public bool IsOnStack
		{
			get { return isonstack(Address); }
		}
		
		#region Header
		/// <summary>
		/// Gets the length of the current <c>PariObject</c> in 32-bit words, excluding codewords and without
		/// recursion.
		/// </summary>
		/// <value>The length of the current <c>PariObject</c>.</value>
		/// <remarks>
		/// The semantics of this property vary according to type.  For a <see cref="PariInteger"/>, for
		/// example, it is the number of words used to represent the integer (excluding leading zeroes), and
		/// for a <see cref="Vector"/>, it is the number of elements in the vector.
		/// </remarks>
		#endregion
		public int Length
		{
			get { return glength(Address); }
		}
		
		#region Header
		/// <summary>
		/// Gets the number of 32-bit words allocated to the current <c>PariObject</c>, including codewords and
		/// leading zeroes, and without recursion.
		/// </summary>
		/// <value>The size of the current <c>PariObject</c>, in 32-bit words.</value>
		#endregion
		public int Size
		{
			get
			{
				unsafe
				{
					return (int)(*((uint*)Address.ToPointer()) & LengthMask);
				}
			}
		}
		
		#region Header
		//TODO: Rewrite to support GMP.
		/// <summary>
		/// Gets the specific data type that this <c>PariObject</c> represents.
		/// </summary>
		/// <value>A <see cref="PariType"/> value indicating the data type.</value>
		/// <remarks>
		/// If the implementation of this property is not overridden by a derived class, the default implementation
		/// queries the unmanaged data, making it more reliable than the <c>is</c> operator.
		/// <para/>
		/// This property is not guaranteed to function correctly if PARI is compiled with the GMP.
		/// </remarks>
		#endregion
		public virtual PariType Type
		{
			get
			{
				unsafe
				{
					return (PariType)(*((uint*)Address.ToPointer()) & TypeMask);
				}
			}
		}
		
		protected uint GetElement(int index)
		{
			unsafe
			{
				return *((uint*)(Address + (index << 2)).ToPointer());
			}
		}
		
		protected int GetElementSigned(int index)
		{
			unsafe
			{
				return *((int*)(Address + (index << 2)).ToPointer());
			}
		}
		
		/// <inheritdoc/>
		public override string ToString()
		{
			IntPtr cStr = GENtostr(Address);
			string str = Marshal.PtrToStringAnsi(cStr);
			
			Marshal.FreeCoTaskMem(cStr);
			return str;
		}
		
		public string ToTexString()
		{
			IntPtr cStr = GENtoTeXstr(Address);
			string str = Marshal.PtrToStringAnsi(cStr);
			
			Marshal.FreeCoTaskMem(cStr);
			return str;
		}
		
		public void WriteBinary(string fileName)
		{
			using (FileStream stream = new FileInfo(fileName).Open(FileMode.Append))
			{
				writebin(stream.SafeFileHandle.DangerousGetHandle(), Address);
			}
			return;
		}
		
		public void WriteBinary(FileInfo file)
		{
			using (FileStream stream = file.Open(FileMode.Append))
			{
				writebin(stream.SafeFileHandle.DangerousGetHandle(), Address);
			}
			return;
		}
		
		public void WriteBinary(FileStream stream)
		{
			if (stream == null)
				throw new ArgumentNullException("stream");
			
			if (stream.SafeFileHandle.IsClosed)
				throw new ArgumentException("Method was called on a closed stream.", "stream");
			
			writebin(stream.SafeFileHandle.DangerousGetHandle(), Address);
			return;
		}
		
		#region External PARI functions
		//Do not import gadd, gmul, etc., as they are not strictly type safe.
		
		[DllImport(GP.DllName)]
		protected static extern IntPtr compo(IntPtr x, int n);
		
		[DllImport(GP.DllName)]
		protected static extern int glength(IntPtr x);
		
		[DllImport(GP.DllName)]
		protected static extern int hash_GEN(IntPtr x);
		
		[DllImport(GP.DllName)]
		protected static extern bool isonstack(IntPtr x);
		
		#region I/O functions
		[DllImport(GP.DllName, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true)]
		protected static extern void writebin(string filename, IntPtr x);
		
		[DllImport(GP.DllName)]
		protected static extern void writebin(IntPtr file, IntPtr x);
		#endregion
		
		#region Generic arithmetic functions
		[DllImport(GP.DllName)]
		protected static extern IntPtr gdiventres(IntPtr x, IntPtr y);
		
		[DllImport(GP.DllName)]
		protected static extern bool gdvd(IntPtr x, IntPtr y);
		#endregion
		
		#region Generic conversion funtions
		[DllImport(GP.DllName)]
		protected static extern IntPtr GENtostr(IntPtr x);
		
		[DllImport(GP.DllName)]
		protected static extern IntPtr GENtoTeXstr(IntPtr x);
		
		[DllImport(GP.DllName)]
		protected static extern double gtodouble(IntPtr x); //TODO: Investigate overflow detection.
		
		[DllImport(GP.DllName)]
		protected static extern int gtolong(IntPtr x);
		#endregion
		#endregion
		
		internal PariObject(IntPtr address)
		{
			Address = address;
		}
		
		internal protected PariObject() {}
	}
}
