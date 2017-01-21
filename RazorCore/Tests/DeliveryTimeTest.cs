using System;
using System.Linq;
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
				var deliveryTime = new DeliveryTime(DeliveryRegularity.OncePerMonth, deliveryDays);
			});
		}

		[Test]
		public void Ctor_WhenDeliveryDaysHasOneDay_ReturnsThisOneDay()
		{
			var day = 1;

			var deliveryTime = new DeliveryTime(DeliveryRegularity.OncePerMonth, day);

			Assert.AreEqual(day, deliveryTime.DeliveryDays.Single());
		}

		[Test]
		public void Ctor_WhenDeliveryDaysHasTwoDays_ReturnsTheseTwoDay()
		{
			var day = new[] { 1, 2 };

			var deliveryTime = new DeliveryTime(DeliveryRegularity.TwicePerMonth, day);

			Assert.AreEqual(day.First(), deliveryTime.DeliveryDays.First());
			Assert.AreEqual(day.Last(), deliveryTime.DeliveryDays.Last());
		}
	}
}