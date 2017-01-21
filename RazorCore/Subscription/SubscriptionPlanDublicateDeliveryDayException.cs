using System;

namespace RazorCore.Subscription
{
	public class SubscriptionPlanDublicateDeliveryDayException : Exception
	{
		public SubscriptionPlanDublicateDeliveryDayException(string msg)
			: base(msg)
		{

		}
	}
}