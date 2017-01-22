using System;
using System.Collections.Generic;

namespace RazorCore.History
{
	public interface ISubscriptionInterval
	{
		DateTime FromDate { get; }
		DateTime ToDate { get; }

		double GetOneDeliveryCost();
		List<DateTime> GetDeliveryDates();
	}
}