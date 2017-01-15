using NUnit.Framework;
using RazorCore.Cash;
using RazorCore.History;

namespace RazorCore.Tests
{
	[TestFixture]
	public class CashCalculatorTest
	{
		[Test]
		public void GetCashByDate_WhenEmptyHistory_Returns0()
		{
			var cashCalculator = new CashCalculator(new SubscriptionHistory());
			var date = Helper.GenerateSubscrDate("15 jan 2017");
			var resultCash = cashCalculator.GetCashByDate(date);

			Assert.AreEqual(0, resultCash);
		}
	}
}