using System;

namespace RazorCore.Subscription
{
	public interface IDeliveryInfo
	{
		bool IsDeliveryDay(DateTime checkDate);
	}
}