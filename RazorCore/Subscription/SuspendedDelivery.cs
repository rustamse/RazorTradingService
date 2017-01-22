using System;

namespace RazorCore.Subscription
{
	public class SuspendedDelivery : IDeliveryInfo
	{
		public string GetDescription()
		{
			return "SuspendedDelivery";
		}

		public bool IsDeliveryDay(DateTime checkDate)
		{
			return false;
		}
	}
}