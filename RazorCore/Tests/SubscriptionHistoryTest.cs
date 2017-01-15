using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using RazorCore.History;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	[TestFixture]
	public class SubscriptionHistoryTest
	{
		[Test]
		public void GetHistory_WhenAddOneSubscriptionPlan_ReturnsOneSubscriptionPlan()
		{
			var subscriptionHistory = new SubscriptionHistory();

			subscriptionHistory.AddSubscrption(GenerateStdSubscrPlan(), GenerateSubscrDate("15 jan 2017"));

			Assert.AreEqual(1, subscriptionHistory.GetHistory().Count());
		}

		[Test]
		public void GetHistory_WhenAddOneSubscriptionPlan_ReturnsSubscriptionPlanWithSameDate()
		{
			var subscriptionHistory = new SubscriptionHistory();
			var fromTime = GenerateSubscrDate("15 jan 2017");
			subscriptionHistory.AddSubscrption(GenerateStdSubscrPlan(), fromTime);

			Assert.AreEqual(fromTime, subscriptionHistory.GetHistory().First().FromDate);
		}

		[Test]
		public void GetHistory_WhenAddSecondSubscriptionPlan_ReturnsTwoItemsHistory()
		{
			var subscriptionHistory = new SubscriptionHistory();
			subscriptionHistory.AddSubscrption(GenerateStdSubscrPlan(), GenerateSubscrDate("15 jan 2017"));
			subscriptionHistory.AddSubscrption(GenerateStdSubscrPlan(), GenerateSubscrDate("20 jan 2017"));

			Assert.AreEqual(2, subscriptionHistory.GetHistory().Count());
		}

		[Test]
		public void GetHistory_WhenAddSecondWithBiggerDate_ReturnsAddedSubscriptionLastInHistory()
		{
			var initialDate = GenerateSubscrDate("15 jan 2017");
			var addedDate = GenerateSubscrDate("20 jan 2017");

			var subscriptionHistory = new SubscriptionHistory();
			subscriptionHistory.AddSubscrption(GenerateStdSubscrPlan(), initialDate);
			subscriptionHistory.AddSubscrption(GenerateStdSubscrPlan(), addedDate);

			Assert.AreEqual(addedDate, subscriptionHistory.GetHistory().Last().FromDate);
		}

		[Test]
		public void GetHistory_WhenAddSecondWithSmallerDate_ReturnsAddedSubscriptionFirstInHistory()
		{
			var initialDate = GenerateSubscrDate("15 jan 2017");
			var addedDate = GenerateSubscrDate("10 jan 2017");

			var subscriptionHistory = new SubscriptionHistory();
			subscriptionHistory.AddSubscrption(GenerateStdSubscrPlan(), initialDate);
			subscriptionHistory.AddSubscrption(GenerateStdSubscrPlan(), addedDate);

			Assert.AreEqual(addedDate, subscriptionHistory.GetHistory().First().FromDate);
		}

		[Test]
		public void GetHistory_WhenAddSecondWithTheSameDate_ReturnsOneItemHistoryWhichEqualsToAdded()
		{
			var initialDate = GenerateSubscrDate("15 jan 2017");
			var addedDate = GenerateSubscrDate("15 jan 2017");
			var addedSubscrPlan = GenerateStdSubscrPlan();

			var subscriptionHistory = new SubscriptionHistory();
			subscriptionHistory.AddSubscrption(GenerateStdSubscrPlan(), initialDate);
			subscriptionHistory.AddSubscrption(addedSubscrPlan, addedDate);

			Assert.AreEqual(1, subscriptionHistory.GetHistory().Count());
			Assert.AreEqual(addedSubscrPlan, subscriptionHistory.GetHistory().First().SubscriptionPlan);
		}

		private static SubscriptionPlan GenerateStdSubscrPlan()
		{
			return new SubscriptionPlan(SubscriptionTypes.Razor, DeliveryRegularity.OncePerMonth, new DeliveryTime(1));
		}

		private static DateTime GenerateSubscrDate(string date)
		{
			var generateSubscrDate = DateTime.Parse(date, CultureInfo.InvariantCulture);
			return generateSubscrDate.Date;
		}
	}
}