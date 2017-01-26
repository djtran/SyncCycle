using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using BluetoothLE.Core;
using BluetoothLE.Core.Events;

using Xamarin.Forms;

namespace SyncCycle.DataVisuals
{
    class BTDeviceButton : Button
    {
        public List<string> DeviceNames = new List<string>();
        public List<IDevice> DeviceList = new List<IDevice>();
        IDevice connected = null;
        DataPage pageToUpdate;

        public BTDeviceButton(DataPage page)
        {
            pageToUpdate = page;
            Text = "Search for BLE devices";
            Clicked += OnButtonClicked;
        }
        void OnButtonClicked(object sender, EventArgs args)
        {
            // discover some devices
            pageToUpdate.updateSearchBox("Searching...");

            App.BluetoothAdapter.StartScanningForDevices();
            foreach (IDevice each in App.BluetoothAdapter.ConnectedDevices)
            {
                pageToUpdate.updateSearchBox(each.Name);
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

                pageToUpdate.updateSearchBox(b);
            }
        }

        void connectToDevice(object sender, EventArgs args)
        {
            pageToUpdate.updateSearchBox("Attempting to connect");
            Button b = (Button)sender;
            connected = DeviceList.Find(x => x.Name == b.Text);

            if (connected != null)
            {
                pageToUpdate.updateSearchBox("Device connecting to: " + connected.Name);
                App.BluetoothAdapter.ConnectToDevice(connected);
                App.BluetoothAdapter.DeviceConnected += connectSuccess;
                App.BluetoothAdapter.DeviceFailedToConnect += connectFail;
            }
        }

        private void connectFail(object sender, DeviceConnectionEventArgs e)
        {
            pageToUpdate.DisplayAlert("Failed!", "Failed to connect to " + e.Device.Name + " with error message : " + e.ErrorMessage, "Aw man :<");
            connected = null;
            pageToUpdate.updateSearchBox("Failed to connect");
        }

        private void connectSuccess(object sender, DeviceConnectionEventArgs e)
        {
            pageToUpdate.DisplayAlert("Connected!", "You've connected to " + e.Device.Name, "Wow!");
            e.Device.ServiceDiscovered += DeviceOnServiceDiscovered;
            e.Device.DiscoverServices();
            pageToUpdate.updateSearchBox("Connected to " + e.Device.Name);

            Button com = new Button()
            {
                Text = "Communicate with Device"
            };
            com.Clicked += readTest;

            pageToUpdate.updateContainer(com);
        }

        private void readTest(object sender, EventArgs e)
        {
            pageToUpdate.updateSearchBox("Attempting to read" + connected.Name);
            connected.Services[0].Characteristics[0].Read();
            Console.WriteLine(connected.Services[0].Characteristics[0].StringValue);
            pageToUpdate.updateSearchBox(connected.Services[0].Characteristics[0].StringValue);

        }

        private void DeviceOnServiceDiscovered(object sender, ServiceDiscoveredEventArgs e)
        {
            e.Service.DiscoverCharacteristics();
        }
    }
}
