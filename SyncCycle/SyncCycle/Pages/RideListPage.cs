﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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

        public RideListPage()
        {
            BackgroundColor = Color.FromRgb(29, 17, 96);

            list.CollectionChanged += List_CollectionChanged;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Button toggleRide = new Button()
            {
                Text = "Start a new Ride",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.End,
                HeightRequest = 20,
            };
            toggleRide.Clicked += OnRideToggle;

            container.Children.Add(scroller);
            container.Children.Add(toggleRide);

            //Debug page stacking and navigation up and down the stack.
            //addRideListItem("12172016Ride1");
            //addRideListItem("12222016Ride1");
            //addRideListItem("01102017Ride3");
            //addRideListItem("01172017Ride4");
            //addRideListItem("01222017Ride2");
            //addRideListItem("01302017Ride4");
            //addRideListItem("01312017Ride1");
            //addRideListItem("02172017Ride4");
            //addRideListItem("02212017Ride1");
            //addRideListItem("02222017Ride2");

            Content = container;
        }

        void OnRideToggle(object sender, EventArgs e)
        {
            var butt = (Button)sender;
            if(butt.Text.Contains("Start"))
            {
                string req = "startRide";
                byte[] data = Encoding.ASCII.GetBytes(req);

                App.BluetoothHandler.writeReq.Write(data);


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
       
        private void List_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach(View la in e.NewItems)
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
                BackgroundColor = Color.FromRgb(0,121,193),
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
