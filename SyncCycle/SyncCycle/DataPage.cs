using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using BluetoothLE.Core;

using Xamarin.Forms;

namespace SyncCycle
{
    class DataPage : ContentPage
    {

        private List<IDevice> DeviceList;
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
            App.BluetoothAdapter.StartScanningForDevices();

        }

        private void DeviceDiscovered(object sender, BluetoothLE.Core.Events.DeviceDiscoveredEventArgs e)
        {
            if(DeviceList.All(X => X.Id != e.Device.Id))
            {
                DeviceList.Add(e.Device);
                search.Children.Add(new Label { Text = e.Device.Name});
            }
        }

    }
}
