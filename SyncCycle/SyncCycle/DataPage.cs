using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using BluetoothLE.Core;

using Xamarin.Forms;
using BluetoothLE.Core.Events;

namespace SyncCycle
{
    class DataPage : ContentPage
    {
        private List<string> DeviceNames = new List<string>();
        private List<IDevice> DeviceList = new List<IDevice>();
        
        StackLayout search = new StackLayout()
        {
            VerticalOptions = LayoutOptions.End,
        };

        public DataPage(List<BikeData> data)
        {
            Padding = 20;

            var tableView = new TableView()
            {
                BackgroundColor = Color.FromRgb(18, 123, 189),
                HasUnevenRows = true,
                Intent = TableIntent.Data,
                Root = new TableRoot("Bike Diagnostics"),
                VerticalOptions = LayoutOptions.StartAndExpand,
            };

            foreach (BikeData dataCell in data)
            {
                DataViewCell temp = new DataViewCell();
                temp.BindingContext = dataCell;

                var sect = new TableSection("Some Shitty Section")
                {
                    temp
                };
                tableView.Root.Add(sect);
            }

            Button b = new Button();
            b.Text = "Search for BLE devices";
            b.Clicked += OnButtonClicked;

            Content = new ScrollView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children = {
                        tableView,
                        b,
                        search
                    },
                    Spacing = 10
                }
            };
        }

        void OnButtonClicked(object sender, EventArgs args)
        {
            // discover some devices
            Console.WriteLine("Before Scan");
            App.BluetoothAdapter.StartScanningForDevices();
            Console.WriteLine("After Scan");
        }

        public void DeviceDiscovered(object sender, BluetoothLE.Core.Events.DeviceDiscoveredEventArgs e)
        {


            //Search won't update.. Probably need to SetBinding() in order for it to update the display
            Console.WriteLine("Before Device List");
            if (DeviceList.All(X => X.Id != e.Device.Id))
            {
                DeviceList.Add(e.Device);
                DeviceNames.Add(e.Device.Name);
                Button b = new Button();
                b.Text = e.Device.Name;
                b.Clicked += connectPls;

                search.Children.Add(b);
            }
            Console.WriteLine("After Device List");
        }

        void connectPls(object sender, EventArgs args)
        {
            Button b= (Button) sender;
            App.BluetoothAdapter.ConnectToDevice(DeviceList.Find(x => x.Name == b.Text));
            App.BluetoothAdapter.DeviceConnected += connectSuccess;
            App.BluetoothAdapter.DeviceFailedToConnect += connectFail;
        }

        private void connectFail(object sender, DeviceConnectionEventArgs e)
        {
            DisplayAlert("Failed!", "Failed to connect to " + e.Device.Name + " with error message : " + e.ErrorMessage, "Aw man :<");
        }

        private void connectSuccess(object sender, DeviceConnectionEventArgs e)
        {
            DisplayAlert("Connected!", "You've connected to " + e.Device.Name, "Wow!");
        }
    }
}
