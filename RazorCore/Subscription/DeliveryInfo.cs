using System;
using System.Linq;

namespace RazorCore.Subscription
{
	public class DeliveryInfo : IDeliveryInfo
	{
		public readonly int[] DeliveryDays;

		public readonly DeliveryRegularity DeliveryRegularity;

		public DeliveryInfo(DeliveryRegularity deliveryRegularity, params int[] deliveryDays)
		{
			if (deliveryDays == null)
				throw new ArgumentNullException(nameof(deliveryDays));

			DeliveryRegularity = deliveryRegularity;
			DeliveryDays = deliveryDays;

			CheckDeliveryTime();
		}

		public bool IsDeliveryDay(DateTime checkDate)
		{
			if (DeliveryRegularity == DeliveryRegularity.Suspended)
				return false;

			var isDeliveryDay = DeliveryDays.ToList().Contains(checkDate.Day);
			var isDeliveryMonth = DeliveryRegularity != DeliveryRegularity.OncePerTwoMonths ||
								  checkDate.Month % 2 == 1;
			return isDeliveryDay && isDeliveryMonth;
		}

		private void CheckDeliveryTime()
		{
			switch (DeliveryRegularity)
			{
				case DeliveryRegularity.OncePerTwoMonths:
				case DeliveryRegularity.OncePerMonth:
					if (DeliveryDays.Length > 1)
						throw new ArgumentOutOfRangeException(nameof(DeliveryInfo), "Должен быть выбран только один день доставки для (DeliveryRegularity.OncePerTwoMonths или DeliveryRegularity.OncePerMonth).");

					CheckDeliverDayNumber(DeliveryDays[0]);

					break;

				case DeliveryRegularity.TwicePerMonth:
					if (DeliveryDays.Length != 2)
						throw new ArgumentOutOfRangeException(nameof(DeliveryInfo), "Должно быть выбрано два дня доставки для (DeliveryRegularity.TwicePerMonth).");

					var deliveryDay0 = DeliveryDays[0];
					var deliveryDay1 = DeliveryDays[1];
					CheckDeliverDayNumber(deliveryDay0);
					CheckDeliverDayNumber(deliveryDay1);

					if (deliveryDay0 == deliveryDay1)
						throw new SubscriptionPlanDublicateDeliveryDayException("Должно быть выбрано два различных дня доставки для (DeliveryRegularity.TwicePerMonth).");

					break;
			}
		}

		private void CheckDeliverDayNumber(int deliveryDayNumber)
		{
			if (deliveryDayNumber < 1 || deliveryDayNumber > 28)
				throw new ArgumentOutOfRangeException(nameof(DeliveryInfo),
					$"День доставки должен быть с 1 п 28 число. Выбрано: {deliveryDayNumber}");
		}
	}
}