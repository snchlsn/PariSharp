#if TEST
#region Using directives
using System;
using NUnit.Framework;
#endregion

namespace PariSharp
{
	[TestFixture]
	public class ArithmeticTest
	{
		[Test]
		public void TestFactoringMethods()
		{
			uint u = 0, v = 0;
			
			Assert.AreEqual(2, Arithmetic.Core(18));
			Assert.True(Arithmetic.IsSquarefree(30));
			Assert.IsInstanceOf<Vector>(Arithmetic.Factor(10));
			Assert.IsInstanceOf<Vector>(Arithmetic.FactorPow(10));
			Assert.IsInstanceOf<SmallIntVector>(Arithmetic.Divisors(10));
			Assert.AreEqual(18, Arithmetic.SumDivisors(10));
			Assert.AreEqual(1, Arithmetic.DyadicVal(30));
			Assert.AreEqual(2, Arithmetic.GCD(-14, 30));
			Assert.AreEqual(2, Arithmetic.GCD((uint)14, (uint)30));
			Assert.AreEqual(2, Arithmetic.GCDExtended(14, 30, out u, out v));
			Assert.AreNotEqual(0, u);
			Assert.AreNotEqual(0, v);
			Assert.AreEqual(20, Arithmetic.LCM(4, 10));
			Assert.AreEqual(6, Arithmetic.Totient(7));
			Assert.Pass();
			return;
		}
		
		[Test]
		public void TestMiscMethods()
		{
			Assert.AreEqual(-1, Arithmetic.Moebius(2));
			Assert.AreEqual(6, Arithmetic.DigitalSum(123));
			Assert.Pass();
			return;
		}
		
		[Test]
		public void TestHelperMethods()
		{
			Assert.AreEqual(3, Arithmetic.FactorialDyadicVal(5));
			Assert.AreEqual(4, Arithmetic.FactorialPrimeVal(11, 3));
			Assert.AreEqual(6, Arithmetic.DigitalRoot(12345));
			Assert.Pass();
			return;
		}
		
		[TestFixtureSetUp]
		public void Init()
		{
			GP.Initialize();
			return;
		}
		
		[TestFixtureTearDown]
		public void Dispose()
		{
			GP.ClearStack();
			return;
		}
	}
}
#endif
