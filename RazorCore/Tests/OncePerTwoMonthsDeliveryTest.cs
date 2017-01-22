using System;
using NUnit.Framework;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	[TestFixture]
	public class OncePerTwoMonthsDeliveryTest
	{
		[Test]
		public void Ctor_WhenDeliveryDaysIsNull_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var deliveryInfo = new OncePerTwoMonthsDelivery(null);
			});
		}

		[Test]
		public void IsDeliveryDay_WhenRegOncePerTwoMonthsAndDeliveryDay1AndCheckDateJan1_ReturnsTrue()
		{
			var deliveryDay = new DeliveryDay(1);
			var checkDate = "1 jan 2017".ToDate();

			var deliveryInfo = new OncePerTwoMonthsDelivery(deliveryDay);

			Assert.IsTrue(deliveryInfo.IsDeliveryDay(checkDate));
		}

		[Test]
		public void IsDeliveryDay_WhenRegOncePerTwoMonthsAndDeliveryDay1AndCheckDateFeb1_ReturnsFalse()
		{
			var deliveryDay = new DeliveryDay(1);
			var checkDate = "1 feb 2017".ToDate();

			var deliveryInfo = new OncePerTwoMonthsDelivery(deliveryDay);

			Assert.IsFalse(deliveryInfo.IsDeliveryDay(checkDate));
		}
	}
}