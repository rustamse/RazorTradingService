using System;

namespace RazorCore.Subscription
{
	public class SuspendedDelivery : IDeliveryInfo
	{
		public bool IsDeliveryDay(DateTime checkDate)
		{
			return false;
		}
	}
}