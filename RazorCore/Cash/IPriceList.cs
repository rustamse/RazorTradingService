using RazorCore.Subscription;

namespace RazorCore.Cash
{
	public interface IPriceList
	{
		double GetPrice(SubscriptionTypes subscriptionType);
	}
}