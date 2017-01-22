using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RazorCore.Subscription;

namespace RazorCore.History
{
	public class SubscriptionHistory : ISubscriptionHistory
	{
		private readonly List<ISubscriptionInterval> _historyItems = new List<ISubscriptionInterval>();

		public void AddSubscrption(IProductInfo productInfo, IDeliveryInfo deliveryInfo,
			DateTime fromDate, DateTime toDate)
		{
			if (productInfo == null)
				throw new ArgumentNullException(nameof(productInfo));
			if (deliveryInfo == null)
				throw new ArgumentNullException(nameof(deliveryInfo));

			var subscriptionInterval = new SubscriptionInterval(productInfo, deliveryInfo,
				fromDate.Date, toDate.Date);
			_historyItems.Add(subscriptionInterval);

			SortHistoryItemsByAsc();
		}

		public ReadOnlyCollection<ISubscriptionInterval> GetHistory()
		{
			return new ReadOnlyCollection<ISubscriptionInterval>(_historyItems);
		}

		private void SortHistoryItemsByAsc()
		{
			_historyItems.Sort((item, historyItem) => item.FromDate.CompareTo(historyItem.FromDate));
		}
	}
}