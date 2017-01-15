using System;
using System.Collections.Generic;
using System.Linq;
using RazorCore.Subscription;

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

		public double GetCashByDate(DateTime calculationDate)
		{
			if (!_cashIntervalsProvider.GetIntervals().Any())
				return 0;

			return _cashIntervalsProvider.GetIntervals()
				.Sum(cashInterval => GetCashForOneInterval(calculationDate, cashInterval));
		}

		private double GetCashForOneInterval(DateTime calculationDate, CashInterval cashInterval)
		{
			var deliveryDates = cashInterval.GetDeliveryDates(calculationDate, cashInterval);

			return deliveryDates.Sum(deliveryDate => _priceList.GetPrice(
				cashInterval.SubscriptionPlan.SubscriptionType));
		}
	}
}
