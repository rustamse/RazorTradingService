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
			subscriptionHistory.AddSubscrption(
				new SubscriptionPlan(SubscriptionTypes.Razor, DeliveryRegularity.OncePerMonth, new DeliveryTime(1)),
				DateTime.Parse("1/01/2017", CultureInfo.InvariantCulture));
			

			Assert.AreEqual(1, subscriptionHistory.GetHistory().Count());
		}
	}
}