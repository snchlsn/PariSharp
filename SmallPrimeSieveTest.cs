#if TEST
#region Using directives
using System;
using NUnit.Framework;
#endregion

namespace PariSharp
{
	[TestFixture]
	public class SmallPrimeSieveTest
	{
		[Test]
		public void TestEnumerator()
		{
			SmallPrimeSieve sieve = new SmallPrimeSieve(2, 7);
			SmallPrimeSieve.Enumerator enumerator = (SmallPrimeSieve.Enumerator)sieve.GetEnumerator();
			
			Assert.IsTrue(enumerator.MoveNext());
			Assert.AreEqual(2, enumerator.Current);
			Assert.IsTrue(enumerator.MoveNext());
			Assert.AreEqual(3, enumerator.Current);
			Assert.IsTrue(enumerator.MoveNext());
			Assert.AreEqual(5, enumerator.Current);
			Assert.IsTrue(enumerator.MoveNext());
			Assert.AreEqual(7, enumerator.Current);
			Assert.IsFalse(enumerator.MoveNext());
			Assert.Pass();
			return;
		}
		
		[Test]
		public void TestMiscMethods()
		{
			Assert.AreEqual(SmallPrimeSieve.PrimePi(uint.MaxValue), SmallPrimeSieve.PrimePi(uint.MaxValue));
			Assert.AreEqual(5, SmallPrimeSieve.NextPrime(4));
			Assert.Throws(typeof(OverflowException), () => { SmallPrimeSieve.NextPrime(uint.MaxValue); });
			Assert.AreEqual(3, SmallPrimeSieve.PrecedingPrime(4));
			Assert.Throws(typeof(ArgumentOutOfRangeException), () => { SmallPrimeSieve.PrecedingPrime(1); });
			Assert.AreEqual(5, SmallPrimeSieve.NthPrime(3));
			Assert.Throws(typeof(ArgumentOutOfRangeException), () => { SmallPrimeSieve.NthPrime(0); });
			Assert.Throws(typeof(OverflowException), () => { SmallPrimeSieve.NthPrime(int.MaxValue); });
			Assert.Pass();
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
