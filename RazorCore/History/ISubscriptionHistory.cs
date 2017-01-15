using System.Collections.Generic;

namespace RazorCore.History
{
	public interface ISubscriptionHistory
	{
		IEnumerable<HistoryItem> GetHistory();
	}
}