using NUnit.Framework;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	[TestFixture]
	public class SuspendedDeliveryTest
	{
		[Test]
		public void IsDeliveryDay_WhenCheckdateMay1_ReturnsFalse()
		{
			var checkDate = "1 may 2017".ToDate();

			var deliveryInfo = new SuspendedDelivery();

			Assert.IsFalse(deliveryInfo.IsDeliveryDay(checkDate));
		}
	}
}