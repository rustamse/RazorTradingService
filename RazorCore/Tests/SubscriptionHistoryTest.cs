using System.Linq;
using NUnit.Framework;
using RazorCore.History;

namespace RazorCore.Tests
{
	[TestFixture]
	public class SubscriptionHistoryTest
	{
		[Test]
		public void GetHistory_WhenNoHistory_ReturnsEmptyHistory()
		{
			var subscriptionHistory = new SubscriptionHistory();

			Assert.IsEmpty(subscriptionHistory.GetHistory());
		}

		[Test]
		public void GetHistory_WhenAddSubscription_ReturnsHistoryWithThisItem()
		{
			var productInfo = ProductInfoBuilder.Create().Build();
			var deliveryInfo = DeliveryInfoBuilder.Create().Build();
			var subscrDate = "1 jan 2017".ToDate();
			var subscriptionHistory = new SubscriptionHistory();
			subscriptionHistory.AddSubscription(productInfo, deliveryInfo, subscrDate);

			var subscriptionInterval = subscriptionHistory.GetHistory().Single();

			Assert.AreEqual(subscrDate, subscriptionInterval.FromDate);
			Assert.AreEqual(subscrDate, subscriptionInterval.ToDate);
		}

		[Test]
		public void UpdateSubscriptionEndDate_WhenNewEndFeb1_ReturnsHistoryLastItemToDateFeb1()
		{
			var newEndDate = "1 feb 2017".ToDate();

			var productInfo = ProductInfoBuilder.Create().Build();
			var deliveryInfo = DeliveryInfoBuilder.Create().Build();
			var subscrDate = "1 jan 2017".ToDate();
			var subscriptionHistory = new SubscriptionHistory();
			subscriptionHistory.AddSubscription(productInfo, deliveryInfo, subscrDate);

			subscriptionHistory.UpdateSubscriptionEndDate(newEndDate);

			Assert.AreEqual(newEndDate, subscriptionHistory.GetHistory().Last().ToDate);
		}

		[Test]
		public void AddSubscription_WhenAddSubscriptionBeforeHistory_ThrowsSubscriptionHistoryOldIntervalException()
		{
			var beforeHistoryDate = "1 jan 2016".ToDate();

			var productInfo = ProductInfoBuilder.Create().Build();
			var deliveryInfo = DeliveryInfoBuilder.Create().Build();
			var subscrDate = "1 jan 2017".ToDate();
			var subscriptionHistory = new SubscriptionHistory();
			subscriptionHistory.AddSubscription(productInfo, deliveryInfo, subscrDate);

			Assert.Throws<SubscriptionHistoryOldIntervalException>(() =>
			{
				subscriptionHistory.AddSubscription(productInfo, deliveryInfo, beforeHistoryDate);
			});
		}

		[Test]
		public void AddSubscription_WhenAddSubscriptionTwiceWithSameDate_ReturnsOneItemHistoryWithCostOfNewPrice()
		{
			var newPrice = 99;
			var subscrDate = "1 jan 2017".ToDate();

			var productInfo = ProductInfoBuilder.Create().Build();
			var deliveryInfo = DeliveryInfoBuilder.Create().Build();
			var subscriptionHistory = new SubscriptionHistory();
			subscriptionHistory.AddSubscription(productInfo, deliveryInfo, subscrDate);

			var newProductInfo = ProductInfoBuilder.Create().WithPrice(newPrice).Build();
			subscriptionHistory.AddSubscription(newProductInfo, deliveryInfo, subscrDate);

			var historyOneDeliveryCost = subscriptionHistory.GetHistory().Single().GetOneDeliveryCost();
			Assert.AreEqual(newPrice, historyOneDeliveryCost);
			
		}
	}
}