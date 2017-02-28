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

        public List<IService> services = new List<IService>();
        SettingsPage pageToUpdate;

        public BTDeviceButton(SettingsPage page)
        {
            pageToUpdate = page;
            Text = "Search for BLE devices";
            Clicked += OnButtonClicked;
            BackgroundColor = Color.FromRgb(192, 192, 192);
            TextColor = Color.FromRgb(24,24,24);
            Margin = 20;
        }
        void OnButtonClicked(object sender, EventArgs args)
        {
            // discover some devices
            pageToUpdate.updateSearchBox("Searching...");

            App.BluetoothAdapter.StartScanningForDevices();
            foreach (IDevice each in App.BluetoothAdapter.ConnectedDevices)
            {
                pageToUpdate.updateSearchBox(each.Name);
                Console.WriteLine(each.Name);
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
            Device.BeginInvokeOnMainThread(() =>
            {
                pageToUpdate.DisplayAlert("Failed!", "Failed to connect to " + e.Device.Name + " with error message : " + e.ErrorMessage, "Aw man :<");
                connected = null;
                pageToUpdate.updateSearchBox("Failed to connect");
            });
        }

        private void connectSuccess(object sender, DeviceConnectionEventArgs e)
        {
            e.Device.ServiceDiscovered += DeviceOnServiceDiscovered;
            e.Device.DiscoverServices();
            pageToUpdate.updateSearchBox("Connected to " + e.Device.Name);


            Device.BeginInvokeOnMainThread(() => {
                pageToUpdate.DisplayAlert("Connected!", "You've connected to " + e.Device.Name, "Wow!");
                Button com = new Button()
                {
                    Text = "Communicate with Device"
                };
                com.Clicked += readTest;

                pageToUpdate.updateContainer(com);
            });
        }

        private void readTest(object sender, EventArgs e)
        {
            pageToUpdate.updateSearchBox("Attempting to read " + connected.Name + ", there are " + services.Count + " services available.");
            if(services.Count > 0)
            {
                foreach (IService iserv in services)
                {

                    if (iserv.Characteristics.Count > 0)
                    {
                        foreach(ICharacteristic each in iserv.Characteristics)
                        {
                            if(each.CanRead)
                            {
                                pageToUpdate.updateSearchBox(each.Uuid);
                                pageToUpdate.updateSearchBox(each.StringValue);
                            }
                        }
                    }
                }
            }
        }

        private void DeviceOnServiceDiscovered(object sender, ServiceDiscoveredEventArgs e)
        {
            services.Add(e.Service);
            
            e.Service.CharacteristicDiscovered += printCharacteristics;
            e.Service.DiscoverCharacteristics();
            Console.WriteLine("Service ID Discovered: " + e.Service.Id + "Is Primary? : " + e.Service.IsPrimary);
            Console.WriteLine("Discovering Characteristics");
        }

        private void printCharacteristics(object sender, CharacteristicDiscoveredEventArgs e)
        {
            Console.WriteLine("Characteristic discovered : " + e.Characteristic.Uuid.ToString());
        }
    }
}
