using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Timers;

using Xamarin.Forms;

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

        StackLayout container = new StackLayout()
        {
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            Spacing = 5
        };

        Button toggleRide = new Button()
        {
            Text = "Start a new Ride",
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.End,
            HeightRequest = 200,
        };

        Timer readTimer;

        public RideListPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = Color.FromRgb(29, 17, 96);

            list.CollectionChanged += List_CollectionChanged;
            container.Children.Add(scroller);

            toggleRide.Clicked += OnRideToggle;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (App.BluetoothHandler.writeReq != null && container.Children.Count < 2)
            {
                container.Children.Add(toggleRide);
            }
            else
            {
                if(container.Children.Count > 1 && App.BluetoothHandler.connected == null)
                {
                    container.Children.RemoveAt(1);
                }
            }

                //Debug page stacking and navigation up and down the stack.
                //addRideListItem("12172016Ride1");
                //addRideListItem("12222016Ride1");

                Content = container;   
        }

        void OnRideToggle(object sender, EventArgs e)
        {
            var butt = (Button)sender;
            if (butt.Text.Contains("Start"))
            {
                string req = "startRide";
                byte[] data = Encoding.ASCII.GetBytes(req);
                App.BluetoothHandler.writeReq.Write(data);
                App.BluetoothHandler.writeLoc.Write(data);
                Console.WriteLine("Writing data");


                readTimer = new Timer(200);
                readTimer.Elapsed += readTimerCallback;
                readTimer.Start();

                butt.Text = "End current ride";

            }
            else
            {
                string req = "endRide";
                byte[] data = Encoding.ASCII.GetBytes(req);

                App.BluetoothHandler.writeReq.Write(data);

                butt.Text = "Start a new ride";
            }
        }

        private void readTimerCallback(object sender, ElapsedEventArgs args){

            readTimer.Dispose();
            readTimer = null;
            App.BluetoothHandler.readRide.Read();
            Console.WriteLine("Read data : " + App.BluetoothHandler.readRide.StringValue);
            addRideListItem(App.BluetoothHandler.readRide.StringValue);

        }

        private void List_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (View la in e.NewItems)
            {
                container.Children.Insert(0, la);
            }

            scroller.Content = container;
            Content = scroller;
        }

        public void addRideListItem(string rideID)
        {

            int month = Int32.Parse(rideID.Substring(0, 2));
            int day = Int32.Parse(rideID.Substring(2, 2));
            int year = Int32.Parse(rideID.Substring(4, 4));

            var date = new DateTime(year, month, day);

            var instance = date.ToString("MMM dd, yyyy : ") + "Ride " + rideID[rideID.Length - 1];

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


            var tapRecognizer = new TapGestureRecognizer();
            tapRecognizer.Tapped += (s, e) =>
            {
                this.Navigation.PushAsync(new DataPage(rideID));
            };

            frame.GestureRecognizers.Add(tapRecognizer);

            list.Add(frame);
        }






    }
}
