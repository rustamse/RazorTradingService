using System;
using RazorCore.Subscription;

namespace RazorCore.History
{
	public class HistoryItem
	{
		public readonly SubscriptionPlan SubscriptionPlan;

		public readonly DateTime FromDate;

		public HistoryItem(SubscriptionPlan subscriptionPlan, DateTime fromDate)
		{
			if (subscriptionPlan == null)
				throw new ArgumentNullException(nameof(subscriptionPlan));

			SubscriptionPlan = subscriptionPlan;
			FromDate = fromDate;
		}
	}
}