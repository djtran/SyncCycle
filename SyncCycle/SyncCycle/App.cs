using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using BluetoothLE.Core;
using BluetoothLE.Core.Events;
using System.Collections;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using Akavache;

namespace SyncCycle
{
    /// <summary>
    ///// ******* OVERVIEW *******
    /////
    ///// NuGet library         -   Purpose
    ///
    ///// Xamarin.Forms.Maps    -   Cross-platform maps using native APIs
    ///// Xamarin.BluetoothLE   -   Bluetooth LE Connectivity and communications.
    ///// OxyPlot               -   Data Visualization - Charts and Plots
    /// 
    /// 
    /// </summary>

    public class App : Application
	{
        private static readonly IAdapter _bluetoothAdapter;
        public static IAdapter BluetoothAdapter { get { return _bluetoothAdapter; } }
        private static BTDeviceButton _bluetoothHandler;

        public static BTDeviceButton BluetoothHandler { get { return _bluetoothHandler; } }

        static App()
        {
            _bluetoothAdapter = DependencyService.Get<IAdapter>();

            _bluetoothHandler = new BTDeviceButton();
            BluetoothAdapter.DeviceDiscovered += _bluetoothHandler.DeviceDiscovered;

            _bluetoothAdapter.ScanTimeout = TimeSpan.FromSeconds(3);
            _bluetoothAdapter.ConnectionTimeout = TimeSpan.FromSeconds(3);


        }

        public App ()
        {
            BlobCache.ApplicationName = "SyncCycle";
            BlobCache.EnsureInitialized();
            

            var rideListContainer = new NavigationPage(new RideListPage());
            rideListContainer.Title = "Rides";

            var settingspage = new SettingsPage();
            _bluetoothHandler.addPage(settingspage);
            
            // The root page of your application
            var appContainer = new TabbedPage();

            appContainer.Children.Add(new NavPage());
            appContainer.Children.Add(rideListContainer);
            appContainer.Children.Add(settingspage);

            MainPage = appContainer;
        }

       

        protected override void OnStart ()
		{
            // Handle when your app starts
        }

		protected override void OnSleep ()
		{
            // Handle when your app sleeps
            //_bluetoothHandler.toggleTimer(false);
            BlobCache.Shutdown().Wait();

            if(App.BluetoothHandler.connected != null)
            {
                App.BluetoothHandler.connected.Disconnect();
            }

		}

		protected override void OnResume ()
		{
            // Handle when your app resumes
            //_bluetoothHandler.toggleTimer(true);
		}
	}
}
