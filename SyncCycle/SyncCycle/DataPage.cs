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
        IDevice connected = null;
        

        public List<BikeData> Data = new List<BikeData>();
        
        StackLayout search = new StackLayout()
        {
            VerticalOptions = LayoutOptions.End,
        };

        ScrollView searchWrap = new ScrollView()
        {
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        StackLayout container = new StackLayout()
        {
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            Spacing = 10
        };

        public DataPage(List<BikeData> data)
        {
            Padding = 20;
            Data = data;

            var tableView = new TableView()
            {
                BackgroundColor = Color.FromRgb(18, 123, 189),
                HasUnevenRows = true,
                Intent = TableIntent.Data,
                Root = new TableRoot("Bike Diagnostics"),
                VerticalOptions = LayoutOptions.Start,
                HeightRequest = 175
            };

            foreach (BikeData dataCell in data)
            {
                DataViewCell temp = new DataViewCell();
                temp.BindingContext = dataCell;

                var sect = new TableSection("Some Section")
                {
                    temp
                };
                tableView.Root.Add(sect);
            }

            Button b = new Button();
            b.Text = "Search for BLE devices";
            b.Clicked += OnButtonClicked;

            container.Children.Add(tableView);
            container.Children.Add(searchWrap);
            container.Children.Add(b);

            Content = container;

            App.BluetoothAdapter.ScanTimeoutElapsed += timedout;
        }

        private void timedout(object sender, EventArgs e)
        {
            updateSearchBox("Search ended.");   
        }

        void OnButtonClicked(object sender, EventArgs args)
        {
            // discover some devices
            search.Children.Add(new Label { Text = "Searching..." });
            searchWrap.Content = search;
            App.BluetoothAdapter.StartScanningForDevices();
            foreach(IDevice each in App.BluetoothAdapter.ConnectedDevices)
            {
                updateSearchBox(each.Name);
            }
        }

        public void DeviceDiscovered(object sender, BluetoothLE.Core.Events.DeviceDiscoveredEventArgs e)
        {
            //If any devices aren't currently in the list
            if (DeviceList.All(X => X.Id != e.Device.Id))
            {
                //Add the devices and make a button for them.
                DeviceList.Add(e.Device);
                DeviceNames.Add(e.Device.Name);
                Button b = new Button();
                b.Text = e.Device.Name;
                b.Clicked += connectToDevice;

                search.Children.Add(b);
            }
            searchWrap.Content = search;
        }

        void connectToDevice(object sender, EventArgs args)
        {
            updateSearchBox("Attempting to connect");
            Button b= (Button) sender;
            connected = DeviceList.Find(x => x.Name == b.Text);
            
            if (connected != null)
            {
                updateSearchBox("Device connecting to: " + connected.Name);
                App.BluetoothAdapter.ConnectToDevice(connected);
                App.BluetoothAdapter.DeviceConnected += connectSuccess;
                App.BluetoothAdapter.DeviceFailedToConnect += connectFail;
            }
        }

        private void connectFail(object sender, DeviceConnectionEventArgs e)
        {
            DisplayAlert("Failed!", "Failed to connect to " + e.Device.Name + " with error message : " + e.ErrorMessage, "Aw man :<");
            connected = null;
            updateSearchBox("Failed to connect");
        }

        private void connectSuccess(object sender, DeviceConnectionEventArgs e)
        {
            DisplayAlert("Connected!", "You've connected to " + e.Device.Name, "Wow!");
            e.Device.ServiceDiscovered += DeviceOnServiceDiscovered;
            e.Device.DiscoverServices();
            updateSearchBox("Connected to " + e.Device.Name);

            Button com = new Button()
            {
                Text = "Communicate with Device"
            };
            com.Clicked += readTest;

            container.Children.Add(com);
        }

        private void readTest(object sender, EventArgs e)
        {
            updateSearchBox("Attempting to read" + connected.Name);
            connected.Services[0].Characteristics[0].Read();
            Console.WriteLine(connected.Services[0].Characteristics[0].StringValue);
            updateSearchBox(connected.Services[0].Characteristics[0].StringValue);

        }

        private void DeviceOnServiceDiscovered(object sender, ServiceDiscoveredEventArgs e)
        {
            e.Service.DiscoverCharacteristics();
        }

        void updateSearchBox(string words)
        {
            search.Children.Add(new Label { Text = words });
            searchWrap.Content = search;
        }
    }
}
