using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using BluetoothLE.Core;
using BluetoothLE.Core.Events;
using System.Timers;

using Xamarin.Forms;

namespace SyncCycle
{
    public class BTDeviceButton
    {
        public List<string> DeviceNames = new List<string>();
        public List<IDevice> DeviceList = new List<IDevice>();
        IDevice connected = null;
        public IService service = null;

        public ICharacteristic writeReq;
        public ICharacteristic writeLoc;
        public ICharacteristic readRide;
        public ICharacteristic subscribe;

        Guid serviceID = new Guid("28545278768c471993afc529485f9ab0");

        public SettingsPage pageToUpdate;

        Timer connectTimer;

        public BTDeviceButton()
        {
            connectTimer = new Timer();
            connectTimer.Elapsed += OnButtonClicked;
            connectTimer.Interval = 2000;
            connectTimer.Start();
            
            //Text = "Search for BLE devices";
            //Clicked += OnButtonClicked;
            //BackgroundColor = Color.FromRgb(192, 192, 192);
            //TextColor = Color.FromRgb(24,24,24);
            //Margin = 20;
        }

        public void toggleTimer(bool on)
        {
            if(on)
            {
                connectTimer.Start();
            }
            else
            {
                connectTimer.Stop();
            }
        }

        public void addPage(SettingsPage page)
        {
            pageToUpdate = page;
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
            if(e.Device.Name.ToLower() == "synccycle")
            {
                App.BluetoothAdapter.ConnectToDevice(e.Device);
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
            connected = e.Device;
            e.Device.ServiceDiscovered += DeviceOnServiceDiscovered;
            e.Device.DiscoverServices();
            pageToUpdate.updateSearchBox("Connected to " + e.Device.Name);

            connectTimer.Stop();

        }

        private void DeviceOnServiceDiscovered(object sender, ServiceDiscoveredEventArgs e)
        {
            if(e.Service.Id == serviceID)
            {
                service = e.Service;

                service.CharacteristicDiscovered += saveCharacteristic;
                service.DiscoverCharacteristics();
            }
            Console.WriteLine("Service ID Discovered: " + e.Service.Id + "Is Primary? : " + e.Service.IsPrimary);
        }

        private void saveCharacteristic(object sender, CharacteristicDiscoveredEventArgs e)
        {
            Console.WriteLine("Characteristic discovered : " + e.Characteristic.Uuid.ToString());

            switch(e.Characteristic.Uuid.ToLower())
            {
                //Write Request
                case "28545278768c471993afc5294aaaaaa0":
                    writeReq = e.Characteristic;
                    pageToUpdate.updateSearchBox("Write Request Characteristic saved");
                    break;
                
                //Write Location
                case "28545278768c471993afc5294aaaaaa2":
                    writeLoc = e.Characteristic;
                    pageToUpdate.updateSearchBox("Write Location Characteristic saved");
                    break;
                
                //Read Current Ride
                case "28545278768c471993afc5294bbbbbb0":
                    readRide = e.Characteristic;
                    pageToUpdate.updateSearchBox("Read Ride Characteristic saved");
                    break;
                
                //Subscribe to Data feed
                case "28545278768c471993afc5294cccccc0":
                    subscribe = e.Characteristic;
                    pageToUpdate.updateSearchBox("Subscribe Characteristic saved");
                    break;

                default:
                    Console.WriteLine("Characteristic not from syncCycle : " + e.Characteristic.Uuid);
                    break;
            }

        }
    }
}
