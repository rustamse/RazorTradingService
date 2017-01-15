using System;
using System.Linq;
using RazorCore.History;
using RazorCore.Subscription;

namespace RazorCore.Cash
{
	class CashCalculator
	{
		private readonly ISubscriptionHistory _subscriptionHistory;
		private readonly IPriceList _priceList;

		public CashCalculator(ISubscriptionHistory subscriptionHistory, IPriceList priceList)
		{
			if (subscriptionHistory == null)
				throw new ArgumentNullException(nameof(subscriptionHistory));
			if (priceList == null)
				throw new ArgumentNullException(nameof(priceList));

			_subscriptionHistory = subscriptionHistory;
			_priceList = priceList;
		}

		public double GetCashByDate(DateTime calculationDate)
		{
			if (!_subscriptionHistory.GetHistory().Any())
				return 0;

			var subscriptionTypes = _subscriptionHistory.GetHistory().First().SubscriptionPlan.SubscriptionType;
			return _priceList.GetPrice(subscriptionTypes);
		}
	}
}
