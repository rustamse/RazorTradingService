using System;

namespace RazorCore.Subscription
{
	public interface IDeliveryInfo
	{
		string GetDescription();

		bool IsDeliveryDay(DateTime checkDate);
	}
}