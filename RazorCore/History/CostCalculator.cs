using System;
using System.Linq;

namespace RazorCore.History
{
	public class CostCalculator
	{
		private readonly ISubscriptionHistory _subscriptionHistory;

		public CostCalculator(ISubscriptionHistory subscriptionHistory)
		{
			if (subscriptionHistory == null)
				throw new ArgumentNullException(nameof(subscriptionHistory));

			_subscriptionHistory = subscriptionHistory;
		}

		public double CalculateTotalCost()
		{
			if (!_subscriptionHistory.GetHistory().Any())
				return 0;

			return _subscriptionHistory.GetHistory()
				.Sum(interval => GetCostForOneInterval(interval));
		}

		private double GetCostForOneInterval(ISubscriptionInterval subscriptionInterval)
		{
			var deliveriesCount = subscriptionInterval.GetDeliveryDates()
				.Count;

			var oneDeliveryPrice = subscriptionInterval.GetOneDeliveryPrice();
			return oneDeliveryPrice * deliveriesCount;
		}
	}
}
