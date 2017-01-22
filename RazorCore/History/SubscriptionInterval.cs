using System;
using System.Collections.Generic;
using RazorCore.Subscription;

namespace RazorCore.History
{
	class SubscriptionInterval : ISubscriptionInterval
	{
		public DateTime FromDate { get; }
		public DateTime ToDate { get; private set; }

		private readonly IProductInfo _productInfo;
		private readonly IDeliveryInfo _deliveryInfo;

		public SubscriptionInterval(IProductInfo productInfo, IDeliveryInfo deliveryInfo,
			DateTime fromDate, DateTime toDate)
		{
			if (productInfo == null)
				throw new ArgumentNullException(nameof(productInfo));
			if (deliveryInfo == null)
				throw new ArgumentNullException(nameof(deliveryInfo));
			if (fromDate > toDate)
				throw new ArgumentOutOfRangeException(nameof(toDate));

			_productInfo = productInfo;
			_deliveryInfo = deliveryInfo;
			FromDate = fromDate;
			ToDate = toDate;
		}

		public void ModifyToDate(DateTime newToDate)
		{
			if (newToDate < FromDate)
				throw new ArgumentOutOfRangeException(nameof(newToDate));

			ToDate = newToDate;
		}

		public double GetOneDeliveryCost()
		{
			return _productInfo.Price;
		}

		public List<DateTime> GetDeliveryDates()
		{
			var checkDate = FromDate;

			var deliveryDates = new List<DateTime>();
			while (checkDate <= ToDate)
			{
				var isDeliveryDay = _deliveryInfo.IsDeliveryDay(checkDate);

				if (isDeliveryDay)
					deliveryDates.Add(checkDate);

				checkDate = checkDate.AddDays(1);
			}
			return deliveryDates;
		}

		public ISubscriptionInterval CreateCopy()
		{
			return new SubscriptionInterval(_productInfo, _deliveryInfo, FromDate, ToDate);
		}

		public string GetDescription()
		{
			return $"{_deliveryInfo.GetDescription()} ({FromDate:d} - {ToDate:d}),price:{_productInfo.Price}";
		}
	}
}