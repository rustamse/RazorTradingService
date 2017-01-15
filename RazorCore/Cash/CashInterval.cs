using System;
using System.Collections.Generic;
using System.Linq;
using RazorCore.Subscription;

namespace RazorCore.Cash
{
	public class CashInterval
	{
		public readonly SubscriptionPlan SubscriptionPlan;

		public readonly DateTime FromDate;

		public readonly DateTime ToDate;

		public CashInterval(SubscriptionPlan subscriptionPlan, DateTime fromDate, DateTime toDate)
		{
			if (subscriptionPlan == null)
				throw new ArgumentNullException(nameof(subscriptionPlan));

			SubscriptionPlan = subscriptionPlan;
			FromDate = fromDate;
			ToDate = toDate;
		}

		public List<DateTime> GetDeliveryDates()
		{
			var checkDate = FromDate;

			var deliveryDates = new List<DateTime>();
			while (checkDate <= ToDate)
			{
				if (SubscriptionPlan.DeliveryRegularity == DeliveryRegularity.Suspended)
					break;

				var isDeliveryDay = SubscriptionPlan.DeliveryTime.DeliveryDays.ToList().Contains(checkDate.Day);
				var isDeliveryMonth = SubscriptionPlan.DeliveryRegularity != DeliveryRegularity.OncePerTwoMonths ||
									  (FromDate.Month - checkDate.Month) % 2 == 0;
				if (isDeliveryDay && isDeliveryMonth)
				{
					deliveryDates.Add(checkDate);
				}

				checkDate = checkDate.AddDays(1);
			}
			return deliveryDates;
		}
	}
}