using System;

namespace RazorCore.Subscription
{
	public class SubscriptionPlan
	{
		public readonly SubscriptionTypes SubscriptionType;
		public readonly DeliveryTime DeliveryTime;

		public SubscriptionPlan(SubscriptionTypes subscriptionType,
			DeliveryTime deliveryTime)
		{
			if (deliveryTime == null)
				throw new ArgumentNullException(nameof(deliveryTime));

			SubscriptionType = subscriptionType;
			DeliveryTime = deliveryTime;
		}
	}
}
