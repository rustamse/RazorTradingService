using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RazorCore.Cash;
using RazorCore.History;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	[TestFixture]
	public class CashCalculatorTest
	{
		[Test]
		public void GetCashByDate_WhenEmptyHistory_Returns0()
		{
			var history = new Mock<ISubscriptionHistory>();
			var priceList = new Mock<IPriceList>();
			var cashCalculator = new CashCalculator(history.Object, priceList.Object);
			var date = Helper.GenerateSubscrDate("15 jan 2017");
			var resultCash = cashCalculator.GetCashByDate(date);

			Assert.AreEqual(0, resultCash);
		}

		[Test]
		public void GetCashByDate_WhenHistoryHadOneDelivery_ReturnsCostOfOneDelivery()
		{
			var history = new Mock<ISubscriptionHistory>();
			var priceList = new Mock<IPriceList>();
			history.Setup(subscriptionHistory => subscriptionHistory.GetHistory())
				.Returns(() =>
				{
					var subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor,
						DeliveryRegularity.OncePerMonth, new DeliveryTime(10));
					var historyItems = new List<HistoryItem>
					{
						new HistoryItem(subscriptionPlan, Helper.GenerateSubscrDate("1 jan 2017"))
					};
					return historyItems;
				});
			priceList.Setup(list => list.GetPrice(It.IsAny<SubscriptionTypes>()))
				.Returns(1);
			var cashCalculator = new CashCalculator(history.Object, priceList.Object);
			var date = Helper.GenerateSubscrDate("15 jan 2017");
			var resultCash = cashCalculator.GetCashByDate(date);

			Assert.AreEqual(1, resultCash);
		}
	}
}