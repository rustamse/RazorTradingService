using System.Linq;
using NUnit.Framework;
using RazorCore.History;

namespace RazorCore.Tests
{
	[TestFixture]
	public class SubscriptionHistoryTest
	{
		[Test]
		public void GetHistory_WhenAddOneSubscriptionPlan_ReturnsOneSubscriptionPlan()
		{
			var subscriptionHistory = new SubscriptionHistory();

			subscriptionHistory.AddSubscrption(Helper.GenerateStdSubscrPlan(), Helper.GenerateSubscrDate("15 jan 2017"));

			Assert.AreEqual(1, subscriptionHistory.GetHistory().Count());
		}

		[Test]
		public void GetHistory_WhenAddOneSubscriptionPlan_ReturnsSubscriptionPlanWithSameDate()
		{
			var subscriptionHistory = new SubscriptionHistory();
			var fromTime = Helper.GenerateSubscrDate("15 jan 2017");
			subscriptionHistory.AddSubscrption(Helper.GenerateStdSubscrPlan(), fromTime);

			Assert.AreEqual(fromTime, subscriptionHistory.GetHistory().First().FromDate);
		}

		[Test]
		public void GetHistory_WhenAddSecondSubscriptionPlan_ReturnsTwoItemsHistory()
		{
			var subscriptionHistory = new SubscriptionHistory();
			subscriptionHistory.AddSubscrption(Helper.GenerateStdSubscrPlan(), Helper.GenerateSubscrDate("15 jan 2017"));
			subscriptionHistory.AddSubscrption(Helper.GenerateStdSubscrPlan(), Helper.GenerateSubscrDate("20 jan 2017"));

			Assert.AreEqual(2, subscriptionHistory.GetHistory().Count());
		}

		[Test]
		public void GetHistory_WhenAddSecondWithBiggerDate_ReturnsAddedSubscriptionLastInHistory()
		{
			var initialDate = Helper.GenerateSubscrDate("15 jan 2017");
			var addedDate = Helper.GenerateSubscrDate("20 jan 2017");

			var subscriptionHistory = new SubscriptionHistory();
			subscriptionHistory.AddSubscrption(Helper.GenerateStdSubscrPlan(), initialDate);
			subscriptionHistory.AddSubscrption(Helper.GenerateStdSubscrPlan(), addedDate);

			Assert.AreEqual(addedDate, subscriptionHistory.GetHistory().Last().FromDate);
		}

		[Test]
		public void GetHistory_WhenAddSecondWithSmallerDate_ReturnsAddedSubscriptionFirstInHistory()
		{
			var initialDate = Helper.GenerateSubscrDate("15 jan 2017");
			var addedDate = Helper.GenerateSubscrDate("10 jan 2017");

			var subscriptionHistory = new SubscriptionHistory();
			subscriptionHistory.AddSubscrption(Helper.GenerateStdSubscrPlan(), initialDate);
			subscriptionHistory.AddSubscrption(Helper.GenerateStdSubscrPlan(), addedDate);

			Assert.AreEqual(addedDate, subscriptionHistory.GetHistory().First().FromDate);
		}

		[Test]
		public void GetHistory_WhenAddSecondWithTheSameDate_ReturnsOneItemHistoryWhichEqualsToAdded()
		{
			var initialDate = Helper.GenerateSubscrDate("15 jan 2017");
			var addedDate = Helper.GenerateSubscrDate("15 jan 2017");
			var addedSubscrPlan = Helper.GenerateStdSubscrPlan();

			var subscriptionHistory = new SubscriptionHistory();
			subscriptionHistory.AddSubscrption(Helper.GenerateStdSubscrPlan(), initialDate);
			subscriptionHistory.AddSubscrption(addedSubscrPlan, addedDate);

			Assert.AreEqual(1, subscriptionHistory.GetHistory().Count());
			Assert.AreEqual(addedSubscrPlan, subscriptionHistory.GetHistory().First().SubscriptionPlan);
		}
	}
}