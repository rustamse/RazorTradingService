using System;
using System.Linq;
using NUnit.Framework;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	[TestFixture]
	public class DeliveryInfoTest
	{
		[Test]
		public void Ctor_WhenDaysIsNull_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				int[] deliveryDays = null;

				// ReSharper disable once UnusedVariable
				// ReSharper disable once ExpressionIsAlwaysNull
				var deliveryInfo = new DeliveryInfo(DeliveryRegularity.OncePerMonth, deliveryDays);
			});
		}

		[Test]
		public void Ctor_WhenDeliveryDaysHasOneDay_ReturnsThisOneDay()
		{
			var deliveryDay = 1;

			var deliveryInfo = new DeliveryInfo(DeliveryRegularity.OncePerMonth, deliveryDay);

			Assert.AreEqual(deliveryDay, deliveryInfo.DeliveryDays.Single());
		}

		[Test]
		public void Ctor_WhenDeliveryDaysHasTwoDays_ReturnsTheseTwoDay()
		{
			var deliveryDays = new[] { 1, 2 };

			var deliveryInfo = new DeliveryInfo(DeliveryRegularity.TwicePerMonth, deliveryDays);

			Assert.AreEqual(deliveryDays.First(), deliveryInfo.DeliveryDays.First());
			Assert.AreEqual(deliveryDays.Last(), deliveryInfo.DeliveryDays.Last());
		}

		[Test]
		public void IsDeliveryDay_WhenDeliveryDay1AndCheckDateMay1_ReturnsTrue()
		{
			var deliveryDay = 1;
			var checkDate = "1 may 2017".ToDate();

			var deliveryInfo = new DeliveryInfo(DeliveryRegularity.OncePerMonth, deliveryDay);

			var isDeliveryDay = deliveryInfo.IsDeliveryDay(checkDate);

			Assert.IsTrue(isDeliveryDay);
		}

		[Test]
		public void IsDeliveryDay_WhenDeliveryDay1AndCheckDateMay2_ReturnsFalse()
		{
			var deliveryDay = 1;
			var checkDate = "2 may 2017".ToDate();

			var deliveryInfo = new DeliveryInfo(DeliveryRegularity.OncePerMonth, deliveryDay);

			var isDeliveryDay = deliveryInfo.IsDeliveryDay(checkDate);

			Assert.IsFalse(isDeliveryDay);
		}

		[Test]
		public void IsDeliveryDay_WhenDeliveryDay1And2AndCheckDatesMay1And2_ReturnsTrue()
		{
			var deliveryDay = 1;
			var deliveryDay2 = 2;
			var checkDate = "1 may 2017".ToDate();
			var checkDate2 = "2 may 2017".ToDate();

			var deliveryInfo = new DeliveryInfo(DeliveryRegularity.TwicePerMonth, deliveryDay, deliveryDay2);

			var isDeliveryDay = deliveryInfo.IsDeliveryDay(checkDate);
			var isDeliveryDay2 = deliveryInfo.IsDeliveryDay(checkDate2);

			Assert.IsTrue(isDeliveryDay);
			Assert.IsTrue(isDeliveryDay2);
		}

		[Test]
		public void IsDeliveryDay_WhenDeliveryDay1And2AndCheckDatesMay3And4_ReturnsFalse()
		{
			var deliveryDay = 1;
			var deliveryDay2 = 2;
			var checkDate = "3 may 2017".ToDate();
			var checkDate2 = "4 may 2017".ToDate();

			var deliveryInfo = new DeliveryInfo(DeliveryRegularity.TwicePerMonth, deliveryDay, deliveryDay2);

			var isDeliveryDay = deliveryInfo.IsDeliveryDay(checkDate);
			var isDeliveryDay2 = deliveryInfo.IsDeliveryDay(checkDate2);

			Assert.IsFalse(isDeliveryDay);
			Assert.IsFalse(isDeliveryDay2);
		}

		[Test]
		public void IsDeliveryDay_WhenRegOncePerTwoMonthsAndDeliveryDay1AndCheckDateJan1_ReturnsTrue()
		{
			var deliveryDay = 1;
			var checkDate = "1 jan 2017".ToDate();

			var deliveryInfo = new DeliveryInfo(DeliveryRegularity.OncePerTwoMonths, deliveryDay);

			var isDeliveryDay = deliveryInfo.IsDeliveryDay(checkDate);

			Assert.IsTrue(isDeliveryDay);
		}

		[Test]
		public void IsDeliveryDay_WhenRegOncePerTwoMonthsAndDeliveryDay1AndCheckDateFeb1_ReturnsFalse()
		{
			var deliveryDay = 1;
			var checkDate = "1 feb 2017".ToDate();

			var deliveryInfo = new DeliveryInfo(DeliveryRegularity.OncePerTwoMonths, deliveryDay);

			var isDeliveryDay = deliveryInfo.IsDeliveryDay(checkDate);

			Assert.IsFalse(isDeliveryDay);
		}
	}
}