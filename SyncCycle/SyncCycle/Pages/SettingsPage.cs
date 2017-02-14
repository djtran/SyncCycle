using SyncCycle.DataVisuals;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SyncCycle
{
    class SettingsPage : ContentPage
    {

        public BTDeviceButton b;

        StackLayout search = new StackLayout()
        {
            VerticalOptions = LayoutOptions.End,
        };

        ScrollView searchWrap = new ScrollView()
        {
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            HeightRequest = 100,
        };

        StackLayout container = new StackLayout()
        {
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            Spacing = 5
        };

        public SettingsPage()
        {

            Padding = 20;

            Title = "Settings";
            BackgroundColor = Color.FromRgb(29,17,96);

            b = new BTDeviceButton(this);
            App.BluetoothAdapter.DeviceDiscovered += b.DeviceDiscovered;

            container.Children.Add(searchWrap);
            container.Children.Add(b);

            Content = container;
            App.BluetoothAdapter.ScanTimeoutElapsed += timedout;
        }

        private void timedout(object sender, EventArgs e)
        {
            updateSearchBox("Search ended.");
        }

        public void updateSearchBox(string words)
        {
            Device.BeginInvokeOnMainThread(() => {
                search.Children.Add(new Label { Text = words });
                searchWrap.Content = search;
            });

        }

        public void updateSearchBox(View visual)
        {
            Device.BeginInvokeOnMainThread(() => {
                search.Children.Add(visual);
                searchWrap.Content = search;
            });
        }

        public void updateContainer(View visual)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                container.Children.Add(visual);
                Content = container;
            });
        }
    }
}
