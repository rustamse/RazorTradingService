using System;

namespace RazorCore.Subscription
{
	public class DeliveryDay
	{
		public readonly int DayOfMonth;

		public DeliveryDay(int dayOfMonth)
		{
			if (dayOfMonth < 1 || dayOfMonth > 28)
				throw new ArgumentOutOfRangeException(nameof(dayOfMonth));

			DayOfMonth = dayOfMonth;
		}
	}
}