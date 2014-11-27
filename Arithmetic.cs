#region Using directives
using System;
using System.Runtime.InteropServices;
#endregion

namespace PariSharp
{
	public static class Arithmetic
	{
		public static byte DigitalRoot(uint n)
		{
			while (n > 9)
				n = DigitalSum(n);
			
			return (byte)n;
		}
		
		#region External PARI functions
		[DllImport(GP.DllName, EntryPoint = "core")]
		public static extern uint Core(uint n);
		
		[DllImport(GP.DllName, EntryPoint = "sumdigitsu")]
		public static extern uint DigitalSum(uint n);
		
		[DllImport(GP.DllName, EntryPoint = "vals")]
		public static extern int DyadicVal(int n);
		
		[DllImport(GP.DllName, EntryPoint = "factorial_lval")]
		public static extern int FactorialValuation(uint n, uint p);
		
		[DllImport(GP.DllName, EntryPoint = "uissquarefree")]
		public static extern bool IsSquareFree(uint n);
		
		[DllImport(GP.DllName, EntryPoint = "moebiusu")]
		public static extern int Moebius(uint n);
		
		[DllImport(GP.DllName, EntryPoint = "eulerphiu")]
		public static extern uint Totient(uint n);
		#endregion
	}
}
