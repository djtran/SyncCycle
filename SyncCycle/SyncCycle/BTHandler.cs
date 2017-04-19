using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Timers;

using Xamarin.Forms;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Extensions;
using Plugin.BLE.Abstractions.Exceptions;

namespace SyncCycle
{
    public class BTHandler
    {
        public List<string> DeviceNames = new List<string>();
        public List<IDevice> DeviceList = new List<IDevice>();
        public IDevice connected = null;
        public IService service = null;

        public ICharacteristic writeReq;
        public ICharacteristic writeLoc;
        public ICharacteristic readRide;
        public ICharacteristic subscribe;

        Guid serviceID = Guid.Parse("12ab");

        public SettingsPage pageToUpdate;

        public IAdapter adapter;
        public IBluetoothLE ble;

        public BTHandler()
        {
        }

        public void init()
        {
            ble = CrossBluetoothLE.Current;
            adapter = ble.Adapter;
            adapter.ScanTimeout = 3000;
            adapter.ScanMode = ScanMode.Balanced;
            adapter.DeviceDiscovered += Adapter_DeviceDiscovered;
            adapter.DeviceConnected += Adapter_DeviceConnected;
            adapter.DeviceConnectionLost += Adapter_DeviceConnectionLost;
            adapter.DeviceDisconnected += Adapter_DeviceDisconnected;
        }

        public void addPage(SettingsPage page)
        {
            pageToUpdate = page;
        }
        public async void startSearch()
        {
            if (!adapter.IsScanning && ble.State == BluetoothState.On)
            {
                pageToUpdate.updateSearchBox("Searching");
                await adapter.StartScanningForDevicesAsync();
            }
        }

        private async void Adapter_DeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {

            if (e.Device.Name != null)
            {
                Console.WriteLine(e.Device.Name.ToLower());
                DeviceList.Add(e.Device);
                if (e.Device.Name.ToLower() == "synccycle")
                {
                    try
                    {
                        await adapter.ConnectToDeviceAsync(e.Device);
                    }
                    catch (DeviceConnectionException ex)
                    {
                        connected = null;
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            pageToUpdate.updateSearchBox("Failed to connect to " + ex.DeviceName + " with error message : " + ex.Message);
                        });
                    }

                    //App.BluetoothAdapter.ConnectToDevice(e.Device);
                    //App.BluetoothAdapter.DeviceConnected += connectSuccess;
                    //App.BluetoothAdapter.DeviceFailedToConnect += connectFail;
                    //App.BluetoothAdapter.StopScanningForDevices();
                    //connectTimer.Dispose();
                }
            }
        }

        private async void Adapter_DeviceConnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            connected = e.Device;
            pageToUpdate.updateSearchBox("Connected to device: " + e.Device.Name);
            try {
                service = await connected.GetServiceAsync(serviceID);
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    pageToUpdate.updateSearchBox("Failed to get services: " + ex.Message);
                });
            }
        }

        private async void getCharacteristics()
        {
            writeReq = await service.GetCharacteristicAsync(Guid.Parse("28545278-768c-4719-93af-c5294aaaaaa0"));
            writeLoc = await service.GetCharacteristicAsync(Guid.Parse("28545278-768c-4719-93af-c5294aaaaaa2"));
            readRide = await service.GetCharacteristicAsync(Guid.Parse("28545278-768c-4719-93af-c5294bbbbbb0"));
            subscribe = await service.GetCharacteristicAsync(Guid.Parse("28545278-768c-4719-93af-c5294cccccc4"));
            Console.WriteLine("Write Req " + writeReq.Id);
            Console.WriteLine("Write Loc " + writeLoc.Id);
            Console.WriteLine("Read Ride " + readRide.Id);
            Console.WriteLine("Subscribe " + subscribe.Id);
        }

        private void Adapter_DeviceConnectionLost(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceErrorEventArgs e)
        {
            if (subscribe != null)
            {
                subscribe.StopUpdatesAsync();
            }
            connected = null;
            service = null;
            writeReq = null;
            writeLoc = null;
            readRide = null;
            subscribe = null;
            startSearch();
        }

        private void Adapter_DeviceDisconnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {

            if(subscribe != null)
            {
                subscribe.StopUpdatesAsync();
            }
            connected = null;
            service = null;
            writeReq = null;
            writeLoc = null;
            readRide = null;
            subscribe = null;
        }
    }
}
