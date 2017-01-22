using System;
using NUnit.Framework;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	[TestFixture]
	public class OncePerMonthDeliveryTest
	{
		[Test]
		public void Ctor_WhenDeliveryDaysIsNull_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var deliveryInfo = new OncePerMonthDelivery(null);
			});
		}

		[Test]
		public void IsDeliveryDay_WhenDeliveryDays1AndCheckDateMay1_ReturnsTrue()
		{
			var deliveryDay = new DeliveryDay(1);
			var checkDate = "1 may 2017".ToDate();

			var deliveryInfo = new OncePerMonthDelivery(deliveryDay);

			Assert.IsTrue(deliveryInfo.IsDeliveryDay(checkDate));
		}

		[Test]
		public void IsDeliveryDay_WhenDeliveryDays1AndCheckDateMay2_ReturnsFalse()
		{
			var deliveryDay = new DeliveryDay(1);
			var checkDate = "2 may 2017".ToDate();

			var deliveryInfo = new OncePerMonthDelivery(deliveryDay);

			Assert.IsFalse(deliveryInfo.IsDeliveryDay(checkDate));
		}
	}
}