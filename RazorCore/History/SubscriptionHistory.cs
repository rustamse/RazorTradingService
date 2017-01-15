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

			var newItemDate = fromTime.Date;

			_historyItems.Add(new HistoryItem(subscriptionPlan, newItemDate));

			SortHistoryItemsByAsc();
		}

		public IEnumerable<HistoryItem> GetHistory()
		{
			return _historyItems;
		}

		private void SortHistoryItemsByAsc()
		{
			_historyItems.Sort((item, historyItem) => item.FromDate.CompareTo(historyItem.FromDate));
		}
	}
}