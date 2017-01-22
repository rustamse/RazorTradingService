using System;
using System.Linq;
using NUnit.Framework;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	[TestFixture]
	public class TwicePerMonthDeliveryTest
	{
		[Test]
		public void Ctor_WhenFirstDaysIsNull_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var deliveryInfo = new TwicePerMonthDelivery(null, new DeliveryDay(1));
			});
		}

		[Test]
		public void Ctor_WhenSecondDaysIsNull_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var deliveryInfo = new TwicePerMonthDelivery(new DeliveryDay(1), null);
			});
		}

		[Test]
		public void Ctor_WhenBothDaysIsNull_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var deliveryInfo = new TwicePerMonthDelivery(null, null);
			});
		}

		[Test]
		public void IsDeliveryDay_WhenDeliveryDays1And2AndCheckDatesMay1And2_ReturnsTrue()
		{
			var deliveryDay = new DeliveryDay(1);
			var deliveryDay2 = new DeliveryDay(2);
			var checkDate = "1 may 2017".ToDate();
			var checkDate2 = "2 may 2017".ToDate();

			var deliveryInfo = new TwicePerMonthDelivery(deliveryDay, deliveryDay2);

			var isDeliveryDay = deliveryInfo.IsDeliveryDay(checkDate);
			var isDeliveryDay2 = deliveryInfo.IsDeliveryDay(checkDate2);

			Assert.IsTrue(isDeliveryDay);
			Assert.IsTrue(isDeliveryDay2);
		}

		[Test]
		public void IsDeliveryDay_WhenDeliveryDays1And2AndCheckDatesMay3And4_ReturnsFalse()
		{
			var deliveryDay = new DeliveryDay(1);
			var deliveryDay2 = new DeliveryDay(2);
			var checkDate = "3 may 2017".ToDate();
			var checkDate2 = "4 may 2017".ToDate();

			var deliveryInfo = new TwicePerMonthDelivery(deliveryDay, deliveryDay2);

			var isDeliveryDay = deliveryInfo.IsDeliveryDay(checkDate);
			var isDeliveryDay2 = deliveryInfo.IsDeliveryDay(checkDate2);

			Assert.IsFalse(isDeliveryDay);
			Assert.IsFalse(isDeliveryDay2);
		}
	}
}