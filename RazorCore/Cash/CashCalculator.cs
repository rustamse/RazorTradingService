using System;
using System.Linq;

namespace RazorCore.Cash
{
	class CashCalculator
	{
		private readonly ICashIntervalsProvider _cashIntervalsProvider;
		private readonly IPriceList _priceList;

		public CashCalculator(ICashIntervalsProvider cashIntervalsProvider, IPriceList priceList)
		{
			if (cashIntervalsProvider == null)
				throw new ArgumentNullException(nameof(cashIntervalsProvider));
			if (priceList == null)
				throw new ArgumentNullException(nameof(priceList));

			_cashIntervalsProvider = cashIntervalsProvider;
			_priceList = priceList;
		}

		public double CalculateTotalCash()
		{
			if (!_cashIntervalsProvider.GetIntervals().Any())
				return 0;

			return _cashIntervalsProvider.GetIntervals()
				.Sum(cashInterval => GetCashForOneInterval(cashInterval));
		}

		private double GetCashForOneInterval(CashInterval cashInterval)
		{
			var deliveryDates = cashInterval.GetDeliveryDates();

			return deliveryDates.Sum(deliveryDate => _priceList.GetPrice(
				cashInterval.SubscriptionPlan.SubscriptionType));
		}
	}
}
