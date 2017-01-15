using System;
using RazorCore.Subscription;

namespace RazorCore.Cash
{
	public class CashInterval
	{
		public readonly SubscriptionPlan SubscriptionPlan;

		public readonly DateTime FromDate;

		public readonly DateTime ToDate;

		public CashInterval(SubscriptionPlan subscriptionPlan, DateTime fromDate, DateTime toDate)
		{
			if (subscriptionPlan == null)
				throw new ArgumentNullException(nameof(subscriptionPlan));

			SubscriptionPlan = subscriptionPlan;
			FromDate = fromDate;
			ToDate = toDate;
		}
	}
}