using System;
using System.Collections.ObjectModel;
using RazorCore.Subscription;

namespace RazorCore.History
{
	public interface ISubscriptionHistory
	{
		void AddSubscription(IProductInfo productInfo, IDeliveryInfo deliveryInfo,
			DateTime subscrDate);

		void UpdateSubscriptionEndDate(DateTime toDate);

		ReadOnlyCollection<ISubscriptionInterval> GetHistory();
	}
}