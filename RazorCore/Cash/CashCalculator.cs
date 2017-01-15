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

		public double GetCashByDate(DateTime calculationDate)
		{
			if (!_cashIntervalsProvider.GetIntervals().Any())
				return 0;

			var totalCash = 0.0;
			foreach (var cashInterval in _cashIntervalsProvider.GetIntervals())
			{
				var date = cashInterval.FromDate;

				while (date <= calculationDate && date <= cashInterval.ToDate)
				{
					var isDeliveryDay = cashInterval.SubscriptionPlan.DeliveryTime.DeliveryDays.ToList().Contains(date.Day);
					if (isDeliveryDay)
					{
						var price = _priceList.GetPrice(cashInterval.SubscriptionPlan.SubscriptionType);
						totalCash += price;
					}

					date = date.AddDays(1);
				}
			}

			return totalCash;
		}
	}
}
