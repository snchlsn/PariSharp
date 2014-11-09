using System;

namespace PariSharp
{
	[Flags]
	public enum FactorOptions: int
	{
		None = 0,
		AvoidMPQS = 1,
		NoFirstECM = 2,
		AvoidRhoSQUFOF = 4,
		NoFinalECM = 8
	}
}