using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RazorCore.Subscription;

namespace RazorWpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private readonly CashController _cashController;

		private CalenderBackground _calendarBackground;

		public MainWindow()
		{
			InitializeComponent();

			for (int i = 1; i <= 28; i++)
			{
				DeliveryOncePer2MonthsComboBox.Items.Add(new ComboBoxItem { Content = i.ToString() });
				DeliveryOncePerMonthComboBox.Items.Add(new ComboBoxItem { Content = i.ToString() });
				DeliveryTwicePerMonthComboBox.Items.Add(new ComboBoxItem { Content = i.ToString() });
				DeliveryTwicePerMonthComboBox2.Items.Add(new ComboBoxItem { Content = i.ToString() });
			}

			DeliveryOncePer2MonthsComboBox.SelectedIndex = 0;
			DeliveryOncePerMonthComboBox.SelectedIndex = 0;
			DeliveryTwicePerMonthComboBox.SelectedIndex = 0;
			DeliveryTwicePerMonthComboBox2.SelectedIndex = 14;

			var priceList = new PriceList(1, 9, 19);
			_cashController = new CashController(DateTime.Now.Date, priceList);

			Calendar.DisplayDateChanged += CalendarOnDisplayDateChanged;

			Calendar.SelectedDatesChanged += CalendarOnSelectedDatesChanged;
			Calendar.SelectedDate = _cashController.CurrentDate;

			Update();
		}

		private void CalendarOnDisplayDateChanged(object sender, CalendarDateChangedEventArgs calendarDateChangedEventArgs)
		{
			Update();
		}

		private void CalendarOnSelectedDatesChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
		{
			Update();
		}

		private void OnMakeSubscriptionClick(object sender, RoutedEventArgs e)
		{
			try
			{
				var subscriptionPlan = ParseSubscriptionPlan();

				_cashController.AddOrUpdateSubscriptionPlan(subscriptionPlan);

				Update();
			}
			catch (TwicePerMonthDeliveryDublicateDaysException ex)
			{
				MessageBox.Show(ex.Message, "Неверно выбраны дни доставки.");
			}
		}

		private void Update()
		{
			// ReSharper disable once PossibleInvalidOperationException
			var calendarSelectedDate = Calendar.SelectedDate.Value;
			_cashController.CurrentDate = calendarSelectedDate;

			var deliveryDays = _cashController.GetFutureDeliveryDays(_cashController.CurrentDate.AddYears(100));

			_calendarBackground = new CalenderBackground(Calendar);
			_calendarBackground.AddOverlay("circle", "circle.png");
			_calendarBackground.AddOverlay("tjek", "tjek.png");
			_calendarBackground.AddOverlay("cross", "cross.png");
			_calendarBackground.AddOverlay("box", "box.png");
			_calendarBackground.AddOverlay("gray", "gray.png");

			_calendarBackground.grayoutweekends = "gray";

			Calendar.Background = _calendarBackground.GetBackground(_cashController.CurrentDate);

			foreach (var deliveryDay in deliveryDays)
			{
				_calendarBackground.AddDate(deliveryDay, "circle");
			}
			Calendar.Background = _calendarBackground.GetBackground(_cashController.CurrentDate);
			CashTxt.Text = ((int)_cashController.CalculateTotalCash()).ToString();

			MakeSubscription.Content = _cashController.FindActivePlan() != null ?
				$"Изменить подписку с {calendarSelectedDate.ToString("d")}" :
				$"Оформить подписку с {calendarSelectedDate.ToString("d")}";

			DateTxt.Text = $"Выбор даты (текущая дата: {calendarSelectedDate.ToString("d")})";

			DrawSubscriptionHistory();
		}

		private void DrawSubscriptionHistory()
		{
			SubscrHistoryList.Items.Clear();
			foreach (var historyItem in _cashController.GetSubscriptionHistory())
			{
				SubscrHistoryList.Items.Add($"Начало {historyItem.FromDate:d}, " +
											$"Тип: {historyItem.SubscriptionPlan.ProductType}, " +
											$"Доставка: {historyItem.SubscriptionPlan.DeliveryInfo.DeliveryRegularity}, " +
											$"{historyItem.SubscriptionPlan.DeliveryInfo.DeliveryDays.FirstOrDefault()}, {historyItem.SubscriptionPlan.DeliveryInfo.DeliveryDays.LastOrDefault()}");
			}
		}

		private SubscriptionPlan ParseSubscriptionPlan()
		{
			ProductTypes productType;
			DeliveryRegularity deliveryRegularity;
			TwicePerMonthDelivery twicePerMonthDelivery;

			if (RazorAndGelCheckBox.IsChecked == true)
			{
				productType = ProductTypes.RazorAndGel;
			}
			else if (RazorAndGelAndFoamCheckBox.IsChecked == true)
			{
				productType = ProductTypes.RazorAndGelAndFoam;
			}
			else
			{
				productType = ProductTypes.Razor;
			}

			if (DeliveryOncePer2MonthsCheckBox.IsChecked == true)
			{
				twicePerMonthDelivery = new TwicePerMonthDelivery(DeliveryRegularity.OncePerTwoMonths, DeliveryOncePer2MonthsComboBox.SelectedIndex + 1);
			}
			else if (DeliveryOncePerMonthCheckBox.IsChecked == true)
			{
				twicePerMonthDelivery = new TwicePerMonthDelivery(DeliveryRegularity.OncePerMonth, DeliveryOncePerMonthComboBox.SelectedIndex + 1);
			}
			else if (DeliveryTwicePerMonthCheckBox.IsChecked == true)
			{
				twicePerMonthDelivery = new TwicePerMonthDelivery(DeliveryRegularity.TwicePerMonth, DeliveryTwicePerMonthComboBox.SelectedIndex + 1,
					DeliveryTwicePerMonthComboBox2.SelectedIndex + 1);
			}
			else
			{
				twicePerMonthDelivery = new TwicePerMonthDelivery(DeliveryRegularity.Suspended, 1);
			}

			var subscriptionPlan = new SubscriptionPlan(productType, twicePerMonthDelivery);
			return subscriptionPlan;
		}
	}
}
