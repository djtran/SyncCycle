using Akavache;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Timers;
using System.Reactive.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;

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

		Timer readTimer;

		List<string> names = new List<string>();

		public RideListPage()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromRgb(29, 17, 96);

			list.CollectionChanged += List_CollectionChanged;

			toggleRide.Clicked += OnRideToggle;
            scrollRideStack.Children.Add(new Label()
            {
                Text = "I'm in scrollRideStack",
            });
            addRideListItem("01242017Ride1");
			//loadData();
		}

		void loadData()
		{
			Console.WriteLine("Load data activated");
			var flat = BlobCache.LocalMachine.GetAllKeys().ToEnumerable();

			foreach(var thing in flat)
			{
				foreach(var hopefullyString in thing)
				{
					Console.WriteLine(hopefullyString.ToString());
					names.Add(hopefullyString);
				}
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

            foreach (string each in names)
            {
                addRideListItem(each);
            }

            refreshPage();
		}

		void OnRideToggle(object sender, EventArgs e)
		{
			var butt = (Button)sender;
			if (butt.Text.Contains("Start"))
			{
				string req = "startRide";
				byte[] data = Encoding.ASCII.GetBytes(req);
				App.BluetoothHandler.writeReq.Write(data);
				Console.WriteLine("Writing data");

                App.BluetoothHandler.readRide.ValueUpdated += ReadRide_ValueUpdated;

                if (readTimer != null)
                {
                    readTimer.Stop();
                }
				readTimer = new Timer(400);
				readTimer.Elapsed += readTimerCallback;
				readTimer.Start();

				butt.Text = "End current ride";

			}
			else
			{
				string req = "endRide";
				byte[] data = Encoding.ASCII.GetBytes(req);
				App.BluetoothHandler.writeReq.Write(data);

                if(readTimer != null)
                {
                    readTimer.Stop();
                    readTimer = null;
                }

				butt.Text = "Start a new ride";
			}
		}

        private void ReadRide_ValueUpdated(object sender, BluetoothLE.Core.Events.CharacteristicReadEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine("Read Value updated");
            string ride = e.Characteristic.StringValue;

            foreach(byte b in e.Characteristic.Value)
            {
                Console.WriteLine(b.GetHashCode());
            }

            if(ride != "" && ride != null)
            {
                if(readTimer != null)
                {
                    readTimer.Dispose();
                    readTimer = null;
                }
                addRideListItem(ride);
            }
        }
        

		private void readTimerCallback(object sender, ElapsedEventArgs args){
            App.BluetoothHandler.readRide.Read();
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
                if (!top.Children.Contains(scroller))
                {
                    top.Children.Insert(0, scroller);
                }

                if (App.BluetoothHandler.writeReq != null && scrollRideStack.Children.Count < 2)
                {
                    top.Children.Add(toggleRide);
                }
                scrollRideStack.Children.Add(toggleRide);
                scroller.Content = scrollRideStack;
                //Content = top;

                //Refreshing content = scroller
                Console.WriteLine("Content = scroller triggering next");
                Console.WriteLine("Content = scroller triggering next");
                Console.WriteLine("Content = scroller triggering next");
                Console.WriteLine("Content = scroller triggering next");
                Console.WriteLine("Content = scroller triggering next");

                this.Content = scroller;
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
