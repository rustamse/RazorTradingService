using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RazorCore.Subscription;

namespace RazorCore.History
{
	public class SubscriptionHistory : ISubscriptionHistory
	{
		private readonly List<ISubscriptionInterval> _historyIntervals = new List<ISubscriptionInterval>();

		public void AddSubscription(IProductInfo productInfo, IDeliveryInfo deliveryInfo,
			DateTime subscrDate)
		{
			if (productInfo == null)
				throw new ArgumentNullException(nameof(productInfo));
			if (deliveryInfo == null)
				throw new ArgumentNullException(nameof(deliveryInfo));

			if (_historyIntervals.Any())
			{
				if (subscrDate < _historyIntervals.Last().FromDate)
					throw new SubscriptionHistoryOldIntervalException();

				if (subscrDate == _historyIntervals.Last().FromDate)
					_historyIntervals.Remove(_historyIntervals.Last());
				else
					_historyIntervals.Last().ModifyToDate(subscrDate.AddDays(-1));
			}

			var interval = new SubscriptionInterval(productInfo, deliveryInfo,
				subscrDate, subscrDate);
			_historyIntervals.Add(interval);
		}

		public void UpdateSubscription(DateTime toDate)
		{
			if (!_historyIntervals.Any())
				return;

			var lastInterval = _historyIntervals.Last();

			if (toDate >= lastInterval.FromDate)
			{
				lastInterval.ModifyToDate(toDate);
			}
		}

		public ReadOnlyCollection<ISubscriptionInterval> GetHistory()
		{
			return new ReadOnlyCollection<ISubscriptionInterval>(_historyIntervals);
		}

		public IEnumerable<DateTime> GetFutureDeliveryDays(DateTime endDate)
		{
			yield return DateTime.Now.Date.AddDays(10);
		}
	}
}