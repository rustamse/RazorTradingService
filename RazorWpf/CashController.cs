using System;
using System.Collections.Generic;
using System.Linq;
using RazorCore.Cash;
using RazorCore.History;
using RazorCore.Subscription;

namespace RazorWpf
{
	class CashController
	{
		public DateTime CurrentDate { get; set; }

		private readonly SubscriptionHistory _subscriptionHistory = new SubscriptionHistory();
		private readonly IPriceList _priceList;

		public CashController(DateTime startDate, IPriceList priceList)
		{
			if (priceList == null)
				throw new ArgumentNullException(nameof(priceList));

			_priceList = priceList;
			CurrentDate = startDate;
		}

		public void AddOrUpdateSubscriptionPlan(SubscriptionPlan subscriptionPlan)
		{
			_subscriptionHistory.AddSubscrption(subscriptionPlan, CurrentDate);
		}

		public IEnumerable<HistoryItem> GetSubscriptionHistory()
		{
			return _subscriptionHistory.GetHistory();
		}

		public double CalculateTotalCash()
		{
			var cashIntervalsProvider = new CashIntervalsProvider(_subscriptionHistory, CurrentDate);
			var calc = new CashCalculator(cashIntervalsProvider, _priceList);
			return calc.CalculateTotalCash();
		}

		public List<DateTime> GetFutureDeliveryDays(DateTime maxDeliveryDay)
		{
			var cashIntervalsProvider = new CashIntervalsProvider(_subscriptionHistory, maxDeliveryDay);

			var allDates = new List<DateTime>();
			foreach (var cashInterval in cashIntervalsProvider.GetIntervals())
			{
				allDates.AddRange(cashInterval.GetDeliveryDates());
			}

			var futureDeliveryDays = allDates.Where(time => time >= CurrentDate)
				.ToList();
			return futureDeliveryDays;
		}

		public SubscriptionPlan FindActivePlan()
		{
			var activeCashInterval = FindActiveCashInterval();
			return activeCashInterval?.SubscriptionPlan;
		}

		private CashInterval FindActiveCashInterval()
		{
			var cashIntervalsProvider = new CashIntervalsProvider(_subscriptionHistory, CurrentDate);
			var cashIntervals = cashIntervalsProvider.GetIntervals().ToList();
			if (!cashIntervals.Any())
				return null;

			return cashIntervals.Last();
		}
	}
}