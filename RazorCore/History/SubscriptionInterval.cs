using System;
using System.Collections.Generic;
using RazorCore.Subscription;

namespace RazorCore.History
{
	public class SubscriptionInterval : ISubscriptionInterval
	{
		public readonly IProductInfo ProductInfo;
		public readonly IDeliveryInfo DeliveryInfo;

		public DateTime FromDate { get; }

		public DateTime ToDate { get; }

		public SubscriptionInterval(IProductInfo productInfo, IDeliveryInfo deliveryInfo,
			DateTime fromDate, DateTime toDate)
		{
			if (productInfo == null)
				throw new ArgumentNullException(nameof(productInfo));
			if (deliveryInfo == null)
				throw new ArgumentNullException(nameof(deliveryInfo));
			if(fromDate > toDate)
				throw new ArgumentOutOfRangeException(nameof(toDate));

			ProductInfo = productInfo;
			DeliveryInfo = deliveryInfo;
			FromDate = fromDate;
			ToDate = toDate;
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