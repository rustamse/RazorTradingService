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

		[Test]
		public void GetIntervals_WhenHasHistoryAndCashDateBeforeHistory_ReturnsNoIntervals()
		{
			var historyStartJan1 = Helper.GenerateSubscrDate("1 jan 2017");
			var cashDateJan15 = Helper.GenerateSubscrDate("1 dec 2016");
			var history = GenerateHistoryFromJan1(historyStartJan1);

			var cashIntervalsProvider = new CashIntervalsProvider(history, cashDateJan15);
			var cashIntervals = cashIntervalsProvider.GetIntervals();

			Assert.False(cashIntervals.Any());
		}

		[Test]
		public void GetIntervals_WhenEmptyHistory_ReturnsNoIntervals()
		{
			var cashDateJan15 = Helper.GenerateSubscrDate("1 dec 2016");
			var history = GenerateEmptyHistory();

			var cashIntervalsProvider = new CashIntervalsProvider(history, cashDateJan15);
			var cashIntervals = cashIntervalsProvider.GetIntervals();

			Assert.False(cashIntervals.Any());
		}

		private static ISubscriptionHistory GenerateEmptyHistory()
		{
			var history = new Mock<ISubscriptionHistory>();
			history.Setup(subscriptionHistory => subscriptionHistory.GetHistory())
				.Returns(() => new List<HistoryItem>());
			return history.Object;
		}

		[Test(Description = "Когда история за 2016,2017,2018 год, а дата расчета 2017 год, " +
							"то результирующий интервал содержит только 2016 и 2017 год (до момента даты расчета)")]
		public void GetIntervals_WhenHistoryLongerThanCashDate_ReturnsIntervalsFromStartHistoryToCashDate()
		{
			var cashDateJan15 = Helper.GenerateSubscrDate("15 jan 2017");
			var history = GenerateHistory2016_2017_2018();

			var cashIntervalsProvider = new CashIntervalsProvider(history, cashDateJan15);
			var cashIntervals = cashIntervalsProvider.GetIntervals();

			Assert.AreEqual(2, cashIntervals.Count());
		}

		private static ISubscriptionHistory GenerateHistory2016_2017_2018()
		{
			var history = new Mock<ISubscriptionHistory>();
			history.Setup(subscriptionHistory => subscriptionHistory.GetHistory())
				.Returns(() => new List<HistoryItem>
				{
					new HistoryItem(Helper.GenerateStdSubscrPlan(), Helper.GenerateSubscrDate("1 jan 2016")),
					new HistoryItem(Helper.GenerateStdSubscrPlan(), Helper.GenerateSubscrDate("1 jan 2017")),
					new HistoryItem(Helper.GenerateStdSubscrPlan(), Helper.GenerateSubscrDate("1 jan 2018"))
				});
			return history.Object;
		}
	}
}