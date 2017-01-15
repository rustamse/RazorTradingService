using System;

namespace RazorCore.Subscription
{
	public class SubscriptionPlan
	{
		public readonly SubscriptionTypes SubscriptionTypes;
		public readonly DeliveryRegularity DeliveryRegularity;
		public readonly DeliveryTime DeliveryTime;

		public SubscriptionPlan(SubscriptionTypes subscriptionTypes,
			DeliveryRegularity deliveryRegularity, DeliveryTime deliveryTime)
		{
			if (deliveryTime == null)
				throw new ArgumentNullException(nameof(deliveryTime));

			SubscriptionTypes = subscriptionTypes;
			DeliveryRegularity = deliveryRegularity;
			DeliveryTime = deliveryTime;

			CheckDeliveryTime();
		}

		private void CheckDeliveryTime()
		{
			switch (DeliveryRegularity)
			{
				case DeliveryRegularity.OncePerTwoMonths:
				case DeliveryRegularity.OncePerMonth:
					if (DeliveryTime.DeliveryDays.Length > 1)
						throw new ArgumentOutOfRangeException(nameof(DeliveryTime), "Должен быть выбран только один день доставки для (DeliveryRegularity.OncePerTwoMonths или DeliveryRegularity.OncePerMonth).");

					CheckDeliverDayNumber(DeliveryTime.DeliveryDays[0]);

					break;

				case DeliveryRegularity.TwicePerMonth:
					if (DeliveryTime.DeliveryDays.Length != 2)
						throw new ArgumentOutOfRangeException(nameof(DeliveryTime), "Должно быть выбрано два дня доставки для (DeliveryRegularity.TwicePerMonth).");

					var deliveryDay0 = DeliveryTime.DeliveryDays[0];
					var deliveryDay1 = DeliveryTime.DeliveryDays[1];
					CheckDeliverDayNumber(deliveryDay0);
					CheckDeliverDayNumber(deliveryDay1);

					if (deliveryDay0 == deliveryDay1)
						throw new ArgumentOutOfRangeException(nameof(DeliveryTime), "Должно быть выбрано два различных дня доставки для (DeliveryRegularity.TwicePerMonth).");

					break;
			}
		}

		private void CheckDeliverDayNumber(int deliveryDayNumber)
		{
			if (deliveryDayNumber < 1 || deliveryDayNumber > 28)
				throw new ArgumentOutOfRangeException(nameof(DeliveryTime),
					$"День доставки должен быть с 1 п 28 число. Выбрано: {deliveryDayNumber}");
		}
	}
}
