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

			var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
				DeliveryRegularity.OncePerMonth, new DeliveryTime(deliveryDay));

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

		[Test(Description = "Для доставки раз в месяц НЕ должно быть задано 2 дня доставки.")]
		public void Ctor_WhenRegularityOncePerMonthAndDeliveryTwicePerMonth_ThorwsArgumentOutOfRange()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				var firstDeliveryDay = 1;
				var secondDeliveryDay = 5;

				// ReSharper disable once UnusedVariable
				var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
					DeliveryRegularity.OncePerMonth, new DeliveryTime(firstDeliveryDay, secondDeliveryDay));
			});
		}

		[Test(Description = "Время доставки должно быть задано обязательно (не ноль)")]
		public void Ctor_WhenDeliveryIsNull_ThorwsArgumentNull()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				DeliveryTime nullableDeliveryTime = null;

				// ReSharper disable once UnusedVariable
				var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
					// ReSharper disable once ExpressionIsAlwaysNull
					DeliveryRegularity.OncePerMonth, nullableDeliveryTime);
			});
		}

		[Test(Description = "Дата доставки должна быть числом более 0")]
		public void Ctor_WhenDeliveryTooSmallDayNumber_ThorwsArgumentOutOfRange()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				var tooSmallDeliveryDay = 0;

				// ReSharper disable once UnusedVariable
				var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
					DeliveryRegularity.OncePerMonth, new DeliveryTime(tooSmallDeliveryDay));
			});
		}

		[Test(Description = "Дата доставки должна быть числом менее 29")]
		public void Ctor_WhenDeliveryTooBigDayNumber_ThorwsArgumentOutOfRange()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				var tooBigDeliveryDay = 29;

				// ReSharper disable once UnusedVariable
				var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
					DeliveryRegularity.OncePerMonth, new DeliveryTime(tooBigDeliveryDay));
			});
		}

		[Test(Description = "Для доставки с регулярностью дважды в месяц нельзя задать только 1 день доставки")]
		public void Ctor_WhenRegularityTwicePerMonthAndDeliveryContainsOneDay_ThorwsArgumentOutOfRange()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				var singleDeliveryDay = 1;

				// ReSharper disable once UnusedVariable
				var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
					DeliveryRegularity.TwicePerMonth, new DeliveryTime(singleDeliveryDay));
			});
		}

		[Test]
		public void Ctor_WhenRegularityTwicePerMonthAndDeliveryContainsTwoDifferentDays_SuccessCreation()
		{
			var firstDeliveryDay = 1;
			var secondDeliveryDay = 2;

			// ReSharper disable once UnusedVariable
			var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
				DeliveryRegularity.TwicePerMonth, new DeliveryTime(firstDeliveryDay, secondDeliveryDay));

			Assert.Pass();
		}

		[Test(Description = "При доставке дважды в месяц нельзя чтобы оба дня доставки были одинаковыми")]
		public void Ctor_WhenRegularityTwicePerMonthAndDeliveryContainsTwoSameDays_ThorwsArgumentOutOfRange()
		{
			Assert.Throws<SubscriptionPlanDublicateDeliveryDay>(() =>
			{
				var bothDeliveryDays = 1;

				// ReSharper disable once UnusedVariable
				var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
					DeliveryRegularity.TwicePerMonth, new DeliveryTime(bothDeliveryDays, bothDeliveryDays));
			});
		}

		[Test(Description = "Когда доставляем дважды в месяц нельзя чтобы было выбрано 3 дня доставки")]
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
