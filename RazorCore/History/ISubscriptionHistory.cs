using System.Collections.ObjectModel;

namespace RazorCore.History
{
	public interface ISubscriptionHistory
	{
		ReadOnlyCollection<ISubscriptionInterval> GetHistory();
	}
}