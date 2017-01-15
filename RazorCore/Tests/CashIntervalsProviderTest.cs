using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using RazorCore.Cash;
using RazorCore.History;

namespace RazorCore.Tests
{
	[TestFixture]
	public class CashIntervalsProviderTest
	{
		[Test]
		public void GetIntervals_WhenHasHistory_ReturnsOneInterval()
		{
			var historyStartJan1 = Helper.GenerateSubscrDate("1 jan 2017");
			var cashDateJan15 = Helper.GenerateSubscrDate("15 jan 2017");
			var history = GenerateHistoryFromJan1(historyStartJan1);

			var cashIntervalsProvider = new CashIntervalsProvider(history, cashDateJan15);
			var cashIntervals = cashIntervalsProvider.GetIntervals()
				.ToList();

			Assert.AreEqual(historyStartJan1, cashIntervals.First().FromDate);
			Assert.AreEqual(cashDateJan15, cashIntervals.First().ToDate);
		}

		private static ISubscriptionHistory GenerateHistoryFromJan1(DateTime historyStart)
		{
			var history = new Mock<ISubscriptionHistory>();
			history.Setup(subscriptionHistory => subscriptionHistory.GetHistory())
				.Returns(() => new List<HistoryItem>
				{
					new HistoryItem(Helper.GenerateStdSubscrPlan(), historyStart)
				});
			return history.Object;
		}
	}
}