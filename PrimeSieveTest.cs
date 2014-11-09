#if TEST
using System;
using NUnit.Framework;

namespace PariSharp
{
	[TestFixture]
	public class PrimeSieveTest
	{
		[Test]
		public void TestLowPrimes()
		{
			PrimeSieve sieve = new PrimeSieve(2, 7);
			PrimeSieve.Enumerator enumerator = (PrimeSieve.Enumerator)sieve.GetEnumerator();
			
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
		
		[TestFixtureSetUp]
		public void Init()
		{
			GP.Initialize(500000, 11);
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