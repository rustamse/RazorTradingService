using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RazorCore.History;

namespace RazorCore.Cash
{
	class CashCalculator
	{
		private readonly SubscriptionHistory _subscriptionHistory;

		public CashCalculator(SubscriptionHistory subscriptionHistory)
		{
			if (subscriptionHistory == null)
				throw new ArgumentNullException(nameof(subscriptionHistory));

			_subscriptionHistory = subscriptionHistory;
		}

		public double GetCashByDate(DateTime calculationDate)
		{
			if (!_subscriptionHistory.GetHistory().Any())
				return 0;

			return 0;
		}
	}
}
