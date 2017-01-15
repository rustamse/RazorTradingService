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
		public void GetCashByDate_WhenEmptyHistory_Returns0()
		{
			var history = new Mock<ICashIntervalsProvider>();
			var priceList = new Mock<IPriceList>();
			var cashCalculator = new CashCalculator(history.Object, priceList.Object);
			var date = Helper.GenerateSubscrDate("15 jan 2017");
			var resultCash = cashCalculator.GetCashByDate(date);

			Assert.AreEqual(0, resultCash);
		}

		[Test]
		public void GetCashByDate_WhenHistoryHadOneDelivery_ReturnsCostOfOneDelivery()
		{
			var cashCalculator = InitCashCalculatorWithOneHistoryElement();
			var date = Helper.GenerateSubscrDate("15 jan 2017");
			var resultCash = cashCalculator.GetCashByDate(date);

			Assert.AreEqual(1, resultCash);
		}

		private static CashCalculator InitCashCalculatorWithOneHistoryElement()
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

		[Test]
		public void GetCashByDate_WhenHistoryHasFirstItemWith2DeliveryAndSecondItemWithOtherOneDelivery_ReturnsCostOfThreeDelivery()
		{
			var cashCalculator = InitCashCalculatorWithFirstItemWith2DeliveryAndSecondItemWithOtherOneDelivery();
			var date = Helper.GenerateSubscrDate("30 mar 2017");
			var resultCash = cashCalculator.GetCashByDate(date);

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

		[Test]
		public void GetCashByDate_WhenHistoryHasOneYearAndHasDeliveryOncePerTwoMonths_ReturnsCostOfSixDelivery()
		{
			var cashCalculator = InitCashCalculatorWhenHistoryHasOneYearAndHasDeliveryOncePerTwoMonths();
			var date = Helper.GenerateSubscrDate("1 jan 2018");
			var resultCash = cashCalculator.GetCashByDate(date);

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
		public void GetCashByDate_WhenDeliverySuspended_ReturnsZeroCost()
		{
			var cashCalculator = InitCashCalculatorWhenDeliverySuspended();
			var date = Helper.GenerateSubscrDate("1 jan 2018");
			var resultCash = cashCalculator.GetCashByDate(date);

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
	}
}