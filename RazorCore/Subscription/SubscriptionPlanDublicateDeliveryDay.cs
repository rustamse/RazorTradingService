using System;

namespace RazorCore.Subscription
{
	public class SubscriptionPlanDublicateDeliveryDay : Exception
	{
		public SubscriptionPlanDublicateDeliveryDay(string msg)
			: base(msg)
		{

		}
	}
}