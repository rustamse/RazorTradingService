using System;
using NUnit.Framework;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	[TestFixture]
	public class DeliveryTimeTest
	{
		[Test]
		public void Ctor_WhenDaysIsNull_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				int[] deliveryDays = null;

				// ReSharper disable once UnusedVariable
				// ReSharper disable once ExpressionIsAlwaysNull
				var deliveryTime = new DeliveryTime(deliveryDays);
			});
		}


	}
}