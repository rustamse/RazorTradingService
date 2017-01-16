using System;
using NUnit.Framework;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	[TestFixture]
	public class SubscriptionPlanTest
	{
		[Test]
		public void Ctor_WhenDeliveryDay1_ReturnsDeliveryDay1()
		{
			var deliveryDay = 1;
			var deliveryTime = new DeliveryTime(deliveryDay);
			var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
				DeliveryRegularity.OncePerMonth, deliveryTime);

			Assert.AreEqual(deliveryDay, subscriptionPlan.DeliveryTime.DeliveryDays[0]);
		}

		[Test]
		public void Ctor_WhenRegularityOncePerTwoMonths_ReturnsRegularityOncePerTwoMonths()
		{
			var regularity = DeliveryRegularity.OncePerTwoMonths;
			var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
				regularity, new DeliveryTime(1));

			Assert.AreEqual(regularity, subscriptionPlan.DeliveryRegularity);
		}

		[Test]
		public void Ctor_WhenSubscriptionTypeRazorAndGel_ReturnsSubscriptionTypeRazorAndGel()
		{
			var razorAndGel = SubscriptionTypes.RazorAndGel;
			var subscriptionPlan = new SubscriptionPlan(razorAndGel,
				DeliveryRegularity.OncePerMonth, new DeliveryTime(1));

			Assert.AreEqual(razorAndGel, subscriptionPlan.SubscriptionType);
		}

		[Test]
		public void Ctor_WhenRegularityOncePerMonthAndDeliveryTwicePerMonth_ThorwsArgumentOutOfRange()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
					DeliveryRegularity.OncePerMonth, new DeliveryTime(1, 5));
			});
		}

		[Test]
		public void Ctor_WhenRegularityOncePerMonthAndDeliveryIsNull_ThorwsArgumentNull()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
					DeliveryRegularity.OncePerMonth, null);
			});
		}

		[Test]
		public void Ctor_WhenRegularityOncePerMonthAndDeliveryContainsNull_ThorwsArgumentNull()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
					DeliveryRegularity.OncePerMonth, new DeliveryTime(null));
			});
		}

		[Test]
		public void Ctor_WhenRegularityOncePerMonthAndDeliveryTooSmallDayNumber_ThorwsArgumentOutOfRange()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
					DeliveryRegularity.OncePerMonth, new DeliveryTime(0));
			});
		}

		[Test]
		public void Ctor_WhenRegularityOncePerMonthAndDeliveryTooBigDayNumber_ThorwsArgumentOutOfRange()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
					DeliveryRegularity.OncePerMonth, new DeliveryTime(29));
			});
		}

		[Test]
		public void Ctor_WhenRegularityOncePerMonthAndDeliveryContainsTwoDays_ThorwsArgumentOutOfRange()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
					DeliveryRegularity.OncePerMonth, new DeliveryTime(1, 2));
			});
		}

		[Test]
		public void Ctor_WhenRegularityTwicePerMonthAndDeliveryContainsOneDay_ThorwsArgumentOutOfRange()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
					DeliveryRegularity.TwicePerMonth, new DeliveryTime(1));
			});
		}

		[Test]
		public void Ctor_WhenRegularityTwicePerMonthAndDeliveryContainsTwoDifferentDays_SuccessCreation()
		{
			// ReSharper disable once UnusedVariable
			var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
				DeliveryRegularity.TwicePerMonth, new DeliveryTime(1, 2));

			Assert.Pass();
		}

		[Test]
		public void Ctor_WhenRegularityTwicePerMonthAndDeliveryContainsTwoSameDays_ThorwsArgumentOutOfRange()
		{
			Assert.Throws<SubscriptionPlanDublicateDeliveryDay>(() =>
			{
				// ReSharper disable once UnusedVariable
				var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
					DeliveryRegularity.TwicePerMonth, new DeliveryTime(1, 1));
			});
		}

		[Test]
		public void Ctor_WhenRegularityTwicePerMonthAndDeliveryContainsThreeDays_ThorwsArgumentOutOfRange()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
					DeliveryRegularity.TwicePerMonth, new DeliveryTime(1, 2, 3));
			});
		}
	}
}
