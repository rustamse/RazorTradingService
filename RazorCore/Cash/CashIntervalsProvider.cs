using System;
using System.Collections.Generic;
using System.Linq;
using RazorCore.History;

namespace RazorCore.Cash
{
	public class CashIntervalsProvider : ICashIntervalsProvider
	{
		private readonly ISubscriptionHistory _subscriptionHistory;
		private readonly DateTime _cashCalculationDate;

		public CashIntervalsProvider(ISubscriptionHistory subscriptionHistory, DateTime cashCalculationDate)
		{
			if (subscriptionHistory == null)
				throw new ArgumentNullException(nameof(subscriptionHistory));

			_subscriptionHistory = subscriptionHistory;
			_cashCalculationDate = cashCalculationDate;
		}

		public IEnumerable<CashInterval> GetIntervals()
		{
			var historyItems = _subscriptionHistory.GetHistory()
				.Where(item => item.FromDate <= _cashCalculationDate)
				.ToList();

			var cashIntervals = new List<CashInterval>();

			if (!historyItems.Any())
				return cashIntervals;

			for (int i = 0; i < historyItems.Count - 1; i++)
			{
				var cashInterval = new CashInterval(historyItems[i].SubscriptionPlan, historyItems[i].FromDate, historyItems[i + 1].FromDate);
				cashIntervals.Add(cashInterval);
			}

			var cashIntervalEnd = new CashInterval(historyItems.Last().SubscriptionPlan, historyItems.Last().FromDate, _cashCalculationDate);
			cashIntervals.Add(cashIntervalEnd);
			return cashIntervals;
		}
	}
}