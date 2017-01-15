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

		public List<DateTime> GetDeliveryDates(DateTime calculationDate, CashInterval cashInterval)
		{
			var checkDate = cashInterval.FromDate;

			var deliveryDates = new List<DateTime>();
			while (checkDate <= calculationDate && checkDate <= cashInterval.ToDate)
			{
				if (cashInterval.SubscriptionPlan.DeliveryRegularity == DeliveryRegularity.Suspended)
					break;

				var isDeliveryDay = cashInterval.SubscriptionPlan.DeliveryTime.DeliveryDays.ToList().Contains(checkDate.Day);
				var isDeliveryMonth = cashInterval.SubscriptionPlan.DeliveryRegularity != DeliveryRegularity.OncePerTwoMonths ||
									  (cashInterval.FromDate.Month - checkDate.Month) % 2 == 0;
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