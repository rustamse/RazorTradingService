using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RazorCore.Cash;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	[TestFixture]
	public class CashCalculatorTest
	{
		[Test]
		public void CalculateTotalCash_WhenEmptyHistory_Returns0()
		{
			var history = new Mock<ICashIntervalsProvider>();
			var priceList = new Mock<IPriceList>();
			var cashCalculator = new CashCalculator(history.Object, priceList.Object);

			var resultCash = cashCalculator.CalculateTotalCash();

			Assert.AreEqual(0, resultCash);
		}

		[Test]
		public void CalculateTotalCash_WhenHistoryHasOneDelivery_ReturnsCostOfOneDelivery()
		{
			var cashCalculator = InitCashCalculatorWhenHistoryHasOneDelivery();

			var resultCash = cashCalculator.CalculateTotalCash();

			Assert.AreEqual(1, resultCash);
		}

		private static CashCalculator InitCashCalculatorWhenHistoryHasOneDelivery()
		{
			var history = new Mock<ICashIntervalsProvider>();
			var priceList = new Mock<IPriceList>();
			history.Setup(subscriptionHistory => subscriptionHistory.GetIntervals())
				.Returns(() =>
				{
					var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
						DeliveryRegularity.OncePerMonth, new DeliveryTime(10));
					var cashIntervals = new List<CashInterval>
					{
						new CashInterval(subscriptionPlan, Helper.GenerateSubscrDate("1 jan 2017"),
							Helper.GenerateSubscrDate("1 feb 2017"))
					};
					return cashIntervals;
				});
			priceList.Setup(list => list.GetPrice(It.IsAny<SubscriptionTypes>()))
				.Returns(1);
			var cashCalculator = new CashCalculator(history.Object, priceList.Object);
			return cashCalculator;
		}

		[Test(Description = "Когда у нас 2 достаки по 1 доллару + 1 доставка по 9 долларов, " +
							"то общая стоимость получается 1+1+9")]
		public void CalculateTotalCash_WhenHistoryHasFirstItemWith2DeliveryAndSecondItemWithOtherOneDelivery_ReturnsCostOfThreeDelivery()
		{
			var cashCalculator = InitCashCalculatorWithFirstItemWith2DeliveryAndSecondItemWithOtherOneDelivery();

			var resultCash = cashCalculator.CalculateTotalCash();

			Assert.AreEqual(1 + 1 + 9, resultCash);
		}

		private static CashCalculator InitCashCalculatorWithFirstItemWith2DeliveryAndSecondItemWithOtherOneDelivery()
		{
			var history = new Mock<ICashIntervalsProvider>();
			var priceList = new Mock<IPriceList>();
			history.Setup(subscriptionHistory => subscriptionHistory.GetIntervals())
				.Returns(() =>
				{
					var firstSubscrPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
						DeliveryRegularity.OncePerMonth, new DeliveryTime(10));
					var secondSubscrPlan = new SubscriptionPlan(SubscriptionTypes.RazorAndGel,
						DeliveryRegularity.OncePerMonth, new DeliveryTime(5));
					var cashIntervals = new List<CashInterval>
					{
						new CashInterval(firstSubscrPlan, Helper.GenerateSubscrDate("1 jan 2017"), Helper.GenerateSubscrDate("1 mar 2017")),
						new CashInterval(secondSubscrPlan, Helper.GenerateSubscrDate("2 mar 2017"), Helper.GenerateSubscrDate("1 apr 2017"))
					};
					return cashIntervals;
				});
			priceList.Setup(list => list.GetPrice(It.IsAny<SubscriptionTypes>()))
				.Returns<SubscriptionTypes>(sType =>
				{
					if (sType == SubscriptionTypes.RazorAndGel)
						return 9;
					return 1;
				});
			var cashCalculator = new CashCalculator(history.Object, priceList.Object);
			return cashCalculator;
		}

		[Test(Description = "При доставке раз в 2 месяца за год получается 6 доставок по 1 доллару")]
		public void CalculateTotalCash_WhenHistoryHasOneYearAndHasDeliveryOncePerTwoMonths_ReturnsCostOfSixDelivery()
		{
			var cashCalculator = InitCashCalculatorWhenHistoryHasOneYearAndHasDeliveryOncePerTwoMonths();

			var resultCash = cashCalculator.CalculateTotalCash();

			Assert.AreEqual(1 * 6, resultCash);
		}

		private static CashCalculator InitCashCalculatorWhenHistoryHasOneYearAndHasDeliveryOncePerTwoMonths()
		{
			var history = new Mock<ICashIntervalsProvider>();
			var priceList = new Mock<IPriceList>();
			history.Setup(subscriptionHistory => subscriptionHistory.GetIntervals())
				.Returns(() =>
				{
					var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
						DeliveryRegularity.OncePerTwoMonths, new DeliveryTime(10));
					var cashIntervals = new List<CashInterval>
					{
						new CashInterval(subscriptionPlan, Helper.GenerateSubscrDate("1 jan 2017"),
							Helper.GenerateSubscrDate("1 jan 2018"))
					};
					return cashIntervals;
				});
			priceList.Setup(list => list.GetPrice(It.IsAny<SubscriptionTypes>()))
				.Returns(1);
			var cashCalculator = new CashCalculator(history.Object, priceList.Object);
			return cashCalculator;
		}

		[Test(Description = "Если подписка приостановлена постоянно, то стоимость нулевая.")]
		public void CalculateTotalCash_WhenDeliverySuspended_ReturnsZeroCost()
		{
			var cashCalculator = InitCashCalculatorWhenDeliverySuspended();

			var resultCash = cashCalculator.CalculateTotalCash();

			Assert.AreEqual(0, resultCash);
		}

		private static CashCalculator InitCashCalculatorWhenDeliverySuspended()
		{
			var history = new Mock<ICashIntervalsProvider>();
			var priceList = new Mock<IPriceList>();
			history.Setup(subscriptionHistory => subscriptionHistory.GetIntervals())
				.Returns(() =>
				{
					var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
						DeliveryRegularity.Suspended, new DeliveryTime(10));
					var cashIntervals = new List<CashInterval>
					{
						new CashInterval(subscriptionPlan, Helper.GenerateSubscrDate("1 jan 2017"),
							Helper.GenerateSubscrDate("1 jan 2018"))
					};
					return cashIntervals;
				});
			priceList.Setup(list => list.GetPrice(It.IsAny<SubscriptionTypes>()))
				.Returns(1);
			var cashCalculator = new CashCalculator(history.Object, priceList.Object);
			return cashCalculator;
		}

		[Test(Description = "Если подписка приостановлена постоянно, " +
							"потом была, потом снова приостановлена, " +
							"то стоимость будет равна одной доставке.")]
		public void CalculateTotalCash_WhenDeliveryPresentAfterSuspendedAfterSuspend_ReturnsOneDeliveryCost()
		{
			var cashCalculator = InitCashCalculatorWhenDeliveryPresentAfterSuspendedAfterSuspend();

			var resultCash = cashCalculator.CalculateTotalCash();

			Assert.AreEqual(1, resultCash);
		}

		private static CashCalculator InitCashCalculatorWhenDeliveryPresentAfterSuspendedAfterSuspend()
		{
			var history = new Mock<ICashIntervalsProvider>();
			var priceList = new Mock<IPriceList>();
			history.Setup(subscriptionHistory => subscriptionHistory.GetIntervals())
				.Returns(() =>
				{
					var suspendedPlan1 = new SubscriptionPlan(SubscriptionTypes.Razor,
						DeliveryRegularity.Suspended, new DeliveryTime(10));
					var activePlan2 = new SubscriptionPlan(SubscriptionTypes.Razor,
						DeliveryRegularity.OncePerMonth, new DeliveryTime(10));
					var suspendedPlan3 = new SubscriptionPlan(SubscriptionTypes.Razor,
						DeliveryRegularity.Suspended, new DeliveryTime(10));
					var cashIntervals = new List<CashInterval>
					{
						new CashInterval(suspendedPlan1, Helper.GenerateSubscrDate("1 jan 2017"),
							Helper.GenerateSubscrDate("1 feb 2017")),
						new CashInterval(activePlan2, Helper.GenerateSubscrDate("2 feb 2017"),
							Helper.GenerateSubscrDate("1 mar 2017")),
						new CashInterval(suspendedPlan3, Helper.GenerateSubscrDate("2 mar 2017"),
							Helper.GenerateSubscrDate("1 apr 2017"))
					};
					return cashIntervals;
				});
			priceList.Setup(list => list.GetPrice(It.IsAny<SubscriptionTypes>()))
				.Returns(1);
			var cashCalculator = new CashCalculator(history.Object, priceList.Object);
			return cashCalculator;
		}
	}
}