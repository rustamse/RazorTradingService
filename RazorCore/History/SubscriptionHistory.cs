using System;
using System.Collections.Generic;
using RazorCore.Subscription;

namespace RazorCore.History
{
	public class SubscriptionHistory
	{
		private readonly List<HistoryItem> _historyItems = new List<HistoryItem>();

		public void AddSubscrption(SubscriptionPlan subscriptionPlan, DateTime fromTime)
		{
			if (subscriptionPlan == null)
				throw new ArgumentNullException(nameof(subscriptionPlan));

			_historyItems.Add(new HistoryItem(subscriptionPlan, fromTime));
		}

		public IEnumerable<HistoryItem> GetHistory()
		{
			return _historyItems;
		}
	}
}