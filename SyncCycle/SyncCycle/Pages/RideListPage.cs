using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Timers;
using System.Reactive.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;
using Plugin.BLE;

namespace SyncCycle
{

	class RideListPage : ContentPage
	{
		ObservableCollection<View> list = new ObservableCollection<View>();

		ScrollView scroller = new ScrollView()
		{
			VerticalOptions = LayoutOptions.FillAndExpand,
			HorizontalOptions = LayoutOptions.FillAndExpand,
		};

		StackLayout scrollRideStack = new StackLayout()
		{
			VerticalOptions = LayoutOptions.FillAndExpand,
			HorizontalOptions = LayoutOptions.FillAndExpand,
			Spacing = 5
		};

        StackLayout top = new StackLayout()
        {
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand,
        };

		Button toggleRide = new Button()
		{
			Text = "Start a new Ride",
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.End,
			HeightRequest = 50,
		};
        
		List<string> names = new List<string>();

		public RideListPage()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromRgb(29, 17, 96);

			list.CollectionChanged += List_CollectionChanged;

			toggleRide.Clicked += OnRideToggle;
            //scrollRideStack.Children.Add(new Label()
            //{
            //    Text = "I'm in scrollRideStack",
            //});
            addRideListItem("01242017Ride1");

		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

            foreach (string each in names)
            {
                addRideListItem(each);
            }

            refreshPage();
            if (!CrossBluetoothLE.Current.IsOn)
            {
                DisplayAlert("Bluetooth", "Please enable bluetooth on your phone", "OK");
            }
            else
            {
                if(App.BluetoothHandler.connected == null)
                {
                    App.BluetoothHandler.startSearch();
                }
            }
        }

		async void OnRideToggle(object sender, EventArgs e)
		{
			var butt = (Button)sender;
			if (butt.Text.Contains("Start"))
			{
				string req = "startRide";
				byte[] data = Encoding.ASCII.GetBytes(req);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.BluetoothHandler.writeReq.WriteAsync(data);
                    Console.WriteLine("Writing data");

                    await App.BluetoothHandler.readRide.ReadAsync();
                    updateRideList(App.BluetoothHandler.readRide.StringValue);
                });

                
				butt.Text = "End current ride";

			}
			else
			{
				string req = "endRide";
				byte[] data = Encoding.ASCII.GetBytes(req);
				await App.BluetoothHandler.writeReq.WriteAsync(data);

				butt.Text = "Start a new ride";
			}
		}

        private void updateRideList(string ride)
        {
            if(ride != "" && ride != null)
            {
                addRideListItem(ride);
            }
        }

		private void List_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
            Device.BeginInvokeOnMainThread(() => {
                top.Children.Remove(scroller);
			    foreach (View la in e.NewItems)
			    {
				    scrollRideStack.Children.Insert(0, la);
			    }

			    scroller.Content = scrollRideStack;
            });
            refreshPage();            
		}

        void refreshPage()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (App.BluetoothHandler.writeReq != null && App.BluetoothHandler.readRide != null && App.BluetoothHandler.service != null && App.BluetoothHandler.writeLoc != null && App.BluetoothHandler.subscribe != null)
                {
                    scrollRideStack.Children.Add(toggleRide);
                }
                scroller.Content = scrollRideStack;
                
                Content = scroller;
            });
        }

		public void addRideListItem(string rideID)
		{

			int month = Int32.Parse(rideID.Substring(0, 2));
			int day = Int32.Parse(rideID.Substring(2, 2));
			int year = Int32.Parse(rideID.Substring(4, 4));

			var date = new DateTime(year, month, day);

			var instance = date.ToString("MMM dd, yyyy : ") + "Ride " + rideID[rideID.Length - 1];
            Console.WriteLine();
            Console.WriteLine("Going to add " + instance + " to RideListPage");
            Console.WriteLine();
            Console.WriteLine();

            var item = new Label()
			{
				BackgroundColor = Color.Transparent,
				Text = instance,
				TextColor = Color.White,
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
				FontAttributes = FontAttributes.Bold,

				MinimumHeightRequest = 20,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};

			var frame = new ContentView()
			{
				BackgroundColor = Color.FromRgb(0, 121, 193),
				Padding = 15,
				Content = item,
				//OutlineColor = Color.Black,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};

            Console.WriteLine("Tap Recognizer for new Ride is being made");
            Console.WriteLine();
            Console.WriteLine();
            var tapRecognizer = new TapGestureRecognizer();
			tapRecognizer.Tapped += (s, e) =>
			{
				this.Navigation.PushAsync(new DataPage(rideID));
			};

			frame.GestureRecognizers.Add(tapRecognizer);

            Console.WriteLine("Adding to the frame....");

            list.Add(frame);
        }

	}
}
