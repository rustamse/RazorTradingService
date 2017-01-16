using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RazorCore.Cash;
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
			var subscriptionPlan = ParseSubscriptionPlan();

			_cashController.AddOrUpdateSubscriptionPlan(subscriptionPlan);

			Update();
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
				                            $"Тип: {historyItem.SubscriptionPlan.SubscriptionType}, " +
											$"Доставка: {historyItem.SubscriptionPlan.DeliveryRegularity}, " +
											$"{historyItem.SubscriptionPlan.DeliveryTime.DeliveryDays.FirstOrDefault()}, {historyItem.SubscriptionPlan.DeliveryTime.DeliveryDays.LastOrDefault()}");
			}
		}

		private SubscriptionPlan ParseSubscriptionPlan()
		{
			SubscriptionTypes subscriptionType;
			DeliveryRegularity deliveryRegularity;
			DeliveryTime deliveryTime;

			if (RazorAndGelCheckBox.IsChecked == true)
			{
				subscriptionType = SubscriptionTypes.RazorAndGel;
			}
			else if (RazorAndGelAndFoamCheckBox.IsChecked == true)
			{
				subscriptionType = SubscriptionTypes.RazorAndGelAndFoam;
			}
			else
			{
				subscriptionType = SubscriptionTypes.Razor;
			}

			if (DeliveryOncePer2MonthsCheckBox.IsChecked == true)
			{
				deliveryRegularity = DeliveryRegularity.OncePerTwoMonths;
				deliveryTime = new DeliveryTime(DeliveryOncePer2MonthsComboBox.SelectedIndex + 1);
			}
			else if (DeliveryOncePerMonthCheckBox.IsChecked == true)
			{
				deliveryRegularity = DeliveryRegularity.OncePerMonth;
				deliveryTime = new DeliveryTime(DeliveryOncePerMonthComboBox.SelectedIndex + 1);
			}
			else if (DeliveryTwicePerMonthCheckBox.IsChecked == true)
			{
				deliveryRegularity = DeliveryRegularity.TwicePerMonth;
				deliveryTime = new DeliveryTime(DeliveryTwicePerMonthComboBox.SelectedIndex + 1,
					DeliveryTwicePerMonthComboBox2.SelectedIndex + 1);
			}
			else
			{
				deliveryRegularity = DeliveryRegularity.Suspended;
				deliveryTime = new DeliveryTime(1);
			}

			var subscriptionPlan = new SubscriptionPlan(subscriptionType, deliveryRegularity,
				deliveryTime);
			return subscriptionPlan;
		}
	}
}
