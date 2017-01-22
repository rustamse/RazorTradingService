using System;
using System.Collections.Generic;
using RazorCore.Subscription;

namespace RazorCore.History
{
	public class SubscriptionInterval : ISubscriptionInterval
	{
		public readonly IProductInfo ProductInfo;
		public readonly IDeliveryInfo DeliveryInfo;

		public DateTime FromDate { get; private set; }

		public DateTime ToDate { get; private set; }

		public SubscriptionInterval(IProductInfo productInfo, IDeliveryInfo deliveryInfo,
			DateTime fromDate, DateTime toDate)
		{
			if (productInfo == null)
				throw new ArgumentNullException(nameof(productInfo));
			if (deliveryInfo == null)
				throw new ArgumentNullException(nameof(deliveryInfo));
			if (fromDate > toDate)
				throw new ArgumentOutOfRangeException(nameof(toDate));

			ProductInfo = productInfo;
			DeliveryInfo = deliveryInfo;
			FromDate = fromDate;
			ToDate = toDate;
		}


		public void ModifyFromDate(DateTime newFromDate)
		{
			if (ToDate < newFromDate)
				throw new ArgumentOutOfRangeException(nameof(newFromDate));

			FromDate = newFromDate;
		}

		public void ModifyToDate(DateTime newToDate)
		{
			if (newToDate < FromDate)
				throw new ArgumentOutOfRangeException(nameof(newToDate));

			ToDate = newToDate;
		}

		public double GetOneDeliveryCost()
		{
			return ProductInfo.Price;
		}

		public List<DateTime> GetDeliveryDates()
		{
			var checkDate = FromDate;

			var deliveryDates = new List<DateTime>();
			while (checkDate <= ToDate)
			{
				var isDeliveryDay = DeliveryInfo.IsDeliveryDay(checkDate);

				if (isDeliveryDay)
					deliveryDates.Add(checkDate);

				checkDate = checkDate.AddDays(1);
			}
			return deliveryDates;
		}
	}
}