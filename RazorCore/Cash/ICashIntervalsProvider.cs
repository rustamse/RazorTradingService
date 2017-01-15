using System.Collections.Generic;

namespace RazorCore.Cash
{
	public interface ICashIntervalsProvider
	{
		IEnumerable<CashInterval> GetIntervals();
	}
}