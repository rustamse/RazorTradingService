using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RazorCore.History
{
	public interface ISubscriptionHistory
	{
		ReadOnlyCollection<ISubscriptionInterval> GetHistory();

		IEnumerable<DateTime> GetFutureDeliveryDays(DateTime endDate);
	}
}