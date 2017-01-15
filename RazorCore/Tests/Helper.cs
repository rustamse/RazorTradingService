using System;
using System.Globalization;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	class Helper
	{
		public static SubscriptionPlan GenerateStdSubscrPlan()
		{
			return new SubscriptionPlan(SubscriptionTypes.Razor, DeliveryRegularity.OncePerMonth, new DeliveryTime(1));
		}

		public static DateTime GenerateSubscrDate(string date)
		{
			var generateSubscrDate = DateTime.Parse(date, CultureInfo.InvariantCulture);
			return generateSubscrDate.Date;
		}
	}
}