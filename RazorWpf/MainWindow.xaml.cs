using System;
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

		private readonly CalenderBackground _calendarBackground;

		public MainWindow()
		{
			InitializeComponent();

			for (int i = 1; i <= 28; i++)
			{
				RazorComboBox.Items.Add(new ComboBoxItem { Content = i.ToString() });
				RazorAndGelComboBox.Items.Add(new ComboBoxItem { Content = i.ToString() });
				RazorAndGelAndFoam1ComboBox.Items.Add(new ComboBoxItem { Content = i.ToString() });
				RazorAndGelAndFoam2ComboBox.Items.Add(new ComboBoxItem { Content = i.ToString() });
			}

			RazorComboBox.SelectedIndex = 0;
			RazorAndGelComboBox.SelectedIndex = 0;
			RazorAndGelAndFoam1ComboBox.SelectedIndex = 0;
			RazorAndGelAndFoam2ComboBox.SelectedIndex = 14;

			var priceList = new PriceList(1, 9, 19);
			_cashController = new CashController(DateTime.Now.Date, priceList);

			_calendarBackground = new CalenderBackground(Calendar);
			_calendarBackground.AddOverlay("circle", "circle.png");
			_calendarBackground.AddOverlay("tjek", "tjek.png");
			_calendarBackground.AddOverlay("cross", "cross.png");
			_calendarBackground.AddOverlay("box", "box.png");
			_calendarBackground.AddOverlay("gray", "gray.png");

			_calendarBackground.grayoutweekends = "gray";

			Calendar.Background = _calendarBackground.GetBackground(_cashController.CurrentDate);

			// Update background when changing the displayed month
			Calendar.DisplayDateChanged += CalendarOnDisplayDateChanged;

			Calendar.SelectedDatesChanged += CalendarOnSelectedDatesChanged;
			Calendar.SelectedDate = DateTime.Now.Date;
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
			SubscriptionPlan subscriptionPlan = null;
			if (razorCheckBox.IsChecked == true)
			{
				subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor, DeliveryRegularity.OncePerMonth,
					new DeliveryTime(RazorComboBox.SelectedIndex + 1));
			}
			if (razorAndGelCheckBox.IsChecked == true)
			{
				subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.RazorAndGel, DeliveryRegularity.OncePerMonth,
					new DeliveryTime(RazorAndGelComboBox.SelectedIndex + 1));
			}
			else if (razorAndGelAndFoamCheckBox.IsChecked == true)
			{
				subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.RazorAndGelAndFoam, DeliveryRegularity.OncePerMonth,
					new DeliveryTime(RazorAndGelAndFoam1ComboBox.SelectedIndex + 1, RazorAndGelAndFoam2ComboBox.SelectedIndex + 1));
			}

			_cashController.SetSubscriptionPlan(subscriptionPlan);

			Update();
		}

		private void Update()
		{
			// ReSharper disable once PossibleInvalidOperationException
			var calendarSelectedDate = Calendar.SelectedDate.Value;
			_cashController.CurrentDate = calendarSelectedDate;

			var deliveryDays = _cashController.GetFutureDeliveryDays(_cashController.CurrentDate.AddYears(100));
			foreach (var deliveryDay in deliveryDays)
			{
				_calendarBackground.AddDate(deliveryDay, "circle");
			}
			Calendar.Background = _calendarBackground.GetBackground(_cashController.CurrentDate);
			CashTxt.Text = ((int)_cashController.CalculateTotalCash()).ToString();

			MakeSubscription.Content = $"Оформить подписку с {calendarSelectedDate.ToString("d")}";
		}
	}
}
